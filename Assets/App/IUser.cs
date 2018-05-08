namespace App
{
    /// <summary>
    /// A Persistent user.
    /// </summary>
    public interface IUser : IHasId
    {
        string Name { get; }
        string Email { get; }
    }
}
