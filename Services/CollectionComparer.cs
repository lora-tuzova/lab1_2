namespace lab1_2.Services
{
    public class CollectionComparer : IEqualityComparer<IEnumerable<string>>
    {
        public bool Equals(IEnumerable<string> x, IEnumerable<string> y)
        {
            // Compare the collections of strings by checking if they have the same elements in the same order
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<string> obj)
        {
            // Combine the hash codes of individual elements
            return obj.Aggregate(0, (hash, str) => hash ^ str.GetHashCode());
        }
    }

}
