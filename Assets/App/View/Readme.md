# App.View

These objects are derived from UnityEngine.MonoBehavior.

They are responsible for showing and animating the objects defined by the Models and represented by Agents.

In general, the View objects only know about the Agents. They do not have a direct link to the underlying Models.

In this respoect, the Agents are the ViewModel in the MVVM pattern. The large difference is that Agents are themselves pro-active and work over time via the Flow library.
