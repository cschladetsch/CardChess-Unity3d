# Card Chess

A Unity3d game inspired by TCGs and Chess.

Am taking the unusual option of using an architecture that largely ignores Unity.

Focusing on the gameplay and [Rules](https://github.com/cschladetsch/Chess2/wiki), and only later will add interaction and visuals/audio.

Also, I intend to make all the art assets myself. Well, mostly. I'm using some free sound effects and music assets.

## Building

This game also uses a library I made called [Flow](https://github.com/cschladetsch/Flow). To also clone this code into the repo for this game, use the following commmand:

```
$ git submodule update --recursive
```

That will pull the latest version of the _Flow_ library into the Chess2 source base.

## Testing

There are tests in _"App/Tests_ folder that uses mocked types in _App/Mock_.

## Gameplay

At first, the game will be a 2-4 player hotseat desktop (maxOS and Windows) game.

The main [GameLoop](https://github.com/cschladetsch/Chess2/wiki/gameloop) is documented in the wiki.

Later, if that all works out well, then networking layer will be added between Model and Agent. This has already been architected for via use of Futures for all interactions between Agents and Models.
