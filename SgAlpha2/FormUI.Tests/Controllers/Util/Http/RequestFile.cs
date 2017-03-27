namespace FormUI.Tests.Controllers.Util.Http
{
    public class RequestFile
    {
        private string  _fileName;
        private byte[]  _content;

        public RequestFile(string fileName, byte[] content)
        {
            _fileName = fileName;
            _content = content;
        }

        public string   FileName    { get { return _fileName; } }
        public byte[]   Content     { get { return _content; } }
    }
}
