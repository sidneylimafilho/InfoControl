using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoControl
{
    public sealed class DateTimeInterval
    {
        public DateTimeInterval(DateTime begin, DateTime end) { _beginDate = begin; _endDate = end; }

        private DateTime _beginDate;
        public DateTime BeginDate { get { return _beginDate; } }

        private DateTime _endDate;
        public DateTime EndDate { get { return _endDate; } }

        public static DateTimeInterval ThisYear
        {
            get
            {
                return new DateTimeInterval(
                    new DateTime(DateTime.Today.Year, 1, 1),
                    new DateTime(DateTime.Today.Year, 12, 31));
            }
        }
    }
}
