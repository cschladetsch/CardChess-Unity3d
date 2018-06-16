# App

The top-level of the application source architecture. All symbols reside under the namespace *App*.

The key sub-structures are:

* **Agent**. These are the active agents of the systen and use Flow
* **Common**. Enumerations and structures used by all systems and across all tiers.
* **Database**. Contains databases for cards and other runtime-immutable resources.
* **Mock**. Mock types used for testing.
* **Model**. Contains the data used by the game. Structures in App.Model do not use MonoBehavior and do not require the runtime.
* **Registry**. Generic factory, dependancy-injection, persistence and networking system. Used by all tiers.
* **Service**. Global services, like CardLibrary.
* **View**. World-space runtime visual and audio representations of Agents. This (and only this) level uses _MonoBehaviuour_.
* **Tests**. Editor- and runtime-space unit tests.
* **Util**. System-agnostic utils, for things like container and Monobehavior extensions.

Each of these folders have their own Readme's to further describe the overall system.
