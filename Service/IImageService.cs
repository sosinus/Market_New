using Microsoft.AspNetCore.Http;
using Models;

namespace Service
{
	public interface IImageService
	{
		OperationResult GetImages(string id);

		OperationResult GetImagesForEdit(string id, string userId);

		OperationResult UploadImage(string userId, IFormFileCollection files);

		OperationResult DeleteAllImages(string userId);

		OperationResult DeleteOneImage(string userId, string imageName);

	}
}

