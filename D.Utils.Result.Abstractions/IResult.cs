using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// 通用结果
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// 结果标识
        /// </summary>
        int Code { get; }

        /// <summary>
        /// 返回信息，成功与否都可能有消息返回
        /// </summary>
        string Msg { get; }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        bool IsSuccess();
    }

    /// <summary>
    /// 通用结果
    /// </summary>
    public interface IResult<T>
        : IResult
    {
        /// <summary>
        /// 泛型数据，默认为 defaul(T)
        /// </summary>
        T Data { get; }
    }
}
