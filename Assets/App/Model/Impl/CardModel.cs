using System;
using System.Collections.Generic;
using System.Linq;
using App.Model.Card;
using UniRx;

// event not used
#pragma warning disable 67

namespace App.Model
{
    using Common;
    using Common.Message;

    public class CardModel :
        ModelBase,
        ICardModel
    {
        #region Public Properties

        public ICardTemplate Template { get; }
        public ECardType Type => Template.Type;
        public EPieceType PieceType => Template.PieceType;
        public string Description => Template.FlavourText;

        public IReactiveProperty<IPlayerModel> Player => _player;
        public IReadOnlyReactiveProperty<int> ManaCost => _manaCost;
        public IReadOnlyReactiveProperty<int> Power => _power;
        public IReadOnlyReactiveProperty<int> Health => _health;
        public IReactiveCollection<IItemModel> Items => _items;
        public IReactiveCollection<EAbility> Abilities => _abilities;
        public IReactiveCollection<IEffectModel> Effects => _effects;
        public Response TakeDamage(ICardModel other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Methods
        public CardModel(ICardTemplate template, IOwner owner)
        {
            LogSubject = this;
            Template = template;
            SetOwner(owner);
        }

        int Clamp(int min, int max, int val)
        {
            return val < min ? min : (val > max ? max : val);
        }

        public override bool Construct(IOwner owner)
        {
            if (!base.Construct(owner))
                return false;

            _power = new IntReactiveProperty(Template.Power);
            _health = new IntReactiveProperty(Template.Health);
            _manaCost = new IntReactiveProperty(Template.ManaCost);

            //_power.SkipWhile(n => n < 0).SkipWhile(n => n > 10).AsObservable().

            // make copies as the effects, abilities and items on a card model
            // may change during the game.
            if (Template.Items != null)
                _itemList = Template.Items.ToList();
            if (Template.Abilities != null)
                _abilityList = Template.Abilities.ToList();
            if (Template.Effects != null)
                _effectList = Template.Effects.ToList();

            _items = new ReactiveCollection<IItemModel>(_itemList);
            _abilities = new ReactiveCollection<EAbility>(_abilityList);
            _effects = new ReactiveCollection<IEffectModel>(_effectList);

            _health.Subscribe(h => { if (h <= 0) Info("Died"); });

            _effects.ObserveAdd().Subscribe(e => Info($"Added Effect {e}"));
            _items.ObserveAdd().Subscribe(e => Info($"Added Item {e}"));
            _abilities.ObserveAdd().Subscribe(e => Info($"Added Ability {e}"));

            _effects.ObserveRemove().Subscribe(e => Info($"Removed Effect {e}"));
            _items.ObserveRemove().Subscribe(e => Info($"Removed Item {e}"));
            _abilities.ObserveRemove().Subscribe(e => Info($"Removed Ability {e}"));

            return true;
        }

        int Min(int a, int b) { return a < b ? a : b; }
        int Max(int a, int b) { return a > b ? a : b; }

        public void ChangeHealth(int change)
        {
            _health.Value += change;
        }

        public void ChangeManaCost(int change)
        {
            _manaCost.Value = Max(0, _manaCost.Value + change);
        }

        public void ChangePower(int change)
        {
            _power.Value = Max(0, _power.Value + change);
        }

        public override string ToString()
        {
            return $"{Player}'s {PieceType}";
        }

        #endregion

        #region Private Fields
        private IntReactiveProperty _power;
        private IntReactiveProperty _health;
        private IntReactiveProperty _manaCost;
        private ReactiveProperty<IPlayerModel> _player;

        private ReactiveCollection<IItemModel> _items;
        private ReactiveCollection<EAbility> _abilities;
        private ReactiveCollection<IEffectModel> _effects;

        private List<IItemModel> _itemList;
        private List<EAbility> _abilityList;
        private List<IEffectModel> _effectList;
        #endregion
    }
}


