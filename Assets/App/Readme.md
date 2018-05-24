# App

The top-level of the application source architecture. All symbols reside under the namespace *App*.

The key sub-structures are:

* Action. Describes what actions a Player can make
* Agent. These are the active agents of the systen and use Flow
* Common. Enumerations and structures used by all systems.
* Model. Contains the data used by the game. Structures in App.Model do not use MonoBehavior and do not require the runtime.
* Registry. Generic factory, dependancy-injection, persistence and networking system.
* Service. Global services, like CardLibrary.
* Main. The top-level objects that reside in the scene or are otherwise global to the app
* View. World-space runtime visual and audio representations of Models (via Controllers).
* Tests. Editor- and runtime-space unit tests.

Each of these have their own Readme's to further describe the overall system.
