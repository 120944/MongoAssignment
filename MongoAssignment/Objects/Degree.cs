using MongoDB.Bson;

namespace MongoAssignment {
  public class Degree : IConvertibleToBsonDocument {
    private string course;
    private string school;
    private string level;

    public Degree(string course, string school, string level) {
      this.course = course;
      this.school = school;
      this.level = level;
    }

    public BsonDocument ToBsonDocument() {
      return new BsonDocument 
      {
        { "course" , course },
        { "school" , school },
        { "level" , level }  
      };
    }
  }
}