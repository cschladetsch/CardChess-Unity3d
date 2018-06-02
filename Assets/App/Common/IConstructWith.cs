namespace App.Common
{
    /// <summary>
    /// Adds a creation method
    /// </summary>
    public interface IConstructWith
    {
        bool Construct();
    }

    public interface IConstructWith<in A0>
    {
        bool Construct(A0 agent);
    }

    public interface IConstructWith<in A0, in A1>
    {
        bool Construct(A0 a0, A1 a1);
    }

    public interface IConstructWith<in A0, in A1, in A2>
    {
        bool Construct(A0 a0, A1 a1, A2 a2);
    }
}
