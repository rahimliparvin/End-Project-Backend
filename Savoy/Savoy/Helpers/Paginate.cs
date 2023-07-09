namespace Savoy.Helpers
{
    public class Paginate<T>
    {
        public IEnumerable<T> Datas { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public Paginate(IEnumerable<T> datas, int currentPage, int totalPages)
        {
            Datas = datas;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public bool HasPrevious
        {
            get
            {
                return @CurrentPage > 1;
            }
        }

        public bool HasNext
        {
            get
            {
                return @CurrentPage < TotalPages;
            }
        }
    }
}

