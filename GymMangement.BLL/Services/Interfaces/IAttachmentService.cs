using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangement.BLL.Services.Interfaces
{
    public interface IAttachmentService
    {
        //FileStream   عرض الصورة على الشاشة
        Task<string?> UploadAsync(Stream fileStream , string fileName , string folderName , CancellationToken ct = default);

    }
}
