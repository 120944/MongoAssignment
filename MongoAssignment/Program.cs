using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAssignment {
  class Program {
    protected static IMongoClient _client;
    protected static IMongoDatabase _database;

    static void Main(string[] args) {
      _client = new MongoClient();
      _database = _client.GetDatabase("assignment");

      var employees_doc = _database.GetCollection<BsonDocument>("employees");

      

      Console.Read();
    }
  }
}
