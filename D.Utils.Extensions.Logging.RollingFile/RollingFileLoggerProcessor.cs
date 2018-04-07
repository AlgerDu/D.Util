﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D.Utils.Extensions.Logging.RollingFile
{
    /// <summary>
    /// 将日志向文件中输出
    /// </summary>
    internal class RollingFileLoggerProcessor
    {
        /// <summary>
        /// 当前需要处理一批日志
        /// </summary>
        private readonly List<LogContent> _currentBatchContents;

        /// <summary>
        /// 休息时间
        /// </summary>
        private readonly TimeSpan _interval;

        private readonly string _originalPath;

        /// <summary>
        /// 当前日志应该输出的路径，每次 RollingFile 之后可能变更
        /// </summary>
        private string _currentPath;

        private int _currentIndex;

        private long _maxFileSize;

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

        public RollingFileLoggerProcessor(
            string path,
            long maxFileSize
            )
        {
            _currentBatchContents = new List<LogContent>();
            _interval = TimeSpan.FromSeconds(2);

            _cancellationTokenSource = new CancellationTokenSource();

            _logContentQueue = new BlockingCollection<LogContent>(new ConcurrentQueue<LogContent>());

            _originalPath = path;

            _currentIndex = 0;
            _currentPath = _originalPath;
            RollFile();

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
        private async Task WriteLogContentsAsync(IEnumerable<LogContent> contents, CancellationToken token)
        {
            using (var streamWriter = File.AppendText(_currentPath))
            {
                var builder = new StringBuilder();
                foreach (var content in contents)
                {
                    builder.AppendLine($"{GetLogLevelString(content.LogLevel)}: {content.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz")}[{content.EventId}]");

                    builder.AppendLine($"      {content.Msg}");

                    if (content.Ex != null)
                    {
                        builder.AppendLine(content.Ex.ToString().Replace("\r\n", "      \r\n"));
                    }
                }

                await streamWriter.WriteAsync(builder.ToString());
            }

            RollFile();
        }

        /// <summary>
        /// 有 ILogger 调用，向队列中加入一个 logContent
        /// </summary>
        /// <param name="logContent"></param>
        internal void AddLogContent(LogContent logContent)
        {
            if (!_logContentQueue.IsAddingCompleted)
            {
                _logContentQueue.Add(logContent, _cancellationTokenSource.Token);
            }
        }

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

        /// <summary>
        /// 判断，创建文件
        /// </summary>
        private void RollFile()
        {
            var tmpPath = _originalPath.Replace("{Date}", DateTimeOffset.Now.ToString("yyyyMMdd"));

            if (!File.Exists(_currentPath))
            {
                File.Create(_currentPath);
                return;
            }

            var fileInfo = new FileInfo(_currentPath);

            if (fileInfo.Length <= _maxFileSize)
            {
                return;
            }

            var fileName = Path.GetFileNameWithoutExtension(tmpPath);
            var path = Path.GetFullPath(tmpPath);
            var ext = Path.GetExtension(tmpPath);

            _currentPath = $"{path}{fileName}_{_currentIndex++}.{ext}";
        }

        private string GetLogLevelString(LogLevel logLevel)
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            //IL_0001: Expected I4, but got Unknown
            switch ((int)logLevel)
            {
                case 0:
                    return "trce";
                case 1:
                    return "dbug";
                case 2:
                    return "info";
                case 3:
                    return "warn";
                case 4:
                    return "fail";
                case 5:
                    return "crit";
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }
    }
}
