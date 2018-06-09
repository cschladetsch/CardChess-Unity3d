namespace App.Model
{
    using Common.Message;

    struct TryAction
    {
        public IRequest Request;
        public IResponse Response;

        public TryAction(IRequest request, IResponse response)
        {
            Request = request;
            Response = response;
        }
    }
}
