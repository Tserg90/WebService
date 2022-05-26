using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebService.Model;

namespace WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private FileStoregeContext storage;
        public FileController ()
        {
            storage = new FileStoregeContext();
        }
        public enum UploadState
        {
            Success,
            Failed,
        }
        [HttpGet]
        [Route("/getfile")]
        public async Task<IEnumerable<string>> GetFiles(CancellationToken cancellationToken)
        {
            return await storage.GetFiles(cancellationToken);
        }
        [HttpPost]
        [Route("/upload byte")]
        public UploadState Upload( byte[] content, string fileName, string mime, CancellationToken cancellationToken )
        {
            storage.Save(fileName, mime, content, cancellationToken);
            return UploadState.Success;
        }
        [HttpPost]
        [Route("/upload string")]
        public UploadState Upload1( string content, string fileName, string mime, CancellationToken cancellationToken )
        {
            storage.Save(fileName, mime, Encoding.UTF32.GetBytes(content), cancellationToken);
            return UploadState.Success;
        }
        [HttpPost]
        [Route("/download only content")]
        public async Task<byte[]> Download(string fileName, CancellationToken cancellationToken)
        {
            (byte[], string) file = await storage.Load(fileName, cancellationToken);
            return file.Item1;
        }
        [HttpPost]
        [Route("/download only mime")]
        public async Task<string> Download1(string fileName, CancellationToken cancellationToken)
        {
            (byte[], string) file = await storage.Load(fileName, cancellationToken);
            return file.Item2;
        }
        [HttpPost]
        [Route("/download full")]
        public async Task<(byte[], string)> Download2(string fileName, CancellationToken cancellationToken)
        {
            return await storage.Load(fileName, cancellationToken);
        }
    }
}
