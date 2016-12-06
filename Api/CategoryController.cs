﻿using ltbdb.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Authorize(Policy = "AdministratorOnly")]
	[Route("api/[controller]/[action]")]
	public class CategoryController : Controller
	{
		private readonly CategoryService Category;

		public CategoryController(CategoryService category)
		{
			Category = category;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Category.Get(), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}
	}
}
