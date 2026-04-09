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

        private readonly string Password = ConfigurationManager.AppSettings["GlobalPassword"];

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

            var output = WaitFor("Password:", "Contraseña:");

            if (output.Contains("Password") || output.Contains("Contraseña"))
            {
                _stream.WriteLine(Password);
                WaitForPrompt();
            }
        }

        public string Execute(string command)
        {
            Flush();

            _stream.WriteLine(command);
            Thread.Sleep(CommandWait);

            return CleanAnsi(ReadAll());
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
            return Regex.Replace(input, @"\x1B\[[0-9;]*[A-Za-z]", "");
        }

        public void Dispose()
        {
            _client?.Disconnect();
            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}