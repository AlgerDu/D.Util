using Microsoft.Extensions.Logging;
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
    internal class RollingFileLoggerProcessor : IDisposable
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
            _interval = TimeSpan.FromSeconds(3);

            _cancellationTokenSource = new CancellationTokenSource();

            _logContentQueue = new BlockingCollection<LogContent>(new ConcurrentQueue<LogContent>());

            _originalPath = path;
            _maxFileSize = maxFileSize;

            _currentPath = string.Empty;
            RollFile();

            _outputTask = Task.Factory.StartNew(
                ProcessLogContentQueue
                , null
                , TaskCreationOptions.LongRunning
                );
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// 异步写 LogContents 到正式的目标，比如文件等
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task WriteLogContentsAsync(IEnumerable<LogContent> contents, CancellationToken token)
        {
            try
            {
                using (var streamWriter = File.AppendText(_currentPath))
                {
                    var builder = new StringBuilder();
                    foreach (var content in contents)
                    {
                        FormattingLogContent(builder, content);
                    }

                    await streamWriter.WriteAsync(builder.ToString());
                }

                RollFile();
            }
            catch (Exception ex)
            {
                throw new Exception($"rolling file logger 写入文件异常：{ex}");
            }
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

                await Task.Delay(_interval);
            }
        }

        /// <summary>
        /// 判断，创建文件
        /// </summary>
        private void RollFile()
        {
            if (string.IsNullOrEmpty(_currentPath))
            {
                _currentPath = AnalyseOriginalPath(_originalPath);
                Directory.CreateDirectory(Path.GetDirectoryName(_currentPath));
            }

            if (!File.Exists(_currentPath))
            {
                return;
            }

            var fileInfo = new FileInfo(_currentPath);

            if (fileInfo.Length <= _maxFileSize)
            {
                return;
            }

            var tmpPath = AnalyseOriginalPath(_originalPath);

            var fileName = Path.GetFileNameWithoutExtension(tmpPath);
            var directory = Path.GetDirectoryName(tmpPath);
            var ext = Path.GetExtension(tmpPath);

            Directory.CreateDirectory(directory);

            var index = 1;

            var toChangeNameFile = new List<string>();

            while (File.Exists(tmpPath))
            {
                toChangeNameFile.Add(tmpPath);
                tmpPath = $"{directory}\\{fileName}_{index++}{ext}";
            }

            toChangeNameFile.Reverse();

            foreach (var file in toChangeNameFile)
            {
                File.Move(file, tmpPath);
                tmpPath = file;
            }

            _currentPath = tmpPath;
        }

        private string AnalyseOriginalPath(string originalPath)
        {
            var rst = originalPath
                .Replace("{Date}", DateTimeOffset.Now.ToString("yyyyMMdd"))
                .Replace("{yyyyMMdd}", DateTimeOffset.Now.ToString("yyyyMMdd"))
                .Replace("{yyyy-MM-dd}", DateTimeOffset.Now.ToString("yyyy-MM-dd"));

            return rst;
        }

        private string GetLogLevelString(LogLevel logLevel)
        {
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

        private void FormattingLogContent(StringBuilder builder, LogContent content)
        {
            builder.AppendLine($"{GetLogLevelString(content.LogLevel)}: {content.Timestamp.ToString("yyyy-MM-dd HH:mm:ss fff")}[{content.ThreadID}]");
            builder.AppendLine($"      {content.Category}");
            builder.AppendLine($"      {content.Msg}");

            if (content.EventId.Id != 0)
            {
                builder.Append($"({content.EventId})");
            }

            if (content.Ex != null)
            {
                builder.AppendLine(content.Ex.ToString().Replace("\r\n", "      \r\n"));
            }
        }
    }
}
