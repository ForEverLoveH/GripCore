namespace GripCore.GameSystem.GameWindow
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem1 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem2 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem3 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem4 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem5 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem6 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem7 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem8 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem9 = new HZH_Controls.Controls.NavigationMenuItem();
            HZH_Controls.Controls.NavigationMenuItem navigationMenuItem10 = new HZH_Controls.Controls.NavigationMenuItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.uiTitlePanel3 = new Sunny.UI.UITitlePanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ucProcessLine1 = new HZH_Controls.Controls.UCProcessLine();
            this.uiTitlePanel2 = new Sunny.UI.UITitlePanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ucNavigationMenu1 = new HZH_Controls.Controls.UCNavigationMenu();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.uiTitlePanel1.SuspendLayout();
            this.uiTitlePanel3.SuspendLayout();
            this.uiTitlePanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.uiTitlePanel3);
            this.uiTitlePanel1.Controls.Add(this.ucProcessLine1);
            this.uiTitlePanel1.Controls.Add(this.uiTitlePanel2);
            this.uiTitlePanel1.Controls.Add(this.ucNavigationMenu1);
            this.uiTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(12, 6);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(1334, 713);
            this.uiTitlePanel1.TabIndex = 2;
            this.uiTitlePanel1.Text = "德育龙握力测试系统";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTitlePanel1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // uiTitlePanel3
            // 
            this.uiTitlePanel3.Controls.Add(this.treeView1);
            this.uiTitlePanel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel3.Location = new System.Drawing.Point(4, 106);
            this.uiTitlePanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel3.Name = "uiTitlePanel3";
            this.uiTitlePanel3.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel3.ShowText = false;
            this.uiTitlePanel3.Size = new System.Drawing.Size(342, 583);
            this.uiTitlePanel3.TabIndex = 117;
            this.uiTitlePanel3.Text = "项目信息";
            this.uiTitlePanel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTitlePanel3.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(3, 39);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(335, 541);
            this.treeView1.TabIndex = 2;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // ucProcessLine1
            // 
            this.ucProcessLine1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ucProcessLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucProcessLine1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ucProcessLine1.ForeColor = System.Drawing.Color.Black;
            this.ucProcessLine1.Location = new System.Drawing.Point(0, 699);
            this.ucProcessLine1.Margin = new System.Windows.Forms.Padding(2);
            this.ucProcessLine1.MaxValue = 100;
            this.ucProcessLine1.Name = "ucProcessLine1";
            this.ucProcessLine1.Size = new System.Drawing.Size(1334, 14);
            this.ucProcessLine1.TabIndex = 116;
            this.ucProcessLine1.Text = "ucProcessLine1";
            this.ucProcessLine1.Value = 0;
            this.ucProcessLine1.ValueBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(231)))), ((int)(((byte)(237)))));
            this.ucProcessLine1.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucProcessLine1.ValueTextType = HZH_Controls.Controls.ValueTextType.Percent;
            this.ucProcessLine1.Visible = false;
            // 
            // uiTitlePanel2
            // 
            this.uiTitlePanel2.Controls.Add(this.listView1);
            this.uiTitlePanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel2.Location = new System.Drawing.Point(349, 106);
            this.uiTitlePanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel2.Name = "uiTitlePanel2";
            this.uiTitlePanel2.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.uiTitlePanel2.ShowText = false;
            this.uiTitlePanel2.Size = new System.Drawing.Size(978, 586);
            this.uiTitlePanel2.TabIndex = 2;
            this.uiTitlePanel2.Text = "群组信息";
            this.uiTitlePanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTitlePanel2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(4, 39);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(971, 544);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // ucNavigationMenu1
            // 
            this.ucNavigationMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.ucNavigationMenu1.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucNavigationMenu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            navigationMenuItem1.AnchorRight = false;
            navigationMenuItem1.DataSource = null;
            navigationMenuItem1.HasSplitLintAtTop = false;
            navigationMenuItem1.Icon = null;
            navigationMenuItem2.AnchorRight = false;
            navigationMenuItem2.DataSource = null;
            navigationMenuItem2.HasSplitLintAtTop = false;
            navigationMenuItem2.Icon = null;
            navigationMenuItem2.Items = null;
            navigationMenuItem2.ItemWidth = 100;
            navigationMenuItem2.ShowTip = false;
            navigationMenuItem2.Text = "导入名单";
            navigationMenuItem2.TipText = null;
            navigationMenuItem1.Items = new HZH_Controls.Controls.NavigationMenuItem[] {
        navigationMenuItem2};
            navigationMenuItem1.ItemWidth = 100;
            navigationMenuItem1.ShowTip = false;
            navigationMenuItem1.Text = "系统管理";
            navigationMenuItem1.TipText = null;
            navigationMenuItem3.AnchorRight = false;
            navigationMenuItem3.DataSource = null;
            navigationMenuItem3.HasSplitLintAtTop = false;
            navigationMenuItem3.Icon = null;
            navigationMenuItem4.AnchorRight = false;
            navigationMenuItem4.DataSource = null;
            navigationMenuItem4.HasSplitLintAtTop = false;
            navigationMenuItem4.Icon = null;
            navigationMenuItem4.Items = null;
            navigationMenuItem4.ItemWidth = 100;
            navigationMenuItem4.ShowTip = false;
            navigationMenuItem4.Text = "数据库初始化";
            navigationMenuItem4.TipText = null;
            navigationMenuItem5.AnchorRight = false;
            navigationMenuItem5.DataSource = null;
            navigationMenuItem5.HasSplitLintAtTop = false;
            navigationMenuItem5.Icon = null;
            navigationMenuItem5.Items = null;
            navigationMenuItem5.ItemWidth = 100;
            navigationMenuItem5.ShowTip = false;
            navigationMenuItem5.Text = "系统参数设置";
            navigationMenuItem5.TipText = null;
            navigationMenuItem6.AnchorRight = false;
            navigationMenuItem6.DataSource = null;
            navigationMenuItem6.HasSplitLintAtTop = false;
            navigationMenuItem6.Icon = null;
            navigationMenuItem6.Items = null;
            navigationMenuItem6.ItemWidth = 100;
            navigationMenuItem6.ShowTip = false;
            navigationMenuItem6.Text = "平台设置";
            navigationMenuItem6.TipText = null;
            navigationMenuItem3.Items = new HZH_Controls.Controls.NavigationMenuItem[] {
        navigationMenuItem4,
        navigationMenuItem5,
        navigationMenuItem6};
            navigationMenuItem3.ItemWidth = 100;
            navigationMenuItem3.ShowTip = false;
            navigationMenuItem3.Text = "数据管理";
            navigationMenuItem3.TipText = null;
            navigationMenuItem7.AnchorRight = false;
            navigationMenuItem7.DataSource = null;
            navigationMenuItem7.HasSplitLintAtTop = false;
            navigationMenuItem7.Icon = null;
            navigationMenuItem8.AnchorRight = false;
            navigationMenuItem8.DataSource = null;
            navigationMenuItem8.HasSplitLintAtTop = false;
            navigationMenuItem8.Icon = null;
            navigationMenuItem8.Items = null;
            navigationMenuItem8.ItemWidth = 100;
            navigationMenuItem8.ShowTip = false;
            navigationMenuItem8.Text = "上传成绩";
            navigationMenuItem8.TipText = null;
            navigationMenuItem9.AnchorRight = false;
            navigationMenuItem9.DataSource = null;
            navigationMenuItem9.HasSplitLintAtTop = false;
            navigationMenuItem9.Icon = null;
            navigationMenuItem9.Items = null;
            navigationMenuItem9.ItemWidth = 100;
            navigationMenuItem9.ShowTip = false;
            navigationMenuItem9.Text = "导出成绩";
            navigationMenuItem9.TipText = null;
            navigationMenuItem7.Items = new HZH_Controls.Controls.NavigationMenuItem[] {
        navigationMenuItem8,
        navigationMenuItem9};
            navigationMenuItem7.ItemWidth = 100;
            navigationMenuItem7.ShowTip = false;
            navigationMenuItem7.Text = "成绩管理";
            navigationMenuItem7.TipText = null;
            navigationMenuItem10.AnchorRight = false;
            navigationMenuItem10.DataSource = null;
            navigationMenuItem10.HasSplitLintAtTop = false;
            navigationMenuItem10.Icon = null;
            navigationMenuItem10.Items = null;
            navigationMenuItem10.ItemWidth = 100;
            navigationMenuItem10.ShowTip = false;
            navigationMenuItem10.Text = "启动测试";
            navigationMenuItem10.TipText = null;
            this.ucNavigationMenu1.Items = new HZH_Controls.Controls.NavigationMenuItem[] {
        navigationMenuItem1,
        navigationMenuItem3,
        navigationMenuItem7,
        navigationMenuItem10};
            this.ucNavigationMenu1.Location = new System.Drawing.Point(3, 38);
            this.ucNavigationMenu1.Name = "ucNavigationMenu1";
            this.ucNavigationMenu1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.ucNavigationMenu1.Size = new System.Drawing.Size(1328, 61);
            this.ucNavigationMenu1.TabIndex = 0;
            this.ucNavigationMenu1.TipColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.ucNavigationMenu1.ClickItemed += new System.EventHandler(this.ucNavigationMenu1_ClickItemed);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 725);
            this.Controls.Add(this.uiTitlePanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "主页面";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.uiTitlePanel1.ResumeLayout(false);
            this.uiTitlePanel3.ResumeLayout(false);
            this.uiTitlePanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private HZH_Controls.Controls.UCProcessLine ucProcessLine1;
        private Sunny.UI.UITitlePanel uiTitlePanel2;
        private System.Windows.Forms.ListView listView1;
        private HZH_Controls.Controls.UCNavigationMenu ucNavigationMenu1;
        private System.Windows.Forms.Timer timer1;
        private Sunny.UI.UITitlePanel uiTitlePanel3;
        private System.Windows.Forms.TreeView treeView1;
    }
}