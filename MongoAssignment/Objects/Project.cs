using MongoDB.Bson;

namespace MongoAssignment {
  public class Project : IConvertibleToBsonDocument {
    private int projectid;
    private double budget;
    private int allocatedhours;
    private Headquarters headquarters;

    public Project(int projectid, double budget, int allocatedhours, Headquarters headquarters) {
      this.projectid = projectid;
      this.budget = budget;
      this.allocatedhours = allocatedhours;
      this.headquarters = headquarters;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "projectid" , projectid },
        { "budget" , budget },
        { "allocatedhours" , allocatedhours },
        { "address" , headquarters.ToBsonDocument() }
      };
    }
  }
}