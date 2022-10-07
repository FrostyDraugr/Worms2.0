# Worms2.0
Previous Repo broke
Going for VG, the following features have been implemented:

General:
(G) Only play scene is required

Turn based game:
(G) You can have two players using the same input device taking turns
(VG, large) Support up to 4 players (using the same input device taking turns)

Terrain
(G) Basic Unity terrain or primitives will suffice for a level
(VG, large) Destructible terrain (Custom solution)

(G) A player only controls one worm
(G) Use the built in Character Controller. Add jumping. (Don't use the built-in one, I use a custom solution.)
(G) Has hit points
(VG, small) Implement a custom character controller to control the movement of the worm.
(VG, small) A worm can only move a certain range 
(VG, medium) A player controls a team of (multiple worms) (Depends on the settings in the GameManager)

Camera
(G) Focus camera on active player
(VG, small) Camera movement (With Scrollwheel)

Weapon system
(G) Minimum of two different weapons/attacks, can be of similar functionality, can be bound to an individual button
, like weapon 1 is left mouse button and weapon 2 is right mouse button
(VG, medium) The two types of weapons/attacks must function differently, I.E a pistol and a hand grenade.
The player can switch between the different weapons and using the active weapon on for example left mouse button

---

At the start there will be a countdown before player input is allowed, then the active player will be selected.
Moving using WASD (controller should work, haven't tested it) and activate combat-mode by left clicking,
or running out of movement. Use Spacebar to jump.

Switch between different weapons by Right-clicking, the weapon I recommend is the grenade since
I mostly focused on the destructible terrain.

Aim and fire, the grenade will have a guiding line but the rifle is really boring but it functions,
firing it's magazine over several seconds.

Once only one team is alive, the game will switch on slow-motion.

To change the settings in editor, use the GameManager to control:
Worms per Team
Number of Teams

The game also features a map generator, Use MapHandler settings to control how the world spawns.