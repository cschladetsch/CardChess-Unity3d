using System;
using System.Collections.Generic;

namespace CoLib
{
	public class CommandQueueGroup 
	{
		#region Public methods

		/// <summary>
		/// Creates a queue, which will be updated by the group.
		/// </summary>
		/// <returns>The new queue.</returns>
		public CommandQueue CreateQueue()
		{
			var queue = new CommandQueue();
			AddQueue(queue);
			return queue;
		}

		/// <summary>
		/// Adds the queue, to be updated by the group.
		/// </summary>
		/// <param name="queue">The CommandQueue for the behaviour to update.</param>
		public void AddQueue(CommandQueue queue)
		{
			if (queue == null) {
				throw new ArgumentNullException("queue");
			}

			if (!_queues.Contains(queue) && !_newQueues.Contains(queue)) {
				_newQueues.Add(queue);
			}

			if (_queuesToRemove.Contains(queue)) {
				_queuesToRemove.Remove(queue);
			}
		}

		/// <summary>
		/// Removes a queue the group. 
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

			if (_queues.Contains (queue) && !_queuesToRemove.Contains(queue)) {
				_queuesToRemove.Add(queue);
			}
			if (_newQueues.Contains(queue)) {
				_newQueues.Remove(queue);
			}
		}

		/// <summary>
		/// Updates all the queues in the group by the specified deltaTime.
		/// </summary>
		/// <param name="deltaTime">The amount of time to update all the queues by.</param>
		public void Update(double deltaTime)
		{
			var queuesToUpdate = new Queue<CommandQueue>(_queues);

			do 
			{
				_queues.AddRange(_newQueues);
				foreach (CommandQueue newQueue in _newQueues) {
					queuesToUpdate.Enqueue(newQueue);
				}
				foreach (CommandQueue queueToRemove in _queuesToRemove) {
					_queues.Remove(queueToRemove);
				}
				_newQueues.Clear();
				_queuesToRemove.Clear();

				if (queuesToUpdate.Count > 0) {
					CommandQueue queue = queuesToUpdate.Dequeue();
					if (_queues.Contains(queue)) {
						queue.Update(deltaTime);
					}
				}
			} while (queuesToUpdate.Count > 0);
		}

		#endregion

		#region Private fields

		private List<CommandQueue> _newQueues = new List<CommandQueue>();
		private List<CommandQueue> _queuesToRemove = new List<CommandQueue>();
		private List<CommandQueue> _queues = new List<CommandQueue>();

		#endregion
	}
}
