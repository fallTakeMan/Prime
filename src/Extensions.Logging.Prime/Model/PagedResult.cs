
namespace Extensions.Logging.Prime.Model
{
    internal class PagedResult<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }

        public Paging Paging { get; set; }

        public PagedResult(IEnumerable<T> list, int pageIndex, int pageSize, int total)
        {
            this.Data = list;
            this.Paging = new Paging(pageIndex, pageSize, total);
        }

    }

    internal class Paging
    {
        /// <summary>
        /// 当前分页索引
        /// </summary>
        public int CurrentPageIndex { get; set; } = 1;
        /// <summary>
        /// 分页数据条数
        /// </summary>
        public int PageSize { get; set; } = 20;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get { return (int)Math.Ceiling(TotalCount / (double)PageSize); } }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => CurrentPageIndex > 1;
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage => CurrentPageIndex < PageCount;

        public Paging(int index, int size, int total)
        {
            CurrentPageIndex = index;
            PageSize = size;
            TotalCount = total;
        }

    }
}
