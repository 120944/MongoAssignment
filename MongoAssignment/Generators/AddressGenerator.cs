namespace MongoAssignment.Generators {
  public class AddressGenerator : IRandomGenerator<Address> {

    private string Country { get { return } }
    private string PostalCode { get { } }
    private string City { get { } }
    private string Street { get { } }
    private string Number { get { } }

    public AddressGenerator(int seed) : base(seed) {}

    public override Address Generate() {
      return new Address(Country,PostalCode,City,Street,Number);
    }
  }
}