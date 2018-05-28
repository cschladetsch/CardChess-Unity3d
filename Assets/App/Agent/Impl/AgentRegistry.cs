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
        public INode Agents { get; }

        public AgentRegistry()
        {
            Kernel = Flow.Create.Kernel();
            Factory = Kernel.Factory;
            Agents = Factory.Node();
            Agents.Name = "Agents";
            Kernel.Root.Add(Agents);
        }

        public override IAgent Prepare(IAgent agent)
        {
            agent.OnDestroy += (a) => a.Complete();
            agent.Completed += (a) => (a as IAgent)?.Destroy();

            agent.Kernel = Kernel;
            return Factory.Prepare(agent);
        }
    }
}
