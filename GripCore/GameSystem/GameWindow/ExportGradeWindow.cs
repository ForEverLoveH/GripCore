using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GripCore.GameSystem.GameWindowSys;
using GripCoreModel.GameModel;
using Sunny.UI;

namespace GripCore.GameSystem.GameWindow
{
    public partial class ExportGradeWindow : Form
    {
        public ExportGradeWindow()
        {
            InitializeComponent();
        }

        private SportProjectInfos _sportProjectInfos = new SportProjectInfos();
        public string groupName = string.Empty;
        private bool isAllTest = false;
        private bool isOnlyGroup = false;

        private void ExportGradeWindow_Load(object sender, EventArgs e)
        {
            ExportGradeWindowSys.Instance.LoadingInitData(groupName,uiCheckBox2, uiTextBox1, ref _sportProjectInfos);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            isAllTest = uiCheckBox1.Checked;
            isOnlyGroup = uiCheckBox2.Checked;
            uiButton1.Enabled = false;
            bool result = ExportGradeWindowSys.Instance.OutPutScore(_sportProjectInfos, isOnlyGroup, groupName, isAllTest);
            if (result)
            {
                UIMessageBox.Show("导出成功");
                uiButton1.Enabled = true;
            }
        }

        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            bool s = uiCheckBox2.Checked;
            if(uiCheckBox1.Checked)
            {
                uiCheckBox1.Checked = false;
            }

            if (s)
            {
                uiButton1.Text = "导出当前组";
            }
            else
            {
                uiButton1.Text = "导出全部成绩";
            }
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(uiCheckBox2.Checked)
            {
                uiCheckBox2 .Checked = false;
            }
        }
    }
}
