using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Repositories.Models
{
    public class ApiException : Exception
    {
        private HttpStatusCode _httpStatusCode;
        public HttpStatusCode StatusCode { get => _httpStatusCode; }

        public ApiException() : base() { }

        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            _httpStatusCode = statusCode;
        }
    }
}
