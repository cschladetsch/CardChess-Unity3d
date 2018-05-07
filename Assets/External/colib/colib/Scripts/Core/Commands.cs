using System;
using System.Collections;
using System.Collections.Generic;

namespace CoLib
{

	/// <summary>
	/// The base building block for all commands.
	/// </summary>
	/// <remarks>
	/// This is what the CommandQueue and CommandScheduler update. 
	/// DeltaTime is the time to update the command by. The delegate should
	/// modify deltaTime, subtracting the time it has consumed.
	/// The delegate returns true when it has completed, and false
	/// otherwise. Once the delegate has completed, the next call
	/// should restart it.
	/// </remarks>
	public delegate bool CommandDelegate(ref double deltaTime);

	public delegate void CommandDo();
	public delegate bool CommandCondition();
	public delegate void CommandDuration(double t);
	public delegate CommandDelegate CommandFactory();
	public delegate IEnumerator<CommandDelegate> CommandCoroutine();

	static public partial class Commands
	{
		#region Public methods

		/// <summary>
		/// An <c>CommandDo</c> runs precisely once.
		/// </summary>
		/// <param name="command"> 
		/// The command to execute. Must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate Do(CommandDo command)
		{
			CheckArgumentNonNull(command);
			return (ref double deltaTime) => {
				command();
				return true;
			};
		}

		public static CommandDelegate If(bool @true, CommandDelegate command)
		{
			return @true ? command : None();
		}

		public static CommandDelegate IfElse(bool @true, CommandDelegate @if, CommandDelegate @else)
		{
			return @true ? @if : @else;
		}

		/// <summary>
		/// A command which does nothing. Can be useful as a return value
		/// when null is not expected.
		/// </summary>
		public static CommandDelegate None()
		{
			return (ref double deltaTime) => true;
		}
		
		/// <summary>
		/// An <c>CommandDuration</c> runs over a duration of time.
		/// </summary>
		/// <param name="command">
		/// The command to execute. Must be non-null.
		/// </param>
		/// <param name="duration">
		/// The duration of time, in seconds, to apply the command over. 
		/// Must be greater than or equal to 0.
		/// </param>
		/// <param name="ease">
		/// An easing function to apply to the <c>t</c> parameter of an
		/// <c>CommandDuration</c> delegate. If null, linear easing is used.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentOutOfRange"></exception>
		public static CommandDelegate Duration(CommandDuration command, double duration, CommandEase ease = null)
		{
			CheckArgumentNonNull(command);
			CheckDurationGreaterThanOrEqualToZero(duration);
			if (duration == 0.0) {
				// Sometimes it is convenient to create duration commands with
				// a time of zero, so we have a special case.
				return (ref double deltaTime) => {
					double t = 1.0;
					if (ease != null) { t = ease(t); }
					command(t);
					return true;
				};
			}

			double elapsedTime = 0.0;

			return (ref double deltaTime) => {
				elapsedTime += deltaTime;
				deltaTime = 0.0;
				double t = (elapsedTime / duration);
				t = t < 0.0 ? 0.0 : (t > 1.0 ? 1.0 : t);
				if (ease != null) { t = ease(t); }
				command(t);
				bool finished = elapsedTime >= duration;
				if (finished) { 
					deltaTime = elapsedTime - duration;
					elapsedTime = 0.0; 
				}
				return finished;
			};
		}
		

		/// <summary>
		/// A Wait command does nothing until duration has elapsed
		/// </summary>
		/// <param name="duration"> 
		/// The duration of time, in seconds, to wait. Must be greater than 0.
		/// </param>
		/// <exception cref="System.ArgumentOutOfRange"></exception>
		public static CommandDelegate WaitForSeconds(double duration)
		{
			CheckDurationGreaterThanZero(duration);
			double elapsedTime = 0.0;
			return (ref double deltaTime) => {
				elapsedTime += deltaTime;
				deltaTime = 0.0f;
				bool finished = elapsedTime >= duration;
				if (finished) { 
					deltaTime = elapsedTime - duration;
					elapsedTime = 0.0f; 
				}
				return finished;
			};
		}

		/// <summary>
		/// Waits a specified number of calls to update. This ignores time althogether.
		/// </summary>
		/// <param name="frameCount">
		/// The number of frames to wait. Must be > 0.
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException"></exception>
		public static CommandDelegate WaitForFrames(int frameCount)
		{
			if (frameCount <= 0) { throw new System.ArgumentOutOfRangeException("frameCount",frameCount, "frameCount must be > 0."); }
			int counter = frameCount;
			return (ref double deltaTime) => {
				if (counter > 0) {
					--counter;
					deltaTime = 0;
					return false;
				}
				counter = frameCount;
				return true;

			};
		}
		
