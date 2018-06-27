using UnityEngine;
using UnityEngine.UI;

using App.Agent;
using UniRx;

namespace App.View.Impl1
{
    public class EndTurnButtonView
        : ViewBase<IEndTurnButtonAgent>
    {
        public Button Button;
        public Image Image;

        public override void SetAgent(IPlayerView player, IEndTurnButtonAgent agent)
        {
            base.SetAgent(player, agent);
            Button.BindToInteractable(Agent.Model.Interactive);
            Agent.Model.PlayerHasOptions.Subscribe(SetColor);
        }

        private void SetColor(bool hasOptions)
        {
            Image.GetComponent<Renderer>().material.color = hasOptions ? Color.white : Color.green;
        }
    }
}
