# Assets

The project uese an architecture not unlike MVVC, except with clearer distinctions between tiers:

1. **Registry**. This is parameterised over the type of thing to manage. A _Registry_ provides Unique Ids used for persistence and networking, as well as a dependancy-injection system. A different _Registry\<T\>_ is used by each layer.
1. **Model**. Persistent databases and models. Provides reactive properties. Save/Load and Replay.
1. **Agent**. Dynamic and uses the Flow library based on a Kernel of co-routines. Networking and flow contol. Changes from the Model layer propagate up via reactive systems.
1. **View**. Provides the Unity3d layer. Neither _Model_ nor _Agent_ layers reference Unity3d assemblies at all. The _View_ layer binds Views to Agents and provide all the animation for sound, music, models, etc.

Note that each layer has no linkage at all to later layers. Models do not know about Agents, and Agents do not know about Views.

## They key folders are:

* **App**. The main application code.
* **External**. Third-party tools.
* **Models**. 3D Models. Preferably .blend.
* **Scenes**. 
* **Textures**. Preferably png.
* **Prefabs**. 
* **Sfx**. Sound effects.
* **Music**. Music tracks.
* **Tmp**. Ignored by git.

Each folder has its own _Readme.md_ for more information.