		/// <summary>
		/// A Parallel command executes several commands in parallel. It finishes
		/// when the last command has finished.
		/// </summary>
		/// <param name="command"> 
		/// The command to execute. Must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate Parallel(params CommandDelegate[] commands)
		{
			foreach (var command in commands) {
				CheckArgumentNonNull(command);
			}

			// Optimization.
			if (commands.Length == 0) { return Commands.None(); }
			if (commands.Length == 1) { return commands[0]; }

			BitArray finishedCommands = new BitArray(commands.Length, false);

			return (ref double deltaTime) => {
				bool finished = true;
				double smallestDeltaTime = deltaTime;
				for (int i = 0; i < commands.Length; ++ i) {
					if (finishedCommands[i]) { continue; }
					double deltaTimeCopy = deltaTime;
					bool thisFinished = commands[i](ref deltaTimeCopy); 
					finishedCommands[i] = thisFinished;
					finished = finished && thisFinished;
					smallestDeltaTime = System.Math.Min(deltaTimeCopy, smallestDeltaTime);
				}

				if (finished) {
					finishedCommands.SetAll(false);
				}
				deltaTime = smallestDeltaTime;
				return finished;
			};
		}
		
		/// <summary>
		/// A Sequence command executes several commands sequentially.
		/// </summary>
		/// <param name="commands"> 
		/// A parameter list of commands to execute sequentially. All commands must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate Sequence(params CommandDelegate[] commands)
		{
			foreach (var command in commands) {
				CheckArgumentNonNull(command);
			}

			// Optimization.
			if (commands.Length == 0) { return Commands.None(); }
			if (commands.Length == 1) { return commands[0]; }

			int index = 0;
			return (ref double  deltaTime) => {
				bool finished = true;
				while (finished) {
					finished = commands[index](ref deltaTime);
					if (finished) { index += 1; }
					if (index == commands.Length) {
						index = 0;
						return true;
					}
				}
				return false;
			};
		}
		
		/// <summary>
		/// A  Queue command allows Commands to be nested recursively in queues. Queues
		/// are different to Sequences in that they are depletable, (so be careful if
		/// you are wrapping a queue in a Repeat command).
		/// </summary>
		/// <param name="queue"> 
		/// The queue to execute. Must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate Queue(CommandQueue queue)
		{
			CheckArgumentNonNull(queue, "queue");
			return (ref double deltaTime) => {
				return queue.Update(ref deltaTime);
			};
		}
		
		/// <summary>
		/// A Condition command allows branching behaviour. After a condition evaluates to <c>true</c>
		/// then onTrue will be evaluated until it finishes. Otherise onFalse will be evaluated, (if it 
		/// isn't null). When nested in a Repeat command, conditions will be re-evaluated once for every
		/// repeat.
		/// </summary>
		/// <param name="condition"> 
		/// The condition to evaluate. Must be non-null.
		/// </param>
		/// <param name="onTrue"> 
		/// The command to execute if condition evaluates to true. Must be non-null.
		/// </param>
		/// <param name="onFalse"> 
		/// The command to execute if condition evaluates to false.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate Condition(CommandCondition condition, CommandDelegate onTrue, CommandDelegate onFalse = null)
		{
			CheckArgumentNonNull(condition, "condition");
			CheckArgumentNonNull(onTrue, "onTrue");
			CommandDelegate result = onFalse;
			return Sequence(
				Commands.Do( () => {
					result = onFalse;
					if (condition()) {
						result = onTrue;
					}
				}),
				(ref double deltaTime) => {
					if (result != null) {
						return result(ref deltaTime);
					}
					return true;
				}
			);
		}
			
		// <summary>
		/// Require the specified condition to be true to continue executing the given command. 
		/// </summary>
		/// <param name='condition'>
		/// A condition which must remain true to continue executing the commands. Must be non-null.
		/// </param>
		/// <param name='command'>
		/// A deferred command. Must be non-null. This command is deferred, so that if the require condition fails,
		/// the command can be safely restarted.
		/// </param>
		/// <remarks>
		/// The condition is only re-evaluated on new calls to Update, or after the child command finishes and restarts.
	    /// This means that if the condition suddenly becomes false while the command is executing, it
	    /// won't be immediately escaped.
		/// </remarks>
		/// <example>
		/// <code>
		/// 	CommandQueue queue = new CommandQueue();
		/// 	queue.Enqueue(
		/// 		Commands.Require( () => someObject != null,
		/// 			() => Commands.MoveTo(someObject, somePosition, someDuration)
		/// 		)
		/// 	);
		/// </code>
		/// </example>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate Require(CommandCondition condition,  CommandFactory deferredCommand)
		{
			CheckArgumentNonNull(condition, "condition");
			CheckArgumentNonNull(deferredCommand, "deferredCommand");

			CommandDelegate command = null;			
			
			return (ref double deltaTime) => {
				if (command == null) {
					command = deferredCommand();
				}
				if (command == null) { return true; }

				bool finished = condition() ? command(ref deltaTime) : true;
				if (finished) {
					command = null;
				}
				return finished;
			};
		}

