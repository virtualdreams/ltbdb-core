using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Services;
using ltbdb.WebAPI.V1.Contracts.Requests;
using ltbdb.WebAPI.V1.Filter;

namespace ltbdb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ValidationFilter]
	public class ImageController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;
		private readonly ImageService ImageService;

		public ImageController(IMapper mapper, IOptionsSnapshot<Settings> settings, BookService book, CategoryService category, TagService tag, ImageService image)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
			ImageService = image;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id, string type)
		{
			var _book = await BookService.GetByIdAsync(id);
			if (_book == null)
				return NotFound();

			var _thumbnail = String.Equals(type, "thumbnail");
			var _file = ImageService.GetPhysicalPath(_book.Filename, _thumbnail ? ImageType.Thumbnail : ImageType.Normal);
			if (String.IsNullOrEmpty(_file))
				return NotFound();

			return PhysicalFile(_file, MediaTypeNames.Image.Jpeg);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromForm]ImageRequest model)
		{
			try
			{
				var _book = await BookService.GetByIdAsync(id);
				if (_book == null)
					return NotFound();

				await BookService.SetImageAsync(_book.Id, model.Image.OpenReadStream());

				return NoContent();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var _book = await BookService.GetByIdAsync(id);
			if (_book == null)
				return NotFound();

			await BookService.SetImageAsync(_book.Id, null);

			return NoContent();
		}
	}
}