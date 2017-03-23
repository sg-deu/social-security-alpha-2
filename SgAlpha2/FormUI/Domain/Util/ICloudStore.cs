namespace FormUI.Domain.Util
{
    public interface ICloudStore
    {
        void Store(string folder, string cloudFilename, string metadataFilename, byte[] content, string contentType = "application/octet-stream");
    }
}