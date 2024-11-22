using System;

namespace WEB_253502_POBORTSEVA.UI.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)

        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
