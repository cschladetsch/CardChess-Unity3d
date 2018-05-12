using System.Collections.Generic;

namespace App.Model
{
    public interface IModel : IHasId
    {
    }

    public interface ICreated : IModel
    {
        bool Create();
    }

    public interface ICreated<A0> : IModel
    {
        bool Create(A0 a0);
    }

    public interface ICreated<A0, A1> : IModel
    {
        bool Create(A0 a0, A1 a1);
    }

    // A card in a Hand or Deck or Collection
    public interface ICard : IHasId
    {
        string Name { get; }
        string Description { get; }
        int Attack { get; }
        int Health { get; }
        IList<IEffect> Effects { get; }
    }
}
