# Ball Game — Money Gain Plinko

Welcome to **Ball Game**, a fast, satisfying, mobile-style **plinko/ball drop** experience built in Unity. Drop balls, rack up stars, collect gems, and work toward timed payouts—while ads and IAP give players extra ways to keep the action going.

---

## ✨ Quick Highlights

- **Tap to drop** balls into a lively plinko arena
- **Stars = score**, **gems = payout progress**
- **Timed gem cycles** reset every few hours
- **Rewarded ads** and **IAP** for extra balls
- **Audio toggle**, **loading screen**, and responsive layout

---

## 🖼️ Screenshots

> **Add your screenshots here**
>
> - `![Main Menu](path/to/screenshot-main-menu.png)`
> - `![Gameplay](path/to/screenshot-gameplay.png)`
> - `![Payout Screen](path/to/screenshot-payout.png)`
>
> Tip: Keep images around **1280×720** for consistent layout.

---

## 🎮 Gameplay Overview

- **Drop balls** with a tap/click—spawn positions are clamped between screen borders.
- **Limited ball inventory** adds strategy—run out and you’ll hit the out‑of‑balls screen.
- **Collect stars** to boost score, and **gems** to fill the payout meter.
- **Bonus boxes** grant instant star or gem rewards.
- **Destructible obstacles** add a little chaos and respawn over time.

---

## ⭐ Core Features

- **Persistent progress** (score + balls saved between sessions)
- **3‑hour gem payout cycle** with automatic reset
- **Payout screen** with progress tracking and redeem flow
- **Rewarded ads** for extra balls
- **In‑app purchase** pack (100 balls)
- **Audio manager** with mute toggle + saved preference
- **Loading scene** with progress bar + minimum load duration
- **Responsive game area scaling** for different screen sizes

---

## 🔁 Gameplay Loop

1. **Start a run** from the main menu
2. **Drop balls** into the board and watch them bounce
3. **Collect stars/gems** from pickups and bonus boxes
4. **Monitor gem progress** toward the payout threshold
5. **Redeem** when you reach the goal

---

## 🗺️ Scenes

- **LoadingScene** → async load into the main menu
- **0_MainMenu** → new game, load game, settings, and info panels
- **1_Level** → main gameplay scene
- **PaymentScene** → gem redemption and payout UI

---

## 🕹️ Controls

- **Mouse / Touch:** tap or click to drop a ball

---

## 💰 Monetization & Rewards

- **Rewarded Ads**: optional video ads grant extra balls
- **IAP**: consumable ball pack (100 balls)
- **Gems**: accumulated toward payouts on a timed cycle

---

## 🧠 Data & Persistence

- **PlayerPrefs** stores balls remaining and score
- **Gem payouts** track a timed cycle and reset on expiry

---

## 🧰 Tech Stack

- **Unity** 2022.3 (LTS)
- **Google Mobile Ads** (rewarded ads)
- **Unity IAP + Unity Gaming Services**

---

## 🚀 Getting Started

### Requirements

- **Unity Hub**
- **Unity 2022.3 LTS** (recommended)

### Open the Project

1. Open **Unity Hub** → **Add project from disk**
2. Select the root folder of this repository
3. Open with **Unity 2022.3 LTS**

### Play in Editor

1. Open the **LoadingScene** or **0_MainMenu** scene
2. Press **Play** in the editor

---

## 📁 Project Structure

- `Assets/Scenes/` — Unity scenes (menu, gameplay, payment, loading)
- `Assets/Scripts/` — core gameplay + UI logic
- `Assets/Prefabs/` — reusable game objects
- `ProjectSettings/` — Unity project configuration

---

## 🤝 Contributing

- Keep scripts small and focused
- Prefer reusable prefabs over scene‑specific duplication
- Document new features in this README

---