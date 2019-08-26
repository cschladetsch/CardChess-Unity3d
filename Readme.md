# Card Chess

[![CodeFactor](https://www.codefactor.io/repository/github/cschladetsch/chess2/badge)](https://www.codefactor.io/repository/github/cschladetsch/chess2)
[![License](https://img.shields.io/github/license/cschladetsch/chess2.svg?label=License&maxAge=86400)](./LICENSE.txt)

A Unity3d game inspired by TCGs and Chess.

Am taking the unusual option of using an architecture that largely ignores Unity.

Focusing on the gameplay and [Rules](https://github.com/cschladetsch/Chess2/wiki), and only later will add interaction and visuals/audio.

Also, I intend to make all the art assets myself. Well, mostly. I'm using some free sound effects and music assets.

## Building

I am currently using **Unity3d 2018.1.4f1**. YMMV for earlier or later releases of Unity3d. The game will always build on the latest version of Unity3d within a week or so of its release.

This source repo also uses a library I made called [Flow](https://github.com/cschladetsch/Flow). This is included as a Git submodule. To also clone this code into the repo for this game, use the following single commmand:

```bash
# git clone --recursive https://github.com/cschladetsch/Chess2.git
```

By default, the repo installs into a folder called _Chess2_. This is a temporary name and will change. Within this Readme and throughout the project documentation, the root folder of the repo will be referred to as **$ROOT_DIR**.

Or, if you cloned the base repo first (or use a version of git older than 1.9), then you have to 'manually' update the _Flow_ library with:

```bash
# git clone https://github.com/cschladetsch/Chess2.git
# cd Chess2
# git submodule update --init --recursive
```

The main scene is in _Scenes/Main_.

#### Models

Some models are .fbx and will import directly into Unity3d with no further requirements. However, some models may be _.blend_ files. These are native scene files for [Blender](https://www.blender.org/download/). You will need to download **Blender** to import these _.blend_ files.

## Notes

If you get a bunch of errors about "namespace Flow" not named, etc, or problems to do with anything named _Flow_, then you have not got the Flow submodule that lives in _Assets/External/Flow_.

There are a number of ways to fix this. The easiest is to just:

```bash
# cd $ROOT_DIR
# git submodule update --init --recursive
```

## Testing

There are tests in _"App/Tests_ folder that uses mocked types in _App/Mock_.

## Gameplay

At first, the game will be a 2-4 player hotseat desktop (maxOS and Windows) game.

The main [GameLoop](https://github.com/cschladetsch/Chess2/wiki/gameloop) is documented in the wiki.

Later, if that all works out well, then networking layer will be added between Model and Agent. This has already been architected for via use of Futures for all interactions between Agents and Models.
