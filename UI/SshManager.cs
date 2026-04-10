using Renci.SshNet;
using System.Configuration;
using System.Text.RegularExpressions;

namespace UI
{
    public class SshManager : IDisposable
    {
        private SshClient _client;
        private ShellStream _stream;

        private readonly string MainHost = ConfigurationManager.AppSettings["MainHost"];
        private readonly string MainUser = ConfigurationManager.AppSettings["MainUser"];
        private readonly string PemPath = ConfigurationManager.AppSettings["PemPath"];

        private readonly string BastionHost = ConfigurationManager.AppSettings["BastionHost"];
        private readonly string BastionUser = ConfigurationManager.AppSettings["BastionUser"];

        /// <summary>Contraseña SSH (bastión, concentrador). Trimeada al leer.</summary>
        private readonly string Password = ConfigurationManager.AppSettings["GlobalPassword"]?.Trim() ?? "";

        /// <summary>Contraseña de <c>su -</c> (root). Si la clave existe vacía o no está, se usa <see cref="Password"/>.</summary>
        private readonly string SuPassword = ObtenerSuRootPassword();

        private static string ObtenerSuRootPassword()
        {
            var su = ConfigurationManager.AppSettings["SuRootPassword"]?.Trim();
            if (!string.IsNullOrEmpty(su))
                return su;
            return ConfigurationManager.AppSettings["GlobalPassword"]?.Trim() ?? "";
        }

        private readonly int Timeout = int.Parse(ConfigurationManager.AppSettings["ShellTimeoutMs"]);
        private readonly int CommandWait = int.Parse(ConfigurationManager.AppSettings["CommandWaitMs"]);

        public void ConnectMain()
        {
            var keyFile = new PrivateKeyFile(PemPath);

            var connectionInfo = new ConnectionInfo(
                MainHost,
                MainUser,
                new PrivateKeyAuthenticationMethod(MainUser, keyFile)
            );

            _client = new SshClient(connectionInfo);
            _client.Connect();

            _stream = _client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);

            Thread.Sleep(1500);
            Flush();
        }

        public void ConnectBastion()
        {
            _stream.WriteLine($"ssh {BastionUser}@{BastionHost}");

            var output = WaitFor("password:", "yes/no");

            if (output.Contains("yes/no"))
            {
                _stream.WriteLine("yes");
                output = WaitFor("password:");
            }

            if (output.Contains("password:"))
            {
                _stream.WriteLine(Password);
                WaitForPrompt();
            }
        }

        public void ConnectToConcentrador(string host)
        {
            _stream.WriteLine($"ssh concentrador@{host}");

            var output = WaitFor("password:", "yes/no");

            if (output.Contains("yes/no"))
            {
                _stream.WriteLine("yes");
                output = WaitFor("password:");
            }

            if (output.Contains("password:"))
            {
                _stream.WriteLine(Password);
                WaitForPrompt();
            }
        }

        public void BecomeRoot()
        {
            _stream.WriteLine("su -");

            // No usar WaitFor() genérico: devuelve al ver '$' del prompt anterior (user@host:~$)
            // y se pierde el prompt de su, o se envía la contraseña en mal momento → Fallo de autenticación.
            var output = WaitForSuPasswordPrompt();

            if (IndicaFalloSuSinPedirPassword(output))
                return;

            if (ContienePromptPasswordSu(output))
            {
                _stream.WriteLine(SuPassword);
                WaitForPrompt();
            }
        }

        /// <summary>Espera solo el prompt de contraseña de su (locale), sin cortar al ver el $ del prompt previo.</summary>
        private string WaitForSuPasswordPrompt()
        {
            var start = Environment.TickCount;
            var buffer = "";

            while (Environment.TickCount - start < Timeout)
            {
                if (_stream.DataAvailable)
                {
                    buffer += _stream.Read();
                    var clean = CleanAnsi(buffer);

                    if (ContienePromptPasswordSu(clean))
                        return clean;

                    if (IndicaFalloSuSinPedirPassword(clean))
                        return clean;
                }

                Thread.Sleep(200);
            }

            throw new Exception("Timeout esperando prompt de contraseña (su).\n" + CleanAnsi(buffer));
        }

