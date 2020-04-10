using App.Common;

namespace App.Model
{
    /// <inheritdoc />
    /// <summary>
    /// Model for a player in the game
    /// </summary>
    public class PlayerModel
        : PlayerModelBase
    {
        public PlayerModel(EColor color) : base(color)
        {
        }

        public override void StartGame()
        {
            base.StartGame();
            Info($"{this} StartGame");
        }
    }
}
