namespace SharedKernel.Responses;

public class PageApiResponse(int pageNumber, int pageSize) : ApiResponse
{
    public int PageNumber
    {
        get { return _pageNumber > 0 ? _pageNumber : 1; }
        private set
        {
            if (value > 0)
            {
                _pageNumber = value;
            }
        }
    }
    public int PageSize
    {
        get { return _pageSize > 0 ? _pageSize : 1; }
        private set
        {
            if (value > 0)
            {
                _pageSize = value;
            }
        }
    }
    private int TotalRecords { get; set; }
    public int TotalPages
    {
        get
        {
            var t = (double)TotalRecords / PageSize;
            var res = (int)Math.Ceiling(t);
            return res;
        }
    }

    private int _pageNumber = pageNumber;
    private int _pageSize = pageSize;
}
