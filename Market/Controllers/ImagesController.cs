using Microsoft.AspNetCore.Mvc;
using Service;
using System.Linq;

namespace Market.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : Controller
	{
		private readonly IImageService _imageService;
		public ImagesController(IImageService imageService)
		{
			_imageService = imageService;
		}

		[HttpGet("{id}")]
		public IActionResult GetImages(string id)
		{
			return Ok(_imageService.GetImages(id).Data);
		}

		[HttpGet]
		[Route("ForEdit/{id}")]
		public IActionResult GetImagesForEdit(string id)
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			return Ok(_imageService.GetImagesForEdit(id, userId));
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("upload")]
		public IActionResult Upload()
		{
			string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			var files = Request.Form.Files;
			return Ok(_imageService.UploadImage(userId, files));
		}

		[HttpDelete]
		[Route("deleteAll")]
		public IActionResult DeleteAll()
		{
			var userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			return Ok(_imageService.DeleteAllImages(userId));
		}

		[HttpDelete]
		[Route("deleteOne/{imageName}")]
		public IActionResult DeleteOne(string imageName)
		{
			var userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
			return Ok(_imageService.DeleteOneImage(userId, imageName));
		}
	}
}