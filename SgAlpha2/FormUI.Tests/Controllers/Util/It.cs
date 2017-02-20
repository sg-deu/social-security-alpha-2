namespace FormUI.Tests.Controllers.Util
{
    public static class It
    {
        public static T IsAny<T>()
        {
            return default(T);
        }
    }
}
