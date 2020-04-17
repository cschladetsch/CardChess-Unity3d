namespace App.Model.Impl
{
    using UniRx;
    using Dekuple.Model;
    
    /// <inheritdoc cref="IPopupModel" />
    public class PopupModel
        : ModelBase
        , IPopupModel
    {
        public IReadOnlyReactiveProperty<string> Title => _title;
        public IReadOnlyReactiveProperty<string> Text => _text;

        private ReactiveProperty<string> _title = new ReactiveProperty<string>();
        private ReactiveProperty<string> _text = new ReactiveProperty<string>();

        public void Set(string title, string text)
        {
            _title.Value = title;
            _text.Value = text;
        }
    }
}


