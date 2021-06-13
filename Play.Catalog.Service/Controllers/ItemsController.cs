using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers 
{
	// https://localhost:5001/items
	[ApiController]
	[Route("items")]
	public class ItemsController : ControllerBase
	{
		// making readonly cause we dont want to modify, only create during controller construction
		// static so list does not get created everytime someone invokes a method belonging here
		private static readonly List<ItemDto> items = new()
		{
			new ItemDto(Guid.NewGuid(), "Potion", "Restore a small amount of health", 5, System.DateTimeOffset.UtcNow),
			new ItemDto(Guid.NewGuid(), "Antidote", "Cures posion", 7, System.DateTimeOffset.UtcNow),
			new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, System.DateTimeOffset.UtcNow)
		};

		// GET /items
		[HttpGet]
		public IEnumerable<ItemDto> Get()
		{
			return items;
		}

		// GET /items/{id}
		[HttpGet("{id}")]
		public ItemDto GetById(Guid id) 
		{
			var item = items.Where(item => item.Id == id).SingleOrDefault();
			return item;
		}

		// POST /items
		[HttpPost]
		public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
		{
			var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
			items.Add(item);

			return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
		}

		// PUT /items/{id}
		[HttpPut("{id}")]
		public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
		{
			var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

			var updatedItem = existingItem with{
				Name = updateItemDto.Name,
				Description = updateItemDto.Description,
				Price = updateItemDto.Price,
			};

			var index = items.FindIndex(existingItem => existingItem.Id == id);
			items[index] = updatedItem;

			return NoContent();
		}

		// DELETE /items/{id}
		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			var index = items.FindIndex(existingItem => existingItem.Id == id);
			items.RemoveAt(index);

			return NoContent();
		}
	}
}