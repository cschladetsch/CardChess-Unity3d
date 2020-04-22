# Card Chess ![Kings](Doc/Kings.png)
[![Build status](https://ci.appveyor.com/api/projects/status/github/cschladetsch/cardchess?svg=true)](https://ci.appveyor.com/project/cschladetsch/cardchess)
[![License](https://img.shields.io/github/license/cschladetsch/cardchess.svg?label=License&maxAge=86400)](./LICENSE)

**The CI Build is failing because it is a Unity3d project, and the use of git-submodules. Keeping the badge as a reminder to everyone about these issues.**

ChardChess Unity3d game inspired by Trading Card Games such as [Magic the Gathering](https://magic.wizards.com/en) and [Chess](https://lichess.org/).

Am taking the unusual option of using an architecture that largely ignores Unity.

Focusing on the gameplay and [Rules](https://github.com/cschladetsch/CardChess/wiki), and only later will added interaction and visuals/audio.

The main scene is in _Scenes/Main_.

## Building

The build and Packages require an installation of [WorkFolder](https://github.com/cschladetsch/WorkFolder) mapped to the w-drive.

I'll cover this all in more detail when I expect others to try to build it.

## Testing

There are tests in _App/Tests_ folder that uses mocked types in _App/Mock_.

These tests are currently broken because I changed the rules without fixing the tests to match.

## Gameplay

At first, the game will be a 2 player hotseat desktop (macOS and Windows) game.

The main [GameLoop](https://github.com/cschladetsch/CardCHess/wiki/gameloop) is well documented in the wiki.

Later, if that all works out well, then networking layer will be added between Model and Agent. This has already been architected for via use of Futures for all interactions between Agents and Models.

## Discord
There's also a [discord server](https://discord.gg/c8SmrE) connected to GitHub. 


