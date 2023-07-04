using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameSystemModel;
using GripCore.GameSystem.GameWindow;
using GripCoreModel;
using GripCoreModel.GameModel;
using HZH_Controls;
using MiniExcelLibs;
using Newtonsoft.Json;
using Sunny.UI;

namespace GripCore.GameSystem.GameWindowSys
{
    public  class PersonImportWindowSys
    {
        public static PersonImportWindowSys Instance;
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;
        private PersonImportWindow PersonImportWindow = null;
        public void Awake()
        {
            Instance = this;
        }

        public void InitListViewHeader(ListView listView1)
        {
            ListViewHelper.InitListViewHeader(listView1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="listView1"></param>
        /// <param name="localValues"></param>
        /// <param name="uiLabel5"></param>
        public bool LoadingStudentDataFormServer(string groupNums, ListView listView1,
            ref Dictionary<string, string> localValues, UILabel uiLabel5)
        {
           LoadingWindow loadingWindow = new LoadingWindow();
            new Thread((ThreadStart)delegate()
            {
                loadingWindow.Show();
            }).Start();
            try
            {
                List<LocalInfos> localInfos = this.freeSql.Select<LocalInfos>().ToList();
                localValues = new Dictionary<string, string>();
                foreach (LocalInfos li in localInfos)
                {
                    localValues.Add(li.key, li.value);
                }
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName =  localValues["TestManUserName"];
                RequestParameter.TestManPassword =  localValues["TestManPassword"];
                string ExamId0 = localValues["ExamId"];
                ExamId0 = ExamId0.Substring(ExamId0.IndexOf('_') + 1);
                string MachineCode0 =  localValues["MachineCode"];
                MachineCode0 = MachineCode0.Substring(MachineCode0.IndexOf('_') + 1);
                RequestParameter.ExamId = ExamId0;
                RequestParameter.MachineCode = MachineCode0;
                RequestParameter.GroupNums = (groupNums ?? "");
                string JsonStr = JsonConvert.SerializeObject(RequestParameter);
                string url =  localValues["Platform"] + RequestUrl.GetGroupStudentUrl;
                List<FormItemModel> formDatas = new List<FormItemModel>();
                formDatas.Add(new FormItemModel
                {
                    Key = "data",
                    Value = JsonStr
                });
                HttpUpLoad httpUpload = new HttpUpLoad() ;
                string result = string.Empty;
                try
                {
                    result = HttpUpLoad.PostForm(url, formDatas, null, null, null, 20000);
                }
                catch (Exception ex)
                { 
                    LoggerHelper.Debug(ex);
                   UIMessageBox.ShowError( "请检查网络");
                }
                string[] strs = GetGroupStudent.CheckJson(result);
                GetGroupStudent upload_Result = null;
                bool flag = strs[0] == "1";
                if (flag)
                {
                    upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                }
                else
                {
                    upload_Result = new GetGroupStudent();
                    upload_Result.Error = strs[1];
                }
                bool bFlag = false;
                bool flag2 = upload_Result == null || upload_Result.Results == null || upload_Result.Results.groups.Count == 0;
                if (flag2)
                {
                    string error = string.Empty;
                    try
                    {
                        error = upload_Result.Error;
                    }
                    catch (Exception)
                    {
                        error = string.Empty;
                    }
                    UIMessageBox.ShowError( "提交错误,错误码:[" + error + "]");
                }
                else
                {
                    bFlag = true;
                }
                if ( bFlag)
                {
                    DownLoadingStudentExcel(listView1,upload_Result,uiLabel5);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            finally
            {
                loadingWindow.Invoke((EventHandler)delegate { loadingWindow.Close(); });
                loadingWindow.Dispose();
            }
        }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="listView1"></param>
            /// <param name="groupStudent"></param>
            /// <param name="uiLabel5"></param>
            private void DownLoadingStudentExcel(ListView listView1, GetGroupStudent groupStudent, UILabel uiLabel5)
            {
                List<GroupItems> groupItems =groupStudent.Results.groups;
                List<InputData> datas = new List<InputData>();
                int step = 0;
                listView1.BeginUpdate();
                listView1.Items.Clear();
                foreach (var item in groupItems)
                {
                    string groupID = item.GroupID;
                    string groupName = item.GroupName;
                    foreach (var  studentInfo in item.StudentInfos)
                    {
                        InputData data = new InputData();
                        data.ID = step;
                        data.ExamDate = studentInfo.ExamTime;
                        data.SchoolName = studentInfo.SchoolName;
                        data.GroupName = studentInfo.GradeName;
                        data.ClassName = studentInfo.ClassName;
                        data.Name = studentInfo.Name;
                        data.Sex = studentInfo.Sex;
                        data.IDNumber = studentInfo.IDNumber;
                        data.GroupName = groupID;
                        ListViewItem listViewItem = new ListViewItem()
                        {
                            Text = step.ToString(),
                        };
                        listViewItem.SubItems.Add(studentInfo.ExamTime);
                        listViewItem.SubItems.Add(studentInfo.SchoolName);
                        listViewItem.SubItems.Add(studentInfo.GradeName);
                        listViewItem.SubItems.Add(studentInfo.ClassName);
                        listViewItem.SubItems.Add(studentInfo.Name);
                        listViewItem.SubItems.Add(studentInfo.Sex);
                        listViewItem.SubItems.Add(groupID);
                        listView1.Items.Insert(listView1.Items.Count, listViewItem);
                        datas.Add(data);
                        step += 1;
                    }
                
                }
                listView1.EndUpdate();
                string path = Path.Combine(Application.StartupPath, "\\模板\\下载名单\\");
                if (!System.IO.Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, $"downList{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                ExcelUtils.OutPutExcel(path,datas);
                uiLabel5.Text = string.Format("下载成功，共{0}人", step - 1);
                uiLabel5.ForeColor=Color.Red;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool ShowPersonImportWindow()
            {
                PersonImportWindow = new PersonImportWindow();
                var sl =  PersonImportWindow.ShowDialog();
                if (sl == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void OpenLocalExcelFile(UITextBox uiTextBox1)
            {
                string path = string.Empty;
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;      //该值确定是否可以选择多个文件
                dialog.Title = "请选择文件";     //弹窗的标题
                dialog.InitialDirectory = Application.StartupPath + "\\";    //默认打开的文件夹的位置
                dialog.Filter = "MicroSoft Excel文件(*.xlsx)|*.xlsx";       //筛选文件
                dialog.ShowHelp = true;     //是否显示“帮助”按钮
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = dialog.FileName;
                }
                if (!String.IsNullOrEmpty(path))
                {
                    uiTextBox1.Text = path;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool SaveTemplateExcel()
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "xls files(*.xls)|*.xls|xlsx file(*.xlsx)|*.xlsx|All files(*.*)|*.*";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.FileName = $"导入模板{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    string path = Application.StartupPath + "\\excel\\output.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        path = saveFileDialog.FileName;
                        File.Copy(@"./模板/导入名单模板.xlsx", path);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception exception)
                {
                    LoggerHelper .Debug(exception);
                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="isDeleteBeforeImport"></param>
            /// <param name="uiLabel5"></param>
            /// <returns></returns>
            public bool ExcelInputDataBase(object obj, bool isDeleteBeforeImport, UILabel uiLabel5)
            {
                  LoadingWindow loading = new LoadingWindow();
                new Thread((ThreadStart)delegate ()
                {
                    loading.Show();
                }).Start();
                bool isRes = false;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    if (isDeleteBeforeImport)
                    {
                        string[] datas = new string[] { "DbGroupInfos", "DbPersonInfos", "ResultInfos", "LogInfos" };
                        int result1 = freeSql.Delete<DbGroupInfos>().Where("1=1").ExecuteAffrows();
                        int result2 = freeSql.Delete<DbPersonInfos>().Where("1=1").ExecuteAffrows();
                        int result3 = freeSql.Delete<ResultInfos>().Where("1=1").ExecuteAffrows();
                        int result4 = freeSql.Update<LogInfos>().Set(a => a.State == -404).Where("1=1").ExecuteAffrows();
                    }
                    string path = obj as string;
                    if (!String.IsNullOrEmpty(path))
                    {
                        SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                        var rows = MiniExcel.Query<InputData>(path).ToList();
                        HashSet<string> set = new HashSet<string>();
                        for (int i = 0; i < rows.Count; i++)
                        {
                            string[] examTime = rows[i].ExamDate.Split(' ');
                            set.Add(rows[i].Grade + "#" + examTime[0]);
                        }
                        List<string> rolesList = new List<string>();
                        rolesList.AddRange(set);
                        freeSql.Select<DbGroupInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                        List<DbGroupInfos> insertDbGroupInfosList = new List<DbGroupInfos>();
                        for (int i = 0; i < rolesList.Count; i++)
                        {
                            maxSortId++;
                            string role = rolesList[i];
                            string[] roles = role.Split("#");
                            string groupName = roles[0];
                            string examTime = roles[1];
                            DbGroupInfos dbGroupInfos = new DbGroupInfos();
                            dbGroupInfos.Name = groupName;
                            dbGroupInfos.CreateTime = examTime;
                            dbGroupInfos.SortId = maxSortId;
                            dbGroupInfos.IsRemoved = 0;
                            dbGroupInfos.ProjectId = 0.ToString();
                            dbGroupInfos.IsAllTested = 0;
                            insertDbGroupInfosList.Add(dbGroupInfos);
                        }
                        int sy = freeSql.InsertOrUpdate<DbGroupInfos>().SetSource(insertDbGroupInfosList).IfExistsDoNothing().ExecuteAffrows();
                        freeSql.Select<DbPersonInfos>().Aggregate(x => x.Max(x.Key.SortId), out maxSortId);
                        List<DbPersonInfos> personInfos = new List<DbPersonInfos>();
                        foreach (var row in rows)
                        {
                            maxSortId++;
                            string personID = row.IDNumber.ToString();
                            string name = row.Name.ToString();
                            int sex = row.Sex == "男" ? 0 : 1;
                            string SchoolName = row.SchoolName;
                            string GradeName = row.Grade;
                            string classNumber = row.ClassName;
                            string GroupName = row.GroupName;
                            string[] examTimes = row.ExamDate.Split(' ');
                            string examTime = examTimes[0];
                            DbPersonInfos dbPersonInfos = new DbPersonInfos();
                            dbPersonInfos.CreateTime = examTime;
                            dbPersonInfos.SortId = maxSortId;
                            dbPersonInfos.ProjectId = "0";
                            dbPersonInfos.SchoolName = SchoolName;
                            dbPersonInfos.GradeName = GradeName;
                            dbPersonInfos.ClassNumber = classNumber;
                            dbPersonInfos.GroupName = GroupName;
                            dbPersonInfos.Name = name;
                            dbPersonInfos.IdNumber = personID;
                            dbPersonInfos.Sex = sex;
                            dbPersonInfos.State = 0;
                            dbPersonInfos.FinalScore = -1;
                            dbPersonInfos.uploadState = 0;
                            personInfos.Add(dbPersonInfos);
                        }
                        int reslut = freeSql.InsertOrUpdate<DbPersonInfos>().SetSource(personInfos).IfExistsDoNothing().ExecuteAffrows();
                        if (reslut == 0) { isRes = false; }
                        else { isRes = true; }
                        sw.Stop();
                        string time = (sw.ElapsedMilliseconds / 1000).ToString("0.000") + "秒";
                        uiLabel5.Text = $"耗时：{time},实际插入:{reslut},重复：{rows.Count - reslut}";
                    }
                    if (isRes)

                        return true;
                    else
                        return false; 
                }
                catch (Exception ex)
                {
                    LoggerHelper .Debug (ex);
                    return false;
                }
                finally
                {
                    loading.Invoke((EventHandler)delegate { loading.Close(); });
                    loading.Dispose();
                }
            }
    }
}
