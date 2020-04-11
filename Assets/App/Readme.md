# App

The top-level of the application source architecture. All symbols reside under the namespace *App*.

The key sub-folders are:

* **View**. World-space runtime visual and audio representations of Agents. This (and only this) level uses _MonoBehaviuour_.
* **Agent**. These are the active agents of the system and use Flow
* **Model**. Contains the data used by the game. Structures in App.Model do not use MonoBehavior and do not require the runtime.
* **Common**. Enumerations and structures used by all systems and across all tiers.
* **Database**. Contains databases for cards and other runtime-immutable resources.
* **Mock**. Mock types used for testing.
* **Registry**. Generic factory, dependency-injection, persistence and networking system. Used by all tiers.
* **Service**. Global services, like CardLibrary.
* **Test**. Editor- and runtime-space unit tests.
* **Util**. System-agnostic utils, for things like container and MonoBehavior extensions.

Each of these folders have their own Readme's to further describe the overall system.
