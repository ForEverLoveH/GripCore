using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripCore.GameSystem.GameHelper
{
    public  class LoggerHelper
    {
       
            ///记录Error日志
            public static void Error(string errorMsg, Exception ex = null)
            {
                if (ex != null)
                {
                    string message =  GetExceptionMsg(ex);
                    Log.Debug(message);
                    //LogError.Error(message);
                }
                else
                {
                    Log.Debug(errorMsg);
                }
            }

            public static void Debug(Exception ex)
            {
                if (ex != null)
                {
                    string message = GetExceptionMsg(ex);
                    Log.Debug(message);
                }
            }

            /// 记录Info日志
            public static void Info(string msg, Exception ex = null)
            {
                if (ex != null)
                {
                    string message =  GetExceptionMsg(ex);
                    Log.Information(message);
                    //LogInfo.Info(message);
                }
                else
                {
                    Log.Information(msg);
                }
            }

            /// 记录Monitor日志
            public static void Monitor(string msg)
            {
                Log.Information(msg);
                //LogMonitor.Info(msg);
            }

            public static void Warn(string msg, Exception ex = null)
            {
                if (ex != null)
                {
                    string message =  GetExceptionMsg(ex);
                    Log.Warning(message);
                }
                else
                {
                    Log.Warning(msg);
                }
            }

        private static string GetExceptionMsg(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" ");
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
                sb.AppendLine("【异常方法】：" + ex.TargetSite);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + ex.Message);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }

        public static void Fatal(string message)
        {
            Log.Fatal(message);
        }
        
    }
}
