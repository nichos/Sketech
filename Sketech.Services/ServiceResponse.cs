using System;

namespace Sketech.Services
{
    public class ServiceResponse<T>
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
