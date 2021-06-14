namespace Play.Catalog.Service.Settings
{
	public class MongoDbSettings
	{
		public string User { get; init; }
		public string Pass { get; init; }
		public string Db { get; init; }

		public string ConnectionString => $"mongodb+srv://{User}:{Pass}@clusterstorelocator.ddp8q.mongodb.net/{Db}?retryWrites=true&w=majority";
	}
}