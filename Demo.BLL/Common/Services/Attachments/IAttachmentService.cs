using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.Attachments
{
	public interface IAttachmentService
	{
		// Upload , Delete

		Task<string?> UploadAsync(IFormFile file, string folderName);
		bool Delete(string filePath);
	}
}
