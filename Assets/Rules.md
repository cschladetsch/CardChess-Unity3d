# Rules

The game area consists of a NxN checkered square board.

Each player starts with a deck of 50 cards, and a mana pool of 1.

At the start of each turn, the player's total mana is increased by one.

To play a turn, a player may do any of the following, as many times as he wishes as long as they have the mana:

* Place one or more Cards onto the Board
* Move a Piece on the Board.
* Cast a Spell
* Place an Item on a Piece.
* Pass. When a player passes, the turn is over.

## Cards

Cards can be:
* A Piece. Pieces can be placed on the board.
* A Spell. Spells have varying effects.
* An Item. Items are placed on Pieces.

### Pieces

The terms _creature_ and _piece_ are interchangable.

Each piece has an Attack strength and a Health level, as well as a set of Abilities.

In general, base piece types are related to chess pieces:
* King.
* Queen.
* Gryphon. Moves like a knight in chess. Can be Mounted.
* Paladin. Moves like a Pawn.
* Archer. Bishop.
* Cannon. Rook. 
* Dragon. Moves 2 squares in any direction. Can be Mounted.
* Barricade. Guard. Cannot move. Stops other pieces from moving passed it.
* Siege. Some can be moved. Siege pieces attack from a distance.

There are varitions on all basic piece types, with different Attack Health and Abilities. For example _Noble Paladin_ has double starting health. _Black Dragon_ has more Attack but less Health. etc.

### Mounting

All pieces except Cannon, Barricade and Siege can Mount. Creatures that can be Mounted cannot Mount other creatures.

Mounted creatures use combined Attack, Health and Abilities.

Once Mounted, the two creatures remain joined until death.

### Abilities

* Charge. Can move/attack on the turn it is played.
* Lethal. Any damage inflicted by the creature immediately kills the other.
* Paralise. Any damage inflicted by the creature immediately renders the other creature unconscious the other.
* Guard. Any creature within N squares of another friendly creature is Guarded. The Guard creature must be killed or moved before other nearby creature can be attacked.

## Spells

* Damage a Creature.
* Destroy a Creature.
* Creature buff/debuff.
* Area effect buff/debufff.
* Return creature from Graveyard.

## Items
* Can increase the Health/Attack stat of a piece.

## Battles

When a piece lands on an opponents' piece, they Battle. Each takes damage equivalent to the Attack strength of the other piece.

If a creature's Health is reduced to zero after the Battle, it becomes _unconcious_ and can perform no action. It can remain in this state indefinitely.

If a creature's Health is reduced to below zero, it is removed from the Board and placed in the Graveyard.

## Card Types

Cards have three main stats: Mana cost, Attack strength and Health.

* King. 0, 1, 20
* Queen. 5, 5, 5
* Paladin. 1, 1, 2
* etc...

## Ending the game.

The game is lost when a King has less than zero health.


