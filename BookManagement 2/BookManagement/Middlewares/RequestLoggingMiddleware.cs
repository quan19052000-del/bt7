using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BookManagement.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var method = context.Request.Method;
            var path = context.Request.Path.ToString();

            Console.WriteLine($"[{time}] Method: {method} - Path: {path}");

            // Kiểm tra điều kiện
            if (path == "/Book/Detail/0" || path == "/Book/Detail/-1")
            {
                // Xử lý lỗi 400
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Book ID không hợp lệ");
            }
            else
            {
                // Nếu hợp lệ thì mới đi tiếp đến các Middleware/Controller khác
                await _next(context);
            }

            // Dòng này được đưa ra ngoài, nên NÓ SẼ LUÔN CHẠY
            Console.WriteLine($"Status Code: {context.Response.StatusCode}");
        }
    }
}