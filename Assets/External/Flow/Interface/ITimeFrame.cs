// (C) 2012-2018 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
    /// <summary>
    /// Stores information about a time step.
    /// </summary>
    public interface ITimeFrame
    {
        DateTime Last { get; }
        DateTime Now { get; }
        TimeSpan Delta { get; }
    }
}