        private static bool ContienePromptPasswordSu(string clean) =>
            clean.Contains("assword:", StringComparison.OrdinalIgnoreCase)
            || clean.Contains("ontraseña:", StringComparison.OrdinalIgnoreCase);

        private static bool IndicaFalloSuSinPedirPassword(string clean) =>
            clean.Contains("Fallo de autenticación", StringComparison.OrdinalIgnoreCase)
            || clean.Contains("Authentication failure", StringComparison.OrdinalIgnoreCase)
            || (clean.Contains("su:", StringComparison.OrdinalIgnoreCase)
                && clean.Contains("failure", StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Misma secuencia que usa ApiLog y Reinicio: host principal → bastión → concentrador → root.
        /// </summary>
        public void ConnectFullPipeline(string? concentradorHost)
        {
            if (string.IsNullOrWhiteSpace(concentradorHost))
                throw new ArgumentException("Debe seleccionar un concentrador.", nameof(concentradorHost));

            ConnectMain();
            ConnectBastion();
            ConnectToConcentrador(concentradorHost);
            BecomeRoot();
        }

        public string Execute(string command)
        {
            Flush();
            DrainPendingOutput(quietWaitMs: 280, maxTotalMs: 20_000);

            _stream.WriteLine(command);
            Thread.Sleep(CommandWait);

            return CleanAnsi(ReadAll());
        }

        /// <summary>
        /// Igual que <see cref="Execute"/> pero sigue leyendo el stream hasta que deje de llegar datos un tiempo (útil para consultas grandes).
        /// </summary>
        /// <param name="quietWaitMs">Silencio tras el último byte recibido para considerar fin de salida.</param>
        /// <param name="maxMsBeforeFirstByte">Si aún no llegó nada, seguir esperando hasta este límite (evita cortar a 500 ms cuando el servidor tarda en responder).</param>
        public string ExecuteWithDrainedOutput(string command, int? initialWaitMs = null, int quietWaitMs = 450, int maxMsBeforeFirstByte = 60_000)
        {
            Flush();
            // Evita mezclar MOTD/neofetch del login (aún llegando) con la salida de docker/mariadb.
            DrainPendingOutput(quietWaitMs: 280, maxTotalMs: 20_000);

            _stream.WriteLine(command);
            Thread.Sleep(initialWaitMs ?? CommandWait);

            return CleanAnsi(ReadUntilQuiet(quietWaitMs, maxMsBeforeFirstByte));
        }

        /// <summary>
        /// Descarta bytes pendientes hasta silencio. Si no hay nada en buffer, sale al instante (no penaliza cada comando).
        /// </summary>
        private void DrainPendingOutput(int quietWaitMs, int maxTotalMs)
        {
            var lastData = Environment.TickCount64;
            var start = Environment.TickCount64;
            var seenData = false;

            while (Environment.TickCount64 - start < maxTotalMs)
            {
                var hubo = false;
                while (_stream.DataAvailable)
                {
                    _stream.Read();
                    hubo = true;
                    seenData = true;
                    lastData = Environment.TickCount64;
                    Thread.Sleep(15);
                }

                if (!hubo)
                {
                    if (!seenData)
                        break;
                    if (Environment.TickCount64 - lastData >= quietWaitMs)
                        break;
                }

                Thread.Sleep(35);
            }
        }

        private string ReadUntilQuiet(int quietWaitMs, int maxMsBeforeFirstByte)
        {
            var sb = new System.Text.StringBuilder();
            var lastData = Environment.TickCount64;
            var start = Environment.TickCount64;
            var seenData = false;
            const int maxTotalMs = 180_000;

            while (Environment.TickCount64 - start < maxTotalMs)
            {
                var hubo = false;
                while (_stream.DataAvailable)
                {
                    sb.Append(_stream.Read());
                    hubo = true;
                    seenData = true;
                    lastData = Environment.TickCount64;
                    Thread.Sleep(25);
                }

                if (!hubo)
                {
                    if (!seenData)
                    {
                        if (Environment.TickCount64 - start >= maxMsBeforeFirstByte)
                            break;
                    }
                    else if (Environment.TickCount64 - lastData >= quietWaitMs)
                        break;
                }

                Thread.Sleep(35);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Ejecuta un comando y emite cada línea completa (p. ej. IDs de <c>docker restart</c>) hasta detectar el prompt del shell.
        /// </summary>
        public void ExecuteStreamingLines(string command, Action<string> onLine, CancellationToken cancellationToken = default)
        {
            Flush();
            _stream.WriteLine(command);
            Thread.Sleep(200);

            var buffer = "";
            var start = Environment.TickCount64;
            const long maxMs = 3_600_000L;

            while (Environment.TickCount64 - start < maxMs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                while (_stream.DataAvailable)
                {
                    buffer += CleanAnsi(_stream.Read());
                    Thread.Sleep(15);
                }

                if (ProcessStreamingBuffer(ref buffer, onLine))
                    return;

                Thread.Sleep(40);
            }
        }

        /// <returns><see langword="true"/> si se detectó el prompt del shell (fin del comando).</returns>
        private static bool ProcessStreamingBuffer(ref string buffer, Action<string> onLine)
        {
            while (true)
            {
                var ix = buffer.IndexOf('\n');
                if (ix < 0)
                    break;

                var line = buffer[..ix].TrimEnd('\r');
                buffer = buffer[(ix + 1)..];

                var trimmed = line.Trim();
                if (trimmed.Length == 0)
                    continue;

                if (IsShellPromptLine(trimmed))
                    return true;

                onLine(trimmed);
            }

            var tail = buffer.TrimEnd();
            if (tail.Length > 0 && IsShellPromptLine(tail))
            {
                buffer = "";
                return true;
            }

            return false;
        }

        public static bool IsDockerContainerIdLine(string line)
        {
            var t = line.Trim();
            return t.Length >= 12 && t.Length <= 64 && Regex.IsMatch(t, @"^[a-fA-F0-9]+$");
        }

        private static bool IsShellPromptLine(string line)
        {
            var t = line.TrimEnd();
            if (t.Length == 0)
                return false;

            if (IsDockerContainerIdLine(t))
                return false;

            if (t.Contains("└──>", StringComparison.Ordinal))
                return true;

            if (t.Contains('@', StringComparison.Ordinal) &&
                (t.EndsWith("#", StringComparison.Ordinal) || t.EndsWith("$", StringComparison.Ordinal)))
                return true;

            return Regex.IsMatch(t, @"^.+@.+[#$]\s*$");
        }

        /// <summary>Comillas simples para bash (docker restart 'nombre').</summary>
        public static string BashSingleQuote(string value)
        {
            return "'" + value.Replace("'", "'\\''") + "'";
        }

        // 🔥 NUEVO: espera prompt real
        private void WaitForPrompt()
        {
            WaitFor("$", "#", "└──>", ">");
        }

        private string WaitFor(params string[] texts)
        {
            var start = Environment.TickCount;
            var buffer = "";

            while (Environment.TickCount - start < Timeout)
            {
                if (_stream.DataAvailable)
                {
                    buffer += _stream.Read();

                    var clean = CleanAnsi(buffer);

                    // 🔥 detección automática de prompt real
                    if (clean.Contains("└──>") || clean.Contains("$") || clean.Contains("#"))
                        return clean;

                    foreach (var t in texts)
                    {
                        if (clean.Contains(t))
                            return clean;
                    }
                }

                Thread.Sleep(200);
            }

            throw new Exception("Timeout esperando respuesta SSH.\n" + buffer);
        }

        private void Flush()
        {
            while (_stream.DataAvailable)
            {
                _stream.Read();
                Thread.Sleep(50);
            }
        }

        private string ReadAll()
        {
            var result = "";

            while (_stream.DataAvailable)
            {
                result += _stream.Read();
                Thread.Sleep(100);
            }

            return result;
        }

        // 🔥 LIMPIAR ANSI (colores basura)
        private string CleanAnsi(string input)
        {
            return Regex.Replace(input, @"\x1B\[[\d?;]*[A-Za-z]", "");
        }

        public void Dispose()
        {
            _client?.Disconnect();
            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}