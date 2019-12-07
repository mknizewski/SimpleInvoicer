namespace SimpleInvoicer.Application.Factory
{
    public static class ServiceFactory
    {
        public static I CreateInstance<T, I>() where T : I, new()
        {
            var instance = new T();
            return instance;
        }
    }
}
