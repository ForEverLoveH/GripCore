using System.ComponentModel;

namespace GripCore.GameSystem.MyControl
{
    partial class MyControll
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.mExamNum = new Sunny.UI.UILabel();
            this.mName = new Sunny.UI.UILabel();
            this.mGrade = new Sunny.UI.UILabel();
            this.RoundCbx = new System.Windows.Forms.ComboBox();
            this.stateCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mtitle = new Sunny.UI.UILabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mState = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mExamNum);
            this.panel1.Controls.Add(this.mName);
            this.panel1.Controls.Add(this.mGrade);
            this.panel1.Controls.Add(this.RoundCbx);
            this.panel1.Controls.Add(this.stateCbx);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.mtitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 253);
            this.panel1.TabIndex = 0;
            // 
            // mExamNum
            // 
            this.mExamNum.Font = new System.Drawing.Font("Œ¢»Ì—≈∫⁄", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mExamNum.Location = new System.Drawing.Point(66, 39);
            this.mExamNum.Name = "mExamNum";
            this.mExamNum.Size = new System.Drawing.Size(100, 17);
            this.mExamNum.TabIndex = 10;
            this.mExamNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mExamNum.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mName
            // 
            this.mName.Font = new System.Drawing.Font("Œ¢»Ì—≈∫⁄", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mName.Location = new System.Drawing.Point(66, 71);
            this.mName.Name = "mName";
            this.mName.Size = new System.Drawing.Size(100, 23);
            this.mName.TabIndex = 9;
            this.mName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mName.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mGrade
            // 
            this.mGrade.Font = new System.Drawing.Font("Œ¢»Ì—≈∫⁄", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mGrade.Location = new System.Drawing.Point(66, 117);
            this.mGrade.Name = "mGrade";
            this.mGrade.Size = new System.Drawing.Size(100, 21);
            this.mGrade.TabIndex = 8;
            this.mGrade.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mGrade.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // RoundCbx
            // 
            this.RoundCbx.FormattingEnabled = true;
            this.RoundCbx.Location = new System.Drawing.Point(65, 160);
            this.RoundCbx.Name = "RoundCbx";
            this.RoundCbx.Size = new System.Drawing.Size(101, 20);
            this.RoundCbx.TabIndex = 7;
            // 
            // stateCbx
            // 
            this.stateCbx.FormattingEnabled = true;
            this.stateCbx.Items.AddRange(new object[] {
            "Œ¥≤‚ ‘",
            "“—≤‚ ‘",
            "÷–ÕÀ",
            "»±øº",
            "∑∏πÊ",
            "∆˙»®"});
            this.stateCbx.Location = new System.Drawing.Point(65, 201);
            this.stateCbx.Name = "stateCbx";
            this.stateCbx.Size = new System.Drawing.Size(101, 20);
            this.stateCbx.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("ÀŒÃÂ", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(15, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "◊¥Ã¨£∫";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("ÀŒÃÂ", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 14);
            this.label4.TabIndex = 4;
            this.label4.Text = "¬÷¥Œ£∫";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("ÀŒÃÂ", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "≥…º®£∫";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ÀŒÃÂ", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "–’√˚£∫";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ÀŒÃÂ", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "øº∫≈£∫";
            // 
            // mtitle
            // 
            this.mtitle.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.mtitle.Font = new System.Drawing.Font("Œ¢»Ì—≈∫⁄", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtitle.Location = new System.Drawing.Point(0, 0);
            this.mtitle.Name = "mtitle";
            this.mtitle.Size = new System.Drawing.Size(197, 23);
            this.mtitle.TabIndex = 0;
            this.mtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mtitle.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 231);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(188, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // mState
            // 
            this.mState.Name = "mState";
            this.mState.Size = new System.Drawing.Size(68, 17);
            this.mState.Text = "…Ë±∏Œ¥¡¨Ω”";
            // 
            // MyControll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "MyControll";
            this.Size = new System.Drawing.Size(188, 253);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Sunny.UI.UILabel mExamNum;
        private Sunny.UI.UILabel mName;
        private Sunny.UI.UILabel mGrade;
        private System.Windows.Forms.ComboBox RoundCbx;
        private System.Windows.Forms.ComboBox stateCbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Sunny.UI.UILabel mtitle;
        private System.Windows.Forms.ToolStripStatusLabel mState;
    }
}