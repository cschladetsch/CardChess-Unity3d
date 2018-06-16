# Card Chess

A Unity3d game inspired by TCGs and Chess.

Am taking the unusual option of using an architecture that largely ignores Unity.

Focusing on the gameplay and [Rules](https://github.com/cschladetsch/Chess2/wiki), and only later will add interaction and visuals/audio.

Also, I intend to make all the art assets myself. Well, mostly.

## Testing

There are Both stand-alone and Unity-based [Unit tests](https://github.com/cschladetsch/Chess2/tree/master/Assets/App/Test/Editor)  provided for Model and Agent layers. I doubt there will ever be one for the View layer, and the Model and Agent layers both rely on Registry so serve as a test of that as well.  

## Gameplay

At first, the game will be a 2-4 player hotseat desktop (maxOS and Windows) game.

The main [GameLoop](https://github.com/cschladetsch/Chess2/wiki/gameloop) is documented in the wiki.

Later, if that all works out well, then networking layer will be added between Model and Agent. This has already been architected for via use of Futures for all interactions between Agents and Models.
