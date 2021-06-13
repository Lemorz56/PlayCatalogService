using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
	public class ItemsRepository
	{
		private const string collectionName = "items";
		private readonly IMongoCollection<Item> dbCollection;
		private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

		public ItemsRepository()
		{
			var mongoClient = new MongoClient("mongodb+srv://Lemorz56:Hibbert66!@clusterstorelocator.ddp8q.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
			var database = mongoClient.GetDatabase("NETMicroservice");
			dbCollection = database.GetCollection<Item>(collectionName);
		}

		// Get all items in db in a list
		public async Task<IReadOnlyCollection<Item>> GetAllAsync()
		{
			return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
		}

		// Get specific item
		public async Task<Item> GetAsync(Guid id)
		{
			FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
			return await dbCollection.Find(filter).FirstOrDefaultAsync();
		}

		// Add item to db
		public async Task CreateAsync(Item entity)
		{
			if (entity == null)
			{
				throw new ArgumentException(nameof(entity));
			}

			await dbCollection.InsertOneAsync(entity);
		}

		public async Task UpdateAsync(Item entity)
		{
			if (entity == null)
			{
				throw new ArgumentException(nameof(entity));
			}

			FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
			await dbCollection.ReplaceOneAsync(filter, entity);
		}

		public async Task RemoveAsync(Guid id)
		{
			FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
			await dbCollection.DeleteOneAsync(filter);
		}
	}
}