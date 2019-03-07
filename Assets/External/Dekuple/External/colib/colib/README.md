CoLib
=

CoLib is a control flow library designed to simplify writing complex timed logic
in Unity. The library is modular and highly extensible, making heavy use of closures.
It's design is similar to the Cocos2d and LibGDX Action systems.

Compatibilty
-
CoLib is compatible with Unity 5.0 and upward.

Basic Usage
-

Below is a simple tweening example:

    void Start()
    {
        double duration = 8.0;
        Vector2 secondPosition = new Vector2(0.0f, 10.0f, 0.0f);
        Vector2 finalPosition = new Vector2(10.0f, 0.0f, 0.0f);

        // Move the current gameObject secondPosition over 8 seconds,
        // then to finalPosition over 8 seconds.
        gameObject.Schedule(
            Commands.MoveTo(gameObject, secondPosition, duration),
            Commands.MoveTo(gameObject, finalPosition, duration)
        );
    }

Commands are executed sequentially. Alternatively, to execute commands
in parallel, you can do the following:

    void Start()
    {
        gameObject.Schedule(
            Commands.Parallel(
                Commands.MoveTo(gameObject, new Vector2(10.0f, 0.0f, 0.0f), 8.0f),
                Commands.TintTo(gameObject, Color.blue, 4.0f)
            )
        );
    }

Using this complex sequences can be built.

    void Start()
    {
        // Turns blue, and at the same time waits for 2 seconds before moving.
        gameObject.Schedule(
            Commands.Parallel(
              Commands.TintTo(gameObject, Color.blue, 4.0f)
              Commands.Sequence(
                  Commands.WaitForSeconds(2.0f),
                  Commands.MoveBy(gameObject, new Vector2(-5.0f, 0.0f, 0.0f), 4.0f)
              )
            )
        );
    }

So far these examples have all used extension method shortcuts to run the  commands.
However, it is also possible to run them manually using a CommandQueue.

    private CommandQueue _queue = new CommandQueue();

    void Start()
    {
        _queue.Enqueue(
          Commands.MoveTo(gameObject, new Vector2(10.0f, 0.0f, 0.0f), 8.0f)
        );
    }

    void Update()
    {
        _queue.Update(Time.deltaTime);
    }

The CommandQueue runs commands in the order they are enqueued. It will wait
for the current command to finish running, before starting to run the next
command.

    private CommandQueue _queue = new CommandQueue();

    void Update()
    {
        _queue.Update(Time.deltaTime);
    }

    void GoBlue()
    {
        _queue.Enqueue(
            Commands.TintTo(gameObject, Color.blue, 10.0f * 60.0f, Ease.InOutHermite())
        );
    }

    void GoGray()
    {
        _queue.Enqueue(
            Commands.TintTo(gameObject, Color.gray, 2.0f)
        );
    }

    void Say(string text, float duration = 5.0f)
    {
        // The Do Command allows callbacks to be written in the middle of a
        // sequence. In this example we used a closure, but we could create the
        // command with a method reference.
        _queue.Enqueue(
            Commands.Do( () => {
                guiText.enabled = true;
                guiText.text = text;
            }),
            Commands.WaitForSeconds(duration),
            Commands.Do( () => {
                guiText.enabled = false;
                guiText.text = "";
            })
        );
    }

    void Start()
    {
        // The gameObject will display the following text
        Say("I can hold my breath for 10 whole minutes!");
        // ... then it will tint itself blue over 10 minutes.
        GoBlue();
        // ... then gray over 2 seconds
        GoGray();
        // ... and finally it will display this text:
        Say("*gasp*");
    }

The CommandScheduler is another way of running commands manually. It will run
every command it is given at the same time.

    private CommandScheduler _scheduler;

    void Update()
    {
      _scheduler.Update(Time.deltaTime);
    }

    void Start()
    {
        // Both these commands will be scheduled to run at the same time
        // over a second.
        _scheduler.Add(
          Commands.MoveBy(gameObject, Vector3.forward * 3, 1.0)
        );
        _scheduler.Add(
          Commands.RotateBy(gameObject, Quaternion.Euler(180f, 0f, 0f), 1.0)
        );
    }

As we saw earlier, there are some convenience methods provided to make this
easier. These methods will add a CommandBehaviour to the current gameObject, which
can either run commands on a scheduler, or create new CommandQueues for us to use.

    void Start()
    {
        // Create a new command queue, which will be updated by the gameObject.
        CommandQueue newQueue = gameObject.Queue();
        // Remove the Queue, this will stop it from being updated.
        gameObject.RemoveQueue(newQueue);
        // Schedule a command to run.
        gameObject.Schedule( Commands.Log("Hello World"));
    }

Anatomy of a Command
-
All commands conform to the following signature:

    delegate bool CommandDelegate(ref double deltaTime);

