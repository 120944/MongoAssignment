using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace MongoAssignment {
  public class Employee : IConvertibleToBsonDocument {
    private int bsn;
    private string name;
    private string surname;
    private Address residence;
    private List<Address> addresses = new List<Address>();
    private List<Degree> degrees = new List<Degree>();
    private Headquarters headquarters;
    private List<Position> positions = new List<Position>();

    public Employee(int bsn, string name, string surname, Address residence, Headquarters headquarters) {
      this.bsn = bsn;
      this.name = name;
      this.surname = surname;
      this.residence = residence;
      this.headquarters = headquarters;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "bsn" , bsn },
        { "name" , name },
        { "surname" , surname },
        { "residence" , residence.ToBsonDocument() },
        { "addresses" , new BsonArray(addresses.Select(i => i.ToBsonDocument())) },
        { "degrees" , new BsonArray(degrees.Select(i => i.ToBsonDocument())) },
        { "headquarters" , headquarters.ToBsonDocument() },
        { "positions" , new BsonArray(positions.Select(i => i.ToBsonDocument())) }
      };
    }
  }
}