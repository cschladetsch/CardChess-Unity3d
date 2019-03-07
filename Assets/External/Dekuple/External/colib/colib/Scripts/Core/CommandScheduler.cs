using System.Collections.Generic;

namespace CoLib
{

/// <summary>
/// CommandScheduler takes commands, and runs them in parallel.
/// The CommandScheduler is single threaded, but with a single
/// call to update, several different commands can be updated
/// and running at the same time. 
/// </summary>
public class CommandScheduler
{
	#region Properties

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="CommandScheduler"/> is paused.
	/// </summary>
	/// <value>
	/// <c>true</c> if paused; otherwise, <c>false</c>.
	/// </value>
	public bool Paused { get; set; }

	#endregion
	
	#region Public Methods

	/// <summary>
	/// Initializes a new instance of the <see cref="CommandScheduler"/> class. Unlike the
	/// <c>CommandQueue</c>, which executes actions sequentially, the CommandScheduler will
	/// execute any action given to it in parallel.
	/// </summary>>
	public CommandScheduler() {}
	
	/// <summary>
	/// Add a command to be executed in parallel.
	/// </summary>
	/// <param name="command"> 
	/// The command to execute. Should be non-null.
	/// </param>
	/// <exception cref="System.ArgumentNullException"></exception>
	public void Add(CommandDelegate command)
	{
		if (command == null) {
			throw new System.ArgumentNullException("command");
		}
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(command);
		_queues.AddLast(queue);
	}
	
	/// <summary>
	/// Updates the scheduler's deltaTime. This will in turn update the deltaTimes of any
	/// command running om this scheduler.
	/// </summary>
	/// <param name='deltaTime'>
	/// The deltaTime, in seconds. Must be >= 0.
	/// </param>
	/// <exception cref="System.ArgumentOutOfRangeException"></exception>
	public void Update(double deltaTime)
	{
		if (deltaTime < 0.0) {
			throw new System.ArgumentOutOfRangeException("deltaTime","deltaTime is expected to be positive.");
		}
		if (!Paused) {
			var node = _queues.First;
		
			while (node != null) {
				var next = node.Next;
				bool finished = node.Value.Update(deltaTime);
				if (finished) {
					_queues.Remove(node);
				}
				
				node = next;
			}
		}
	}
	
	#endregion
	
	#region Private fields
	
	private LinkedList<CommandQueue> _queues = new LinkedList<CommandQueue>();
	
	#endregion
}

}