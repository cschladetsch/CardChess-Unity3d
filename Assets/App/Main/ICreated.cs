namespace App
{
    public interface ICreated
    {
        bool Create();
    }

    public interface ICreated<in A0>
    {
        bool Create(A0 a0);
    }

    public interface ICreated<in A0, in A1>
    {
        bool Create(A0 a0, A1 a1);
    }

    public interface ICreated<in A0, in A1, in A2>
    {
        bool Create(A0 a0, A1 a1, A2 a2);
    }
}
