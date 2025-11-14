using GatiNetwork.Core.RecordStructs;
using System.Reflection;
namespace GatiNetwork.Core.Packets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketProtocolAttribute<TProtocolCodeGroup> : Attribute
        where TProtocolCodeGroup : notnull, IProtocolCodeGroup
    {
        private static readonly Dictionary<Type, List<int>> _cachedCodeGroup = [];

        public ProtocolCode ProtocolCode { get; }

        public PacketProtocolAttribute(int protocolCode)
        {
            var protocolGroupType = typeof(TProtocolCodeGroup);
            if(false == _cachedCodeGroup.TryGetValue(protocolGroupType, out var group))
            {
                group = [.. protocolGroupType
                    .GetFields(BindingFlags.Static)
                    .Where(p => true == p.IsLiteral && false == p.IsInitOnly)
                    .Select(p => (int)p.GetValue(null)!)];
                _cachedCodeGroup[protocolGroupType] = group;
            }

            if (false == group.Contains(protocolCode))
            {
                throw new ArgumentException($"ProtocolCode {protocolCode}가 {protocolGroupType.FullName}에 정의되어 있지 않습니다.");
            }

            ProtocolCode = protocolCode;
        }

    }
}
