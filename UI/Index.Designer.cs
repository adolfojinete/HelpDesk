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
            btnApiLog = new Button();
            btnDetener = new Button();
            panelLogsTop = new Panel();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            txtBuscarEnLog = new Label();
            txtSearchLog = new TextBox();
            btnBuscarEnLog = new Button();
            panelTop.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panelLogsTop.SuspendLayout();
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
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(1192, 592);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Reinicio";
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1192, 592);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Otros";
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
            // txtSearchLog
            // 
            txtSearchLog.Location = new Point(421, 9);
            txtSearchLog.Name = "txtSearchLog";
            txtSearchLog.Size = new Size(280, 31);
            txtSearchLog.TabIndex = 4;
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
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
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