		/// <summary>
		/// Loops over the specified commands, re-evaluating the condition at the start of every loop.
		/// </summary>
		/// <param name='condition'>
		/// A condition which must remain true to continue executing the commands. Must be non-null.
		/// </param>
		/// <param name='commands'>
		/// A list of commands to be executed while condition is true. Must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate While(CommandCondition condition, params CommandDelegate[] commands)
		{
			CheckArgumentNonNull(condition, "condition");
			
			CommandDelegate sequence;
			if (commands.Length == 0) {
				sequence = Commands.WaitForFrames(1);
			} else {
				sequence = Commands.Sequence(commands);
			}

			bool finished = true;
			return (ref double deltaTime) => {
				if (!finished) {
					finished = sequence(ref deltaTime);
				}
				while (finished) {
					if (!condition()) {
						return true;
					}
					finished = sequence(ref deltaTime);
				}
				return false;
			};
		}
		
		/// <summary>
		/// The Repeat command repeats a delegate a given number of times.
		/// </summary>
		/// <param name="repeatCount"> 
		/// The number of times to repeat the given command. Must be > 0.
		/// </param>
		/// <param name='commands'>
		/// The command sto repeat. All of the basic commands, (except for Queue),
		/// are repeatable without side-effects. When writing your own Commands,
		/// be careful to make sure state inside the command can be reset. A simple
		/// way to do this is to wrap the command inside a Sequence.
		/// <code>
		/// 	int counter = 0;
		/// 	CommandDelegate someCommand = Commands.Sequence(
		/// 		Commands.Do(delegate() {
		/// 			// Reset state here. 
		/// 			counter = 0;
		/// 		}),
		/// 		Commands.While(() => {
		/// 			counter++;
		/// 			Debug.Log(counter);
		/// 			return (counter <= 5);
		/// 		})
		/// 	);
		/// </code>
		/// Must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentOutOfRangeException"></exception>
		public static CommandDelegate Repeat(int repeatCount, params CommandDelegate[] commands)
		{
			if (repeatCount <= 0) { 
				throw new System.ArgumentOutOfRangeException("repeatCount",repeatCount, "repeatCount must be > 0."); 
			}
			foreach (var command in commands) {
				CheckArgumentNonNull(command);
			}
			CommandDelegate sequence = Commands.Sequence(commands);
			int count = 0;
			return (ref double deltaTime) => {
				bool finished = true;
				while (finished && count < repeatCount) {
					finished = sequence(ref deltaTime);
					if (finished) { count++; }
				}
				count %= repeatCount;
				return finished;
			};
		}
		
		/// <summary>
		/// Repeats a command forever.
		/// </summary>
		/// <remarks>
		/// Make sure that the commands you are repeating will consume some time,
		/// otherwise this will create a infinite loop.
		/// </remarks>
		/// <param name="commands"> 
		/// The commands to execute. Must be non-null.
		/// </param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static CommandDelegate RepeatForever(params CommandDelegate[] commands)
		{
			foreach (var command in commands) {
				CheckArgumentNonNull(command);
			}
			CommandDelegate sequence = Commands.Sequence(commands);
			return (ref double deltaTime) => {
				bool finished = true;
				while (finished) {
					finished = sequence(ref deltaTime);
				}
				return false;
			};
		}
		
