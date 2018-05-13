// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{

    public interface ITimedBarrier : IBarrier, ITimesOut
    {
    }

    public interface ITimedTrigger : ITrigger, ITimesOut
    {
    }

    public interface ITimedNode : INode, ITimesOut
    {
    }
}
