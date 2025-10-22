namespace GatiToolkit.Core.WeightedRandom
{
    public class WeightedRandomTable
    {
        public Guid ID { get; init; }

        public string Name { get; set; } = string.Empty;

        public Dictionary<uint, WeightedRandomItem> Rows { get; set; } = [];

        private uint _rowIDCounter = 1;

        public string Description { get; set; } = string.Empty;

        public uint MinWeight { get; set; } = 1;

        private IReadOnlyDictionary<uint, uint> Prefix
        {
            get
            {
                uint acc = 0;
                return Rows.ToDictionary(p => p.Key, p => acc += p.Value.Weight);
            }
        }

        public IReadOnlyDictionary<uint, double> Rates
        {
            get
            {
                return Rows.ToDictionary(
                    p => p.Key,
                    p => (double)p.Value.Weight / Prefix.Last().Value * 100
                );
            }
        }

        public WeightedRandomTable(Guid id, params uint[] weights)
        {
            ID = id;
            foreach (var weight in weights)
            {
                AddRow(weight);
            }
        }

        public IReadOnlyDictionary<uint, uint> Pick(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            Dictionary<uint, uint> result = [];

            var prefix = Prefix;
            if (prefix.Count == 0)
            {
                return result;
            }

            uint total = prefix.Last().Value;


            var keys = prefix.Keys.ToArray();
            var values = prefix.Values.ToArray();

            for (int i = 0; i < count; ++i)
            {
                uint roll = (uint)Random.Shared.NextInt64(0, total);

                uint low = 0;
                uint high = (uint)values.Length - 1;
                while(low < high)
                {
                    uint mid = (low + high) >> 1;
                    if (roll < values[mid])
                    {
                        high = mid;
                    }
                    else
                    {
                        low = mid + 1;
                    }
                }

                uint key = keys[low];

                if (false == result.ContainsKey(key))
                {
                    result[key] = 0;
                }

                ++result[key];
            }

            return result;
        }

        public uint AddRow(uint weight, string name = "", string description = "")
        {
            WeightedRandomItem row = new(_rowIDCounter++)
            {
                Weight = (weight < MinWeight) ? MinWeight : weight,
                Name = name,
                Description = description
            };
            Rows.Add(row.ID, row);

            return row.ID;
        }

        public uint[] AddRows(params uint[] weights)
        {
            List<uint> ids = [];
            weights.ToList().ForEach(weight => ids.Add(AddRow(weight)));
            return [.. ids];
        }

        public bool RemoveRow(uint id)
        {
            return Rows.Remove(id);
        }

        public void ChangeWeight(uint id, uint newWeight)
        {
            if (false == Rows.ContainsKey(id))
            {
                return;
            }

            Rows[id].Weight = (newWeight < MinWeight) ? MinWeight : newWeight;
        }

        public void ChangeWeights(IReadOnlyDictionary<uint, uint> newWeights)
        {
            foreach (var (id, newWeight) in newWeights)
            {
                if (false == Rows.ContainsKey(id))
                {
                    continue;
                }

                Rows[id].Weight = (newWeight < MinWeight) ? MinWeight : newWeight;
            }
        }
    }
}