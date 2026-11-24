# The Lost Pointer: Race for the 4.0

A 2D action-adventure platformer built with Unity for **WIG3005 Game Development** (Semester 2, Session 25/26), Universiti Malaya.

> **Lecturer:** Prof. Madya Dr. Mohamad Nizam bin Ayub

---

## About the Game

A system glitch has shattered a student's CGPA into digital fragments scattered across campus. Race against the clock through three faculty environments, recover every pointer fragment and reach the end of each stage before time runs out, while avoiding corrupted system entities.

- **Genre:** 2D action-adventure platformer (single-player, PvE)
- **Platform:** PC (Windows, macOS, Linux)
- **Resolution:** 1920 x 1080 (Full HD)
- **Target audience:** Players aged 16+, especially university students

### Core Mechanics

- **CGPA as health:** taking damage from enemies or hazards lowers your CGPA. At 0, the level restarts.
- **Countdown timer:** run out of time and the level restarts.
- **CGPA status:**

| CGPA | Status | Effect |
|---|---|---|
| 3.70 - 4.00 | Honours | Gold-themed victory screen |
| 1.50 - 3.69 | Safe | Normal view |
| < 1.50 | Danger | Red pulsing tint and vignette |

### Playable Characters

| Character | Faculty |
|---|---|
| **Null** (The Code Pioneer) | Computer Science and Information Technology (FSKTM) |
| **Justice** (The Order Keeper) | Law (FOL) |
| **Heal** (The Survival Expert) | Medicine (FOM) |

### Enemies

- **Mr Bin:** a rolling trash robot that shoots junk and hurts on contact
- **Whiplash:** a spider-like projector robot that fires straight, flat lasers
- **TingTingShi:** the Lead Examiner (secret levels only); players must catch her exam papers and throw them back

---

## Levels

| Level | Faculty | Normal | Secret |
|---|---|---|---|
| 1 | FSKTM | Yes | Yes |
| 2 | Law (FOL) | Yes | Yes |
| 3 | Medicine (FOM) | Yes | Yes |

> **This repository currently contains the FSKTM Normal Level** (`FSKTM Normal Level` scene) and its related assets and scripts.

### FSKTM Normal Level Features

- Server-room themed platforming with moving platforms
- Collectible keys and matching key-door system
- Collectible CGPA pickups
- Enemies: Mr Bin and Whiplash
- CGPA health system and level timer
- Level completion screen with score and high scores

---

## Getting Started

### Requirements

- **Unity 6** (version `6000.4.7f1`). Please use the same version to avoid compatibility issues.
- Git

### Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/CynthiaOng17/FSKTM-Game.git
   ```
2. Open **Unity Hub** -> **Add** -> **Add project from disk** and select the cloned folder.
3. Open the project with Unity `6000.4.7f1`. The first import may take a few minutes while Unity regenerates the `Library` folder.
4. In the Project window, open the scene from `Assets/Scenes/` (e.g. **FSKTM Normal Level**).
5. Press **Play**.

### Controls

| Action | Key |
|---|---|
| Move left / right | _(fill in, e.g. A / D or arrow keys)_ |
| Jump | _(fill in, e.g. Space)_ |

---

## Project Structure

```
Assets/
├── Audio/
├── Prefabs/
├── Scenes/
├── Scripts/
└── ...
Packages/
ProjectSettings/
```

---

## Tools Used

- **Unity 6**: game engine
- **Unity Version Control**: team collaboration
- **Visual Studio Code**: C# scripting
- **GitHub**: source hosting
