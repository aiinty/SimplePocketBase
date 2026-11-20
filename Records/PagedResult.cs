using System.Collections.Generic;

namespace SimplePocketBase.Records
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}
