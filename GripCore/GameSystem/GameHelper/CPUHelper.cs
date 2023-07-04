using System;
using System.Management;

namespace GripCore.GameSystem.GameHelper
{
    public class CPUHelper
    {
        public static string GetCpuID()
        {
            try
            {
                string cpuInfo = ""; //cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }

                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch(Exception exception)
            {
                LoggerHelper.Debug(exception);
                return "unknow";
            }
            finally
            {

            }

        }
    }
}