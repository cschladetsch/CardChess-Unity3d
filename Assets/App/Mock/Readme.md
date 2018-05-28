# App.Mock

Contains mock objects for testing and development.

These were all in the unit-test assembly. However, it became necessary to move them to the main game assembly to enable runtime testing scenarios without having to build up entire game systems from scratch each time.

The code-stripper should remove all this stuff for release builds. But just be aware that *none* of the code in _App.Mock_ namespace is required for a shipping title.

There was some internal debate about where to put all this stuff: App.Model.Mock, App.Agent.Mock, etc or App.Mock.Model, App.Mock.Agent etc.

I ended up deciding on the latter because it will be easier to strip out later as needed. You're welcome.
_
## App.Mock.Model

Contains mock models for Cards, Decks, Hands, Players.

## App.Mock.Agents

Contains mock agents for Cards, Decks, Hands, Players.

## App.Mock.View

Not implemented. Moving all the Model and Agent mock code into the main assembly was to allow for realtime testing scenarios of View (Monobehavior) based systems.

I tried using Unity 2018's Edit/Runtime testing system and just couldn't get it all to work. So here we are: test mock code is in the main assembly.

