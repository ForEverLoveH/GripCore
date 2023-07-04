using System;
using System.Collections.Generic;
using GripCore.GameSystem.GameWindowSys;
using System.Windows.Forms;
using HZH_Controls.Forms;
using Sunny.UI;
using GripCoreModel.GameModel;
using Newtonsoft.Json;
using GripCore.GameSystem.GameSystemModel;
using GripCore.GameSystem.GameHelper;

namespace GripCore.GameSystem.GameWindow
{
    public partial class PlatFormSettingWindow : Form
    {
        public PlatFormSettingWindow()
        {
            InitializeComponent();
        }

        private string MachineCode = string.Empty;
        private  string ExamId=  String.Empty;
        string Platform = String.Empty;
        string Platforms = String.Empty;
        public Dictionary<string, string> localValues = null;
        

        private void PlatFormSettingWindow_Load(object sender, System.EventArgs e)
        {
            PlatFormSettingWindowSys.Instance.LoadingPlatFormSettingWindowInitData(ref MachineCode, ref ExamId, ref Platforms, ref Platform, uiComboBox1, uiComboBox2, uiComboBox3, ref localValues);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            uiComboBox3.Items.Clear();
            string url = uiComboBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                FrmTips.ShowTipsError(this, "网址为空 ！！");
                return;
            }
            else
            {
                if(GetExamNum())
                {
                    UIMessageBox.ShowSuccess("获取成功！！！");
                    uiComboBox3.SelectedIndex = 1;
                }
                else
                {
                    UIMessageBox.ShowError("获取失败！！");
                    return;
                }
            }

        }

        private bool  GetExamNum()
        {
            uiComboBox3.Items.Clear();
            string url = uiComboBox2.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                UIMessageBox.ShowError("网址为空！！");
                return false;
            }
            else
            {
                url += RequestUrl.GetExamListUrl;
                string jsonData = JsonConvert.SerializeObject(new RequestParameter
                {
                    AdminUserName = localValues["AdminUserName"],
                    TestManUserName = localValues["TestManUserName"],
                    TestManPassword = localValues["TestManPassword"],
                }) ;
                List<FormItemModel> items = new List<FormItemModel>();
                items.Add(new FormItemModel
                {
                    Key = "data",
                    Value = jsonData
                });
                string strExcep=string.Empty;
                try
                {
                    strExcep = HttpUpLoad.PostForm(url, items, null, null, null, 20000);
                }
                catch(Exception ex)
                {
                    throw new Exception("请检查网络连接");
                }
                var res = JsonConvert.DeserializeObject<GetExamList>(strExcep);
                if (res == null || res.results == null || res.results.Count == 0)
                {
                    string es = string.Empty;
                    try
                    {
                        es = res.Error;
                    }
                    catch(Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                       es= string.Empty;
                    }
                    UIMessageBox.ShowError($"提交错误！！错误码为{es}");
                    return false;
                }
                else
                {
                    foreach(var item in res.results)
                    {
                        string sl = item.title + "_" + item.exam_id;
                        uiComboBox3.Items.Add(sl);
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void uiButton2_Click(object sender, EventArgs e)
        {
             uiComboBox1.Items.Clear();
             string examID = uiComboBox3.Text;
             if (string.IsNullOrWhiteSpace(ExamId))
             {
                 FrmTips.ShowTipsError(this, "考试id为空！！");
                 return;
             }
             else
             {
                 if (examID.IndexOf('_') != -1)
                 {
                     examID = examID.Substring(examID.IndexOf('_') + 1);
                 }

                 string url = uiComboBox2.Text.Trim();
                 if (string.IsNullOrWhiteSpace(url))
                 {
                     FrmTips.ShowTipsError(this, "网址为空 ！！");
                     return;
                 }
                 else
                 {
                    if  (GetEquipMentCode(examID, url, localValues, uiComboBox1))
                    {
                        UIMessageBox.ShowSuccess("获取成功");
                        uiComboBox1.SelectedIndex = 1;
                           
                    }
                    else
                    {
                        UIMessageBox.ShowError("获取失败！！");
                        return;
                    }
                 }
             }
        }

        private bool GetEquipMentCode(string examID, string url, Dictionary<string, string> localValues, UIComboBox uiComboBox1)
        {
            try
            {
                url += RequestUrl.GetMachineCodeListUrl;
                string jsonstr = JsonConvert.SerializeObject(new RequestParameter()
                {
                    AdminUserName = localValues["AdminUserName"],
                    TestManUserName = localValues["TestManUserName"],
                    TestManPassword = localValues["TestManPassword"],
                    ExamId = examID,
                });
                List<FormItemModel> formItemModels = new List<FormItemModel>();
                formItemModels.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = jsonstr,
                });
                //HttpUpLoad httpUpLoad = new HttpUpLoad();
                string res = string.Empty;
                try
                {
                    res = HttpUpLoad.PostForm(url, formItemModels, null, null, null, 20000);
                }
                catch (Exception exception)
                {
                    LoggerHelper.Debug(exception);
                    return false;
                }

                GetMachineCodeList machineCodeList = JsonConvert.DeserializeObject<GetMachineCodeList>(res);
                if (machineCodeList == null || machineCodeList.Results == null || machineCodeList.Results.Count == 0)
                {
                    string err = string.Empty;
                    try
                    {
                        err = machineCodeList.Error;
                    }
                    catch (Exception e)
                    {
                        LoggerHelper.Debug(e);
                        err = string.Empty;
                    }

                    UIMessageBox.ShowError($"提交错误，错误码为{err}");
                    return false;
                }
                else
                {
                    foreach (var item in machineCodeList.Results)
                    {
                        string str = item.title + "_" + item.machineCode;
                        uiComboBox1.Items.Add(str);

                    }
                    return true;
                }

            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (PlatFormSettingWindowSys.Instance.SaveDataToDataBase(uiComboBox1, uiComboBox2, uiComboBox3))
            {
                DialogResult dialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                DialogResult dialog = DialogResult.Cancel;
                this.Close();
            }
        }

        
    }
}