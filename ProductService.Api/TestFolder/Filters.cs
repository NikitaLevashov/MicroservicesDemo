using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.RegularExpressions;

namespace ProductService.Api.TestFolder
{
    public class Filters
    {
    }

    public class SimpleResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.Now.ToString("dd/MM/yyyy hh-mm-ss"));
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // реализация отсутствует
        }
    }

    public class SimpleResourceFilterAsync : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            await Task.Run(() => 2 + 2 );
            await next();
        }
    }

    public class FakeNotFoundResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.Result = new ContentResult { Content = "Resource is not found" };
        }
    }

    public class WhitespaceAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var response = context.HttpContext.Response;
            // Если sitemap, то ничего не делаем
            if (context.HttpContext.Request.Path.ToString() == "/sitemap.xml") return;
            if (response.Body == null) return;
            response.Body = new SpaceCleaner(response.Body);
        }

        // вспомогательный класс для удаления пробелов
        private class SpaceCleaner : Stream
        {
            private readonly Stream outputStream;
            StringBuilder _s = new StringBuilder();

            public SpaceCleaner(Stream filterStream)
            {
                if (filterStream == null)
                    throw new ArgumentNullException("filterStream is not determined");
                outputStream = filterStream;
            }

            public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                var html = Encoding.UTF8.GetString(buffer, offset, count);
                //регулярное выражение для поиска пробелов между тегами
                var reg = new Regex(@"(?<=\s)\s+(?![^<>]*</pre>)");
                html = reg.Replace(html, string.Empty);
                buffer = Encoding.UTF8.GetBytes(html);
                await outputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
            // нереализованные методы Stream
            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
            public override bool CanRead { get { return false; } }
            public override bool CanSeek { get { return false; } }
            public override bool CanWrite { get { return true; } }
            public override long Length { get { throw new NotSupportedException(); } }
            public override long Position
            {
                get { throw new NotSupportedException(); }
                set { throw new NotSupportedException(); }
            }
            public override void Flush()
            {
                outputStream.Flush();
            }
            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }
            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }
        }
    }


}
