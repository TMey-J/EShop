using EShop.Application.Constants.Common;
using Newtonsoft.Json;
using System.Net;

namespace EShop.Api
{
    public class ApiResult
        (bool isSuccess, HttpStatusCode statusCode, string? messege = Messages.Successful, object? data = null)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        public string? Messege { get; set; } = messege;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; } = data;
    }
}
