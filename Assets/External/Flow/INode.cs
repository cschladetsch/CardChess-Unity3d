// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    /// A Node is a Group that steps all referenced Generators when it itself is Stepped, and similarly for Pre and Post.
    public interface INode : IGroup
    {
        void Add(params IGenerator[] gens);
    }
}