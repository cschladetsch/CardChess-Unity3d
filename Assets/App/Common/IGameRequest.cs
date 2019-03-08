using App.Common.Message;
using Dekuple;

namespace App.Common
{
    public interface IGameRequest : IRequest
    {
        EActionType Action { get; }
    }
}
