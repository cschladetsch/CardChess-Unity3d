# App

The top-level of the application source architecture. All symbols reside under the namespace *App*.

The key sub-structures are:

* Model. Contains the data used by the game. Structures in App.Model do not use MonoBehavior and do not require the runtime.
* Agent. These are the active agents of the systen and use Flow
* Action. Describes what actions a Player can make
* Main. The top-level objects that reside in the scene or are otherwise global to the app
* View/World. World-space runtime visual and audio representations of Models (via Controllers).
* View/Interface. Canvas-space runtime representations of Models (via Controllers)
* Tests. Editor- and runtime-space unit tests.

Each of these have their own Readme's to further describe the overall system.