A command, or CommandDelegate, is a method or (more commonly) a closure which consumes an
amount of time and then completes. The first argument deltaTime is the amount of time to
update the command by. The command will subtract the amount of time it has used, then output
it back into deltaTime. When the command has completed, it will return true, otherwise it
will return false to indicate further updates are required. After a command has completed,
further calls to it will restart it. Here is the implementation of the Do command:

	public static CommandDelegate Do(CommandDo command)
	{
		return (ref double deltaTime) => {
			command();
			// Don't modify the time, because the command happens instantly.
			return true;
		};
	}

Commands are designed to be highly flexible and composable. Here are few basic commands which
can be used to compose more complex ones:

        // Do an action. This action won't consume any time.
        Commands.Do(() => Debug.Log("Hello"))

        // Do an action over a duration. The input value t is normalized between 0 and
        // 1, and  can have a custom easing function applied.
        Commands.Duration( t => a = (b - c) * t + c, 3.0, Ease.Smooth())

        // Wait 3 seconds
        Commands.WaitForSeconds(3.0)

        // Do a series of commands in sequence, (as we've seen before).
        Commands.Sequence(
            Commands.WaitForSeconds(3.0),
            Commands.Do( () => Debug.Log("A"))
        )

        // Do commands in parallel
        Commands.Parallel(
            Commands.Duration( t => Debug.Log(t), 3.0)
            Commands.Do( () => Debug.Log("B")),
        )

        // Repeat a command 5 times.
        Commands.Repeat(
            5, Commands.Do( () => Debug.Log("Hello"))
        )

        // Repeat a command forever, (be careful, this should take some amount of time
        // otherwise it will be an infinite loop).
        Commands.RepeatForever(
            Commands.Do( () => Debug.Log("Hello"))
            Commands.WaitForSeconds(3.0),
            Comands.Do( () => Debug.Log("World"))
        )

Coroutines
-

CoLib has it's own version of Coroutines which can be useful for expressing certain kinds
of logic efficiently which regular Commands can't. They look like this :

    IEnumerator<CommandDelegate> CoroutineMethod(int firstVal, int secondVal, int thirdVal)
    {
        Debug.Log(firstVal);
        yield return Commands.WaitForSeconds(1.0f); // You can return any CommandDelegate here.
        Debug.Log(secondVal);
        yield return null; // Wait a single frame.
        Debug.Log(thirdVal);

        // Launch another coroutine without any parameters.
        yield return Commands.Coroutine(ASecondCoroutine);

        // Execute whatever CommandDelegates we want in parallel, and wait for them to finish
        // before returning.
        yield return Commands.Parallel(
            Commands.Coroutine( () => AThirdCoroutine()),
            Commands.Coroutine( () => AForthCoroutine())
        );
        yield break; // Force exits the coroutine.
     }

    void Start()
    {
        gameObject.Schedule(
            Commands.Coroutine( () => CoroutineCommand(1,2,3) )
        );
    }

More complex examples can be found in the Examples folder.

Tweening
-
CoLib comes with a light weight tweening library built in. The following types can be
tweened.

        float, double, short, int, long, Vector2, Vector3, Vector4, Colour, Quaternion, Rect

Fields on objects are captured by using the Ref<T> struct.

        float someVariable = 0f;
        Ref<float> ref = new Ref(
            () => someVariable, // Getter
            t => someVariable = t // Setter
        );
        double duration = 3.0;
        gameObject.Schedule(
            Commands.ChangeTo(ref, 4f, duration, Ease.Smooth()),
            Commands.ChangeBy(ref, -1f, duration, Ease.Smooth()),
            Commands.ChangeFrom(ref, 0f, duration, Ease.Smooth())
        );

There are a number of extensions which cut this syntax down for commonly tweened types,
including transforms, gameObjects, materials, renderers, and uGUI components.

    gameObject.Schedule(
        Commands.MoveBy(transform, Vector3.forward * 10, 0.3, Ease.OutBounce()),
        Commands.TintTo(material, Color.Red, 0.3, Ease.InSin()),
        Commands.AlphaTo(material.ToColorPropertyRef("_Tint"), Color.Green, 0.5, Ease.OutQuad()),
        Commands.RotateFrom(transform, Quaternion.Euler(0, 180f, 0f), 0.3, Ease.Linear())
    );

For custom types you would like to ease, you can write your own interpolators using
the IInterpolator interface, or you can make the type implement the IInterpolatable
interface. If you are feeling really adventurous, you could even write your own
custom command.

Eases
-

Like with Commands, Eases are typically implemented with closures. They use the following
signature:

    delegate double CommandEase(double t);

An easing function takes an input value t where an uneased t ranges from 0 <= t <= 1 .
Some easing functions, (such as BackEase) return values outside the range 0 <= t <= 1.
For a given valid easing function, f(t),  f(0) = 0 and f(1) = 1. The library implements
the full list of penner easing functions, and a few utilities to help compose your own
hybrid eases. Try using Ease.Smooth(), which produces a nice Hermite curve.

License
-

[Modified BSD](License.txt)

*&copy; 2015 Darcy Rayner*
