namespace Community.API.Utilities.Wrappers
{
    public interface IHttpContextWrapper
    {
        T? GetFeature<T>();
    }
}
