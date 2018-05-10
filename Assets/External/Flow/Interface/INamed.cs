// (C) 2012-2018 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
    public delegate void NameChangeHandler(INamed named, string newName, string oldName);

    /// Fires its Renamed event when its Name property is changed.
    public interface INamed
    {
        event NameChangeHandler Renamed;
        string Name { get; set; }
    }
}
