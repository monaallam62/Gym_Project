using GymMangement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Classes
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly long _MaxFileSize = 5 * 1024 * 1024;
        private readonly string[] _AllowExtensions = {".png",".jpeg",".jpg"};


        public AttachmentService(ILogger<AttachmentService> logger , IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public bool Delete(string fileName, string folderName)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            try
            {
                if (!File.Exists(fullPath)) return false;
                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Failed To Delete File!");
                return false;
            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName)) return null;

            var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(fullPath)) return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(fullPath).ToLower();
            var contentType = extension switch
                {
                    ".png" => "image/png",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    _ => "application/octet-stream" //Binary Data
                };
            return (stream, contentType);
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0 ) return null;

            // Check File Size 
            if(fileStream.Length > _MaxFileSize )
            {
                _logger.LogError($"File Rejected : Too Large {fileStream.Length} Bytes");
                return null;
            }

            // Check Extensions 

            var Extension = Path.GetExtension( fileName );
            if(string.IsNullOrWhiteSpace(Extension) || !_AllowExtensions.Contains(Extension))
            {
                _logger.LogError($"File Rejected : This Extension Not Allowed!");
                return null;
            }

            // Locate Folder {MembersPhoto}
            //ContentRootPath => PL ,IWebHostEnvironment

            var uploadsFolder = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory( uploadsFolder );
            // If The Folder Exist It Will Return It , And If There No Folder It Will Create It 

            var storedFileName = $"{Guid.NewGuid()}{fileName}";
             
            var FilePath = Path.Combine( uploadsFolder, storedFileName );
            //D:\C#\Gym Project\Gym\Gym_Project\MembersPhoto\PhotoName.jpg

            try
            {
                // File Stream
                using var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Failed To Upload Photo!");
                return null;
            }


        }
    }
}
