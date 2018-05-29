namespace App.Model
{
    public interface IEffectModel
        : ICardModel
    {
        int Radius { get; }
        int NumTurns { get; }
    }
}
