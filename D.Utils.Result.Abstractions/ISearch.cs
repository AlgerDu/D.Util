using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Utils
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISearchResult<T> : IResult
    {
        /// <summary>
        /// 每页数量，为 -1 时代表不分页
        /// </summary>
        /// <returns></returns>
        long PageSize { get; }

        /// <summary>
        /// 页码，不分页时返回 1
        /// </summary>
        /// <returns></returns>
        long PageIndex { get; }

        /// <summary>
        /// 总记录数
        /// </summary>
        /// <returns></returns>
        long RecodCount { get; }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Datas { get; }
    }
}
