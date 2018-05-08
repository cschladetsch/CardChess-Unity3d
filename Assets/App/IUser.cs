namespace App
{
    public interface IUser : IHasId
    {
        string Name { get; }
    }
}