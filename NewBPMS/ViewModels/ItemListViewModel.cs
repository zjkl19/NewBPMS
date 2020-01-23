using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewBPMS.ViewModels
{
    public class ItemListViewModel<T> where T : class
    {
        public IEnumerable<T> ItemViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
