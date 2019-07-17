using System;
using System.Threading;

namespace TygaSoft.CustomThread
{
    public abstract class BaseThread : IThread
    {
        #region 私有

        private ThreadStart threadStart;
        private Thread thread;

        #endregion

        #region 构造函数

        public BaseThread() { }

        #endregion

        #region 公有

        public bool IsBackground { get; set; }

        /// <summary>
        /// 线程委托函数
        /// </summary>
        public abstract void ThreadWork();

        #endregion

        #region IThread成员

        /// <summary>
        /// 线程启动
        /// </summary>
        public virtual void ThreadStart()
        {
            threadStart = new ThreadStart(this.ThreadWork);
            thread = new Thread(threadStart);
            thread.Name = "Thread_" + Guid.NewGuid().ToString("N");
            thread.IsBackground = IsBackground;
            thread.Start();
        }

        /// <summary>
        /// 线程终止
        /// </summary>
        public virtual void ThreadStop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 线程暂停
        /// </summary>
        public virtual void ThreadSuspend()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 线程继续
        /// </summary>
        public virtual void ThreadContinue()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
