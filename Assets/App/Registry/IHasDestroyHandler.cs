namespace App.Registry
{
    public delegate void DestroyedHandler<T>(T model);

    public interface IHasDestroyHandler<T>
    {
        event DestroyedHandler<T> OnDestroy;
    }
}
