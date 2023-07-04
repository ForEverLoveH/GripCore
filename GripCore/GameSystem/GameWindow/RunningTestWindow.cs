using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameSystemModel;
using GripCore.GameSystem.GameWindowSys;
using GripCore.GameSystem.MyControl;
using GripCoreModel;
using GripCoreModel.GameModel;
using HZH_Controls;
using Newtonsoft.Json;
using Sunny.UI;
using ListView = System.Windows.Forms.ListView;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace GripCore.GameSystem.GameWindow
{
    public partial class RunningTestWindow : Form
    {
        public RunningTestWindow()
        {
            InitializeComponent();
        }

        private string _portName = "CH340";
        private bool isMatchingDevice = false;
        private string _groupName = string.Empty;
        private int _nowRound = 0;
        private StringBuilder writeScoreLog = new StringBuilder();
        private string AutoMatchLog = Application.StartupPath + "\\Data\\AutoMatchLog.log";
        private string AutoUploadLog = Application.StartupPath + "\\Data\\AutoUploadLog.log";
        private string AutoPrintLog = Application.StartupPath + "\\Data\\AutoPrintLog.log";


        private SportProjectInfos sportProjectInfos = new SportProjectInfos();
        private List<SerialReader> SerialReaders = new List<SerialReader>();
        private List<String> ConnectionPort = new List<string>();
        private List<MyControl.MyControll> myControlls = new List<MyControl.MyControll>();
        private Dictionary<string, string> localInfo = new Dictionary<string, string>();
        private NFC_Helper USBWatcher = new NFC_Helper();


        private void RunningTestWindow_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            string code = "程序集版本:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            toolStripStatusLabel1.Text = code;
            sportProjectInfos = RunningTestingWindowSys.Instance.LoadingSportProjectInfo();
            if (!InitUserControll())
            {
                this.Close();
            }
            else
            {
                int round = sportProjectInfos.RountCount;
                if (round > 0)
                {
                    for (int i = 0; i < round; i++)
                    {
                        RoundCbx.Items.Add($"第{i + 1}轮");
                    }

                    RoundCbx.SelectedIndex = 0;
                }

                List<LocalInfos> localInfosList = RunningTestingWindowSys.Instance.GetLocalInfo();
                foreach (var itsm in localInfosList)
                {
                    localInfo.Add(itsm.key, itsm.value);
                }

                USBWatcher.AddUSBEventWatcher(USBEventHandler, USBEventHandler, new TimeSpan(0, 0, 1));
                RunningTestingWindowSys.Instance.InitListViewHead(sportProjectInfos.RountCount, listView1);
                RunningTestingWindowSys.Instance.UpDataGroupData(GroupCombox, _groupName);
                loadLocalData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void loadLocalData()
        {
            try
            {
                string[] strg = File.ReadAllLines(AutoMatchLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        uiCheckBox1.Checked = true;
                    }
                    else
                    {
                        uiCheckBox1.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoUploadLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        uiCheckBox2.Checked = true;
                    }
                    else
                    {
                        uiCheckBox2.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoPrintLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        uiCheckBox3.Checked = true;
                    }
                    else
                    {
                        uiCheckBox3.Checked = false;
                    }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventHandler(object sender, EventArrivedEventArgs e)
        {
            //暂未实现
            var watcher = sender as ManagementEventWatcher;
            watcher.Stop();

            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                Console.WriteLine("设备连接");
                if (isMatchingDevice)
                {
                    RefreshComPorts(); ///扫描串口
                }
            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                if (!isMatchingDevice)
                {
                    Console.WriteLine("设备断开");
                    List<string> list = CheckPortISConnected();
                    if (list.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var l in list)
                        {
                            sb.AppendLine($"{l}断开!");
                        }

                        MessageBox.Show(sb.ToString());
                    }
                    
                }
            }

            watcher.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshComPorts()
        {
            try
            {
                string[] portNames = GetPortDeviceName();
                if (portNames.Length == 0)
                {
                    MessageBox.Show($"未找到{_portName}串口,请检查驱动");
                    MatchBtnSwitch(true);
                    return;
                }

                foreach (var port in portNames)
                {
                    CheckPortISConnected();
                    //已连接则跳过
                    if (ConnectionPort.Contains(port)) continue;
                    int step = 0;
                    foreach (var sr in SerialReaders)
                    {
                        if (sr != null && !sr.IsComOpen())
                        {
                            string strException = string.Empty;
                            int nRet = sr.OpenCom(port, 9600, out strException);
                            if (nRet == 0)
                            {
                                //连接成功
                                myControlls[step].p_toolState = $"{port}已连接";
                                myControlls[step].p_toolStateColor = Color.Green;
                                myControlls[step].p_TitleColor = Color.MediumSpringGreen;
                            }
                            else
                            {
                                MessageBox.Show($"{port}连接失败\n错误:{strException},请检查");
                            }

                            break;
                        }

                        step++;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
            finally
            {
                try
                {
                    CheckPortISConnected();
                    if (ConnectionPort.Count == myControlls.Count)
                    {
                        ControlHelper.ThreadInvokerControl(this, () =>
                        {
                            MatchBtnSwitch(true);
                            string portSavePath = Application.StartupPath + "\\Data\\portSave.log";
                            File.WriteAllLines(portSavePath, ConnectionPort);
                            MessageBox.Show("设备串口匹配完成");
                        });
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<string> CheckPortISConnected()
        {
            List<string> breakPorts = new List<string>();
            ConnectionPort.Clear();
            int step = 0;
            foreach (var sr in SerialReaders)
            {
                step++;
                if (sr != null)
                {
                    try
                    {
                        if (sr.IsComOpen())
                        {
                            ConnectionPort.Add(sr.iSerialPort.PortName);
                        }
                        else
                        {
                            breakPorts.Add(sr.iSerialPort.PortName);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                    }
                }
                else
                {
                    breakPorts.Add($"设备{step}号");
                }
            }

            return breakPorts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param> 
        private void MatchBtnSwitch(bool flag)
        {
            if (flag)
            {
                uiButton3.Text = "匹配设备";
                uiButton3.BackColor = Color.White;
                isMatchingDevice = false;
            }
            else
            {
                uiButton3.Text = "匹配中";
                uiButton3.BackColor = Color.Red;
                isMatchingDevice = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private string[] GetPortDeviceName(string portName = "")
        {
            if (string.IsNullOrEmpty(portName)) portName = _portName;
            List<string> strs = new List<string>();
            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("select * from Win32_PnPEntity where Name like '%(COM%'"))
            {
                var hardInfos = searcher.Get();
                foreach (var hardInfo in hardInfos)
                {
                    if (hardInfo.Properties["Name"].Value != null)
                    {
                        string deviceName = hardInfo.Properties["Name"].Value.ToString();
                        if (deviceName.Contains(portName))
                        {
                            int a = deviceName.IndexOf("(COM") + 1; //a会等于1
                            string str = deviceName.Substring(a, deviceName.Length - a);
                            a = str.IndexOf(")"); //a会等于1
                            str = str.Substring(0, a);
                            strs.Add(str);
                        }
                    }
                }
            }

            return strs.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool InitUserControll()
        {
            try
            {
                CloseAllSerialReader();
                bool S = RunningTestingWindowSys.Instance.ShowRunningMachineSetWindow();
                if (S == false)
                {
                    return false;
                }
                else
                {
                    int machineCount = RunningTestingWindowSys.Instance.GetMachineCount();
                    _portName = RunningTestingWindowSys.Instance.GetCurrentPortName();
                    myControlls.Clear();
                    flowLayoutPanel1.Controls.Clear();
                    List<string> lis = new List<string>();
                    for (int i = 0; i < sportProjectInfos.RountCount; i++)
                    {
                        lis.Add($"第{i + 1}轮");
                    }

                    for (int i = 0; i < machineCount; i++)
                    {
                        MyControll us = new MyControll()
                        {
                            p_Title = $"第{i + 1}号设备",
                            p_RoundCountItems = lis,

                        };
                        myControlls.Add(us);
                        flowLayoutPanel1.Controls.Add(us);
                        SerialReader reader = new SerialReader(i);
                        reader.AnalyCallback = AnalyData;
                        reader.ReceiveCallback = ReceiveData;
                        reader.SendCallback = SendData;
                        SerialReaders.Add(reader);
                    }

                    return true;
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }

        }

        /// <summary>
        /// 关闭所有串口
        /// </summary>
        private void CloseAllSerialReader()
        {
            foreach (var item in SerialReaders)
            {
                if (item != null)
                {
                    item.CloseCom();
                }
            }

            SerialReaders.Clear();
            ConnectionPort.Clear();
            foreach (var it in myControlls)
            {
                it.p_toolState = "设备未连接";
                it.p_toolStateColor = Color.Red;
                it.p_toolStateColor = SystemColors.ControlLight;
            }

            GC.Collect();
        }

        /// <summary>
        /// 导入最近匹配
        /// 
        /// </summary>
        private void ImportRecentMatch()
        {
            string portSavePath = Application.StartupPath + "\\Data\\portSave.log";
            string[] portName = File.ReadAllLines(portSavePath);
            ConnectionPort.Clear();
            foreach (var port in portName)
            {
                CheckPortISConnected();
                if (!ConnectionPort.Contains(port))
                {
                    int step = 0;
                    foreach (var sr in SerialReaders)
                    {
                        if (sr != null || sr.IsComOpen())
                        {
                            string str = String.Empty;
                            int net = sr.OpenCom(port, 9600, out str);
                            if (net == 0)
                            {
                                myControlls[step].p_toolState = port + "已连接 ";
                                myControlls[step].p_toolStateColor = Color.Aquamarine;
                                myControlls[step].p_TitleColor = Color.Aqua;
                                ConnectionPort.Add(port);
                            }
                            else
                            {
                                UIMessageBox.ShowError(port + "连接失败\n错误：" + str + ".请检查");
                            }

                            step++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AutoMatchStudent()
        {
            try
            {
                ClearMatchStudent();
                List<DbPersonInfos> dbPersonInfos =
                    RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox,sportProjectInfos);
                int nlen = SerialReaders.Count;
                int step = 0;
                int i = 0;
                foreach (var dpi in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos =
                        RunningTestingWindowSys.Instance.GetResultInfo(dpi.Id.ToString(), _nowRound);
                    i++;
                    if (resultInfos.Count != 0) continue;
                    listView1.Items[i - 1].Selected = true;
                    myControlls[step].p_IdNumber = dpi.IdNumber;
                    myControlls[step].p_Name = dpi.Name;
                    myControlls[step].p_Score = "0 kg";
                    myControlls[step].p_RoundCountSelectIndex = resultInfos.Count;
                    myControlls[step].p_MStateIndex = 0;
                    step++;
                    if (step >= myControlls.Count) break;
                }

                if (step == 0)
                {
                    if (GroupCombox.SelectedIndex >= 0
                        && GroupCombox.SelectedIndex < GroupCombox.Items.Count - 2)
                    {
                        GroupCombox.SelectedIndex++;
                        RoundCbx.SelectedIndex = 0;
                        AutoMatchStudent();
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ChooseMatchStudent()
        {
            try
            {
                ClearMatchStudent();
                if (listView1.SelectedItems.Count == 0) return;
                int step = 0;
                List<DbPersonInfos> dbPersonInfos = RunningTestingWindowSys.Instance.GetDBPersonInfo(GroupCombox);
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string txt = item.SubItems[3].Text;
                    var sl =  RunningTestingWindowSys.Instance.GetResultInfo(txt);
                    if (sl.Count == 0)
                    {
                        DbPersonInfos dbPersonInf = dbPersonInfos.Find(A => A.IdNumber == txt);
                        myControlls[step].p_IdNumber = txt;
                        myControlls[step].p_Name = dbPersonInf.Name;
                        myControlls[step].p_Score = "0 kg";
                        myControlls[step].p_RoundCountSelectIndex = sl.Count;
                        myControlls[step].p_MStateIndex = 0;
                        step++;
                        if (myControlls.Count < step)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearMatchStudent()
        {
            int nlen = myControlls.Count;
            for (int i = 0; i < nlen; i++)
            {
                myControlls[i].p_IdNumber = "未分配";
                myControlls[i].p_Name = "未分配";
                myControlls[i].p_Score = "0 kg";
                myControlls[i].p_RoundCountSelectIndex = 0;
                myControlls[i].p_MStateIndex = 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpLoadCurrentGroupScore()
        { 
            HZH_Controls.ControlHelper.ThreadInvokerControl(this,()=>
            {
                uiButton9.Text = "上传中";
                uiButton9.ForeColor=Color.Red;
            });
            string group = GroupCombox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(group))
                StartUpLoadUpLoadCurrentGroupScore(group);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        private void StartUpLoadUpLoadCurrentGroupScore(string groupName)
        {
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(UpLoadUpLoadCurrentGroup);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(groupName);
        }

        private void UpLoadUpLoadCurrentGroup(object obj)
        {
            bool  sl =  UpLoadUpLoadCurrentStudentGroup(obj);
           if (sl == true)
           {
               ControlHelper.ThreadInvokerControl(this,() =>
               {
                   uiButton9.Text = "上传本组";
                   uiButton9.ForeColor=Color.Aquamarine;
                   UIMessageBox.ShowSuccess("上传成功！！");
                   RunningTestingWindowSys.Instance.UpDataGroupData(GroupCombox, _groupName);
               });
           }
           else
           {
               uiButton9.Text = "上传本组";
               uiButton9.ForeColor=Color.Aquamarine;
               UIMessageBox.ShowError("上传失败！！");
           }
        }

        private bool  UpLoadUpLoadCurrentStudentGroup(object obj)
        {
            LoadingWindow loadingWindow = new LoadingWindow();
            try
            {
                HZH_Controls.ControlHelper.ThreadInvokerControl(this, () => { loadingWindow.ShowDialog(); });
                string cpuID = CPUHelper.GetCpuID();
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInofo = new Dictionary<string, string>();
                List<LocalInfos> localInfosList = MainWindowSys.Instance.GetLocalInfos();
                foreach (var item in localInfosList)
                {
                    localInofo.Add(item.key, item.value);
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
                foreach (var gInfo in groupInfosList)
                {

                    List<DbPersonInfos> dbPersonInfosList = MainWindowSys.Instance.GetPersonInfos(gInfo.Name);
                    StringBuilder resultSB = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    bool isBestScore = sportProjectInfos.BestScore == 0;
                    foreach (var stu in dbPersonInfosList)
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
                                        if (isBestScore && maxScore < 0.0)
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
                                        if (isBestScore == false && maxScore > resInfo.Result)
                                        {
                                            maxScore = resInfo.Result;
                                            state = resInfo.State;
                                        }
                                    }
                                }

                                examTime = resInfo.CreateTime;
                            }

                            if (state > 0)
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
                                            "时间：", infos.CreateTime, ",考号", infos.IdNumber, ",", infos.Remark, ";"
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
                                    Application.StartupPath, "\\Scores\\", sportProjectInfos.Name, "\\", stu.GroupName,
                                    "\\",
                                });
                                DateTime.TryParse(examTime, out DateTime dateTime);
                                string dataStr = dateTime.ToString("yyyyMMMMdd");
                                string stss = string.Concat(new string[]
                                {
                                    dataStr, "-", stu.GroupName, "-", stu.IdNumber, "_1"
                                });
                                if (Directory.Exists(scoreRoot))
                                {
                                    List<DirectoryInfo> directoryInfos =
                                        new DirectoryInfo(scoreRoot).GetDirectories().ToList<DirectoryInfo>();
                                    string dirEndWith = "_" + stu.IdNumber + "_" + stu.Name;
                                    DirectoryInfo directoryInfo = directoryInfos.Find(a =>
                                        a.Name.EndsWith(dirEndWith)
                                    );
                                    if (directoryInfo != null)
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
                                                foreach (var file in fileInfos)
                                                {
                                                    dic_Images.Add(step.ToString() ?? "", file.Name);
                                                    step++;
                                                }
                                            }

                                            step = 1;
                                            fileInfos = new DirectoryInfo(dtuDir).GetFiles("*.txt");
                                            if (fileInfos.Length != 0)
                                            {
                                                foreach (var file in fileInfos)
                                                {
                                                    dic_txt.Add(step.ToString() ?? "", file.Name);
                                                    step++;
                                                }
                                            }

                                            step = 1;
                                            fileInfos = new DirectoryInfo(dtuDir).GetFiles("*.mp4");
                                            if (fileInfos.Length != 0)
                                            {
                                                foreach (var file in fileInfos)
                                                {
                                                    dic_Video.Add(step.ToString() ?? "", file.Name);
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
                                map.Add(stu.IdNumber, stu.Id.ToString());
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
                            Key = "data", Value = jsonStr,
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
                                        string errorStr = uploadResult.Match(value);
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

                            WriteLoggerHelper.Instance.WriteDefaultLog(errorList, gInfo);
                        }

                    }
                }

                WriteLoggerHelper.Instance.WriteSucessLog(successList);
                return true;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return false;
            }
            finally
            {
                HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
                {
                    loadingWindow.Close();
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveData(byte[] data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btArySendData"></param>
        private void SendData(MachineMsgCode btArySendData)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void AnalyData(SerialMsg data)
        {
            if (data != null)
            {
                switch (data.mms.type)
                {
                    case 1:
                        string temp = myControlls[data.number].p_toolState;
                        try
                        {
                            string sll = ConnectionPort[data.number];
                            myControlls[data.number].p_toolState = sll + "设备连接成功！";
                        }
                        catch (Exception ex)
                        {
                            myControlls[data.number].p_toolState = temp;
                            LoggerHelper.Debug(ex);
                        }

                        break;
                    case 2:
                        bool sl = data.mms.code == 1;
                        if (sl)
                        {
                            Console.WriteLine("测试开始");
                        }

                        break;
                    case 3:
                        try
                        {
                            double s = data.mms.wl_result;
                            int index = data.number;
                            if (s > 0.0)
                            {
                                string idNum = myControlls[index].p_IdNumber;
                                string stuName = myControlls[index].p_Name;
                                writeScoreLog.AppendLine(string.Concat(new string[]
                                {
                                    "考号:",
                                    idNum,
                                    ",姓名:",
                                    stuName,
                                    ",成绩:",
                                    s.ToString(),
                                }));

                            }

                            myControlls[index].p_Score = s.ToString()+"kg";
                            if (myControlls[index].p_MStateIndex == 0)
                            {
                                myControlls[index].p_MStateIndex = 1;
                            }

                            SerialReaders[index].SendMessage(data.mms);
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Debug(ex);
                        }

                        break;
                }
            }
        }

        #region 页面事件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            InitUserControll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            CloseAllSerialReader();
            int len = myControlls.Count;
            for (int i = 0; i < len; i++)
            {
                //初始化访问读写器实例
                SerialReader reader = new SerialReader(i);
                //回调函数
                reader.AnalyCallback = AnalyData;
                reader.ReceiveCallback = ReceiveData;
                reader.SendCallback = SendData;
                SerialReaders.Add(reader);
            }

            //导入最近匹配
            ImportRecentMatch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            CloseAllSerialReader();
            int nlen = myControlls.Count;
            for (int i = 0; i < nlen; i++)
            {
                //初始化访问读写器实例
                SerialReader reader = new SerialReader(i);
                //回调函数
                reader.AnalyCallback = AnalyData;
                reader.ReceiveCallback = ReceiveData;
                reader.SendCallback = SendData;
                SerialReaders.Add(reader);
            }

            //匹配设备
            MatchBtnSwitch(isMatchingDevice);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox,sportProjectInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.RefreshGetGroup(GroupCombox);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoundCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RoundCbx.SelectedIndex >= 0)
            {
                _nowRound = RoundCbx.SelectedIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton5_Click(object sender, EventArgs e)
        {
            AutoMatchStudent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton6_Click(object sender, EventArgs e)
        {
            ChooseMatchStudent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void uiButton7_Click(object sender, EventArgs e)
        {
            List<string> lis = CheckPortISConnected();
            if (lis.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in lis)
                {
                    stringBuilder.AppendLine($"{item}断开");
                }
                UIMessageBox.ShowError(stringBuilder.ToString());
            }
            else
            {
                MachineMsgCode code = new MachineMsgCode()
                {
                    type = 2,
                };
                int step = 0;
                foreach (var items in SerialReaders)
                {
                    if (!string.IsNullOrWhiteSpace(myControlls[step].p_IdNumber) ||
                        myControlls[step].p_IdNumber != "未分配")
                    {
                        if (items != null && items.IsComOpen())
                        {
                            items.SendMessage(code);
                        }
                        step++;
                    }
                }
            }
        }
        /// <summary>
        /// 保存成绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton8_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance. WriteScoreIntoDb(listView1,sportProjectInfos,GroupCombox,myControlls);
            if (uiCheckBox1.Checked)
            {
                AutoMatchStudent();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton9_Click(object sender, EventArgs e)
        {
            UpLoadCurrentGroupScore();
        }
        private void uiButton10_Click(object sender, EventArgs e)
        {
            string groupName = GroupCombox.Text;
            new Thread((ThreadStart)delegate
            {
                RunningTestingWindowSys.Instance  .PrintCurrentScore(groupName,sportProjectInfos);
            }).Start();
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListView lv1 = (ListView)sender;
            ListViewItem lvi1 = lv1.GetItemAt(e.X, e.Y);
            if (lvi1 != null && e.Button == MouseButtons.Right)
            {
                this.cmsListViewItem.Show(lv1, e.X, e.Y);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (writeScoreLog.Length > 0)
            {
                File.AppendAllText("成绩日志.txt",writeScoreLog.ToString());
                writeScoreLog.Clear();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 缺考ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1,"缺考",_nowRound,GroupCombox,sportProjectInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 中退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1,"中退",_nowRound,GroupCombox,sportProjectInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 犯规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1," 犯规",_nowRound,GroupCombox,sportProjectInfos);
        }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void 弃权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1,"弃权",_nowRound,GroupCombox,sportProjectInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修正成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    return;
                }
                else
                {
                    string idNumber = listView1.SelectedItems[0].SubItems[3].Text;
                    
                    FromRoundWindow fcr = new FromRoundWindow();
                    fcr._idnumber = idNumber;
                    fcr.mode = 1;
                    fcr.ShowDialog(); 
                    RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox,sportProjectInfos);
                    
                }

            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 成绩重测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    return;
                }
                else
                {
                    string idNumber = listView1.SelectedItems[0].SubItems[3].Text;

                    FromRoundWindow fcr = new FromRoundWindow();
                    fcr._idnumber = idNumber;
                    fcr.mode = 0;
                    fcr.ShowDialog();
                    RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox,sportProjectInfos);
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return;
            }
        }
        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            int value = uiCheckBox1.Checked ? 1 : 0;
            File.WriteAllText(AutoMatchLog,value.ToString());

        }

        private void uiCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            int value = uiCheckBox2.Checked ? 1 : 0;
            File.WriteAllText(AutoUploadLog,value.ToString());
        }

        private void uiCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            int value = uiCheckBox2.Checked ? 1 : 0;
            File.WriteAllText(AutoPrintLog,value.ToString());
        }
        
        #endregion


       
    }
}
