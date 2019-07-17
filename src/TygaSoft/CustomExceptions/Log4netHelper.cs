using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace TygaSoft.CustomExceptions
{
    public class Log4netHelper
    {
        ILog log;

        /// <summary>
        /// 执行log4net的Debug日志记录
        /// </summary>
        /// <param name="message"></param>
        public void ExecuteLogDebug(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (log == null) log = LogManager.GetLogger("");
            log.Debug(message);
        }

        /// <summary>
        /// 执行log4net的Error日志记录
        /// </summary>
        /// <param name="message"></param>
        public void ExecuteLogError(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (log == null) log = LogManager.GetLogger("");
            log.Error(message);
        }

        /// <summary>
        /// 执行log4net的Fatal日志记录
        /// </summary>
        /// <param name="message"></param>
        public void ExecuteLogFatal(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (log == null) log = LogManager.GetLogger("");
            log.Fatal(message);
        }

        /// <summary>
        /// 执行log4net的Info日志记录
        /// </summary>
        /// <param name="message"></param>
        public void ExecuteLogInfo(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (log == null) log = LogManager.GetLogger("");
            log.Info(message);
        }

        /// <summary>
        /// 执行log4net的Warn日志记录
        /// </summary>
        /// <param name="message"></param>
        public void ExecuteLogWarn(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (log == null) log = LogManager.GetLogger("");
            log.Warn(message);
        }
    }
}
