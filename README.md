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

## 🗺️ Scenes

- **LoadingScene** → async load into the main menu
- **0_MainMenu** → new game, load game, settings, and info panels
- **1_Level** → main gameplay scene
- **PaymentScene** → gem redemption and payout UI

---

## 🕹️ Controls

- **Mouse / Touch:** tap or click to drop a ball

---

## 🧰 Tech Stack

- **Unity** 2022.3 (LTS)
- **Google Mobile Ads** (rewarded ads)
- **Unity IAP + Unity Gaming Services**

---

## 📁 Project Structure

- `Assets/Scenes/` — Unity scenes (menu, gameplay, payment, loading)
- `Assets/Scripts/` — core gameplay + UI logic
- `Assets/Prefabs/` — reusable game objects

---

If you’d like a richer README (build steps, contribution guide, or playtest notes), just say the word!
