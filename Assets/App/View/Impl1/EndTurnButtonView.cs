using UnityEngine;
using UnityEngine.UI;

using UniRx;

namespace App.View.Impl1
{
    using Agent;

    /// <summary>
    /// Scene view of the EndTurn button.
    ///
    /// Can be in a few states:
    /// <ul>Not active (is other player's turn)</ul>
    /// <ul>Active - current player's turn and there are things left to do</ul>
    /// <ul>Highlighted - current player's turn, but there is nothing left to do</ul>
    /// </summary>
    public class EndTurnButtonView
        : ViewBase<IEndTurnButtonAgent>
        , IEndTurnButtonView
    {
        public Button Button;
        public Image Image;

        public override void SetAgent(IPlayerView player, IEndTurnButtonAgent agent)
        {
            base.SetAgent(player, agent);
            Button.BindToInteractable(Agent.Model.Interactive);
            Agent.Model.PlayerHasOptions.Subscribe(SetColor);
            //Agent.Model.Interactive.Subscribe(i => Image.material.color = i ? Color.white : Color.grey);
        }

        private void SetColor(bool hasOptions)
        {
            // TODO: this is actually tri-state: not my turn, my turn and something to do, my turn and nothing to do
            Image.material.color = hasOptions ? Color.white : Color.green;
        }
    }
}
