﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;

		public CategoryController(IMapper mapper, IOptionsSnapshot<Settings> settings, BookService book)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? ofs)
		{
			var _books = await BookService.GetAsync();
			var _page = _books.Skip(ofs ?? 0).Take(Options.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, Options.ItemsPerPage, _books.Count());

			var view = new BookViewAllContainer
			{
				Books = books,
				PageOffset = offset
			};

			return View(view);
		}

		[HttpGet]
		public async Task<IActionResult> View(string id, int? ofs)
		{
			var _books = await BookService.GetByCategoryAsync(id ?? String.Empty);
			if (_books.Count() == 0)
				return NotFound();

			var _page = _books.Skip(ofs ?? 0).Take(Options.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, Options.ItemsPerPage, _books.Count());

			var view = new BookViewCategoryContainer
			{
				Books = books,
				Category = id,
				PageOffset = offset
			};

			return View(view);
		}
	}
}
