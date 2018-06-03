namespace App.Common.Message
{
    using Model;

    public class DrawCard : RequestBase
    {
        public DrawCard(IPlayerModel player)
            : base(player, EActionType.DrawCard)
        {
        }
    }
}
