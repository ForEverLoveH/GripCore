using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameWindowSys;
using GripCoreModel.GameModel;
using HZH_Controls.Forms;
using MiniExcelLibs;
using Sunny.UI;

namespace GripCore.GameSystem.GameWindow
{
    public partial class PersonImportWindow : Form
    {
        public PersonImportWindow()
        {
            InitializeComponent();
        }
        string paths = string.Empty;
        private Dictionary<string, string> localValues = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonImportWindow_Load(object sender, EventArgs e)
        {
            PersonImportWindowSys.Instance.InitListViewHeader(listView1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            PlatFormSettingWindowSys.Instance.ShowPlatFormSettingWindow();
        }
        /// <summary>
        /// 拉取名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            string names = uiTextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(names))
                PersonImportWindowSys.Instance.LoadingStudentDataFormServer(names, listView1, ref localValues,uiLabel5);
            else
            {
                UIMessageBox.ShowError("请输入你需要的数据   ");
                return;
            }
        }
        /// <summary>
        /// 浏览本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            PersonImportWindowSys.Instance.OpenLocalExcelFile(uiTextBox2);
            string path = uiTextBox2.Text.Trim();
            if (!string.IsNullOrWhiteSpace(path ))
            {
                try
                {
                    var rows = MiniExcel.Query<InputData>(path).ToList();
                    
                    listView1.BeginUpdate();
                    listView1.View = View.Details;
                    listView1.Items.Clear();
                    int step = 1;
                    foreach (var row in rows)
                    {
                        ListViewItem li = new ListViewItem();
                        li.Text = step.ToString();
                        li.SubItems.Add(row.ExamDate);
                        li.SubItems.Add(row.SchoolName);
                        li.SubItems.Add(row.Grade);
                        li.SubItems.Add(row.ClassName);
                        li.SubItems.Add(row.IDNumber);
                        li.SubItems.Add(row.Name);
                        li.SubItems.Add(row.Sex);
                        li.SubItems.Add(row.GroupName);
                        listView1.Items.Insert(listView1.Items.Count, li);
                        step++;
                    }

                    listView1.EndUpdate();
                    paths = path;
                    MessageBox.Show("读取成功");
                }
                catch (Exception exception)
                {
                    LoggerHelper.Debug(exception);
                    return;
                }
            }
        }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         
        private void uiButton4_Click(object sender, EventArgs e)
        {
            if (PersonImportWindowSys.Instance.SaveTemplateExcel())
            {
                FrmTips.ShowTipsSuccess(this, "获取成功!!");
            }
            else
            {
                FrmTips.ShowTips(this, "获取失败！！");
                return;
            }
        }

         private bool isDeleteBeforeImport = false;
         private  bool isImport = false;
         private void uiButton7_Click(object sender, EventArgs e)
         {
              isDeleteBeforeImport = true;
              if (string.IsNullOrEmpty(paths))
              {
                  UIMessageBox.ShowError("路径错误！！");
                  return;
              }
              ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ExcelInputDataBase);
              Thread thread = new Thread(parameterizedThreadStart);
              thread.IsBackground = true;
              thread.Start(paths);
         }

         private void ExcelInputDataBase(object obj)
         {
             HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
             {
                 
                 if (PersonImportWindowSys.Instance.ExcelInputDataBase(obj, isDeleteBeforeImport, uiLabel5))
                 {
                     UIMessageBox.ShowSuccess("导入成功");
                      
                     isImport = true;
                 }
                 else
                 {
                     UIMessageBox.ShowError("导入失败");
                     return;
                 }
             });
         }
         private void uiButton6_Click(object sender, EventArgs e)
         {
             if (string.IsNullOrEmpty(paths))
             {
                 UIMessageBox.ShowError("路径错误！！");
                 return;
             }
             ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ExcelInputDataBase);
             Thread thread = new Thread(parameterizedThreadStart);
             thread.IsBackground = true;
             thread.Start(paths);
         }
    }
}
