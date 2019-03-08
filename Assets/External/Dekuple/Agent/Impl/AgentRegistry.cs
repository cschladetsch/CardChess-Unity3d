using Flow;

namespace Dekuple.Agent
{
    using Registry;

    /// <summary>
    /// Factory and registry for creating agents.
    ///
    /// Agents have a Model, and are also ITransient
    /// and do work over time in a Flow.Kernel.
    /// </summary>
    public class AgentRegistry
        : Registry<IAgent>
    {
        public IKernel Kernel { get; }
        public IFactory Factory { get; }

        public AgentRegistry()
        {
            Kernel = Create.Kernel();
            Factory = Kernel.Factory;
        }

        public override IAgent Prepare(IAgent agent)
        {
            base.Prepare(agent);
            agent.OnDestroyed += a => a.Complete();
            return Factory.Prepare(agent);
        }
    }
}
