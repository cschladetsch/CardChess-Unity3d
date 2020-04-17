namespace App.View.Impl
{
    using UniRx;
    using TMPro;
    using Dekuple.View.Impl;
    using Agent;

    /// <inheritdoc cref="IPopupView" />
    public class PopupView
        : ViewBase<IPopupAgent>
        , IPopupView
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Text;

        public override bool AddSubscriptions()
        {
            base.AddSubscriptions();
            Agent.Model.Title.Subscribe(t => Title.text = t).AddTo(this);
            Agent.Model.Text.Subscribe(t => Text.text = t).AddTo(this);
            return true;
        }
    }
}
