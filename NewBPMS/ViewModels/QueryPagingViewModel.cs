using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels
{
    /// <summary>
    /// 查询-分页 视图
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryPagingViewModel<T>
                             where T : new()
    {
        public QueryPagingViewModel()
        {

        }

        public QueryPagingViewModel(T queryItems)
        {
            QueryItems = queryItems;
        }

        public T QueryItems { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
