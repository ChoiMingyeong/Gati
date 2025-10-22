using System.Net.NetworkInformation;

namespace GatiToolkit.Core.WeightedRandom
{
    public class WeightedRandomItem
    {
        public uint ID { get; init; }

        public uint Weight { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public WeightedRandomItem(uint id)
        {
            ID = id;
        }

        public WeightedRandomItem(uint id, uint weight, string name = "", string description = "")
        {
            ID = id;
            Weight = weight;
            Name = name;
            Description = description;
        }
    }
}