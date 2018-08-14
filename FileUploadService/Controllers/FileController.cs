using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FileUploadService.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/");

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                File.WriteAllBytes(Path.Combine(root, filename), buffer);
            }

            return Ok();
        }
    }
}
