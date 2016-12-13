using MongoDB.Bson;

namespace MongoAssignment {
  public class Address : IConvertibleToBsonDocument {
    private string country;
    private string postalcode;
    private string city;
    private string street;
    private string number;

    public Address(string country, string postalcode, string city, string street, string number) {
      this.country = country;
      this.postalcode = postalcode;
      this.city = city;
      this.street = street;
      this.number = number;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "country" , country },
        { "postalcode" , postalcode },
        { "city" , city },
        { "street" , street },
        { "number" , number }
      };
    }
  }
}