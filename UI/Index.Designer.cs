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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
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
            panelReinicioPie = new Panel();
            btnReiniciar = new Button();
            lblReinicioBusy = new Label();
            progressBarReinicio = new ProgressBar();
            lblReinicioSinDockers = new Label();
            panelReinicioTodosBar = new Panel();
            chkTodosDocker = new CheckBox();
            flowLayoutPanelDockers = new FlowLayoutPanel();
            panelReinicioToolbar = new Panel();
            lblCargarDockersBusy = new Label();
            progressBarCargarDockers = new ProgressBar();
            btnCargarDockers = new Button();
            tabPage3 = new TabPage();
            splitContainerSql = new SplitContainer();
            dgvSqlListaTablas = new DataGridView();
            panelSqlDerecho = new Panel();
            tabControlSqlScripts = new TabControl();
            tabPageSqlScript1 = new TabPage();
            splitSqlEditorResultados = new SplitContainer();
            txtSqlEditor = new TextBox();
            dgvSqlResultados = new DataGridView();
            panelSqlBarEjecutar = new Panel();
            tableLayoutPanelSqlEjecutar = new TableLayoutPanel();
            btnSqlEjecutar = new Button();
            btnSqlNuevoScript = new Button();
            progressBarSqlConsulta = new ProgressBar();
            lblSqlConsultaBusy = new Label();
            btnSqlExportarExcel = new Button();
            panelSqlToolbar = new Panel();
            lblSqlBusy = new Label();
            progressBarSql = new ProgressBar();
            btnSqlConectar = new Button();
            contextMenuSqlTablas = new ContextMenuStrip(components);
            toolStripMenuItemSqlGenerar = new ToolStripMenuItem();
            contextMenuSqlScriptPestaña = new ContextMenuStrip(components);
            toolStripMenuItemCerrarPestañaSql = new ToolStripMenuItem();
            panelTop.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panelLogsTop.SuspendLayout();
            tabPage2.SuspendLayout();
            panelReinicioBody.SuspendLayout();
            panelReinicioPie.SuspendLayout();
            panelReinicioTodosBar.SuspendLayout();
            panelReinicioToolbar.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerSql).BeginInit();
            splitContainerSql.Panel1.SuspendLayout();
            splitContainerSql.Panel2.SuspendLayout();
            splitContainerSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSqlListaTablas).BeginInit();
            panelSqlDerecho.SuspendLayout();
            tabPageSqlScript1.SuspendLayout();
            tabControlSqlScripts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitSqlEditorResultados).BeginInit();
            splitSqlEditorResultados.Panel1.SuspendLayout();
            splitSqlEditorResultados.Panel2.SuspendLayout();
            splitSqlEditorResultados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSqlResultados).BeginInit();
            panelSqlBarEjecutar.SuspendLayout();
            tableLayoutPanelSqlEjecutar.SuspendLayout();
            panelSqlToolbar.SuspendLayout();
            contextMenuSqlTablas.SuspendLayout();
            contextMenuSqlScriptPestaña.SuspendLayout();
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
            ddlConcentrador.SelectedIndexChanged += ddlConcentrador_SelectedIndexChanged;
            ddlConcentrador.SelectionChangeCommitted += ddlConcentrador_SelectionChangeCommitted;
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
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
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
            panelReinicioBody.Controls.Add(panelReinicioPie);
            panelReinicioBody.Controls.Add(lblReinicioSinDockers);
            panelReinicioBody.Controls.Add(panelReinicioTodosBar);
            panelReinicioBody.Controls.Add(flowLayoutPanelDockers);
            panelReinicioBody.Dock = DockStyle.Fill;
            panelReinicioBody.Location = new Point(8, 64);
            panelReinicioBody.Name = "panelReinicioBody";
            panelReinicioBody.Size = new Size(1176, 520);
            panelReinicioBody.TabIndex = 1;
            // 
            // panelReinicioPie
            // 
            panelReinicioPie.Controls.Add(btnReiniciar);
            panelReinicioPie.Controls.Add(lblReinicioBusy);
            panelReinicioPie.Controls.Add(progressBarReinicio);
            panelReinicioPie.Dock = DockStyle.Bottom;
            panelReinicioPie.Location = new Point(0, 432);
            panelReinicioPie.Name = "panelReinicioPie";
            panelReinicioPie.Padding = new Padding(0, 4, 0, 8);
            panelReinicioPie.Size = new Size(1176, 88);
            panelReinicioPie.TabIndex = 4;
            panelReinicioPie.Visible = false;
            // 
            // btnReiniciar
            // 
            btnReiniciar.Location = new Point(0, 38);
            btnReiniciar.Name = "btnReiniciar";
            btnReiniciar.Size = new Size(180, 36);
            btnReiniciar.TabIndex = 2;
            btnReiniciar.Text = "Reiniciar";
            btnReiniciar.UseVisualStyleBackColor = true;
            btnReiniciar.Visible = false;
            btnReiniciar.Click += btnReiniciar_Click;
            // 
            // lblReinicioBusy
            // 
            lblReinicioBusy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblReinicioBusy.AutoEllipsis = true;
            lblReinicioBusy.ForeColor = SystemColors.GrayText;
            lblReinicioBusy.Location = new Point(208, 6);
            lblReinicioBusy.Name = "lblReinicioBusy";
            lblReinicioBusy.Size = new Size(960, 28);
            lblReinicioBusy.TabIndex = 1;
            lblReinicioBusy.Text = "Reiniciando contenedores en el concentrador…";
            lblReinicioBusy.TextAlign = ContentAlignment.MiddleLeft;
            lblReinicioBusy.Visible = false;
            // 
            // progressBarReinicio
            // 
            progressBarReinicio.Location = new Point(0, 4);
            progressBarReinicio.MarqueeAnimationSpeed = 35;
            progressBarReinicio.Name = "progressBarReinicio";
            progressBarReinicio.Size = new Size(200, 26);
            progressBarReinicio.Style = ProgressBarStyle.Marquee;
            progressBarReinicio.TabIndex = 0;
            progressBarReinicio.Visible = false;
            // 
            // lblReinicioSinDockers
            // 
            lblReinicioSinDockers.AutoSize = true;
            lblReinicioSinDockers.Dock = DockStyle.Top;
            lblReinicioSinDockers.ForeColor = SystemColors.GrayText;
            lblReinicioSinDockers.Location = new Point(0, 44);
            lblReinicioSinDockers.Name = "lblReinicioSinDockers";
            lblReinicioSinDockers.Padding = new Padding(0, 8, 0, 0);
            lblReinicioSinDockers.Size = new Size(615, 33);
            lblReinicioSinDockers.TabIndex = 0;
            lblReinicioSinDockers.Text = "No se encontraron contenedores Docker en este concentrador seleccionado.";
            lblReinicioSinDockers.Visible = false;
            // 
            // panelReinicioTodosBar
            // 
            panelReinicioTodosBar.Controls.Add(chkTodosDocker);
            panelReinicioTodosBar.Dock = DockStyle.Top;
            panelReinicioTodosBar.Location = new Point(0, 0);
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
            // flowLayoutPanelDockers
            // 
            flowLayoutPanelDockers.AutoScroll = true;
            flowLayoutPanelDockers.Dock = DockStyle.Fill;
            flowLayoutPanelDockers.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelDockers.Location = new Point(0, 0);
            flowLayoutPanelDockers.Name = "flowLayoutPanelDockers";
            flowLayoutPanelDockers.Padding = new Padding(3, 0, 3, 8);
            flowLayoutPanelDockers.Size = new Size(1176, 520);
            flowLayoutPanelDockers.TabIndex = 2;
            flowLayoutPanelDockers.Visible = false;
            flowLayoutPanelDockers.WrapContents = false;
            // 
            // panelReinicioToolbar
            // 
            panelReinicioToolbar.Controls.Add(lblCargarDockersBusy);
            panelReinicioToolbar.Controls.Add(progressBarCargarDockers);
            panelReinicioToolbar.Controls.Add(btnCargarDockers);
            panelReinicioToolbar.Dock = DockStyle.Top;
            panelReinicioToolbar.Location = new Point(8, 8);
            panelReinicioToolbar.Name = "panelReinicioToolbar";
            panelReinicioToolbar.Padding = new Padding(0, 0, 0, 8);
            panelReinicioToolbar.Size = new Size(1176, 56);
            panelReinicioToolbar.TabIndex = 0;
            // 
            // lblCargarDockersBusy
            // 
            lblCargarDockersBusy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCargarDockersBusy.AutoEllipsis = true;
            lblCargarDockersBusy.ForeColor = SystemColors.GrayText;
            lblCargarDockersBusy.Location = new Point(416, 11);
            lblCargarDockersBusy.Name = "lblCargarDockersBusy";
            lblCargarDockersBusy.Size = new Size(752, 28);
            lblCargarDockersBusy.TabIndex = 5;
            lblCargarDockersBusy.Text = "Obteniendo contenedores en el concentrador…";
            lblCargarDockersBusy.TextAlign = ContentAlignment.MiddleLeft;
            lblCargarDockersBusy.Visible = false;
            // 
            // progressBarCargarDockers
            // 
            progressBarCargarDockers.Location = new Point(228, 10);
            progressBarCargarDockers.MarqueeAnimationSpeed = 35;
            progressBarCargarDockers.Name = "progressBarCargarDockers";
            progressBarCargarDockers.Size = new Size(180, 26);
            progressBarCargarDockers.Style = ProgressBarStyle.Marquee;
            progressBarCargarDockers.TabIndex = 4;
            progressBarCargarDockers.Visible = false;
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
            tabPage3.Controls.Add(splitContainerSql);
            tabPage3.Controls.Add(panelSqlToolbar);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(8);
            tabPage3.Size = new Size(1192, 592);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "SQL";
            // 
            // splitContainerSql
            // 
            splitContainerSql.Dock = DockStyle.Fill;
            splitContainerSql.Location = new Point(8, 64);
            splitContainerSql.Name = "splitContainerSql";
            // 
            // splitContainerSql.Panel1
            // 
            splitContainerSql.Panel1.Controls.Add(dgvSqlListaTablas);
            splitContainerSql.Panel1MinSize = 80;
            // 
            // splitContainerSql.Panel2
            // 
            splitContainerSql.Panel2.Controls.Add(panelSqlDerecho);
            splitContainerSql.Panel2MinSize = 80;
            splitContainerSql.Size = new Size(1176, 520);
            splitContainerSql.SplitterDistance = 80;
            splitContainerSql.SplitterWidth = 6;
            splitContainerSql.TabIndex = 1;
            // 
            // dgvSqlListaTablas
            // 
            dgvSqlListaTablas.AllowUserToAddRows = false;
            dgvSqlListaTablas.AllowUserToDeleteRows = false;
            dgvSqlListaTablas.AllowUserToResizeRows = false;
            dgvSqlListaTablas.BackgroundColor = SystemColors.Window;
            dgvSqlListaTablas.BorderStyle = BorderStyle.Fixed3D;
            dgvSqlListaTablas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSqlListaTablas.Dock = DockStyle.Fill;
            dgvSqlListaTablas.Location = new Point(0, 0);
            dgvSqlListaTablas.MultiSelect = false;
            dgvSqlListaTablas.Name = "dgvSqlListaTablas";
            dgvSqlListaTablas.ReadOnly = true;
            dgvSqlListaTablas.RowHeadersVisible = false;
            dgvSqlListaTablas.RowHeadersWidth = 62;
            dgvSqlListaTablas.RowTemplate.Height = 28;
            dgvSqlListaTablas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSqlListaTablas.Size = new Size(80, 520);
            dgvSqlListaTablas.TabIndex = 0;
            dgvSqlListaTablas.MouseDown += dgvSqlListaTablas_MouseDown;
            // 
            // panelSqlDerecho
            // 
            panelSqlDerecho.Controls.Add(tabControlSqlScripts);
            panelSqlDerecho.Controls.Add(panelSqlBarEjecutar);
            panelSqlDerecho.Dock = DockStyle.Fill;
            panelSqlDerecho.Location = new Point(0, 0);
            panelSqlDerecho.Name = "panelSqlDerecho";
            panelSqlDerecho.Size = new Size(1090, 520);
            panelSqlDerecho.TabIndex = 0;
            // 
            // tabControlSqlScripts
            // 
            tabControlSqlScripts.Controls.Add(tabPageSqlScript1);
            tabControlSqlScripts.Dock = DockStyle.Fill;
            tabControlSqlScripts.Location = new Point(0, 56);
            tabControlSqlScripts.Name = "tabControlSqlScripts";
            tabControlSqlScripts.Padding = new Point(4, 3);
            tabControlSqlScripts.SelectedIndex = 0;
            tabControlSqlScripts.Size = new Size(1090, 464);
            tabControlSqlScripts.TabIndex = 2;
            tabControlSqlScripts.SelectedIndexChanged += tabControlSqlScripts_SelectedIndexChanged;
            tabControlSqlScripts.MouseDown += tabControlSqlScripts_MouseDown;
            // 
            // tabPageSqlScript1
            // 
            tabPageSqlScript1.Controls.Add(splitSqlEditorResultados);
            tabPageSqlScript1.Location = new Point(4, 37);
            tabPageSqlScript1.Name = "tabPageSqlScript1";
            tabPageSqlScript1.Padding = new Padding(3);
            tabPageSqlScript1.Size = new Size(1074, 417);
            tabPageSqlScript1.TabIndex = 0;
            tabPageSqlScript1.Text = "Script 1";
            tabPageSqlScript1.UseVisualStyleBackColor = true;
            // 
            // splitSqlEditorResultados
            // 
            splitSqlEditorResultados.Dock = DockStyle.Fill;
            splitSqlEditorResultados.Location = new Point(3, 3);
            splitSqlEditorResultados.Name = "splitSqlEditorResultados";
            splitSqlEditorResultados.Orientation = Orientation.Horizontal;
            // 
            // splitSqlEditorResultados.Panel1
            // 
            splitSqlEditorResultados.Panel1.Controls.Add(txtSqlEditor);
            splitSqlEditorResultados.Panel1MinSize = 80;
            // 
            // splitSqlEditorResultados.Panel2
            // 
            splitSqlEditorResultados.Panel2.Controls.Add(dgvSqlResultados);
            splitSqlEditorResultados.Panel2MinSize = 80;
            splitSqlEditorResultados.Size = new Size(1090, 464);
            splitSqlEditorResultados.SplitterDistance = 80;
            splitSqlEditorResultados.SplitterWidth = 6;
            splitSqlEditorResultados.TabIndex = 1;
            // 
            // txtSqlEditor
            // 
            txtSqlEditor.AcceptsReturn = true;
            txtSqlEditor.AcceptsTab = true;
            txtSqlEditor.Dock = DockStyle.Fill;
            txtSqlEditor.Font = new Font("Consolas", 10F);
            txtSqlEditor.Location = new Point(0, 0);
            txtSqlEditor.Multiline = true;
            txtSqlEditor.Name = "txtSqlEditor";
            txtSqlEditor.ScrollBars = ScrollBars.Vertical;
            txtSqlEditor.Size = new Size(1090, 80);
            txtSqlEditor.TabIndex = 0;
            txtSqlEditor.WordWrap = false;
            // 
            // dgvSqlResultados
            // 
            dgvSqlResultados.AllowUserToAddRows = false;
            dgvSqlResultados.AllowUserToDeleteRows = false;
            dgvSqlResultados.BackgroundColor = SystemColors.Window;
            dgvSqlResultados.BorderStyle = BorderStyle.Fixed3D;
            dgvSqlResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSqlResultados.Dock = DockStyle.Fill;
            dgvSqlResultados.Location = new Point(0, 0);
            dgvSqlResultados.MultiSelect = false;
            dgvSqlResultados.Name = "dgvSqlResultados";
            dgvSqlResultados.ReadOnly = true;
            dgvSqlResultados.RowHeadersWidth = 40;
            dgvSqlResultados.RowTemplate.Height = 26;
            dgvSqlResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSqlResultados.Size = new Size(1090, 378);
            dgvSqlResultados.TabIndex = 0;
            // 
            // panelSqlBarEjecutar
            // 
            panelSqlBarEjecutar.Controls.Add(tableLayoutPanelSqlEjecutar);
            panelSqlBarEjecutar.Dock = DockStyle.Top;
            panelSqlBarEjecutar.Location = new Point(0, 0);
            panelSqlBarEjecutar.Name = "panelSqlBarEjecutar";
            panelSqlBarEjecutar.Padding = new Padding(0, 0, 0, 8);
            panelSqlBarEjecutar.Size = new Size(1090, 56);
            panelSqlBarEjecutar.TabIndex = 0;
            // 
            // tableLayoutPanelSqlEjecutar
            // 
            tableLayoutPanelSqlEjecutar.ColumnCount = 5;
            tableLayoutPanelSqlEjecutar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanelSqlEjecutar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanelSqlEjecutar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanelSqlEjecutar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelSqlEjecutar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanelSqlEjecutar.Controls.Add(btnSqlEjecutar, 0, 0);
            tableLayoutPanelSqlEjecutar.Controls.Add(btnSqlNuevoScript, 1, 0);
            tableLayoutPanelSqlEjecutar.Controls.Add(progressBarSqlConsulta, 2, 0);
            tableLayoutPanelSqlEjecutar.Controls.Add(lblSqlConsultaBusy, 3, 0);
            tableLayoutPanelSqlEjecutar.Controls.Add(btnSqlExportarExcel, 4, 0);
            tableLayoutPanelSqlEjecutar.Dock = DockStyle.Fill;
            tableLayoutPanelSqlEjecutar.Location = new Point(0, 0);
            tableLayoutPanelSqlEjecutar.Margin = new Padding(0);
            tableLayoutPanelSqlEjecutar.Name = "tableLayoutPanelSqlEjecutar";
            tableLayoutPanelSqlEjecutar.Padding = new Padding(0, 6, 0, 0);
            tableLayoutPanelSqlEjecutar.RowCount = 1;
            tableLayoutPanelSqlEjecutar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelSqlEjecutar.Size = new Size(1090, 48);
            tableLayoutPanelSqlEjecutar.TabIndex = 0;
            // 
            // btnSqlEjecutar
            // 
            btnSqlEjecutar.Anchor = AnchorStyles.Left;
            btnSqlEjecutar.Location = new Point(0, 9);
            btnSqlEjecutar.Margin = new Padding(0, 0, 8, 0);
            btnSqlEjecutar.Name = "btnSqlEjecutar";
            btnSqlEjecutar.Size = new Size(140, 36);
            btnSqlEjecutar.TabIndex = 0;
            btnSqlEjecutar.Text = "Ejecutar";
            btnSqlEjecutar.UseVisualStyleBackColor = true;
            btnSqlEjecutar.Click += btnSqlEjecutar_Click;
            // 
            // btnSqlNuevoScript
            // 
            btnSqlNuevoScript.Anchor = AnchorStyles.Left;
            btnSqlNuevoScript.Location = new Point(148, 9);
            btnSqlNuevoScript.Margin = new Padding(0, 0, 8, 0);
            btnSqlNuevoScript.Name = "btnSqlNuevoScript";
            btnSqlNuevoScript.Size = new Size(150, 36);
            btnSqlNuevoScript.TabIndex = 4;
            btnSqlNuevoScript.Text = "Nuevo script";
            btnSqlNuevoScript.UseVisualStyleBackColor = true;
            btnSqlNuevoScript.Click += btnSqlNuevoScript_Click;
            // 
            // progressBarSqlConsulta
            // 
            progressBarSqlConsulta.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            progressBarSqlConsulta.Location = new Point(306, 14);
            progressBarSqlConsulta.Margin = new Padding(0, 0, 8, 0);
            progressBarSqlConsulta.MarqueeAnimationSpeed = 35;
            progressBarSqlConsulta.Name = "progressBarSqlConsulta";
            progressBarSqlConsulta.Size = new Size(192, 26);
            progressBarSqlConsulta.Style = ProgressBarStyle.Marquee;
            progressBarSqlConsulta.TabIndex = 1;
            progressBarSqlConsulta.Visible = false;
            // 
            // lblSqlConsultaBusy
            // 
            lblSqlConsultaBusy.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblSqlConsultaBusy.AutoEllipsis = true;
            lblSqlConsultaBusy.ForeColor = SystemColors.GrayText;
            lblSqlConsultaBusy.Location = new Point(506, 14);
            lblSqlConsultaBusy.Margin = new Padding(0, 0, 8, 0);
            lblSqlConsultaBusy.Name = "lblSqlConsultaBusy";
            lblSqlConsultaBusy.Size = new Size(564, 25);
            lblSqlConsultaBusy.TabIndex = 2;
            lblSqlConsultaBusy.Text = "Ejecutando consulta en el concentrador…";
            lblSqlConsultaBusy.TextAlign = ContentAlignment.MiddleLeft;
            lblSqlConsultaBusy.Visible = false;
            // 
            // btnSqlExportarExcel
            // 
            btnSqlExportarExcel.Anchor = AnchorStyles.Right;
            btnSqlExportarExcel.Location = new Point(920, 9);
            btnSqlExportarExcel.Margin = new Padding(0);
            btnSqlExportarExcel.Name = "btnSqlExportarExcel";
            btnSqlExportarExcel.Size = new Size(170, 36);
            btnSqlExportarExcel.TabIndex = 3;
            btnSqlExportarExcel.Text = "Exportar a Excel";
            btnSqlExportarExcel.UseVisualStyleBackColor = true;
            btnSqlExportarExcel.Click += btnSqlExportarExcel_Click;
            // 
            // panelSqlToolbar
            // 
            panelSqlToolbar.Controls.Add(lblSqlBusy);
            panelSqlToolbar.Controls.Add(progressBarSql);
            panelSqlToolbar.Controls.Add(btnSqlConectar);
            panelSqlToolbar.Dock = DockStyle.Top;
            panelSqlToolbar.Location = new Point(8, 8);
            panelSqlToolbar.Name = "panelSqlToolbar";
            panelSqlToolbar.Padding = new Padding(0, 0, 0, 8);
            panelSqlToolbar.Size = new Size(1176, 56);
            panelSqlToolbar.TabIndex = 0;
            // 
            // lblSqlBusy
            // 
            lblSqlBusy.AutoEllipsis = true;
            lblSqlBusy.AutoSize = true;
            lblSqlBusy.ForeColor = SystemColors.GrayText;
            lblSqlBusy.Location = new Point(358, 13);
            lblSqlBusy.MaximumSize = new Size(520, 0);
            lblSqlBusy.Name = "lblSqlBusy";
            lblSqlBusy.Size = new Size(324, 25);
            lblSqlBusy.TabIndex = 2;
            lblSqlBusy.Text = "Obteniendo tablas en el concentrador…";
            lblSqlBusy.Visible = false;
            // 
            // progressBarSql
            // 
            progressBarSql.Location = new Point(172, 10);
            progressBarSql.MarqueeAnimationSpeed = 35;
            progressBarSql.Name = "progressBarSql";
            progressBarSql.Size = new Size(180, 26);
            progressBarSql.Style = ProgressBarStyle.Marquee;
            progressBarSql.TabIndex = 1;
            progressBarSql.Visible = false;
            // 
            // btnSqlConectar
            // 
            btnSqlConectar.Location = new Point(0, 6);
            btnSqlConectar.Name = "btnSqlConectar";
            btnSqlConectar.Size = new Size(160, 36);
            btnSqlConectar.TabIndex = 0;
            btnSqlConectar.Text = "Conectar";
            btnSqlConectar.UseVisualStyleBackColor = true;
            btnSqlConectar.Click += btnSqlConectar_Click;
            // 
            // contextMenuSqlTablas
            // 
            contextMenuSqlTablas.ImageScalingSize = new Size(24, 24);
            contextMenuSqlTablas.Items.AddRange(new ToolStripItem[] { toolStripMenuItemSqlGenerar });
            contextMenuSqlTablas.Name = "contextMenuSqlTablas";
            contextMenuSqlTablas.Size = new Size(196, 36);
            // 
            // toolStripMenuItemSqlGenerar
            // 
            toolStripMenuItemSqlGenerar.Name = "toolStripMenuItemSqlGenerar";
            toolStripMenuItemSqlGenerar.Size = new Size(195, 32);
            toolStripMenuItemSqlGenerar.Text = "Generar Script";
            toolStripMenuItemSqlGenerar.Click += toolStripMenuItemSqlGenerar_Click;
            // 
            // contextMenuSqlScriptPestaña
            // 
            contextMenuSqlScriptPestaña.ImageScalingSize = new Size(24, 24);
            contextMenuSqlScriptPestaña.Items.AddRange(new ToolStripItem[] { toolStripMenuItemCerrarPestañaSql });
            contextMenuSqlScriptPestaña.Name = "contextMenuSqlScriptPestaña";
            contextMenuSqlScriptPestaña.Size = new Size(220, 36);
            // 
            // toolStripMenuItemCerrarPestañaSql
            // 
            toolStripMenuItemCerrarPestañaSql.Name = "toolStripMenuItemCerrarPestañaSql";
            toolStripMenuItemCerrarPestañaSql.Size = new Size(219, 32);
            toolStripMenuItemCerrarPestañaSql.Text = "Cerrar esta pestaña";
            toolStripMenuItemCerrarPestañaSql.Click += toolStripMenuItemCerrarPestañaSql_Click;
            // 
            // Index
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(tabControl1);
            Controls.Add(panelTop);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
            panelReinicioPie.ResumeLayout(false);
            panelReinicioTodosBar.ResumeLayout(false);
            panelReinicioTodosBar.PerformLayout();
            panelReinicioToolbar.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            splitContainerSql.Panel1.ResumeLayout(false);
            splitContainerSql.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerSql).EndInit();
            splitContainerSql.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSqlListaTablas).EndInit();
            panelSqlDerecho.ResumeLayout(false);
            tabPageSqlScript1.ResumeLayout(false);
            tabControlSqlScripts.ResumeLayout(false);
            splitSqlEditorResultados.Panel1.ResumeLayout(false);
            splitSqlEditorResultados.Panel1.PerformLayout();
            splitSqlEditorResultados.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitSqlEditorResultados).EndInit();
            splitSqlEditorResultados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSqlResultados).EndInit();
            panelSqlBarEjecutar.ResumeLayout(false);
            tableLayoutPanelSqlEjecutar.ResumeLayout(false);
            panelSqlToolbar.ResumeLayout(false);
            panelSqlToolbar.PerformLayout();
            contextMenuSqlTablas.ResumeLayout(false);
            contextMenuSqlScriptPestaña.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel panelReinicioToolbar;
        private Button btnCargarDockers;
        private ProgressBar progressBarCargarDockers;
        private Label lblCargarDockersBusy;
        private Button btnReiniciar;
        private ProgressBar progressBarReinicio;
        private Label lblReinicioBusy;
        private Panel panelReinicioBody;
        private Panel panelReinicioPie;
        private Label lblReinicioSinDockers;
        private Panel panelReinicioTodosBar;
        private CheckBox chkTodosDocker;
        private FlowLayoutPanel flowLayoutPanelDockers;
        private TabPage tabPage3;
        private Panel panelSqlToolbar;
        private Label lblSqlBusy;
        private ProgressBar progressBarSql;
        private Button btnSqlConectar;
        private SplitContainer splitContainerSql;
        private DataGridView dgvSqlListaTablas;
        private Panel panelSqlDerecho;
        private TabControl tabControlSqlScripts;
        private TabPage tabPageSqlScript1;
        private Panel panelSqlBarEjecutar;
        private TableLayoutPanel tableLayoutPanelSqlEjecutar;
        private Button btnSqlEjecutar;
        private Button btnSqlNuevoScript;
        private ProgressBar progressBarSqlConsulta;
        private Label lblSqlConsultaBusy;
        private Button btnSqlExportarExcel;
        private SplitContainer splitSqlEditorResultados;
        private TextBox txtSqlEditor;
        private DataGridView dgvSqlResultados;
        private ContextMenuStrip contextMenuSqlTablas;
        private ToolStripMenuItem toolStripMenuItemSqlGenerar;
        private ContextMenuStrip contextMenuSqlScriptPestaña;
        private ToolStripMenuItem toolStripMenuItemCerrarPestañaSql;

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