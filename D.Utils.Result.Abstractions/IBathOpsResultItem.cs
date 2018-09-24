using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Utils
{
    /// <summary>
    /// 批量操作 item
    /// </summary>
    public interface IBathOpsResultItem
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        int Code { get; }

        /// <summary>
        /// item 在批量操作中顺序
        /// </summary>
        long Index { get; }

        /// <summary>
        /// 返回消息
        /// </summary>
        string Msg { get; }
    }

    /// <summary>
    /// 批量操作结果
    /// </summary>
    public interface IBathOpsResult : IResult
    {
        /// <summary>
        /// 操作的总数
        /// </summary>
        long OpsCount { get; }

        /// <summary>
        /// 结果数据
        /// </summary>
        IEnumerable<IBathOpsResultItem> Items { get; }
    }

    /// <summary>
    /// 批量操作 item（泛型）
    /// </summary>
    public interface IBathOpsResultItem<T> : IBathOpsResultItem
    {
        /// <summary>
        /// 数据
        /// </summary>
        T Data { get; }
    }

    /// <summary>
    /// 批量操作结果（泛型）
    /// </summary>
    public interface IBathOpsResult<T> : IResult
    {
        /// <summary>
        /// 操作的总数
        /// </summary>
        long OpsCount { get; }

        /// <summary>
        /// 结果数据
        /// </summary>
        IEnumerable<IBathOpsResultItem<T>> Items { get; }
    }
}
