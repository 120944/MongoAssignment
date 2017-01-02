using MongoDB.Bson;

namespace MongoAssignment {
  public class Project : IConvertibleToBsonDocument {
    private int projectid;
    private string projectname;
    private double budget;
    private int allocatedhours;
    private string buildingname;

    public Project(int projectid, string projectname, double budget, int allocatedhours, string buildingname) {
      this.projectid = projectid;
      this.projectname = projectname;
      this.budget = budget;
      this.allocatedhours = allocatedhours;
      this.buildingname = buildingname;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "projectid" , projectid },
        { "projectname" , projectname },
        { "budget" , budget },
        { "allocatedhours" , allocatedhours },
        { "buildingname" , buildingname }
      };
    }
  }
}