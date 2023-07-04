using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameSystemModel;
using GripCoreModel;
using GripCoreModel.GameModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls.Controls;
using Newtonsoft.Json;
using HZH_Controls;

namespace GripCore.GameSystem.GameWindowSys
{
    public  class MainWindowSys
    {
        public static MainWindowSys Instance;
        public  void Awake()
        {
            Instance = this;
        }
        IFreeSql freeSql = FreeSqlHelper.Sqlite;
        private WriteLoggerHelper WriteLoggerHelper = new WriteLoggerHelper();
        public void UpdataTreeview(TreeView treeView1)
        {
            treeView1.Nodes.Clear();
            try
            {
                List<TreeViewModel> treeViewModels = new List<TreeViewModel>();
                treeView1.Nodes.Clear();
                var lists = freeSql.Select<DbPersonInfos>().Distinct().ToList(a => new { a.CreateTime, a.SchoolName, a.GroupName });
                // Console.WriteLine();
                foreach (var item in lists)
                {
                    TreeViewModel treeViewModel = treeViewModels.Find(a => a.CreateTime == item.CreateTime);
                    if (treeViewModel == null)
                    {
                        treeViewModels.Add(new TreeViewModel { CreateTime = item.CreateTime, schoolModels = new List<TreeViewSchoolModel>() });
                    }
                    treeViewModel = treeViewModels.Find(a => a.CreateTime == item.CreateTime);
                    if (treeViewModel != null)
                    {
                        TreeViewSchoolModel treeViewSchoolsModel = treeViewModel.schoolModels.Find(a => a.schoolName == item.SchoolName);
                        if (treeViewSchoolsModel == null)
                        {
                            treeViewModel.schoolModels.Add(new TreeViewSchoolModel()
                            {
                                schoolName = item.SchoolName,
                                Groups = new List<string> { item.GroupName }
                            });
                        }
                        else
                        {
                            treeViewSchoolsModel.Groups.Add(item.GroupName);
                        }
                    }
                }
                for (int i = 0; i < treeViewModels.Count; i++)
                {
                    TreeNode tn1 = new TreeNode(treeViewModels[i].CreateTime);
                    List<TreeViewSchoolModel> treeViewSchoolsModel = treeViewModels[i].schoolModels;
                    for (int j = 0; j < treeViewSchoolsModel.Count; j++)
                    {
                        TreeNode tn2 = new TreeNode(treeViewSchoolsModel[j].schoolName);
                        foreach (var group in treeViewSchoolsModel[j].Groups)
                        {
                            tn2.Nodes.Add(group);
                        }
                        tn1.Nodes.Add(tn2);
                    }
                    treeView1.Nodes.Add(tn1);

                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="schoolName"></param>
        /// <param name="groupName"></param>
        /// <param name="listView1"></param>
        public void UpdataGroupDataView(string createTime, string schoolName, string groupName, ListView listView1)
        {
            try
            {
                if (string.IsNullOrEmpty(createTime) || string.IsNullOrEmpty(schoolName) || string.IsNullOrEmpty(groupName))
                {
                    return;
                }
                else
                {
                    SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                    List<DbPersonInfos> personInfos = freeSql.Select<DbPersonInfos>().Where(a => a.CreateTime == createTime && a.SchoolName == schoolName && a.GroupName == groupName).ToList();
                    if (personInfos.Count > 0)
                    {
                        int step = 1;
                        listView1.Items.Clear();  
                        listView1.Columns.Clear();
                        listView1.BeginUpdate();
                        ListViewHelper. InitListViewHeader( listView1 ,sportProjectInfos.RountCount);
                        Font font = new Font(Control.DefaultFont, FontStyle.Bold);
                        bool isBestScore = sportProjectInfos.BestScore == 0 ? true : false;
                        foreach (DbPersonInfos dbPersonInfos in personInfos)
                        {
                            ListViewItem listViewItem = new ListViewItem()
                            {
                                UseItemStyleForSubItems = false,
                                Text = step.ToString(),
                            };
                            listViewItem.SubItems.Add(dbPersonInfos.CreateTime);
                            listViewItem.SubItems.Add(dbPersonInfos.SchoolName);
                            listViewItem.SubItems.Add(dbPersonInfos.GroupName);
                            listViewItem.SubItems.Add(dbPersonInfos.Name);
                            listViewItem.SubItems.Add(dbPersonInfos.IdNumber);
                            List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dbPersonInfos.Id.ToString() && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
                            int resultRound = 0;
                            double maxScore = 1000;
                            if (isBestScore)
                                maxScore = 0;
                            bool getScore = false;
                            foreach (var result in resultInfos)
                            {
                                if (result.State != 1)
                                {
                                    string st = ResultStateType.Match(result.State);
                                    listViewItem.SubItems.Add(st);
                                    listViewItem.SubItems[listViewItem.SubItems.Count - 1].ForeColor = Color.Red;
                                }
                                else
                                {
                                    getScore = true;
                                    listViewItem.SubItems.Add(result.Result.ToString());
                                    listViewItem.SubItems[listViewItem.SubItems.Count - 1].BackColor = Color.MediumSpringGreen;
                                    if (isBestScore)
                                    {
                                        if (maxScore < result.Result)
                                        {
                                            maxScore = result.Result;
                                        }

                                    }
                                    else
                                    {
                                        if (maxScore > result.Result)
                                        {
                                            maxScore = result.Result;
                                        }
                                    }
                                }
                                listViewItem.SubItems[listViewItem.SubItems.Count - 1].Font = font;
                                if (result.uploadState == 0)
                                {
                                    listViewItem.SubItems.Add("未上传");
                                    listViewItem.SubItems[listViewItem.SubItems.Count - 1].ForeColor = Color.Red;
                                }
                                else
                                {
                                    if (result.uploadState== 1)
                                    {
                                        listViewItem.SubItems.Add("已上传");
                                        listViewItem.SubItems[listViewItem.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                                        listViewItem.SubItems[listViewItem.SubItems.Count - 1].Font = font;
                                    }

                                }
                                resultRound++;

                            }
                            for (int i = resultRound; i < sportProjectInfos.RountCount; i++)
                            {
                                listViewItem.SubItems.Add("未测试");
                                listViewItem.SubItems.Add("未上传");
                            }
                            if (getScore)
                            {
                                listViewItem.SubItems.Add(maxScore.ToString());

                            }
                            else
                            {
                                listViewItem.SubItems.Add("未测试");
                            }
                            step++;
                            listView1.Items.Insert(listView1.Items.Count, listViewItem);

                        }
                        listView1.EndUpdate();

                    }
                    else
                    {
                        UIMessageBox.ShowError("数据错误，请重试！！");
                        return;
                    }
                }
            }
            catch(Exception  ex)
            {
                LoggerHelper.Debug(ex);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShowPersonImportWindow()
        {
            return PersonImportWindowSys.Instance.ShowPersonImportWindow();
        }

        public void ShowPlatFormSettingWindow()
        {
             PlatFormSettingWindowSys.Instance.ShowPlatFormSettingWindow();
        }

        public  void ShowSystemSettingWindow()
        {
            SystemSettingWindowSys.Instance.ShowSystemSettingWindow();
        }

        public void ShowExportGradeWindow(string groupName)
        {
            ExportGradeWindowSys.Instance.ShowExportGradeWindow(groupName);
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void ShowRunningTestWindow()
        {
            RunningTestingWindowSys.Instance .ShowRunningWindow();
        }

        public List<LocalInfos> GetLocalInfos()
        {
            return freeSql.Select<LocalInfos>().ToList();
        }

        public SportProjectInfos GetSportProjectInfos()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }

        public List<DbGroupInfos> GetDBGroupInfos(string groupName)
        {
            return freeSql.Select<DbGroupInfos>().Where(a => a.Name == groupName).ToList();
        }

        public List<DbGroupInfos> GetDBGroupInfos()
        {
            return freeSql.Select<DbGroupInfos>().ToList();
        }

        public List<DbPersonInfos> GetPersonInfos(string Name)
        {
            return freeSql.Select<DbPersonInfos>().Where(A => A.GradeName == Name).ToList();
        }

        public List<ResultInfos> GetResultInfos(string stuIdNumber)
        {
            return freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == stuIdNumber).ToList();
        }

        public List<LogInfos> GetLogInfos(string stuIdNumber)
        {
            return freeSql.Select<LogInfos>().Where(a => a.IdNumber == stuIdNumber).ToList();
        }
    }
}
