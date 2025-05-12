using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.Attachments
{
	public class AttachmentService : IAttachmentService
	{
		// Allowed Extension {".png", ".jpg" , ".jpeg"}

		public readonly List<string> _allowedExtension = new() { ".png", ".jpg", ".jpeg" };

		// MAX SIZE => 2MB
		public const int _allowedMaxSize = 2_097_152;

		public async Task<string?> UploadAsync(IFormFile file, string folderName)
		{
			// 1] Validation For File Extensions => {".png", ".jpg" , ".jpeg"}

			var extension = Path.GetExtension(file.FileName); // Doaa.jpeg

			if (!_allowedExtension.Contains(extension))
				return null;

			// 2] Validation For Max Size [2GB]
			if (file.Length > _allowedMaxSize)
				return null;

			// 3] Get Located Folder Path

			var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			// 4] Must Image Name Be Unique
			var fileName = $"{Guid.NewGuid()}{extension}";

			// 5] Get FilePath [FolderPath + FileName]
			var filePath = Path.Combine(folderPath, fileName);

			// 6] Save File As Streams [Data per Time]

			var fileStream = new FileStream(filePath, FileMode.Create);

			// 7] Copy File To FileStream

			await file.CopyToAsync(fileStream);

			// 8] Return FileName
			return fileName;


		}
		public bool Delete(string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				return true;
			}

			return false;
		}

		/*
			* File streaming :
			1]You Open the Image File
			FileStream establishes a connection between your program and the image file.
			The file is opened in read, write, or both modes.
			
			2]You Read or Write Binary Data
			Images are stored as bytes (0s and 1s), not as human-readable text.
			
			3]You Close the File After Use
			Keeping an image file open too long can cause file locks or memory issues.
			Use using to ensure proper closure.
		*/


		/*
		 FileMode.Create	Creates a new file. Overwrites if the file already exists.
		 FileMode.Open	    Opens an existing file. Throws an exception if it doesn't exist.
		 FileMode.Append	Opens the file if it exists or creates a new one. Data is written to the end of the file.
		 FileMode.Truncate	Opens an existing file and clears its content.
		 FileMode.OpenOrCreate	Opens the file if it exists or creates a new one.
		 */
	}
}
