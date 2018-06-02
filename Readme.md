# Card Chess

A Unity3d game inspired by TCGs and Chess.

Am taking the unusual option of using an architecture that largely ignores Unity.

Focusing on the gameplay and [rules](https://github.com/cschladetsch/Chess2/wiki), and only later will add interaction and visuals/audio.

Also, I intend to make all the art assets myself. Well, mostly.

The project uese an architecture not unlike MVVC, except with clearer distinctions between tiers:

1. **Registry**. This is parameterised over the type of thing to manage. A _Registry_ provides Unique Ids used for persistence and networking, as well as a dependancy-injection system. A different egistry\<T\> is used by all subsequent layers.
1. **Model**. Persistent and provides reactive properties. Save/Load and Replay.
1. **Agent**. Dynamic and uses the Flow library based on a Kernel of co-routines. Networking and flow contol.
1. **View**. Provides the Unity3d layer. Both Model and Agent do not reference Unity3d assemblies at all. The View layer binds Views to Agents and provide all the perceptual animation.

Note that each layer has no linkage at all to latter laters. Models do not know about Agents, and Agents do not know about Views.

## Testing

There are Both stand-alone and Unity-based Unit tests provided for Model and Agent layers. I doubt there will ever be one for the View layer, and the Model and Agent layers both rely on Registry so serve as a test of that as well. 

## Gameplay

At first, the game will be a 2-4 player hotseat desktop (maxOS and Windows) game.

Later, if that all works out well, then networking layer will be added between Model and Agent. This has already been architected for via use of Futures for all interactions between Agents and Models.




