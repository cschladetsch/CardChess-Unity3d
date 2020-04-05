using Dekuple;

namespace App.Model
{
    public struct RequestResponse
    {
        public IRequest Request;
        public IResponse Response;

        public override string ToString() => $"{Request} {Response}";
    }
}