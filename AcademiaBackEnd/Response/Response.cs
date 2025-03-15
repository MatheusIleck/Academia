using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AcademiaBackEnd.Response
{
    public class Response<TData>
    {
        private int _statusCode = 200;

        [JsonConstructor]
        public Response() => _statusCode = 200;

        public Response(TData? data,
       int statusCode = 200,
       string? message = null)
        {
            Data = data;
            _statusCode = statusCode;
            Message = message;

        }
        public TData? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => _statusCode is >= 200 and <= 299;
    }
}
