using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameWindow;
using GripCoreModel.GameModel;
using Sunny.UI;

namespace GripCore.GameSystem.GameWindowSys
{
    public class SystemSettingWindowSys
    {
        public static SystemSettingWindowSys Instance;
        private SystemSettingWindow _systemSettingWindow = null;

        public void Awake()
        {
            Instance = this;
        }

        private IFreeSql freeSql = FreeSqlHelper.Sqlite;
        public void LoadingInitData(UITextBox uiTextBox1, UIComboBox uiComboBox1, UIComboBox uiComboBox2,
            UIComboBox uiComboBox3, UIComboBox uiComboBox4, ref SportProjectInfos sportProjectInfos)
        {
            sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                uiTextBox1.Text = sportProjectInfos.Name;
                uiComboBox1.SelectedIndex = sportProjectInfos.RountCount;
                uiComboBox2.SelectedIndex = sportProjectInfos.BestScore;
                uiComboBox3.SelectedIndex = sportProjectInfos.TestMethod;
                uiComboBox4.SelectedIndex = sportProjectInfos.FloatType;
            }
        }

        public   void ChangeRoundCount(int index )
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.RountCount == index).Where("1=1").ExecuteAffrows();
        }

        public void UpdataBestScoreMode(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.BestScore == index).Where("1=1").ExecuteAffrows();
        }

        public void UpDataTestMethod(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.TestMethod == index).Where("1=1").ExecuteAffrows();
        }

        public void UpdataFloatType(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.FloatType == index).Where("1=1").ExecuteAffrows();
        }

        public  void SetFrmClosedData(string name)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.Name == name).Where("1=1").ExecuteAffrows();
        }

        public void ShowSystemSettingWindow()
        {
            _systemSettingWindow = new SystemSettingWindow();
            _systemSettingWindow.Show();
        }
    }
}