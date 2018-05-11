using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adic;

namespace App
{
    public class Injection : Adic.ContextRoot
    {
        public override void SetupContainers()
        {
            AddContainer<InjectionContainer>()
                .Bind<Arbiter>().ToSingleton()
                .Bind<Model.IHand>().To<Model.Hand>()
                .Bind<Model.IDeck>().To<Model.Deck>()
                ;
        }

        public override void Init()
        {
        }
    }
}
