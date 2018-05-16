namespace App.Common
{
    public interface IEffect
    {
        EAbility Type { get; }
        int Attack { get; }
        int Health { get; }
        int Radius { get; }
        int NumTurns { get; }
    }
}
