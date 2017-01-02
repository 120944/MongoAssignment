using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace MongoAssignment {
  public class Employee : IConvertibleToBsonDocument {
    private int bsn;
    private string name;
    private string surname;

    public Employee(int bsn, string name, string surname) {
      this.bsn = bsn;
      this.name = name;
      this.surname = surname;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "bsn" , bsn },
        { "name" , name },
        { "surname" , surname }
      };
    }
  }
}