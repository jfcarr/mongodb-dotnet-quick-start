using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DotMongo
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var databaseName = "testdb";

				// Get database reference.
				var mongoDatabase = GetDatabaseReference("localhost", 27017, databaseName);
				Console.WriteLine($"Connected to database {databaseName}");

				// Get a reference to the "people" collection inside testdb.
				var collection = mongoDatabase.GetCollection<BsonDocument>("people");

				// We're retrieving all documents in the collection,
				// but we still need an empty filter.
				var filter = new BsonDocument();
				var count = 0;

				// Open a cursor with all the matching documents.
				using (var cursor = collection.FindSync<BsonDocument>(filter))
				{
					// Iterate through the cursor
					while (cursor.MoveNext())
					{
						// Get documents at the current cursor location.
						var batch = cursor.Current;

						foreach (var document in batch)
						{
							// Get values from the current document, then display them.
							var firstName = document.GetElement("firstName").Value.ToString();
							var lastName = document.GetElement("lastName").Value.ToString();

							Console.WriteLine($"Full name: {firstName} {lastName}");
							count++;
						}
					}
				}
				Console.WriteLine($"Total records: {count}");

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public static IMongoDatabase GetDatabaseReference(string hostName, int portNumber, string databaseName)
		{
			string connectionString = $"mongodb://{hostName}:{portNumber}";

			// Connect to MongoDB
			var mongoClient = new MongoClient(connectionString);

			// Get a reference to the specified database
			var mongoDatabase = mongoClient.GetDatabase(databaseName);

			return mongoDatabase;
		}


	}
}
