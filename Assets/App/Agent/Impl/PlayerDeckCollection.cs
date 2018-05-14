using App.Model;

namespace App.Agent
{
    public class PlayerDeckCollection : CardCollection<ICardInstance>
    {
        public override int MaxCards => 50;
    }
}