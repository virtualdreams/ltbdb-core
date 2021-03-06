using LtbDb.Options;
using LtbDb.Services;
using LtbDb.WebAPI.V1.Contracts.Requests;
using LtbDb.WebAPI.V1.Contracts.Responses;
using LtbDb.WebAPI.V1.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;

namespace LtbDb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[ValidationFilter]
	public class UserController : ControllerBase
	{
		private readonly AppSettings AppSettings;
		private readonly BearerTokenService Token;

		public UserController(IOptionsSnapshot<AppSettings> settings, BearerTokenService token)
		{
			AppSettings = settings.Value;
			Token = token;
		}

		[HttpPost("authenticate")]
		public IActionResult Post([FromBody] AuthRequest model)
		{
			try
			{
				if (AppSettings.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && AppSettings.Password.Equals(model.Password))
				{
					var _response = new AuthSuccessResponse
					{
						Token = Token.CreateToken(AppSettings.AccessTokenKey, model.Username, "Administrator", AppSettings.AccessTokenExpire),
						Type = "Bearer",
						ExpiresIn = AppSettings.AccessTokenExpire
					};

					return Ok(_response);
				}

				return Unauthorized();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}