using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class SearchController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public SearchController(IMapper mapper, Settings settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Options = settings;
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
		public IActionResult Search(string q, int? ofs)
		{
			var _books = Book.Search(q ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(Options.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, Options.ItemsPerPage, _books.Count());

			var view = new BookViewSearchContainer
			{
				Books = books,
				Query = q,
				PageOffset = offset
			};

			return View(view);
		}

		[HttpGet]
		public IActionResult SearchTitle(string term)
		{
			return Json(Book.Suggestions(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public IActionResult SearchCategory(string term)
		{
			return Json(Category.Suggestions(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public IActionResult SearchTag(string term)
		{
			return Json(Tag.Suggestions(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}