using App.Model;

namespace App.Agent
{
    using Common;

    public class PlayerHandCollection : CardCollection<ICardInstance>
    {
        public override int MaxCards => 7;
    }
}
