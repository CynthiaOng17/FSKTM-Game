FOL SPECIAL LEVEL - DOCUMENTATION
===================================
Developer: Chang Cai Ting
Scene: FOL_SpecialLevel

GAMEPLAY:
- Player: Justice (Faculty of Law)
- 3 Zones: Evidence Hall, Trial Gate, Boss Arena

CONTROLS:
- A/D = Move left/right
- Space = Jump
- F = Catch exam paper
- G = Throw paper back

ENEMIES:
- Whiplash: Shoots horizontal laser (-0.15 CGPA per hit)
- TingTingShi: Throws dual exam papers (-0.25 CGPA per hit)

WIN CONDITION:
- Collect 3 fragments in Zone 1
- Pass through law info gate in Zone 2
- Defeat TingTingShi in Zone 3
- Reach WinZone at end of Zone 3
- CGPA must be ≥ 3.75 for Dean List

LOSE CONDITION:
- CGPA drops to 0
- Timer runs out (3 minutes)

REQUIRED TAGS:
- Player, Enemy, ExamPaper, Whiplash, Ground, Laser

REQUIRED LAYERS:
- Ground

ENTRY POINT:
- Justice starts at X:-6, Y:0

EXIT POINT:
- WinZone at end of Zone 3

SCENE LOADING:
- Load scene: FOL_SpecialLevel
- After win: returns to main menu (needs team integration)