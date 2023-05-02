using System.Net;
using System.Text;
using System.Web;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Server is waiting for connections...");
            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.HttpMethod == "GET")
                {
                    await GetHandlerAsync(context);
                } else if (context.Request.HttpMethod == "POST")
                {
                    await PostHandlerAsync(context);
                }
            }
        }
        static async Task PostHandlerAsync(HttpListenerContext context)
        {
            var message = string.Empty;
            using (var sr = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                message = await sr.ReadToEndAsync();
            }

            var postData = HttpUtility.ParseQueryString(message);
            Console.WriteLine($"Message from client: {message}");
            var name = postData["name"];
            var age = postData["age"];
            message = postData["message"];
            var html = $"<html><head></head><body> Hello {name}, your age is {age} </body></html>";
            var buf = Encoding.UTF8.GetBytes(html);
            context.Response.ContentLength64 = buf.Length;
            await context.Response.OutputStream.WriteAsync(buf, 0, buf.Length);
            context.Response.OutputStream.Close();
        }
        static async Task GetHandlerAsync(HttpListenerContext context)
        {
            var message = context.Request.QueryString["message"];
            var name = context.Request.QueryString["name"];
            var age = context.Request.QueryString["age"];
            Console.WriteLine($"Message from client: {message}");
            var html = $"<html><head></head><body> Hello {name}, your age is {age} </body></html>";
            var buf = Encoding.UTF8.GetBytes(html);
            context.Response.ContentLength64 = buf.Length;
            await context.Response.OutputStream.WriteAsync(buf, 0, buf.Length);
            context.Response.OutputStream.Close();
        }
    }
}