namespace App.View.Impl1
{
    using UnityEngine;
    using UnityEngine.UI;
<<<<<<< HEAD
    using UniRx;
    using CoLib;
    using Dekuple.Agent;
    using Dekuple.View.Impl;
=======
    using TMPro;
    using CoLib;
    using Dekuple;
    using Dekuple.View;
    using Dekuple.View.Impl;
    using UniRx;
    using Common.Message;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
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
        public TextMeshProUGUI Text;

        private Ref<Vector3> _scale;

<<<<<<< HEAD
        public override void SetAgent(IAgent player)
        {
            // base.SetAgent(player, agent);
=======
        public override void SetAgent(IViewBase owner, IEndTurnButtonAgent agent)
        {
            var player = owner as IPlayerView;
            Assert.IsNotNull(player);
            base.SetAgent(player, agent);
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
            Agent.Model.Interactive.Subscribe(SetInteractive);
            Agent.Model.PlayerHasOptions.Subscribe(SetColor);
            
            Button.Bind(() => player.PushRequest(new TurnEnd(player.Agent.Model), TurnEnded));

            // pulsate the end button when there's nothing left to do
            _scale = Image.transform.ToScaleRef();
            _Queue.Sequence(
                Cmd.RepeatForever(
                    Cmd.PulsateScale(_scale, 0.085f, 1.2)
                    )
                )
             ;
            _Queue.Paused = true;
        }
        
        private void TurnEnded(IResponse obj)
        {
            // _AudioSource.PlayOneShot(EndTurnClips[0]);
            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.Success);
            Info($"TurnEnded for {obj.Request.Owner}");
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
