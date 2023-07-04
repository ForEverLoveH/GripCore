using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameWindowSys;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GripCore.GameSystem.GameSystemModel;
using GripCoreModel;
using GripCoreModel.GameModel;
using HZH_Controls;
using HZH_Controls.Forms;
using Newtonsoft.Json;

namespace GripCore.GameSystem.GameWindow
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string createTime = string.Empty;
        private string schoolName = string.Empty;
        private string groupName = string.Empty;
        private void MainWindow_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string code = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = "德育龙体育测试系统" + code;
            MainWindowSys.Instance.UpdataTreeview(treeView1);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            string txt = e.Node.Text;
            string fullpath = e.Node.FullPath;
            string[] paths = fullpath.Split('\\');
            if (e.Node.Level == 0)
            {
                createTime = paths[0];
            }
            else if (e.Node.Level == 1)
            {
                createTime = paths[0];
                schoolName = paths[1];

            }
            else if (e.Node.Level == 2)
            {
                createTime = paths[0];
                schoolName = paths[1];
                groupName = paths[2];
            }
            MainWindowSys.Instance.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
        }

        private void ucNavigationMenu1_ClickItemed(object sender, EventArgs e)
        {
            var sl = ucNavigationMenu1.SelectItem.Text;
            if(string.IsNullOrEmpty(sl))
            {
                UIMessageBox.ShowError("请先选择数据！！");
                return;
            }
            else
            {
                switch (sl)
                {
                    case "导入名单":
                        if (MainWindowSys.Instance.ShowPersonImportWindow())
                        {
                            MainWindowSys.Instance.UpdataTreeview(treeView1);
                        }
                        break;
                    case "系统参数设置":
                        MainWindowSys.Instance.ShowSystemSettingWindow();
                        break;
                    case "数据库的初始化":
                        break;
                    case "平台设置":
                        MainWindowSys.Instance.ShowPlatFormSettingWindow();
                        break;
                    case"上传成绩":
                        Thread newThread = new Thread(new ParameterizedThreadStart((o) =>
                        {
                            UpLoadingGradeToServer();
                        }));
                        newThread.IsBackground = true;
                        newThread.Start();
                        break;
                    case "导出成绩":
                         MainWindowSys.Instance.ShowExportGradeWindow(groupName);
                        break;
                    case "启动测试":
                        MainWindowSys.Instance.ShowRunningTestWindow();
                        break;
                        
                }
            }
        }
       private int proMax = 0;
       private int proVal= 0;
        private void UpLoadingGradeToServer()
        {
            if (treeView1.SelectedNode != null)
            {
                String path = treeView1.SelectedNode.FullPath;
                string[] fsp = path.Split('\\');
                string projectName = string.Empty;
                string schoolName = string.Empty;
                string groupName    = string.Empty;
                if (fsp.Length > 0&&fsp.Length==3)
                {
                    projectName = fsp[0];
                    schoolName = fsp[1];
                    groupName = fsp[2];  
                }
                if (string.IsNullOrEmpty(projectName)&&string.IsNullOrEmpty(schoolName)&&string.IsNullOrEmpty(groupName))
                {
                    FrmTips.ShowTipsError(this, "请先选择上传的成绩项目！！");
                    return;
                }

                string outMes = UpLoadingGradeDataToServer(fsp );
                var str = outMes.Trim();
                if (string.IsNullOrEmpty(outMes))
                {
                    MessageBox.Show("上传成功");
                    
                    HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
                    {
                        
                        MainWindowSys.Instance.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
                    });
                    
                }
                else
                {
                    MessageBox.Show(outMes);
                }

                if (!string.IsNullOrEmpty(projectName))
                {
                    ControlHelper.ThreadInvokerControl(this,
                        () =>
                        {
                            MainWindowSys.Instance.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
                        });
                }
            }
            else
            {
                UIMessageBox.ShowError("请先选择项目数据！！");
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string UpLoadingGradeDataToServer(Object obj)
        {
            string res = string.Empty;
            try
            {
                string cpuID = CPUHelper.GetCpuID();
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInofo = new Dictionary<string, string>();
                List<LocalInfos> localInfosList = MainWindowSys.Instance.GetLocalInfos();
                foreach (var  item in localInfosList)
                {
                    localInofo.Add(item.key,item.value);
                }

                string groupName = obj as string;
                SportProjectInfos sportProjectInfos = MainWindowSys.Instance.GetSportProjectInfos();
                List<DbGroupInfos> groupInfosList = new List<DbGroupInfos>();
                if (!string.IsNullOrEmpty(groupName))
                    groupInfosList = MainWindowSys.Instance.GetDBGroupInfos(groupName);
                else
                    groupInfosList = MainWindowSys.Instance.GetDBGroupInfos();
                UploadResultsRequestParameter urrp = new UploadResultsRequestParameter()
                {
                    AdminUserName = localInofo["AdminUserName"],
                    TestManUserName = localInofo["TestManUserName"],
                    TestManPassword = localInofo["TestManPassword"],
                };
                string machineCode = localInofo["MachineCode"];
                string examID = localInofo["ExamId"];
                if (machineCode.IndexOf("_") != -1)
                    machineCode = machineCode.Substring(machineCode.IndexOf("_") + 1);
                if (examID.IndexOf("_") != -1)
                    examID = examID.Substring(examID.IndexOf("_") + 1);
                urrp.MachineCode = machineCode;
                urrp.ExamId = examID;
                StringBuilder messageSb = new StringBuilder();
                StringBuilder logWrite = new StringBuilder();
                proMax = groupInfosList.Count;
                proVal = 0;
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    ucProcessLine1.Visible = true;
                    ucProcessLine1.Value = 0;
                    timer1.Start();
                });
                foreach (var  gInfo in groupInfosList)
                {
                    proVal++;
                    List<DbPersonInfos> dbPersonInfosList = MainWindowSys.Instance.GetPersonInfos(gInfo.Name);
                    StringBuilder resultSB = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    bool isBestScore = sportProjectInfos.BestScore == 0;
                    foreach (var  stu in dbPersonInfosList)
                    {
                        List<ResultInfos> resultInfosList = MainWindowSys.Instance.GetResultInfos(stu.IdNumber);
                        if (resultInfosList.Count != 0)
                        {
                            int state = -1;
                            string examTime = string.Empty;
                            double maxScore = 999;
                            if (isBestScore)
                            {
                                maxScore = 0.0;
                            }
                            foreach (var resInfo in resultInfosList)
                            {
                                if (resInfo.State > 0)
                                {
                                    if (resInfo.State != 1)
                                    {
                                        if (isBestScore&& maxScore<0.0)
                                        {
                                            maxScore = 0.0;
                                            state = resInfo.State;
                                        }
                                        else
                                        {
                                            if (!isBestScore && maxScore > 9999)
                                            {
                                                maxScore = 999;
                                                state = resInfo.State;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (isBestScore && maxScore < resInfo.Result)
                                    {
                                        maxScore = resInfo.Result;
                                        state = resInfo.State;
                                    }
                                    else
                                    {
                                        if (isBestScore==false && maxScore>resInfo.Result)
                                        {
                                            maxScore = resInfo.Result;
                                            state = resInfo.State;
                                        }
                                    }
                                }
                                examTime = resInfo.CreateTime;
                            }
                            if (state>0)
                            {
                                if (state != 1)
                                    maxScore = 0.0;
                                List<RoundsItem> roundsItems = new List<RoundsItem>();
                                RoundsItem roundsItem = new RoundsItem()
                                {
                                    RoundId = 1,
                                    State = ResultStateType.Match(state),
                                    Time = examTime,
                                };
                                StringBuilder logsb = new StringBuilder();
                                try
                                {
                                    List<LogInfos> logInfosList = MainWindowSys.Instance.GetLogInfos(stu.IdNumber);
                                    logInfosList.ForEach(delegate(LogInfos infos)
                                    {
                                        string sts = string.Concat(new string[]
                                        {
                                            "时间：",infos.CreateTime,",考号",infos.IdNumber,"," ,infos.Remark,";"
                                        });
                                        logsb.Append(sts);
                                    });
                                }
                                catch (Exception e)
                                {
                                    LoggerHelper.Debug(e);
                                }
                                roundsItem.Memo = logsb.ToString();
                                roundsItem.Ip = cpuID;
                                roundsItem.Result = maxScore;
                                Dictionary<string, string> dic_Images = new Dictionary<string, string>();
                                Dictionary<string, string> dic_Video = new Dictionary<string, string>();
                                Dictionary<string, string> dic_txt = new Dictionary<string, string>();
                                string scoreRoot = string.Concat(new string[]
                                {
                                     Application.StartupPath,"\\Scores\\",sportProjectInfos.Name,"\\" ,stu.GroupName,"\\",
                                });
                                DateTime.TryParse(examTime, out DateTime dateTime);
                                string dataStr = dateTime.ToString("yyyyMMMMdd");
                                string stss = string.Concat(new string[]
                                {
                                     dataStr,"-",stu.GroupName,"-",stu.IdNumber,"_1"
                                });
                                if (Directory.Exists(scoreRoot))
                                {
                                    List<DirectoryInfo> directoryInfos =
                                        new DirectoryInfo(scoreRoot).GetDirectories().ToList<DirectoryInfo>();
                                    string dirEndWith = "_" + stu.IdNumber + "_" + stu.Name;
                                    DirectoryInfo directoryInfo = directoryInfos.Find(a =>
                                        a.Name.EndsWith(dirEndWith)
                                    );
                                    if (directoryInfo !=null)
                                    {
                                        string dtuDir = Path.Combine(scoreRoot, directoryInfo.Name);
                                        stss = string.Concat(new string[]
                                        {
                                            dataStr, "_", stu.GroupName, "_", directoryInfo.Name, "_1"

                                        });
                                        if (Directory.Exists(dtuDir))
                                        {
                                            int step = 1;
                                            FileInfo[] fileInfos = new DirectoryInfo(dtuDir).GetFiles("*.jpg");
                                            if (fileInfos.Length != 0)
                                            {
                                                foreach (var  file in fileInfos)
                                                {
                                                    dic_Images.Add(step.ToString()?? "",file.Name );
                                                    step++;
                                                }
                                            }

                                            step = 1;
                                            fileInfos = new DirectoryInfo(dtuDir).GetFiles("*.txt");
                                            if (fileInfos.Length != 0)
                                            {
                                                foreach (var  file in fileInfos)
                                                {
                                                    dic_txt.Add(step.ToString()?? "",file.Name );
                                                    step++;
                                                }
                                            }
                                            step = 1;
                                            fileInfos = new DirectoryInfo(dtuDir).GetFiles("*.mp4");
                                            if (fileInfos.Length != 0)
                                            {
                                                foreach (var  file in fileInfos)
                                                {
                                                    dic_Video.Add(step.ToString()?? "",file.Name );
                                                    step++;
                                                }
                                            }
                                        }
                                    }   
                                }
                                roundsItem.GroupNo = stss;
                                roundsItem.Text = dic_txt;
                                roundsItem.Images = dic_Images;
                                roundsItem.Videos = dic_Video;
                                roundsItems.Add(roundsItem);
                                sudentsItems.Add(new SudentsItem()
                                {
                                    SchoolName = stu.SchoolName,
                                    GradeName = stu.GradeName,
                                    ClassNumber = stu.ClassNumber,
                                    Name = stu.Name,
                                    IdNumber = stu.IdNumber,
                                    Rounds = roundsItems
                                });
                                map.Add(stu.IdNumber,stu.Id.ToString());
                            }
                        }
                        
                    }

                    if (sudentsItems.Count > 0)
                    {
                        urrp.Sudents = sudentsItems;
                        string jsonStr = JsonConvert.SerializeObject(urrp);
                        string url = localInofo["Platform"] + RequestUrl.UpLoadResults;
                        List<FormItemModel> formItemModels = new List<FormItemModel>();
                        formItemModels.Add(new FormItemModel()
                        {
                            Key ="data",Value = jsonStr,
                        });
                        logWrite.AppendLine();
                        logWrite.AppendLine();
                        logWrite.AppendLine(jsonStr);
                        string result = HttpUpLoad.PostForm(url, formItemModels, null, null, null, 20000);
                        if (!string.IsNullOrEmpty(result))
                        {
                            var upLoadeResult = JsonConvert.DeserializeObject<Upload_Result>(result);
                            var resltList = upLoadeResult.Result;
                            foreach (var items in sudentsItems)
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                
                                dic.Add("Id", map[items.IdNumber]);
                                dic.Add("IdNumber", items.IdNumber);
                                dic.Add("Name", items.Name);
                                dic.Add("uploadGroup", items.Rounds[0].GroupNo);
                                int value = 0;
                                resltList.Find(a => a.TryGetValue(items.IdNumber, out value));
                                if (value == 1 || value == -4)
                                {
                                    successList.Add(dic);
                                    messageSb.AppendLine(string.Concat(new string[]
                                    {
                                        gInfo.Name,
                                        "组 考号",
                                        items.IdNumber,
                                        "姓名",
                                        items.Name,
                                        "上传失败，"
                                    }));
                                    
                                }
                                else
                                {
                                    if (value != 0)
                                    {
                                         string  errorStr = uploadResult.Match(value);
                                        dic.Add("error", errorStr);
                                        errorList.Add(dic);
                                        messageSb.AppendLine(string.Concat(new string[]
                                        {
                                            $"{gInfo.Name}组 考号:{items.IdNumber} 姓名:{items.Name}上传失败,错误内容:{errorStr}"
                                        }));
                                    }
                                }

                            }
                            string txtPath = Application.StartupPath + "\\Log\\upload\\";
                            if (!Directory.Exists(txtPath))
                            {
                                Directory.CreateDirectory(txtPath);
                            }
                            WriteLoggerHelper.Instance.WriteDefaultLog(errorList,gInfo);
                        }

                    }
                }
                WriteLoggerHelper.Instance.WriteSucessLog(successList);
                res = messageSb.ToString();
                return res;
            }
            catch (Exception exception)
            {
                LoggerHelper .Debug(exception);
                return null;
            }
        }
    }
}
