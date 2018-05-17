using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    class Registry
    {
        private Dictionary<Guid, IModel> Models = new Dictionary<Guid, IModel>();
    }
}