		/// <summary>
		/// Creates a command which runs a coroutine.
		/// </summary>
		/// <param name='command'>
		/// The command to generate the coroutine.
		/// </param>
		/// <remarks>
		/// The reason this method doesn't just except an IEnumerator is that
		/// IEnumerators created from continuations can't be reset, (a continuation is
		/// any method containing a yield statement, and returning an IEnumerator).  This means
		/// that coroutines would break when executed within a repeat command.
		/// By encapsulating the call to create the IEnumerator in a delegate, it is possible for a
		/// user to call the coroutine however they please, and for it to be repeatable.
		/// </remarks>
		/// <example>
		/// <code>
		/// 	private CommandQueue _queue = new CommandQueue();
		/// 
		///     IEnumerator<CommandDelegate> CoroutineMethod(int firstVal, int secondVal, int thirdVal)
		///     {
		///			Debug.Log(firstVal);
		/// 		yield return Commands.WaitForSeconds(1.0f); // You can return any CommandDelegate here.
		/// 		Debug.Log(secondVal);
		/// 		yield return null; // Wait a single frame.
		/// 		Debug.Log(thirdVal);
		/// 		yield break; // Force exits the coroutine.
		///     }
		///     
		///     IEnumerator<CommandDelegate> CoroutineNoArguments()
		///     {
		///        yield return Commands.WaitForSeconds(2.0);
		///     }
		/// 
		///     void Start() 
		/// 	{ 
		/// 		_queue.Enqueue(
		/// 			Commands.Coroutine( () => CoroutineMethod(1,2,3)),
		///             Commands.Coroutine(CoroutineNoArguments)
		/// 		);
		/// 	}
		/// 
		/// 	void Update()
		/// 	{
		/// 		_queue.Update(Time.deltaTime);
		/// 	}
		/// 	
		/// </code>
		/// </example>
		public static CommandDelegate Coroutine(CommandCoroutine command)
		{
			CheckArgumentNonNull(command);
			IEnumerator<CommandDelegate> coroutine = null;
			CommandDelegate currentCommand = null;

			return (ref double deltaTime) => {
				// Create our coroutine, if we don't have one.
				if (coroutine == null) {
					coroutine = command();
					// Finish if we couldn't create a coroutine.
					if (coroutine == null) { return true; } 
				}

				bool finished = true;
				while (finished) {
					// Set the current command.
					if (currentCommand == null) {
						if (!coroutine.MoveNext()) {
							coroutine = null;
							return true;
						}
						currentCommand = coroutine.Current;
						if (currentCommand == null) {
							// Yield return null will wait a frame, like with
							// Unity coroutines.
							currentCommand = Commands.WaitForFrames(1);
						}
					}
					finished = currentCommand(ref deltaTime);
					if (finished) { currentCommand = null; }
				}
				return false;
			};
		}

		/// <summary>
		/// Chooses a random child command to perform. Re-evaluated on repeat.
		/// </summary>
		/// <param name='commands'>
		/// A list of commands to choose from at random. Only one command will be performed.
		/// Null commands can be passed. At least one command must be specified.
		/// </param>
		/// <exception cref='System.ArgumentException'> </exception>
		public static CommandDelegate ChooseRandom(params CommandDelegate[] commands)
		{
			if (commands.Length == 0) {
				throw new System.ArgumentException("Must have at least one command parameter.", "commands");
			}
				
			System.Random random = new System.Random();
			return Commands.Defer( () => commands[random.Next(0, commands.Length)]);
		}

		/// <summary>
		/// Defers the creation of the Command until just before the point of execution.
		/// </summary>
		/// <param name="deferredCommand">
		/// The action which will create the CommandDelegate. 
		/// This must not be null, but it can return a null CommandDelegate.
		/// </param>
		public static CommandDelegate Defer(CommandFactory commandDeferred)
		{
			CheckArgumentNonNull(commandDeferred, "commandDeferred");
			CommandDelegate command = null;
			return Commands.Sequence(
				Commands.Do( () => {
					command = commandDeferred();
				}),
				(ref double deltaTime) => {
					if (command != null) {
						return command(ref deltaTime);
					}
					return true;
				}
			);
		}
		
		/// <summary>
		/// Consumes all the time from the current update, but let's execution continue. 
		/// Useful for compensating for loading bumps.
		/// </summary>
		public static CommandDelegate ConsumeTime()
		{
			return (ref double deltaTime) => {
				deltaTime = double.Epsilon < deltaTime ? double.Epsilon : deltaTime;
				return true;
			};
		}

		/// <summary>
		/// Slows down, or increases the rate at which time flows through the given subcommands.
		/// </summary>
		/// <param name="dilationAmount">The scale of the dilation to perform. For instance, a dilationAmount
		/// of 2 will make time flow twice as quickly. This number must be greater than 0.
		/// </param>
		/// <param name='commands'>
		/// A list of commands to choose from at random. Only one command will be performed.
		/// Null commands can be passed. At least one command must be specified.
		/// </param>	
		public static CommandDelegate DilateTime(double dilationAmount, params CommandDelegate[] commands)
		{
			if (dilationAmount <= 0.0) {
				throw new System.ArgumentOutOfRangeException("dilationAmount");
			}
			var command = Commands.Sequence(commands);
			return (ref double deltaTime) => {
				double newDelta = deltaTime * dilationAmount;
				bool finished = command(ref newDelta);
				deltaTime = newDelta / dilationAmount;
				return finished;
			};
		}

		#endregion

		#region Private methods

		private static void CheckArgumentNonNull(object obj, string argumentName = "command")
		{
			if (obj == null) { 
				throw new System.ArgumentNullException(argumentName); 
			}
		}
		
		private static void CheckDurationGreaterThanZero(double duration)
		{
			if (duration <= 0.0) {
				throw new System.ArgumentOutOfRangeException("duration", duration, "duration must be > 0");
			}
		}

		private static void CheckDurationGreaterThanOrEqualToZero(double duration)
		{
			if (duration < 0.0) {
				throw new System.ArgumentOutOfRangeException("duration", duration, "duration must be >= 0");
			}
		}

		#endregion
	}
}