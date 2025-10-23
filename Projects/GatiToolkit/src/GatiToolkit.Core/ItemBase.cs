using TSID.Creator.NET;

namespace GatiToolkit.Core
{
    public abstract class ItemBase
    {
        public string ID => _id.ToString();
        readonly protected Tsid _id = TsidCreator.GetTsid();

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeDescription(string description)
        {
            Description = description;
        }
    }
}
