using System.Collections.Generic;

namespace CoLib
{

/// <summary>
/// The CommandQueue class is one of core primitives for running commands.
/// It operates, as its name suggests, as a FIFO queue. All Commands Enqueued
/// to the queue run in sequential order. When it is fed time via Update, it
/// will remove Commands from the queue as they complete. 
/// </summary>
public class CommandQueue
{
	#region Properties

	/// <summary>
	/// Returns true iff the queue is doing something
	/// </summary>
	public bool Running {  get { return _currentCommand != null || _commandDelegates.Count != 0; } }
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="CommandQueue"/> is paused.
	/// </summary>
	/// <value>
	/// <c>true</c> if paused; otherwise, <c>false</c>.
	/// </value>
	public bool Paused { get; set; }

	/// <summary>
	/// Gets the elapsed time since the current executing CommandDelegate started.
	/// </summary>
	/// <value>
	/// The elapsed time since the current executing CommandDelegate started. For instance
	/// the if the queue  is half way through a 5 second CommandDuration, <c>ElapsedTime</c>
	/// will be equal to 2.5 seconds.
	/// </value>
	public double DeltaTimeAccumulation { get { return _deltaTimeAccumulation; } }

	/// <summary>
	/// Indicates whether the CommandQueue is currently in an update loop. Update should
	/// never be again while this is true.
	/// </summary>
	/// <value><c>true</c> if updating; otherwise, <c>false</c>.</value>
	public bool Updating { get { return _updateRunning; }}
	
	#endregion

	#region Public Methods
	
	/// <summary>
	/// Enqueue the specified command. Commands are queued up in the order specified.
	/// Multiple calls to <c>Enqueue<</c> result is the same sequential ordering ie. 
	/// <code>
	/// 	CommandQueue queue = new CommandQueue();
	/// 	queue.Enqueue(commandOne);
	/// 	queue.Enqueue(commandTwo);
	/// 	// Is equivalent to
	/// 	queue.Enqueue(commandOne, commandTwo);
	/// </code>
	/// </summary>
	/// <param name='commands'>
	/// The <c>CommandDelegate</c>s to be enqueued. The <c>CommandQueue</c> will 
	/// dequeue the commands over succesive calls to Update. Must be non-null.
	/// </param>
	/// <exception cref="System.ArgumentNullException"></exception>
	public CommandQueue Enqueue(params CommandDelegate[] commands)
	{
		foreach (CommandDelegate command in commands) {
			if (command == null) {
				throw new System.ArgumentNullException();
			}
			_commandDelegates.Enqueue(command);
		}
		return this;
	}

	/// <summary>
	/// Updates the queue with a zero time update. This will make sure the first available command is started.
	/// </summary>
	public void Process()
	{
		// If we are already in an update loop, then just let the queue continue running.
		if (!Updating)
		{
			Update(0.0);
		}
	}

	/// <summary>
	/// Tries to update a queue until it has complete. Note, this can result in an infinite loop if
	/// commands in the queue rely on external state changes.
	/// </summary>
	public void RunToEnd()
	{
		Update(double.MaxValue);
	}
	
	public bool Update(double deltaTime)
	{
		return Update(ref deltaTime);
	}
	
	/// <summary>
	/// Updates the <c>CommandQueue</c>. This causes CommandDelegates to be executed
	/// in the order than are enqueued. Update will return after an <c>CommandDelegate</c>
	/// elects to pause. This method can't be called recursively.
	/// </summary>
	/// <param name='deltaTime'>
	/// The time, in seconds, since the last update. Must be >= 0.
	/// </param>
	/// <returns>
	/// If the queue is finished as no <c>CommandDelegate</c>s remain, returns <c>true</c>,
	/// <c>false</c> otherwise. 
	/// </returns>
	/// <exception cref="System.ArgumentOutOfRangeException"></exception>
	/// <exception cref="System.InvalidOperationException"></exception>
	public bool Update(ref double deltaTime)
	{
		if (deltaTime < 0.0) {
			throw new System.ArgumentOutOfRangeException("deltaTime","deltaTime is expected to be positive.");
		}
		if (_updateRunning) { // Guard against recursive calls.
			throw new System.InvalidOperationException("Update can't be called recursively.");
		}
		_updateRunning = true;
		
		try {
			if (!Paused) {
				_deltaTimeAccumulation += deltaTime;
				bool shouldRun = _commandDelegates.Count != 0 || _currentCommand != null;
				while (shouldRun) {
					if (_currentCommand == null) {
						_currentCommand = _commandDelegates.Dequeue();
					}
			
					bool finished = _currentCommand(ref _deltaTimeAccumulation);
					if (finished) {
						_currentCommand = null;
					}
			
					// Only run again if an action just finished, 
					// (indicated by currentCommand == null), and we have more actions.
					shouldRun = finished && _commandDelegates.Count != 0 && !Paused;
				}
			}
			bool done = _commandDelegates.Count == 0 && _currentCommand == null;
			return done;
		} finally {
			_updateRunning = false;
			deltaTime = _deltaTimeAccumulation;
			if (_currentCommand == null) {
				_deltaTimeAccumulation = 0.0;
			}
		}
	}
	
	#endregion
	
	#region Private fields
	
	private Queue<CommandDelegate> _commandDelegates = new Queue<CommandDelegate>();
	private CommandDelegate _currentCommand = null;
	private double _deltaTimeAccumulation = 0.0;
	private bool _updateRunning = false;
	
	#endregion
	
}
	
}