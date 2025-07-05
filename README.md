<h1 align="center">Stealth FPS Core</h1>

<img src="https://github.com/user-attachments/assets/0c95fa17-40f3-46ad-bd58-1a008f5bcc31" alt="Gameplay Preview" />

## 📌 Project Description

<b>Stealth FPS Core</b> is a flexible and lightweight FPS game template with stealth mechanics. Designed with modularity in mind, this project allows developers, educators, and hobbyists to build upon a foundation that includes player control, patrol-based enemy AI, projectile-based weapons, and noise/visibility-based detection.
<br/><br/>
Players are encouraged to take a more strategic approach than in traditional FPS games - leveraging stealth, distraction, and careful resource use to complete objectives rather than relying on overwhelming firepower.
<br/><br/>
Developed as part of SGD213: Professional Games Programming at the University of the Sunshine Coast.

> For more information refer to the [wiki](https://github.com/Yuliia-Kruta/stealth-fps-core/wiki)

---

## 🎮 Features

### 🧍‍♂️ Player Mechanics
- Walk, sprint, crouch with variable noise levels
- Pick up, swap, and throw items (sticks, stones, grenades)
- Visibility meter UI ("Visibility Eye") indicates exposure to enemies

### 🤖 Enemy AI
- Patrol along invisible, customizable `PatrolNode` paths
- Detect the player by line of sight and noise levels
- Investigate noise, chase player, attack in melee range
- Remember last-seen position if player escapes

### 💣 Throwable Weapons
- **Stick**: Quiet, stuns enemies briefly
- **Stone**: Moderate noise, longer stun
- **Grenade**: Lethal explosion, high noise

### 🕵️ Stealth Systems
- **Visibility**: Raycast-based, affected by obstacles
- **Noise**: Spherical propagation; actions like movement and objects falling generate sounds
- **Feedback**: Debug logs

### 🎨 UI & Feedback
- **Visibility Eye** dynamically updates based on player exposure (e.g., open/closed/red eye sprites)
- **HUD Inventory**: Real-time weapon icons and ammo counters update as the player picks up or switches weapons
- **Menus:**: In-game pause, game over, and win screens
- **Restart & Navigation Controls:**: Game can be paused, restarted, or continued via UI buttons

### 🧩 Modular Architecture
- Components: `PlayerInventory`, `Weapon`, `EnemyAI`, `NoiseSpawner`, etc.
- Designed for extensibility - easily add new weapons, AI types, or mechanics

---

## 🛠️ Development Environment

- **Engine**: Unity 2022.1.21f1  
- **Language**: C#  
- **IDE**: Visual Studio / JetBrains Rider  
- **Version Control**: GitHub  
- **Project Management**: Trello  
- **Communication**: Discord, Email, Google Docs  


---

## 🚀 Getting Started

### Prerequisites
Ensure you have the following installed:

- [Unity Hub](https://unity.com/download)
- Unity Editor **2022.1.21f1** (or compatible version)

### Installation

Clone the repository:

<code>git clone https://github.com/Yuliia-Kruta/stealth-fps-core.git</code>

### Open the Project in Unity
1. Launch **Unity Hub**

2. Click **Add Project**

3. Select the cloned project folder

4. Open the **MainScene**

> ℹ️ As this is a template game, a lot of information is displayed in Unity's debug logs. For best experience, enable Gizmos in the Game view to visualise debug elements like enemy line-of-sight and noise spheres.

![gizmos game view](https://github.com/user-attachments/assets/d252e8a6-4e96-4739-b7aa-ee4f4aae071c)

---

## 🎮 How to Play

- **Move**: `W`, `A`, `S`, `D`
- **Sprint**: `Left Shift`
- **Crouch**: `Left Ctrl`
- **Look Around**: Move your mouse
- **Pick Up Weapons**: `E Button`
- **Throw Weapon**: `R Button`
- **Swap Weapons**: `Mouse Scroll` or number keys `1`, `2`, `3`
- **Pause Game**: `Esc`

### 🧠 Gameplay Tips

- Use **stealth**: stay out of enemy sight and move quietly
- Use **throwables** to distract, stun, or eliminate enemies
- Monitor your **visibility** using the "eye" icon
- Manage your inventory of **sticks**, **stones**, and **grenades** wisely

<h2>Contributors</h2> 
This project was created by Yuliia Kruta in collaboration with Jana Morgan and Matyas Gulyas for the Professional Games Programming class at UniSC.

<h2>Copyright Notice</h2>

Copyright (c) 2025 Yuliia Kruta, Jana Morgan, Matyas Gulyas. All rights reserved.

This project is not licensed for reuse, redistribution, or modification without explicit permission from the authors.
