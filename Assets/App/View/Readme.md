# App.View

These objects are derived from `UnityEngine.MonoBehavior` via `ViewBase`.

In general, before any View hits its `Begin` method, all underlying Models and Agents have been created.

They are responsible for showing and animating the objects defined by the Models and represented by Agents.

In general, the View objects only know about the Agents. They do not have a direct link to the underlying Models.

Of course, they can get to the model by using `this.Agent.Model`.

In this respoect, the Agents are the ViewModel in the MVVM pattern. The large difference is that Agents are themselves pro-active and work over time via the Flow library.
