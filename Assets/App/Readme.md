# App

The top-level of the application source architecture. All symbols reside under the namespace *App*.

The key sub-structures are:

* Model. Contains the data used by the game. Structures in App.Model do not use MonoBehavior and do not require the runtime.
* Controller. These are the active agents of the systen and use MonoBehavior. They reside between the View and Model objects. In the MVVM pattern, the Controller namespace contains the ViewModel elements.
* View
* View/World. World-space runtime visual and audio representations of Models (via Controllers).
* View/Interface. Canvas-space runtime representations of Models (via Controllers)
* Tests. Editor- and runtime-space unit tests.

Each of these have their own Readme's to further describe the overall system.
