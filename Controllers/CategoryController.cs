﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService Book;

		public CategoryController(IMapper mapper, Settings settings, BookService book)
		{
			Mapper = mapper;
			Options = settings;
			Book = book;
		}

		[HttpGet]
		public IActionResult Index(int? ofs)
		{
			var _books = Book.Get().OrderBy(o => o.Category);
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
		public IActionResult View(string id, int? ofs)
		{
			var _books = Book.GetByCategory(id ?? String.Empty);
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
