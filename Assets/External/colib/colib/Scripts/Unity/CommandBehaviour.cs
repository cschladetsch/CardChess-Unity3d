using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoLib
{

/// <summary>
/// The CommandBehaviour is designed to give quick access to scheduling
/// commands. Typically used in conjunction with the GameObject extension
/// methods.
/// </summary>
public class CommandBehaviour : MonoBehaviour
{
	#region Public methods

	/// <summary>
	/// Create a new Queue with the specified commands. The CommandBehaviour
	/// will take care of updating the Queue.
	/// </summary>
	/// <returns> A new Queue that the Commands will run on. </returns>
	/// <param name="commands">Commands to be run sequentially.</param>
	public CommandQueue Queue(params CommandDelegate[] commands)
	{
		var queue = new CommandQueue ();
		if (commands.Length > 0) {
			queue.Enqueue (commands);
		}
		_queues.Add (queue);
		return queue;
	}

	/// <summary>
	/// Adds the queue, to be updated the Behaviour.
	/// </summary>
	/// <param name="queue">The CommandQueue for the behaviour to update.</param>
	public void AddQueue(CommandQueue queue)
	{
		if (queue == null) {
			throw new ArgumentNullException("queue");
		}

		if (!_queues.Contains(queue)) {
			_queues.Add(queue);
		}
	}

	/// <summary>
	/// Removes a queue the behaviour. 
	/// </summary>
	/// <param name="queue">
	/// The CommandQueue to remove. This queue should have been created, or added
	/// to this behaviour already.
	/// </param>
	public void RemoveQueue(CommandQueue queue)
	{
		if (queue == null) {
			throw new ArgumentNullException ("queue");
		}

		if (_queues.Contains (queue)) {
			_queues.Remove (queue);
		}
	}

	/// <summary>
	/// Schedule a list of commands.
	/// </summary>
	/// <param name="commands"> The commands to be executed sequentially.</param>
	public void Schedule(params CommandDelegate[] commands)
	{
		_scheduler.Add(Commands.Sequence(commands));
	}

	#endregion

	#region MonoBehaviour events
	
	private void Update()
	{
		foreach (var queue in _queues) {
			queue.Update (Time.deltaTime);
		}

		_scheduler.Update (Time.deltaTime);
	}
	
	#endregion
	
	#region Private fields

	private HashSet<CommandQueue> _queues = new HashSet<CommandQueue>();
	private CommandScheduler _scheduler = new CommandScheduler();

	#endregion
}

}
