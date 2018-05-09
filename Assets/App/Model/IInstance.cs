namespace App.Model
{
    // An instance of playing a Card. This could be a creature, item, spell, etc.
    public interface IInstance : IHasId
    {
        // the card used to make this instance
        ICard Card { get; }

        // the owner of the instance
        IOwner Owner { get; }
    }
}
