using MongoDB.Bson;

namespace MongoAssignment {
  public class Headquarters : IConvertibleToBsonDocument {
    private string buildingname;
    private int numberofrooms;
    private double monthlyrent;
    private Address address;

    public Headquarters(string buildingname, int numberofrooms, double monthlyrent, Address address) {
      this.buildingname = buildingname;
      this.numberofrooms = numberofrooms;
      this.monthlyrent = monthlyrent;
      this.address = address;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "buildingname" , buildingname },
        { "numberofrooms" , numberofrooms },
        { "monthlyrent" , monthlyrent },
        { "address" , address.ToBsonDocument() }
      };
    }
  }
}