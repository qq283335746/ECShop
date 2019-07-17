using System;
using System.Runtime.Serialization;
using log4net;

namespace TygaSoft.CustomExceptions
{
    public class CustomException : Exception,ISerializable
    {
        public CustomException() { }

        public CustomException(string message)
            : base(message)
        {
            ExLog4net(message);
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (string.IsNullOrEmpty(message)) message = innerException.Message;
            //根据设置是否写入日志
            if (innerException.Data != null && innerException.Data.Count > 0)
            {
                string exSource = string.Empty;
                string logExecute = string.Empty;
                if (innerException.Data.Contains("exSource")) exSource = innerException.Data["exSource"].ToString();
                if (innerException.Data.Contains("logExecute")) logExecute = innerException.Data["logExecute"].ToString();
                ExLog4net(message, exSource, logExecute);
            }
        }

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// log4net日志
        /// </summary>
        /// <param name="message">包含log4net中的Debug、Error、Fatal、Info、Warn，并使用“|”与其它字符串隔开</param>
        private void ExLog4net(string message)
        {
            if (message.LastIndexOf("|") > -1)
            {
                string[] messages = message.Split('|');
                //messages[0]组成格式为：xxx,xxx,xxx...
                if (!string.IsNullOrEmpty(messages[0]))
                {
                    ILog log = LogManager.GetLogger("");
                    string[] items = messages[0].Split(',');
                    foreach (string item in items)
                    {
                        switch (item.ToLower())
                        {
                            case "debug":
                                log.Debug(messages[1]);
                                break;
                            case "error":
                                log.Error(messages[1]);
                                break;
                            case "fatal":
                                log.Fatal(messages[1]);
                                break;
                            case "info":
                                log.Info(messages[1]);
                                break;
                            case "warn":
                                log.Warn(messages[1]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// log4net日志
        /// </summary>
        /// <param name="message">信息记录</param>
        /// <param name="exSource">报错源+方法名+请求报文 组成的字符串</param>
        /// <param name="logExecute">执行log4net日志记录的方法:Debug|Error|Fatal|Info|Warn</param>
        private void ExLog4net(string message, string exSource, string logExecute)
        {
            //如果指定的log4net写入日志的级别为空，则返回
            if (string.IsNullOrEmpty(logExecute)) return;
            string[] executes = logExecute.Split('|');
            ILog log = LogManager.GetLogger(exSource);

            foreach (string item in executes)
            {
                switch (item.ToLower())
                {
                    case "debug":
                        log.Debug(message);
                        break;
                    case "error":
                        log.Error(message);
                        break;
                    case "fatal":
                        log.Fatal(message);
                        break;
                    case "info":
                        log.Info(message);
                        break;
                    case "warn":
                        log.Warn(message);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
