using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Windows.Forms;
using GripCore.GameSystem.GameHelper;
using GripCoreModel;
using GripCoreModel.GameModel;
using Sunny.UI;

namespace GripCore.GameSystem.GameWindow
{
    public partial class FromRoundWindow :HZH_Controls.Forms.FrmWithOKCancel2
    {
        public FromRoundWindow()
        {
            InitializeComponent();
        }
        public int mode = -1;

        private SportProjectInfos sportProjectInfos = null;
        public DbPersonInfos dbPersonInfos = null;
        public string _idnumber = string.Empty;
        bool isNoExam = false;
        IFreeSql fsql = FreeSqlHelper.Sqlite;
        private void FromRoundWindow_Load(object sender, EventArgs e)
        {
            if (mode == 0) this.Title = "轮次清空";
            else if (mode == 1) this.Title = "修正成绩";
            if (!string.IsNullOrEmpty(_idnumber))
            {
                dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == _idnumber).ToOne();
            }

            sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                int roundTotal = sportProjectInfos.RountCount;
                for (int i = 0; i < roundTotal; i++)
                {
                    uiComboBox1.Items.Add($"第{i + 1}轮");
                }

                if (roundTotal > 0) uiComboBox1.SelectedIndex = 0;
            }

            if (dbPersonInfos != null)
            {
                uiTextBox1.Text = dbPersonInfos.IdNumber;
                uiTextBox2.Text = dbPersonInfos.Name;   
            }
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int roundID = uiComboBox1.SelectedIndex + 1;
            if (roundID > 0)
            {
                List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
                    .Where(a => a.PersonIdNumber == _idnumber && a.RoundId == roundID && a.IsRemoved == 0).ToList();
                uiTextBox3.Text = string.Empty;
                if (resultInfos.Count == 0)
                {
                    isNoExam = true;
                    MessageBox.Show("该学生没有参加本轮考试！！");

                }
                else
                {
                    foreach (ResultInfos resultInfo in resultInfos)
                    {
                        if (resultInfo.IsRemoved == 0)
                        {
                            uiTextBox3.Text = resultInfo.Result.ToString();
                        }
                    }
                }
            }
        }

        private  void FromRoundWindow_Closing(object obj,FormClosingEventArgs args)
        {
            if (base.DialogResult == DialogResult.OK)
            {
                int roundid = uiComboBox1.SelectedIndex + 1;
                if (mode == 0)
                {
                    int res = fsql.Delete<ResultInfos>()
                        .Where(a => a.PersonIdNumber == _idnumber && a.RoundId == roundid).ExecuteAffrows();
                    if (res == 1)
                    {
                        UIMessageBox.ShowSuccess("删除成功！！");
                    }
                }
                else
                {
                    if (mode == 1)
                    {
                       
                        double.TryParse(uiTextBox3.Text,out  double fhs);
                        if (isNoExam)
                        {
                            List<ResultInfos> resultInfosList = new List<ResultInfos>();
                            int maxSordid = 0;
                            fsql.Select<ResultInfos>().Aggregate<int>(x => x.Max<int>(x.Key.SortId), out maxSordid);
                            maxSordid++;
                            resultInfosList.Add(new ResultInfos()
                            {
                                CreateTime = DateTime.Now.ToString("yyyy-MM-DD HH:MM:dd"),
                                SortId = maxSordid,
                                PersonId = dbPersonInfos.Name,
                                SportItemType = 0,
                                PersonName = dbPersonInfos.Name,
                                PersonIdNumber = dbPersonInfos.IdNumber,
                                RoundId = roundid,
                                State = 1,
                                IsRemoved = 0,Result = fhs,

                            });
                            int result = fsql.Update<ResultInfos>().Set<bool>(a => a.Result == fhs).Where(a =>
                                    a.PersonIdNumber == _idnumber && a.RoundId == roundid && a.IsRemoved == 0)
                                .ExecuteAffrows();
                            if (result==1)
                            {
                                UIMessageBox.ShowSuccess("修改成功");
                            }
                        }

                    }
                }
            }
        }

         
    }
}