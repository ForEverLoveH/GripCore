using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GripCoreModel;
using GripCoreModel.GameModel;

namespace GripCore.GameSystem.GameHelper
{
    public  class WriteLoggerHelper
    {
        public static WriteLoggerHelper Instance;
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;
        public void Awake() 
        { 
            Instance = this;
            WriteLogger();
        }

        private void WriteLogger()
        {
            string LogFilePath(string LogEvent) => $@"{AppContext.BaseDirectory}ALog\{LogEvent}\log.log";
            string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 50);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()    //输出到控制台
                                      //.WriteTo.File("ALog\\log.txt", rollingInterval: RollingInterval.Hour)
                                      //.WriteTo.Async(a => a.File("00_Logs\\log.log", rollingInterval: RollingInterval.Day))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.Async(a => a.File(LogFilePath("Debug"), rollingInterval: RollingInterval.Minute, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: null)))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.Async(a => a.File(LogFilePath("Information"), rollingInterval: RollingInterval.Minute, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: null)))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.Async(a => a.File(LogFilePath("Warning"), rollingInterval: RollingInterval.Minute, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: null)))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.Async(a => a.File(LogFilePath("Error"), rollingInterval: RollingInterval.Minute, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: null)))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.Async(a => a.File(LogFilePath("Fatal"), rollingInterval: RollingInterval.Minute, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: null)))
                .CreateLogger();

            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            // 后续处理，保存或输出
            Log.Error(str);
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            // 后续处理，保存或输出
            Log.Error(str);
        }

        private string GetExceptionMsg(Exception ex, string backStr)
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
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
        /// <summary>
        /// 写入失败日志
        /// </summary>
        /// <param name="errorList"></param>
        public  void WriteDefaultLog(List<Dictionary<string, string>> errorList, DbGroupInfos gInfo)
        {
            string txtpath = Application.StartupPath + $"\\Log\\upload\\";
            if (!Directory.Exists(txtpath))
            {
                Directory.CreateDirectory(txtpath);
            }
            StringBuilder errorsb = new StringBuilder();
            errorsb.AppendLine($"失败:{errorList.Count}");
            errorsb.AppendLine("****************error***********************");
            foreach (var item in errorList)
            {
                errorsb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]} 错误:{item["error"]}");
            }
            errorsb.AppendLine("*****************error**********************");
            if (errorList.Count != 0)
            {
                string txtpath1 = Path.Combine(txtpath, $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath1, errorsb.ToString());
                errorList.Clear();
            }
        }


        /// <summary>
        /// 写入成功日志
        /// </summary>
        /// <param name="successList"></param>
        public void WriteSucessLog(List<Dictionary<string, string>> successList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"成功:{successList.Count}");
            sb.AppendLine("******************success*********************");
            foreach (var item in successList)
            {
                freeSql.Update<DbPersonInfos>()
                    .Set(a => a.uploadGroup == "1")
                    .Where(a => a.Id == Convert.ToInt32(item["Id"]))
                    .ExecuteAffrows();
                freeSql.Update<ResultInfos>()
                    .Set(a => a.uploadState == 1)
                    .Where(a => a.PersonId == item["Id"])
                    .ExecuteAffrows();
                ;
                sb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]}");
            }

            sb.AppendLine("*******************success********************");
            if (successList.Count != 0)
            {
                string txtpath = Application.StartupPath + $"\\Log\\upload\\";
                txtpath = Path.Combine(txtpath, $"upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath, sb.ToString());
                successList.Clear();
            }
        }
    }
}
