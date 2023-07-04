using GripCore.GameSystem.GameHelper;
using GripCore.GameSystem.GameWindowSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GripCore
{
    public class GameRoot
    {
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern void SetForegroundWindow(IntPtr mainWindowHandle);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private static WriteLoggerHelper WriteLoggerHelper = new WriteLoggerHelper();
        private static MainWindowSys MainWindowSys = new MainWindowSys();
        private  static  PersonImportWindowSys  PersonImportWindowSys  = new PersonImportWindowSys ();
        private static PlatFormSettingWindowSys PlatFormSettingWindowSys = new PlatFormSettingWindowSys();
        private static SystemSettingWindowSys SystemSettingWindowSys = new SystemSettingWindowSys();
        private static ExportGradeWindowSys ExportGradeWindowSys = new ExportGradeWindowSys();
        private static RunningMachineSettingWindowSys RunningMachineSettingWindowSys = new RunningMachineSettingWindowSys();
        private static RunningTestingWindowSys RunningTestWindowSys = new RunningTestingWindowSys();

        public void StartGame()
        {
            WriteLoggerHelper.Awake();
            MainWindowSys.Awake();
            PersonImportWindowSys .Awake();
            PlatFormSettingWindowSys.Awake();
            SystemSettingWindowSys.Awake();
            ExportGradeWindowSys.Awake();
            RunningMachineSettingWindowSys.Awake();
            RunningTestWindowSys.Awake();
        }
    }
}
