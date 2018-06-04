using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Agent;

namespace App.View.Impl1
{
    public class PlayerView
        : ViewBase<IPlayerAgent>
        , IPlayerView
    {
        public ManaBar ManaBar;
        public IHandView Hand;
        public IDeckView Deck;
    }
}
