namespace MongoAssignment.Generators {
  public class EmployeeGenerator : IRandomGenerator<Employee> {
    public EmployeeGenerator(int seed) : base(seed) {
    }

    public override Employee Generate() {
      return new Employee(
        r.Next(1, 10000), 
        Resources.FirstNames[r.Next(Resources.FirstNames.Length)],
        Resources.LastNames[r.Next(Resources.LastNames.Length)]);
    }
  }
}