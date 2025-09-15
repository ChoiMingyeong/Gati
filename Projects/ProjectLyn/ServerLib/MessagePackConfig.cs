using MessagePack;
using MessagePack.Resolvers;

namespace ServerLib
{
    public static class MessagePackConfig
    {
        public static readonly MessagePackSerializerOptions Options =
            MessagePackSerializerOptions.Standard
                .WithResolver(CompositeResolver.Create(
                    StandardResolver.Instance
                ))
                .WithCompression(MessagePackCompression.Lz4Block);
    }
}


