namespace SendOwl.Endpoints
{
    public abstract class Endpoint
    {
        public virtual string Path { get; }
        internal readonly IHttpSerializerClient httpClient;

        public Endpoint(IHttpSerializerClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}
