namespace App.View.Impl
{
    using UnityEngine.UI;
    using UniRx;
    using Dekuple.View.Impl;
    using Agent;

    /// <inheritdoc cref="IPopupView" />
    public class PopupView
        : ViewBase<IPopupAgent>
        , IPopupView
    {
        public Text Title;
        public Text Text;

        public override bool AddSubscriptions()
        {
            if (!base.AddSubscriptions())
                return false;
            
            Agent.Model.Title.Subscribe(t => Title.text = t).AddTo(this);
            Agent.Model.Text.Subscribe(t => Text.text = t).AddTo(this);
            return true;
        }
    }
}
