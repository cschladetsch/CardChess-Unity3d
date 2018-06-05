using Flow;

namespace App.Agent
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
            Kernel = Flow.Create.Kernel();
            Factory = Kernel.Factory;
        }

        public override IAgent Prepare(IAgent agent)
        {
            agent.OnDestroy += (a) => a.Complete();
            //agent.Completed += (a) => (a as IAgent)?.Destroy(); //< creates a recursive cycle

            agent.Kernel = Kernel;
            agent.Construct();

            return Factory.Prepare(agent);
        }
    }
}
