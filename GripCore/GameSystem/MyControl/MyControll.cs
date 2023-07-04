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
        [Description("����"),Category("�Զ�������")]
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
        [Description("������ɫ"), Category("�Զ�������")]
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
        [Description("����"), Category("�Զ�������")]
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
        [Description("����"), Category("�Զ�������")]
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
        [Description("�ɼ�"), Category("�Զ�������")]
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
        [Description("�豸״̬"), Category("�Զ�������")]
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
        [Description("�豸״̬��ɫ"), Category("�Զ�������")]
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
        [Description("�ִ�"), Category("�Զ�������")]
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
        [Description("�ִ�items"), Category("�Զ�������")]
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
        [Description("״̬������"), Category("�Զ�������")]
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
        [Description("״̬items"), Category("�Զ�������")]
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