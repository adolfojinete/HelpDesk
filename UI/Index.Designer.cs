namespace UI
{
    partial class Index
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelTop = new Panel();
            label1 = new Label();
            ddlConcentrador = new ComboBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            richTextBox1 = new RichTextBox();
            panelLogsTop = new Panel();
            btnBuscarEnLog = new Button();
            txtSearchLog = new TextBox();
            txtBuscarEnLog = new Label();
            btnApiLog = new Button();
            btnDetener = new Button();
            tabPage2 = new TabPage();
            panelReinicioBody = new Panel();
            flowLayoutPanelDockers = new FlowLayoutPanel();
            panelReinicioTodosBar = new Panel();
            chkTodosDocker = new CheckBox();
            lblReinicioSinDockers = new Label();
            panelReinicioToolbar = new Panel();
            lblReinicioBusy = new Label();
            progressBarReinicio = new ProgressBar();
            btnReiniciar = new Button();
            btnCargarDockers = new Button();
            tabPage3 = new TabPage();
            panelTop.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panelLogsTop.SuspendLayout();
            tabPage2.SuspendLayout();
            panelReinicioBody.SuspendLayout();
            panelReinicioTodosBar.SuspendLayout();
            panelReinicioToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(label1);
            panelTop.Controls.Add(ddlConcentrador);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(10);
            panelTop.Size = new Size(1200, 70);
            panelTop.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 20);
            label1.Name = "label1";
            label1.Size = new Size(213, 25);
            label1.TabIndex = 0;
            label1.Text = "Nombre de concentrador";
            // 
            // ddlConcentrador
            // 
            ddlConcentrador.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlConcentrador.Location = new Point(240, 16);
            ddlConcentrador.Name = "ddlConcentrador";
            ddlConcentrador.Size = new Size(350, 33);
            ddlConcentrador.TabIndex = 1;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 70);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1200, 630);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(richTextBox1);
            tabPage1.Controls.Add(panelLogsTop);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1192, 592);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Logs";
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Font = new Font("Consolas", 10F);
            richTextBox1.Location = new Point(0, 50);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(1192, 542);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // panelLogsTop
            // 
            panelLogsTop.Controls.Add(btnBuscarEnLog);
            panelLogsTop.Controls.Add(txtSearchLog);
            panelLogsTop.Controls.Add(txtBuscarEnLog);
            panelLogsTop.Controls.Add(btnApiLog);
            panelLogsTop.Controls.Add(btnDetener);
            panelLogsTop.Dock = DockStyle.Top;
            panelLogsTop.Location = new Point(0, 0);
            panelLogsTop.Name = "panelLogsTop";
            panelLogsTop.Size = new Size(1192, 50);
            panelLogsTop.TabIndex = 1;
            // 
            // btnBuscarEnLog
            // 
            btnBuscarEnLog.Location = new Point(707, 6);
            btnBuscarEnLog.Name = "btnBuscarEnLog";
            btnBuscarEnLog.Size = new Size(112, 34);
            btnBuscarEnLog.TabIndex = 5;
            btnBuscarEnLog.Text = "Buscar";
            btnBuscarEnLog.UseVisualStyleBackColor = true;
            btnBuscarEnLog.Click += btnBuscarEnLog_Click;
            // 
            // txtSearchLog
            // 
            txtSearchLog.Location = new Point(421, 9);
            txtSearchLog.Name = "txtSearchLog";
            txtSearchLog.Size = new Size(280, 31);
            txtSearchLog.TabIndex = 4;
            // 
            // txtBuscarEnLog
            // 
            txtBuscarEnLog.AutoSize = true;
            txtBuscarEnLog.Location = new Point(356, 13);
            txtBuscarEnLog.Name = "txtBuscarEnLog";
            txtBuscarEnLog.Size = new Size(63, 25);
            txtBuscarEnLog.TabIndex = 3;
            txtBuscarEnLog.Text = "Buscar";
            // 
            // btnApiLog
            // 
            btnApiLog.Location = new Point(10, 10);
            btnApiLog.Name = "btnApiLog";
            btnApiLog.Size = new Size(100, 30);
            btnApiLog.TabIndex = 1;
            btnApiLog.Text = "ApiLog";
            btnApiLog.Click += btnApiLog_Click;
            // 
            // btnDetener
            // 
            btnDetener.Location = new Point(120, 10);
            btnDetener.Name = "btnDetener";
            btnDetener.Size = new Size(100, 30);
            btnDetener.TabIndex = 2;
            btnDetener.Text = "Detener";
            btnDetener.Click += btnDetener_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(panelReinicioBody);
            tabPage2.Controls.Add(panelReinicioToolbar);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(8);
            tabPage2.Size = new Size(1192, 592);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Reinicio";
            // 
            // panelReinicioBody
            // 
            panelReinicioBody.Controls.Add(flowLayoutPanelDockers);
            panelReinicioBody.Controls.Add(panelReinicioTodosBar);
            panelReinicioBody.Controls.Add(lblReinicioSinDockers);
            panelReinicioBody.Dock = DockStyle.Fill;
            panelReinicioBody.Location = new Point(8, 64);
            panelReinicioBody.Name = "panelReinicioBody";
            panelReinicioBody.Size = new Size(1176, 520);
            panelReinicioBody.TabIndex = 1;
            // 
            // flowLayoutPanelDockers
            // 
            flowLayoutPanelDockers.AutoScroll = true;
            flowLayoutPanelDockers.Dock = DockStyle.Fill;
            flowLayoutPanelDockers.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelDockers.Location = new Point(0, 77);
            flowLayoutPanelDockers.Name = "flowLayoutPanelDockers";
            flowLayoutPanelDockers.Padding = new Padding(3, 0, 3, 8);
            flowLayoutPanelDockers.Size = new Size(1176, 443);
            flowLayoutPanelDockers.TabIndex = 2;
            flowLayoutPanelDockers.Visible = false;
            flowLayoutPanelDockers.WrapContents = false;
            // 
            // panelReinicioTodosBar
            // 
            panelReinicioTodosBar.Controls.Add(chkTodosDocker);
            panelReinicioTodosBar.Dock = DockStyle.Top;
            panelReinicioTodosBar.Location = new Point(0, 33);
            panelReinicioTodosBar.Name = "panelReinicioTodosBar";
            panelReinicioTodosBar.Padding = new Padding(0, 4, 0, 8);
            panelReinicioTodosBar.Size = new Size(1176, 44);
            panelReinicioTodosBar.TabIndex = 1;
            panelReinicioTodosBar.Visible = false;
            // 
            // chkTodosDocker
            // 
            chkTodosDocker.AutoSize = true;
            chkTodosDocker.Checked = true;
            chkTodosDocker.CheckState = CheckState.Checked;
            chkTodosDocker.Location = new Point(3, 7);
            chkTodosDocker.Name = "chkTodosDocker";
            chkTodosDocker.Size = new Size(86, 29);
            chkTodosDocker.TabIndex = 0;
            chkTodosDocker.Text = "Todos";
            chkTodosDocker.UseVisualStyleBackColor = true;
            chkTodosDocker.CheckedChanged += chkTodosDocker_CheckedChanged;
            // 
            // lblReinicioSinDockers
            // 
            lblReinicioSinDockers.AutoSize = true;
            lblReinicioSinDockers.Dock = DockStyle.Top;
            lblReinicioSinDockers.ForeColor = SystemColors.GrayText;
            lblReinicioSinDockers.Location = new Point(0, 0);
            lblReinicioSinDockers.Name = "lblReinicioSinDockers";
            lblReinicioSinDockers.Padding = new Padding(0, 8, 0, 0);
            lblReinicioSinDockers.Size = new Size(615, 33);
            lblReinicioSinDockers.TabIndex = 0;
            lblReinicioSinDockers.Text = "No se encontraron contenedores Docker en este concentrador seleccionado.";
            lblReinicioSinDockers.Visible = false;
            // 
            // panelReinicioToolbar
            // 
            panelReinicioToolbar.Controls.Add(lblReinicioBusy);
            panelReinicioToolbar.Controls.Add(progressBarReinicio);
            panelReinicioToolbar.Controls.Add(btnReiniciar);
            panelReinicioToolbar.Controls.Add(btnCargarDockers);
            panelReinicioToolbar.Dock = DockStyle.Top;
            panelReinicioToolbar.Location = new Point(8, 8);
            panelReinicioToolbar.Name = "panelReinicioToolbar";
            panelReinicioToolbar.Padding = new Padding(0, 0, 0, 8);
            panelReinicioToolbar.Size = new Size(1176, 56);
            panelReinicioToolbar.TabIndex = 0;
            // 
            // lblReinicioBusy
            // 
            lblReinicioBusy.AutoEllipsis = true;
            lblReinicioBusy.AutoSize = true;
            lblReinicioBusy.ForeColor = SystemColors.GrayText;
            lblReinicioBusy.Location = new Point(624, 13);
            lblReinicioBusy.MaximumSize = new Size(520, 0);
            lblReinicioBusy.Name = "lblReinicioBusy";
            lblReinicioBusy.Size = new Size(380, 25);
            lblReinicioBusy.TabIndex = 3;
            lblReinicioBusy.Text = "Reiniciando contenedores en el concentrador…";
            lblReinicioBusy.Visible = false;
            // 
            // progressBarReinicio
            // 
            progressBarReinicio.Location = new Point(418, 10);
            progressBarReinicio.MarqueeAnimationSpeed = 35;
            progressBarReinicio.Name = "progressBarReinicio";
            progressBarReinicio.Size = new Size(200, 26);
            progressBarReinicio.Style = ProgressBarStyle.Marquee;
            progressBarReinicio.TabIndex = 2;
            progressBarReinicio.Visible = false;
            // 
            // btnReiniciar
            // 
            btnReiniciar.Location = new Point(228, 6);
            btnReiniciar.Name = "btnReiniciar";
            btnReiniciar.Size = new Size(180, 36);
            btnReiniciar.TabIndex = 1;
            btnReiniciar.Text = "Reiniciar";
            btnReiniciar.UseVisualStyleBackColor = true;
            btnReiniciar.Visible = false;
            btnReiniciar.Click += btnReiniciar_Click;
            // 
            // btnCargarDockers
            // 
            btnCargarDockers.Location = new Point(0, 6);
            btnCargarDockers.Name = "btnCargarDockers";
            btnCargarDockers.Size = new Size(220, 36);
            btnCargarDockers.TabIndex = 0;
            btnCargarDockers.Text = "Cargar contenedores";
            btnCargarDockers.UseVisualStyleBackColor = true;
            btnCargarDockers.Click += btnCargarDockers_Click;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1192, 592);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Otros";
            // 
            // Index
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(tabControl1);
            Controls.Add(panelTop);
            Name = "Index";
            Text = "HelpDesk Busmatick";
            WindowState = FormWindowState.Maximized;
            Load += Index_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panelLogsTop.ResumeLayout(false);
            panelLogsTop.PerformLayout();
            tabPage2.ResumeLayout(false);
            panelReinicioBody.ResumeLayout(false);
            panelReinicioBody.PerformLayout();
            panelReinicioTodosBar.ResumeLayout(false);
            panelReinicioTodosBar.PerformLayout();
            panelReinicioToolbar.ResumeLayout(false);
            panelReinicioToolbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel panelReinicioToolbar;
        private Button btnCargarDockers;
        private Button btnReiniciar;
        private ProgressBar progressBarReinicio;
        private Label lblReinicioBusy;
        private Panel panelReinicioBody;
        private Label lblReinicioSinDockers;
        private Panel panelReinicioTodosBar;
        private CheckBox chkTodosDocker;
        private FlowLayoutPanel flowLayoutPanelDockers;
        private TabPage tabPage3;

        private Label label1;
        private ComboBox ddlConcentrador;

        private Button btnApiLog;
        private Button btnDetener;
        private RichTextBox richTextBox1;
        private Panel panelLogsTop;
        private Button btnBuscarEnLog;
        private TextBox txtSearchLog;
        private Label txtBuscarEnLog;
    }
}