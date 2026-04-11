using System.Drawing;

namespace UI;

/// <summary>
/// Lista flotante tipo Ctrl+J: <see cref="ToolStripDropDown"/> + <see cref="ListBox"/>.
/// </summary>
internal sealed class SqlSugerenciasPopup : IDisposable
{
    private readonly ToolStripDropDown _drop = new() { AutoSize = false };
    private readonly ListBox _list;
    private Form? _owner;
    private Action<string>? _onAceptar;
    private bool _disposed;

    public SqlSugerenciasPopup()
    {
        _list = new ListBox
        {
            BorderStyle = BorderStyle.None,
            Font = new Font("Consolas", 10f),
            IntegralHeight = false,
            TabStop = true,
        };
        _list.DoubleClick += (_, _) => AceptarSeleccion();
        _list.KeyDown += List_KeyDown;
        _drop.Items.Add(new ToolStripControlHost(_list) { AutoSize = false, Size = new Size(400, 260) });
        _drop.Closed += (_, _) => DesvincularOwner();
    }

    private void List_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            AceptarSeleccion();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            Ocultar();
        }
    }

    private void AceptarSeleccion()
    {
        if (_list.SelectedItem is not string s)
            return;
        _onAceptar?.Invoke(s);
        Ocultar();
    }

    /// <summary>Muestra la lista en pantalla. <paramref name="onAceptar"/> recibe el texto elegido.</summary>
    public void Mostrar(Form owner, Point pantalla, IReadOnlyList<string> items, Action<string> onAceptar)
    {
        Ocultar();
        _onAceptar = onAceptar;
        _owner = owner;
        owner.Move += Owner_Move;
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var x in items)
            _list.Items.Add(x);
        _list.EndUpdate();
        if (_list.Items.Count > 0)
            _list.SelectedIndex = 0;
        _drop.Show(pantalla.X, pantalla.Y);
        _ = BeginInvokeFocusList();
    }

    private async Task BeginInvokeFocusList()
    {
        await Task.Delay(50);
        if (_drop.Visible)
            _list.Focus();
    }

    private void Owner_Move(object? sender, EventArgs e) => Ocultar();

    private void DesvincularOwner()
    {
        if (_owner is not null)
        {
            _owner.Move -= Owner_Move;
            _owner = null;
        }
    }

    public void Ocultar()
    {
        if (_drop.Visible)
            _drop.Close();
        DesvincularOwner();
        _onAceptar = null;
    }

    public bool Visible => _drop.Visible;

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        Ocultar();
        _drop.Dispose();
        _list.Dispose();
    }
}
