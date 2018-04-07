using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D.Utils.Extensions.Logging.RollingFile
{
    /// <summary>
    /// 基类 loggerProvider，封装一些批量的操作
    /// </summary>
    public abstract class DBaseLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// 当前需要处理一批日志
        /// </summary>
        private readonly List<LogContent> _currentBatchContents;

        /// <summary>
        /// 休息时间
        /// </summary>
        private readonly TimeSpan _interval;

        /// <summary>
        /// 处理内存中日志列表的线程
        /// </summary>
        private Task _outputTask;

        /// <summary>
        /// 日志线程控制
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 日志队列
        /// </summary>
        private BlockingCollection<LogContent> _logContentQueue;

        public DBaseLoggerProvider(
            IOptions<DBaseProviderOptions> options
            )
        {
            _currentBatchContents = new List<LogContent>();
            _interval = TimeSpan.FromSeconds(2);

            _cancellationTokenSource = new CancellationTokenSource();

            _logContentQueue = new BlockingCollection<LogContent>(new ConcurrentQueue<LogContent>());

            _outputTask = Task.Factory.StartNew(
                ProcessLogContentQueue
                , null
                , TaskCreationOptions.LongRunning
                );
        }

        /// <summary>
        /// 异步写 LogContents 到正式的目标，比如文件等
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task WriteLogContentsAsync(IEnumerable<LogContent> contents, CancellationToken token);

        /// <summary>
        /// 有 ILogger 调用，向队列中加入一个 logContent
        /// </summary>
        /// <param name="logContent"></param>
        internal void AddMessage(LogContent logContent)
        {
            if (!_logContentQueue.IsAddingCompleted)
            {
                _logContentQueue.Add(logContent, _cancellationTokenSource.Token);
            }
        }

        #region BaseDLoggerProvider
        public ILogger CreateLogger(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 处理队列中的 LogContent
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private async Task ProcessLogContentQueue(object state)
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                while (_logContentQueue.TryTake(out var message))
                {
                    _currentBatchContents.Add(message);
                }

                await WriteLogContentsAsync(_currentBatchContents, _cancellationTokenSource.Token);
                _currentBatchContents.Clear();

                await Task.Delay(_interval, _cancellationTokenSource.Token);
            }
        }
    }
}
