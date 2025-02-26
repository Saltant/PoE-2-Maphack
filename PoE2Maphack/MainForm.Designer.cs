namespace PoE2Maphack;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        flowLayoutPanel1 = new FlowLayoutPanel();
        linkLabel1 = new LinkLabel();
        tableLayoutPanel1 = new TableLayoutPanel();
        panel1 = new Panel();
        deactivateBtn = new RadioButton();
        activateBtn = new RadioButton();
        tableLayoutPanel2 = new TableLayoutPanel();
        doWorkBtn = new Button();
        statusLabel = new Label();
        notifyIcon1 = new NotifyIcon(components);
        contextMenuStrip1 = new ContextMenuStrip(components);
        toolStripMenuItem1 = new ToolStripMenuItem();
        toolStripMenuItem2 = new ToolStripMenuItem();
        closeToolStripMenuItem = new ToolStripMenuItem();
        flowLayoutPanel1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        panel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        contextMenuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Controls.Add(linkLabel1);
        flowLayoutPanel1.Dock = DockStyle.Bottom;
        flowLayoutPanel1.Location = new Point(0, 75);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Padding = new Padding(75, 3, 0, 0);
        flowLayoutPanel1.Size = new Size(271, 23);
        flowLayoutPanel1.TabIndex = 0;
        // 
        // linkLabel1
        // 
        linkLabel1.AutoSize = true;
        linkLabel1.Cursor = Cursors.Hand;
        linkLabel1.Font = new Font("Segoe UI", 9F);
        linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
        linkLabel1.Location = new Point(78, 3);
        linkLabel1.Name = "linkLabel1";
        linkLabel1.Size = new Size(113, 15);
        linkLabel1.TabIndex = 0;
        linkLabel1.TabStop = true;
        linkLabel1.Text = "Available on GitHub";
        linkLabel1.TextAlign = ContentAlignment.MiddleCenter;
        linkLabel1.LinkClicked += OnGithubLinkClick;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(panel1, 1, 0);
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(271, 75);
        tableLayoutPanel1.TabIndex = 1;
        // 
        // panel1
        // 
        panel1.BorderStyle = BorderStyle.FixedSingle;
        panel1.Controls.Add(deactivateBtn);
        panel1.Controls.Add(activateBtn);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(138, 3);
        panel1.Name = "panel1";
        panel1.Size = new Size(130, 69);
        panel1.TabIndex = 0;
        // 
        // deactivateBtn
        // 
        deactivateBtn.AutoSize = true;
        deactivateBtn.Location = new Point(16, 37);
        deactivateBtn.Name = "deactivateBtn";
        deactivateBtn.Size = new Size(103, 19);
        deactivateBtn.TabIndex = 1;
        deactivateBtn.TabStop = true;
        deactivateBtn.Text = "Deactivate MH";
        deactivateBtn.UseVisualStyleBackColor = true;
        // 
        // activateBtn
        // 
        activateBtn.AutoSize = true;
        activateBtn.Location = new Point(16, 12);
        activateBtn.Name = "activateBtn";
        activateBtn.Size = new Size(91, 19);
        activateBtn.TabIndex = 0;
        activateBtn.TabStop = true;
        activateBtn.Text = "Activate MH";
        activateBtn.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 1;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.Controls.Add(doWorkBtn, 0, 1);
        tableLayoutPanel2.Controls.Add(statusLabel, 0, 0);
        tableLayoutPanel2.Location = new Point(3, 3);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 2;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        tableLayoutPanel2.Size = new Size(129, 68);
        tableLayoutPanel2.TabIndex = 1;
        // 
        // doWorkBtn
        // 
        doWorkBtn.Cursor = Cursors.Hand;
        doWorkBtn.Dock = DockStyle.Fill;
        doWorkBtn.FlatStyle = FlatStyle.Flat;
        doWorkBtn.Font = new Font("Segoe UI", 9F);
        doWorkBtn.Location = new Point(3, 40);
        doWorkBtn.Margin = new Padding(3, 0, 3, 3);
        doWorkBtn.Name = "doWorkBtn";
        doWorkBtn.Size = new Size(123, 25);
        doWorkBtn.TabIndex = 0;
        doWorkBtn.Text = "Change State";
        doWorkBtn.TextAlign = ContentAlignment.TopCenter;
        doWorkBtn.UseVisualStyleBackColor = true;
        doWorkBtn.Click += OnChangeStateClick;
        // 
        // statusLabel
        // 
        statusLabel.AutoSize = true;
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        statusLabel.Location = new Point(3, 0);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(123, 40);
        statusLabel.TabIndex = 1;
        statusLabel.Text = "Status: Deactivated";
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // notifyIcon1
        // 
        notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
        notifyIcon1.Text = "PoE 2 Maphack";
        notifyIcon1.Visible = true;
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, closeToolStripMenuItem });
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new Size(153, 70);
        contextMenuStrip1.Text = "Menu";
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(152, 22);
        toolStripMenuItem1.Text = "Activate MH";
        toolStripMenuItem1.Click += ToolStripMenuActivate_Click;
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        toolStripMenuItem2.Size = new Size(152, 22);
        toolStripMenuItem2.Text = "Deactivate MH";
        toolStripMenuItem2.Click += ToolStripMenuDeactivate_Click;
        // 
        // closeToolStripMenuItem
        // 
        closeToolStripMenuItem.Name = "closeToolStripMenuItem";
        closeToolStripMenuItem.Size = new Size(152, 22);
        closeToolStripMenuItem.Text = "Exit";
        closeToolStripMenuItem.Click += ToolStripMenuExit_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(271, 98);
        Controls.Add(tableLayoutPanel1);
        Controls.Add(flowLayoutPanel1);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PoE 2 Maphack v1.0";
        flowLayoutPanel1.ResumeLayout(false);
        flowLayoutPanel1.PerformLayout();
        tableLayoutPanel1.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        contextMenuStrip1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private LinkLabel linkLabel1;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private RadioButton deactivateBtn;
    private RadioButton activateBtn;
    private TableLayoutPanel tableLayoutPanel2;
    private Button doWorkBtn;
    private Label statusLabel;
    private NotifyIcon notifyIcon1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem closeToolStripMenuItem;
}
