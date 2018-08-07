# CharacterController2D
An implementation of a 2D character-controller in Unity that is not driven by Unity's physics system.

**Some of the things implemented so far:**
- Basic AABB collision system.
- Movement system with configurable acceleration/deceleration.
- Jump/gravity system. Both the jump and the gravity are controlled by animation curves, allowing custom behaviours.
- Partial jumps & full length jumps.
- The player can jump after falling from an edge in a short time window (configurable).
- When the player is falling, it can jump even if he's not touching the ground yet. This leeway distance is configurable too.

**Some of the things pending:**
- Slopes
- Moving platforms
- Dashes (horizontal / vertical)
- Wall jump
- Multiple jumps
- Per-region parameters (different max velocity, acceleration, jump curves...)

--- WIP ---
