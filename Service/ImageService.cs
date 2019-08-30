using Microsoft.AspNetCore.Http;
using Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using UnitsOfWork;

namespace Service
{
	public class ImageService : IImageService
	{
		private readonly IUnitOfWork _unitOfWork;
		public ImageService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public OperationResult GetImages(string id)
		{
			if (id != null)
			{
				string folderName = Path.Combine("Resources", "Images", id);
				if (Directory.Exists(folderName))
				{
					string[] images = Directory.GetFiles(folderName);
					return new OperationResult { Data = images, Succeeded = true };
				}
			}
			return new OperationResult { Succeeded = false };
		}

		public OperationResult GetImagesForEdit(string id, string userId)
		{
			if (id != null)
			{
				string folderName = Path.Combine("Resources", "Images", "Temp", userId);
				copyImages(id, userId);
				if (Directory.Exists(folderName))
				{
					string[] images = Directory.GetFiles(folderName);
					return new OperationResult { Data = images, Succeeded = true };
				}
			}
			return new OperationResult();
		}

		public OperationResult UploadImage(string userId, IFormFileCollection files)
		{

			if (userId != null && files.Count > 0)
			{
				string folderName = Path.Combine("Resources", "Images", "Temp", userId);
				if (!Directory.Exists(folderName))
				{
					Directory.CreateDirectory(folderName);
				}
				string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
				List<string> imgPathes = new List<string>();
				foreach (var file in files)
				{
					var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					var fullPath = Path.Combine(pathToSave, fileName);
					var imgPath = Path.Combine(folderName, fileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					imgPathes.Add(imgPath);
				}

				return new OperationResult { Data = imgPathes, Succeeded = true };
			}
			return new OperationResult { Succeeded = false };
		}

		public OperationResult DeleteAllImages(string userId)
		{
			if (userId != null)
			{
				var folderName = Path.Combine("Resources", "Images", "Temp", userId);
				var pathWhereSaved = Path.Combine(Directory.GetCurrentDirectory(), folderName);
				if (Directory.Exists(pathWhereSaved))
				{
					Directory.Delete(pathWhereSaved, true);
				}
				if (!Directory.Exists(pathWhereSaved))
					return new OperationResult { Succeeded = true };
				else
					return new OperationResult { Succeeded = false };
			}
			else
				return new OperationResult { Succeeded = false };
		}

		public OperationResult DeleteOneImage(string userId, string imageName)
		{
			if (userId != null)
			{
				var folderName = Path.Combine("Resources", "Images", "Temp", userId);
				if (Directory.Exists(folderName))
				{
					var pathWhereSaved = Path.Combine(Directory.GetCurrentDirectory(), folderName);
					string file = Path.Combine(pathWhereSaved, imageName);

					System.IO.File.Delete(file);
					string[] imgPathes = Directory.GetFiles(folderName);
					return new OperationResult { Succeeded = true, Data = imgPathes };
				}
			}
			return new OperationResult { Succeeded = false };
		}



		public void copyImages(string id, string userId)
		{
			string dstDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "Temp", userId);
			if (Directory.Exists(dstDir))
			{
				foreach (var file in Directory.GetFiles(dstDir))
				{
					System.IO.File.Delete(file);
				}
			}
			else
				Directory.CreateDirectory(dstDir);


			string srcDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id);

			string[] files = Directory.GetFiles(srcDir);
			foreach (string file in files)
			{
				var fileName = Path.GetFileName(file);
				string dstFile = Path.Combine(dstDir, fileName);
				System.IO.File.Copy(file, dstFile);
			}

		}
	}
}

