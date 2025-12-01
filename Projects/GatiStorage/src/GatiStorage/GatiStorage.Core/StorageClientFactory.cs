namespace GatiStorage.Core
{
    public static class StorageClientFactory
    {
        public static T? Create<T>() where T : IStorageClient
        {
            return Activator.CreateInstance<T>();
        }
    }
}
