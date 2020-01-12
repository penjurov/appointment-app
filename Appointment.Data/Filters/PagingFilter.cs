using Appointment.Data.Helpers;
using System;

namespace Appointment.Data.Filters
{
    [Serializable]
    public class PagingFilter
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int Start
        {
            get
            {
                return (this.PageNumber - 1) * this.PageSize;
            }
        }

        public string SortBy { get; set; }

        public string Direction { get; set; }

        public SortDirection SortDirection
        {
            get
            {
                return Sort.GetDirection(this.Direction);
            }
        }

        public long Count { get; set; }

        public PagingFilter CopyPaging(PagingFilter filter)
        {
            this.PageSize = filter.PageSize;
            this.PageNumber = filter.PageNumber;
            this.SortBy = filter.SortBy;
            this.Direction = filter.Direction;

            return this;
        }
    }
}
