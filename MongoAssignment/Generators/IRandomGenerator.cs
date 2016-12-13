using System;

namespace MongoAssignment.Generators {
  public abstract class IRandomGenerator<T> {
    readonly protected Random r;

    protected IRandomGenerator(int seed) {
      r = new Random(seed);
    }

    public abstract T Generate();
  }
}