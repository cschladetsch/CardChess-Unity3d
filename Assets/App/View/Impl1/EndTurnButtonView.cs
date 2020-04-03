using CoLib;
using Dekuple.Agent;
using Dekuple.View;
using Dekuple.View.Impl;
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

        private Ref<Vector3> _scale;

        public override void SetAgent(IViewBase player, IAgent agent)
        {
            base.SetAgent(player, agent);
            Agent.Model.Interactive.Subscribe(SetInteractive);
            Agent.Model.PlayerHasOptions.Subscribe(SetColor);

            // pulsate the end button when there's nothing left to do
            _scale = Image.transform.ToScaleRef();
            _Queue.Enqueue (
                Commands.RepeatForever(
                    Commands.PulsateScale(_scale, 0.085f, 1.2)
                    )
                )
             ;
            _Queue.Paused = true;
        }

        private void SetInteractive(bool interactive)
        {
            _Queue.Paused = true;
            Button.interactable = interactive;
            if (!Button.interactable)
                Image.material.color = Color.grey;
        }

        private void SetColor(bool hasOptions)
        {
            _scale.Value = Vector3.one;
            _Queue.Paused = true;
            if (!Button.interactable)
            {
                Image.material.color = Color.grey;
                return;
            }
            if (hasOptions)
            {
                Image.material.color = Color.white;
                return;
            }

            Image.material.color = Color.green;
            _Queue.Paused = false;
        }
    }
}
