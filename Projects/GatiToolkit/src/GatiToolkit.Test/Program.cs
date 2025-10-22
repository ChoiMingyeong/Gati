using GatiToolkit.Core.WeightedRandom;

namespace GatiToolkit.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WeightedRandomTable table = new(Guid.NewGuid(), 10, 20, 30, 40)
            {
                Name = "Test Table",
                Description = "This is a test weighted random table."
            };

            foreach (var (id, rate) in table.Rates)
            {
                Console.WriteLine($"Picked ID: {id}, Rate: {rate}%");
            }

            foreach (var (id, count) in table.Pick(100))
            {
                Console.WriteLine($"Picked ID: {id}, Count: {count}");
            }
        }
    }
}
