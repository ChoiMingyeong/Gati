
namespace GatiArchive.Core
{
    public sealed class SplitArchiveWriter : IAsyncDisposable
    {
        //public SplitArchiveWriter(ArchiveOptions opt, )

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
