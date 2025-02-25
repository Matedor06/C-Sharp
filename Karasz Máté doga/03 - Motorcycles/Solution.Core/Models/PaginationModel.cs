using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Models
{
    public class PaginationModel<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
    }
}
