using GripCore.GameSystem.GameWindow;
using Sunny.UI;

namespace GripCore.GameSystem.GameWindowSys
{
    public class RunningMachineSettingWindowSys
    {
        public static RunningMachineSettingWindowSys Instance;
        private RunningMachineSettingWindow RunningMachineSettingWindow = null;

        public void Awake()
        {
            Instance = this;
        }
        public bool  ShowRunningMachineSettingWindow()
        {
             RunningMachineSettingWindow = new RunningMachineSettingWindow();
             var sl  = RunningMachineSettingWindow.ShowDialog();
            if(sl==System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else
                return false;
        }
        private static int machine = 0;
        private static string protNames = string.Empty;
        public void SaveData(UIComboBox uiComboBox2, UIComboBox uiComboBox1, ref int machineCount, ref string portName)
        {
            int.TryParse(uiComboBox1.Text, out machineCount);
            if (machineCount == 0) { machineCount = 5; }

            portName = uiComboBox2.Text;
            machine = machineCount;
            protNames = portName;
        }
        public int GetMachineCount()
        {
            return machine;
        }

        public string GetPortName()
        {
            return protNames;
        }
    }
}