using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GripCore.GameSystem.MyControl
{
    public partial class MyControll : UserControl
    {
        public MyControll()
        {
            InitializeComponent();
        }
        [Description("标题"),Category("自定义属性")]
        public  string p_Title
        {
            get
            {
                return mtitle.Text;
            }
            set
            {
                mtitle.Text = value;
            }
        }
        [Description("标题颜色"), Category("自定义属性")]
        public Color p_TitleColor
        {
            get
            {
                return mtitle.ForeColor;
            }
            set
            {
                mtitle.ForeColor = value;
            }
        }
        [Description("考号"), Category("自定义属性")]
        public  string p_IdNumber
        {
            get
            {
                return mExamNum.Text;
            }
            set
            {
                mExamNum.Text = value;
            }
        }
        [Description("姓名"), Category("自定义属性")]
        public string p_Name
        {
            get
            {
                return mName.Text;
            }
            set
            {
                mName.Text = value;
            }
        }
        [Description("成绩"), Category("自定义属性")]
        public string p_Score
        {
            get
            {
                return mGrade.Text;
            }
            set
            {
                mGrade.Text = value;
            }
        }
        [Description("设备状态"), Category("自定义属性")]
        public string p_toolState
        {
            get
            {
                return mState.Text;
            }
            set
            {
                mState.Text = value;
            }
        }
        [Description("设备状态颜色"), Category("自定义属性")]
        public Color p_toolStateColor
        {
            get
            {
                return mState.ForeColor;
            }
            set
            {
                mState.ForeColor = value;
            }
        }
        [Description("轮次"), Category("自定义属性")]
        public int p_RoundCountSelectIndex
        {
            get
            {
                return RoundCbx.SelectedIndex;
            }
            set
            {
                RoundCbx.SelectedIndex = value;
            }
        }
        [Description("轮次items"), Category("自定义属性")]
        public List<string> p_RoundCountItems
        {
            get
            {
                List<string> items = new List<string>();
                foreach(object item in items)
                {
                    items.Add(item.ToString());
                }
                return items;
            }
            set
            {
                RoundCbx.Items.Clear();
                foreach(string item in value)
                {
                    RoundCbx.Items.Add(item);
                }
            }
        }
        [Description("状态下拉框"), Category("自定义属性")]
        public int p_MStateIndex
        {
            get
            {
                return stateCbx.SelectedIndex;
            }
            set
            {
                stateCbx.SelectedIndex = value;
            }
        }
        [Description("状态items"), Category("自定义属性")]
        public List<string> p_MstateItems
        {
            get
            {
                List<string> items = new List<string>();
                foreach (object item in items)
                {
                    items.Add(item.ToString());
                }
                return items;
            }
            set
            {
                stateCbx.Items.Clear();
                foreach (string item in value)
                {
                    stateCbx.Items.Add(item);
                }
            }
        }

    }
}