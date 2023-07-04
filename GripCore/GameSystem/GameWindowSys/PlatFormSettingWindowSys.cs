using GripCore.GameSystem.GameWindow;
using System;
using System.Collections.Generic;
using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameSystemModel;
using GripCoreModel.GameModel;
using Newtonsoft.Json;
using Sunny.UI;
using Sunny.UI.Win32;

namespace GripCore.GameSystem.GameWindowSys
{
    public class PlatFormSettingWindowSys
    {
        public static PlatFormSettingWindowSys Instance;
        private PlatFormSettingWindow PlatFormSettingWindow = null;
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;

        public void Awake()
        {
            Instance = this;
        }

        public void ShowPlatFormSettingWindow()
        {
            PlatFormSettingWindow = new PlatFormSettingWindow();
            PlatFormSettingWindow.ShowDialog();
        }
        

        public void LoadingPlatFormSettingWindowInitData(ref string machineCode, ref string examId, ref string platforms, ref string platform, UIComboBox uiComboBox1, UIComboBox uiComboBox2, UIComboBox uiComboBox3, ref Dictionary<string, string> localValues)
        {
            List<LocalInfos> localInfos = freeSql.Select<LocalInfos>().ToList();
            localValues = new Dictionary<string, string>();
            foreach (var localInfo in localInfos)
            {
                localValues.Add(localInfo.key, localInfo.value);
                switch (localInfo.key)
                {
                    case "MachineCode":
                        machineCode = localInfo.value;
                        break;
                    case "ExamId":
                        examId = localInfo.value;
                        break;
                    case "Platform":
                        platform = localInfo.value;
                        break;
                    case "Platforms":
                        platforms = localInfo.value;
                        break;
                }
            }
            if (string.IsNullOrEmpty(machineCode))
            {
                UIMessageBox.ShowError("设备码为空！！");
            }
            else
            {
                uiComboBox1.Text = machineCode;
            }
            if (string.IsNullOrEmpty(examId))
            {
                UIMessageBox.ShowError("考试id为空！！");
            }
            else
            {
                uiComboBox3.Text = examId;
            }
            if (string.IsNullOrEmpty(platforms))
            {
                UIMessageBox.ShowError("平台码为空！！");
            }
            else
            {
                string[] ps = platforms.Split(';');
                uiComboBox2.Items.Clear();
                foreach (string p in ps)
                {
                    uiComboBox2.Items.Add(p);
                }
            }
            if (string.IsNullOrEmpty(platform))
            {
                UIMessageBox.ShowError("平台码为空");
            }
            else
            {
                uiComboBox2.Text = platform;
            }
        }

        public bool SaveDataToDataBase(UIComboBox uiComboBox1, UIComboBox uiComboBox2, UIComboBox uiComboBox3)
        {
            try
            {
                string Platform = uiComboBox2.Text;
                string ExamId = uiComboBox3.Text;
                string MachineCode = uiComboBox1.Text;
                int sum = 0;
                int result = freeSql.Update<LocalInfos>().Set(a => a.value == Platform).Where(a => a.key == "Platform").ExecuteAffrows();
                sum += result;
                result = freeSql.Update<LocalInfos>().Set(a => a.value == ExamId).Where(a => a.key == "ExamId").ExecuteAffrows();
                sum += result;
                result = freeSql.Update<LocalInfos>().Set(a => a.value == MachineCode).Where(a => a.key == "MachineCode").ExecuteAffrows();
                sum += result;
                if (sum == 3)
                {
                    UIMessageBox.ShowSuccess("保存成功");
                    return true;

                }
                else
                {
                    UIMessageBox.ShowError("更新失败");
                    return false;
                }
            }
            catch (Exception e)
            {
                 LoggerHelper.Debug(e);
                 
                 return false;
            }
        }     
    }
}