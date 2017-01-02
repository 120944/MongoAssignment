using System;

namespace MongoAssignment.Generators {
  public class ProjectGenerator : IRandomGenerator<Project> {
    public ProjectGenerator(int seed) : base(seed) {
    }

    public override Project Generate() {
      var projectid = r.Next(1, 10000);
      return new Project(
        r.Next(10000, int.MaxValue),
        "PJ0-" + projectid,
        Math.Floor(r.NextDouble()*r.Next(100000,1000000)*100)/100,
        r.Next(2000,20000),
        Resources.Buildings[r.Next(Resources.Buildings.Length)]);
    }
  }
}