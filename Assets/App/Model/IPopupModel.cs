namespace App.Model
{
    using UniRx;
    using Dekuple.Model;

    /// <summary>
    /// Model for a popup.
    /// </summary>
    public interface IPopupModel
        : IModel
    {
        IReadOnlyReactiveProperty<string> Title { get; }
        IReadOnlyReactiveProperty<string> Text { get; }
        
        void Set(string title, string text);
    }
}