# Game Timer UI Setup Guide

## Two Versions Available:

### 1. GameTimerUI (Standard Unity UI Text)
- Uses standard Unity `UI.Text` component
- Lightweight and simple
- Good for basic projects

### 2. GameTimerUI_TMP (TextMeshPro)
- Uses `TextMeshProUGUI` component
- Better text quality and performance
- Includes color coding feature
- **Recommended version**

---

## Setup Instructions:

### Step 1: Create UI Elements in Hierarchy

1. Right-click in Hierarchy → UI → Canvas (if you don't have one)
2. Right-click on Canvas → UI → Panel (optional background)
3. Right-click on Canvas → UI → Text (for standard) OR Text - TextMeshPro (for TMP version)
   - Name it "TimerText"
4. Right-click on Canvas → UI → Text (for standard) OR Text - TextMeshPro (for TMP version)
   - Name it "EnemyCountText"

### Step 2: Position UI Elements

Position the text elements in the top-left or top-right corner:
- **TimerText**: Position at top-right (Anchor: top-right)
- **EnemyCountText**: Position below TimerText

Example positions:
```
TimerText:      X: -10, Y: -10 (from top-right)
EnemyCountText: X: -10, Y: -40 (from top-right)
```

### Step 3: Add GameTimerUI Component

1. Create empty GameObject in Hierarchy: Right-click → Create Empty
2. Name it "GameTimerUI"
3. Add Component → Scripts → **GameTimerUI** (or **GameTimerUI_TMP**)
4. In Inspector:
   - Drag **TimerText** to "Timer Text" field
   - Drag **EnemyCountText** to "Enemy Count Text" field
   - Check "Show Enemy Count" (enabled by default)

### Step 4: Customize Settings (Optional)

In GameTimerUI Inspector:
- **Time Prefix**: Change "Time: " to whatever you want
- **Enemy Prefix**: Change "Enemies: " to whatever you want

If using **GameTimerUI_TMP**:
- **Use Color Coding**: Enable to change colors based on difficulty
- **Easy Color**: Green (0-5 minutes)
- **Medium Color**: Yellow (5-10 minutes)
- **Hard Color**: Red (10+ minutes)

### Step 5: Test

Press Play! You should see:
```
Time: 00:00
Enemies: 0/10
```

The timer will update every frame and show:
- Current game time in MM:SS format
- Current active enemies / Maximum possible enemies

---

## Example HUD Layout:

```
┌─────────────────────────────────┐
│  Time: 05:23        Enemies: 15/35  │  ← Top bar
│                                 │
│                                 │
│          [Game View]            │
│                                 │
│  [Health Bar]                   │  ← Bottom
└─────────────────────────────────┘
```

---

## Features:

✅ Automatic CharacterSpawnController detection  
✅ MM:SS time format  
✅ Current/Max enemy display  
✅ Optional color coding (TMP version)  
✅ Customizable prefixes  
✅ Follows SOLID/DRY principles  

---

## Troubleshooting:

**Timer shows 00:00 always:**
- Make sure CharacterSpawnController is in the scene
- Check Console for warnings

**Enemy count shows 0/0:**
- CharacterSpawnController might not be initialized
- Check that Player has "Player" tag

**Text not visible:**
- Check Canvas render mode
- Make sure text color is not same as background
- Check text size and font

**Using TextMeshPro version but get errors:**
- Install TextMeshPro package: Window → TextMeshPro → Import TMP Essential Resources
- Or use standard GameTimerUI instead
