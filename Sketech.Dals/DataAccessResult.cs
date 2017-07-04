using System;

namespace Sketech.Dals
{
    public class DataAccessResult<T>
    {
        public bool HasError
        {
            get { return Error != null; }
        }

        public Exception Error { get; set; }

        public int TotalResultCount { get; set; }

        public T Value { get; set; }
    }
}
