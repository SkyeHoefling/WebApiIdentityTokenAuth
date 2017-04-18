using System.Collections.Generic;
using System.Net;

namespace WebApiIdentityTokenAuth.Models
{
    public class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<Response> Errors { get; set; }

        public Response(HttpStatusCode code)
        {
            Code = (int)code;
            Errors = new List<Response>();
        }
    }
}