using App.Model;

namespace App.Agent
{
    public class PlayerHandCollection : CardCollection<ICardInstance>
    {
        public override int MaxCards => 7;
    }
}
