using EShop.Application.Constants.Common;
using Newtonsoft.Json;
using System.Net;

namespace EShop.Api
{
    public class ApiResult
        (bool isSuccess, HttpStatusCode statusCode, string message = Messages.Successful, object? data = null)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; } = data;
    }
}
