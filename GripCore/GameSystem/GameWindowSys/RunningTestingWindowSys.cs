using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameWindow;
using GripCoreModel.GameModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GripCore.GameSystem.GameSystemModel;
using GripCore.GameSystem.MyControl;
using GripCoreModel;
using MiniExcelLibs;
using Newtonsoft.Json;

namespace GripCore.GameSystem.GameWindowSys
{
    public class RunningTestingWindowSys
    {
        public static RunningTestingWindowSys Instance;
        private RunningTestWindow RunningTestWindow = null;
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;
        private WriteLoggerHelper WriteLoggerHelper = new WriteLoggerHelper();
        

        public void Awake()
        {
            Instance = this;
        }
        /// <summary>
        /// 
        /// </summary>

        public void ShowRunningWindow()
        {
            RunningTestWindow = new RunningTestWindow();
            RunningTestWindow.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SportProjectInfos LoadingSportProjectInfo()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShowRunningMachineSetWindow()
        {
            return RunningMachineSettingWindowSys.Instance.ShowRunningMachineSettingWindow();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMachineCount()
        {
            return RunningMachineSettingWindowSys.Instance.GetMachineCount();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  string GetCurrentPortName()
        {
            return RunningMachineSettingWindowSys.Instance.GetPortName();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<LocalInfos> GetLocalInfo()
        {
            return freeSql.Select<LocalInfos>().ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundCount"></param>
        /// <param name="listView1"></param>
        public void InitListViewHead(int roundCount, ListView listView1)
        {
            listView1. Columns.Clear();
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 80;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组号";
            Header[sp].Width = 100;
            sp++; 
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 160;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 130;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "最好成绩";
            Header[sp].Width = 120;
            sp++;
            for(int i =1;i<=roundCount;i++)
            {
                Header[sp] = new ColumnHeader();
                Header[sp].Text = $"第{i}轮";
                Header[sp].Width = 120;
                sp++;
                Header[sp] = new ColumnHeader();
                Header[sp].Text = "上传状态";
                Header[sp].Width = 120;
                sp++;

            }
            ColumnHeader[] Headers = new ColumnHeader[sp];
            for(int I = 0; I < Headers.Length; I++)
            {
                Headers[I] = Header[I];
            }
            listView1.Columns.AddRange(Headers);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCombox"></param>
        /// <param name="groupName"></param>

        public  void UpDataGroupData(ComboBox groupCombox, string groupname)
        {
            try
            {
                List<string> list = freeSql.Select<DbGroupInfos>().Distinct().ToList(a => a.Name);
                groupCombox.Items.Clear();
                AutoCompleteStringCollection lstsourece = new AutoCompleteStringCollection();
                foreach (var item in list)
                {
                    groupCombox.Items.Add(item);
                    lstsourece.Add(item);
                }
                groupCombox.AutoCompleteCustomSource = lstsourece;
                if (!string.IsNullOrEmpty(groupname))
                {
                    int index = groupCombox.Items.IndexOf(groupname);
                    if (index >= 0)
                    {
                        groupCombox.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                groupCombox.Items.Clear();
                groupCombox.AutoCompleteCustomSource = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="groupCombox"></param>
        public List<DbPersonInfos> UpDateListView(ListView listView1, ComboBox groupCombox,SportProjectInfos sportProjectInfos)
        {
            List<DbPersonInfos> dbPersonInfos  = new List<DbPersonInfos>();
            try
            {
                listView1.Items.Clear();
                int index = groupCombox.SelectedIndex;
                string groupName = groupCombox.Text;
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                if (dbPersonInfos.Count == 0) return dbPersonInfos;
                int step = 1;
                listView1.BeginUpdate();
                Font font = new Font(Control.DefaultFont, FontStyle.Bold);
                bool isBestScore= false;
                foreach (var dbperson in dbPersonInfos)
                {
                    ListViewItem li = new ListViewItem();
                    li.UseItemStyleForSubItems = false;
                    li.Text = step.ToString();
                    li.SubItems.Add(dbperson.SchoolName);
                    li.SubItems.Add(dbperson.GradeName);
                    li.SubItems.Add(dbperson.IdNumber);
                    li.SubItems.Add(dbperson.Name);
                    li.SubItems.Add("未测试");
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dbperson.Id.ToString() && a.IsRemoved == 0).OrderBy(a => a.SportItemType).ToList();
                    int resultRound = 0;
                    double max = 999.0;
                    if(isBestScore)
                    {
                        max = 0.0;
                    }
                    bool isgetScore=false;
                    foreach (var resultInfo in resultInfos)
                    {
                        if (resultInfo.State != 1)
                        {
                            string st = ResultStateType.Match(resultInfo.State);
                            li.SubItems.Add(st);
                            li.SubItems[li.SubItems.Count-1]. ForeColor= Color.Red  ;
                        }
                        else
                        {
                           isgetScore= true;
                           li.SubItems.Add(resultInfo.Result.ToString());
                            li.SubItems[li.SubItems.Count-1].BackColor= Color.MediumSeaGreen ;
                            if (isBestScore)
                            {
                                if(max<resultInfo.Result)
                                {
                                    max= resultInfo.Result;
                                }
                            }
                            else
                            {
                                if(max>resultInfo.Result)
                                {
                                    max= resultInfo.Result;
                                }
                            }
                        }
                        li.SubItems[li.SubItems.Count-1].Font=font ;
                        if(resultInfo.uploadState==0)
                        {
                            li.SubItems.Add("未上传");
                            li.SubItems[li.SubItems.Count-1].ForeColor=Color.Red;

                        }
                        else
                        {
                            if (resultInfo.uploadState == 1)
                            {
                                li.SubItems.Add("已上传");
                                li.SubItems[li.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                                li.SubItems[li.SubItems.Count - 1].Font = font;
                            }
                        }

                        resultRound++;
                        
                    }

                    for (int i =   resultRound; i < sportProjectInfos.RountCount; i++)
                    {
                         li.SubItems.Add("未测试");
                         li.SubItems.Add("未上传");
                    }

                    if (isgetScore)
                    {
                        li.SubItems[5].Text = max.ToString();
                    }

                    step++;
                    
                    listView1.Items.Insert(listView1.Items.Count, li);
                }
                listView1.EndUpdate();
                return dbPersonInfos;
            }
            catch (Exception exception)
            {
                listView1.Items.Clear();
                dbPersonInfos.Clear();
                LoggerHelper.Debug( exception);
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCombox"></param>
        public void RefreshGetGroup(ComboBox groupCombox,string groupName="")
        {
            try
            {
                List<string> lis = freeSql.Select<DbGroupInfos>().Distinct().ToList(a => a.Name);
                groupCombox.Items.Clear();
                AutoCompleteStringCollection lstsourece = new AutoCompleteStringCollection();
                foreach (var item in lis)
                {
                    groupCombox.Items.Add(item);
                    lstsourece.Add(item);
                }
                groupCombox.AutoCompleteCustomSource = lstsourece;
                if (!string.IsNullOrEmpty(groupName))
                {
                    int index = groupCombox.Items.IndexOf(groupName);
                    if (index > 0)
                    {
                        groupCombox.SelectedIndex = index;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper .Debug(exception);
                return; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toString"></param>
        /// <param name="nowRound"></param>
        /// <returns></returns>
        public List<ResultInfos> GetResultInfo(string st, int nowRound)
        {  
            return  freeSql.Select<ResultInfos>().Where(a => a.PersonId ==  st && a.IsRemoved == 0 && a.RoundId == nowRound + 1).OrderBy(a => a.Id).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public List<ResultInfos> GetResultInfo(string st)
        {
            return  freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == st && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCombox"></param>
        /// <returns></returns>
        public List<DbPersonInfos> GetDBPersonInfo(ComboBox groupCombox)
        {
            return  freeSql.Select<DbPersonInfos>().Where(a => a.GroupName ==groupCombox.Text).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="sportProjectInfos"></param>
        /// <param name="groupCombox"></param>
        /// <param name="myControlls"></param>
        public void WriteScoreIntoDb(ListView listView1, SportProjectInfos sportProjectInfos, ComboBox groupCombox, List<MyControll> myControlls)
        {
            try
            {
                freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                List<ResultInfos> insertResults = new List<ResultInfos>();
                List<DbPersonInfos> dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupCombox.Text).ToList();
                StringBuilder errorsb = new StringBuilder();
                foreach (var controll in myControlls)
                {
                    if (!string.IsNullOrEmpty(controll.p_IdNumber) || controll.p_IdNumber != "未分配")
                    {
                        string idnum = controll.p_IdNumber;
                        int state = controll.p_MStateIndex;
                        int roundid = controll.p_RoundCountSelectIndex;
                        double.TryParse(controll.p_Score, out double score);
                        DbPersonInfos dv = dbPersonInfos.Find(a => a.IdNumber == idnum);
                        if (state == 0)
                        {
                            errorsb.AppendLine(dv.IdNumber + "," + dv.Name + "未完成设置  ");
                        }
                        else
                        {
                            bool flag = false;
                            int  j;
                            for ( int i= roundid; i < sportProjectInfos.RountCount;i++)
                            {
                                List<ResultInfos> res = freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber ==idnum&& a.IsRemoved == 0 && a.RoundId == i).OrderBy(a => a.Id).ToList();
                                if (res.Count == 0)
                                {
                                    flag = true;
                                    maxSortId++;
                                    insertResults.Add(new  ResultInfos()
                                    {
                                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd-hh mm:ss"),
                                        SortId = maxSortId,
                                        IsRemoved = 0,
                                        PersonId=dv.Id.ToString(),
                                        SportItemType=0,
                                        PersonName = dv.Name,
                                        PersonIdNumber = dv.IdNumber,
                                        RoundId = i+1,
                                        Result = score,
                                        State = state,
                                    });
                                    break;     
                                }
                                j = i;
                            }

                            if (flag == false)
                            {
                                errorsb.AppendLine(dv.IdNumber + "," + dv.Name + "轮次已满");
                                
                            }
                        }
                    }
                }
                int result = freeSql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                if (errorsb.Length != 0) 
                    MessageBox.Show(errorsb.ToString());
                if (result > 0)
                    UpDateListView(listView1, groupCombox,sportProjectInfos);
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return;
            }
        }

         
        
        
        public void PrintCurrentScore(string groupName, SportProjectInfos sportProjectInfos)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName)) throw new Exception("未选择组");
                string path = Application.StartupPath + "\\Data\\PrintExcel\\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, $"PrintExcel_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                //序号 项目名称    组别名称 姓名  准考证号 考试状态    第1轮 第2轮 最好成绩
                List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                List<OutPutPrintExcelData> outPutExcelDataList = new List<OutPutPrintExcelData>();
                int step = 1;
                bool isBestScore = sportProjectInfos.BestScore == 0;
                foreach (var dpInfo in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString()).OrderBy(a => a.IsRemoved).ToList();
                    OutPutPrintExcelData opd = new OutPutPrintExcelData();
                    opd.id = step;
                    opd.examtime = dpInfo.CreateTime;
                    opd.school = dpInfo.SchoolName;
                    opd.name= dpInfo.Name;
                    opd.Sex = dpInfo.Sex == 0 ? "男" : "女";
                    opd.idNumber= dpInfo.IdNumber;
                    opd.groupName = dpInfo.GroupName;
                    int state = 0;
                    double max = 9999.0;
                    if (resultInfos.Count > 0)
                    {
                        foreach (var ri in resultInfos)
                        {
                            if (ri.State != 1)
                            {
                                if (isBestScore && max < 0.0)
                                {
                                    max = 0.0;
                                    state = ri.State;
                                }
                                else
                                {
                                    if (!isBestScore && max > 9999.0)
                                    {
                                        max = 9999.0;
                                        state = ri.State;
                                    }
                                }
                            }
                            else
                            {
                                if (ri.State > 0)
                                {
                                    if (isBestScore && max > ri.Result)
                                    {
                                        max = ri.Result;
                                        state = ri.State;
                                        // opd.score = max.ToString();
                                    }

                                    if (isBestScore == false && max > ri.Result)
                                    {
                                        max = ri.Result;
                                        state = ri.State;
                                        opd.score = max.ToString();

                                    }

                                }

                            }
                        }
                        if (state > 0)
                        {
                            if (state != 1)
                            {
                                max = 0.0;
                                opd.score = ResultStateType.Match(state);
                            }
                            else
                            {
                                opd.score = max.ToString();
                            }
                        }
                    }
                    else
                    {
                        if(state==0)
                        {
                            max = 0.0;
                            opd.score = ResultStateType.Match( dpInfo.State);
                        }
                    }
                    outPutExcelDataList.Add(opd);
                    step++;
                }
                MiniExcel.SaveAs(path, outPutExcelDataList);
                if (File.Exists(path))
                {
                    try
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.FileName = path;
                        p.StartInfo.Verb = "print";
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                        throw new Exception("打印机异常");
                    }
                }
                else
                {
                    throw new Exception("导出失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LoggerHelper.Debug(ex);
            }
        }

        public void SetGradeError(ListView listView1, string stateStr, int nowRound, ComboBox groupCombox, SportProjectInfos sportProjectInfos )
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                int state = ResultStateType.ResultStateToInt(stateStr);
                int result = freeSql.Update<ResultInfos>().Set(a => a.State == state)
                    .Where(a => a.PersonIdNumber == idnumber && a.RoundId == nowRound + 1).ExecuteAffrows();
                if (result == 0)
                {
                    freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    List<ResultInfos> insertResults = new List<ResultInfos>();
                    DbPersonInfos dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.IdNumber == idnumber).ToOne();
                    maxSortId++;
                    ResultInfos rinfo = new ResultInfos();
                    rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rinfo.SortId = maxSortId;
                    rinfo.IsRemoved = 0;
                    rinfo.PersonId = dbPersonInfos.Id.ToString();
                    rinfo.SportItemType = 0;
                    rinfo.PersonName = dbPersonInfos.Name;
                    rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                    rinfo.RoundId = nowRound + 1;
                    rinfo.Result = 0;
                    rinfo.State = state;
                    insertResults.Add(rinfo);
                    result = freeSql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                }
                if (result > 0)
                {
                    UpDateListView(listView1, groupCombox,sportProjectInfos);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return;
            }
        }
    }
}