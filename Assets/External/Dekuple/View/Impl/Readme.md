# Dekuple.View.Impl1

Default programmer-art implementation of the views of the agents in the game.

This is called _Impl1_ as it's just a functional implementation with no bling or anything. 

I didn't want to call it 'MockImpl' because it actually works in a real game, and same for calling it 'TestImpl'. It is neither a mock nor a test, but it is also not meant to be the final view implementation. Though of course systems used can be borrowed.

This is meant for demonstration purposes only.

Also, it's a real problem that Unity doesn't allow for interfaces as fields.

The classes in the _Dekuple.View.Impl1_ namespace are hard-coded to other concreate classes in the same namespace.

There's no practical way around this. The only upside is that the entire View layer is fenced off from the rest of the system. An entirely different view system can be used without having to touch any code outside of its own _Dekuple.View.BetterImpl_ namespace.
_
It is expected that some other _Dekuple.View.Impl\*_ namespace will be used in production.