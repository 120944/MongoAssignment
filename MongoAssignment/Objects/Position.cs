using MongoDB.Bson;

namespace MongoAssignment {
  public class Position : IConvertibleToBsonDocument {
    private string positionname;
    private string description;
    private double hourlyfee;
    private int hours;
    private int projectid_ref;

    public Position(string positionname, string description, double hourlyfee, int hours, int projectidRef) {
      this.positionname = positionname;
      this.description = description;
      this.hourlyfee = hourlyfee;
      this.hours = hours;
      projectid_ref = projectidRef;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "positionname" , positionname },
        { "description" , description },
        { "hourlyfee" , hourlyfee },
        { "hours" , hours },
        { "projectid_ref" , projectid_ref }  
      };
    }
  }
}