# Ball Game (Money Gain Plinko)

A Unity-based, mobile-style plinko/ball drop game where players spawn balls, collect stars and gems, and redeem earned gems through a timed payout system. The project includes ad rewards, in-app purchases, and a dedicated payout screen tied to gem accumulation. 【F:Assets/Scripts/BallSpawner.cs†L1-L87】【F:Assets/Scripts/Collectible.cs†L1-L53】【F:Assets/Scripts/PayoutTimerManager.cs†L1-L106】

## Screenshots

> **Drop your screenshots here**
>
> - `![Main Menu](path/to/screenshot-main-menu.png)`
> - `![Gameplay](path/to/screenshot-gameplay.png)`
> - `![Payout Screen](path/to/screenshot-payout.png)`

## Gameplay Overview

- **Tap/click to spawn balls** at the top of the playfield. Spawns are clamped between left/right screen borders to keep the drop inside the game area. 【F:Assets/Scripts/BallSpawner.cs†L1-L87】【F:Assets/Scripts/ScreenEdgePlacer.cs†L1-L93】
- **Limited ball inventory**: each spawn consumes a ball; when you run out, the game stops active balls and shows an out-of-balls panel. 【F:Assets/Scripts/GameManager.cs†L13-L167】
- **Scoring with stars and gems**: stars add score points, and gems add to the gem total (with randomized gem values). 【F:Assets/Scripts/Collectible.cs†L1-L53】
- **Bonus boxes** can reward stars or gems on trigger. 【F:Assets/Scripts/BonusBoxController.cs†L1-L49】
- **Obstacles** can take multiple hits before being destroyed and respawned. 【F:Assets/Scripts/DestructibleObstacle.cs†L1-L31】【F:Assets/Scripts/CollectibleSpawner.cs†L1-L87】
- **Death plane cleanup** removes balls that fall out of bounds and checks for end-of-ball conditions. 【F:Assets/Scripts/DeathPlane.cs†L1-L27】【F:Assets/Scripts/GameManager.cs†L94-L133】

## Features

- **Persistent player progress** for balls and score using PlayerPrefs. 【F:Assets/Scripts/GameData.cs†L1-L69】
- **Gem payout cycle** with a 3-hour timer that resets gems when the cycle ends. 【F:Assets/Scripts/PayoutTimerManager.cs†L1-L106】
- **Payout screen** showing total gems, payout progress, and a redeem flow that posts to a server. 【F:Assets/Scripts/PaymentPageManager.cs†L1-L205】
- **Rewarded ads** to grant extra balls via AdMob rewarded videos. 【F:Assets/Scripts/AdsManager.cs†L1-L145】【F:Assets/Scripts/RewardAdButton.cs†L1-L40】【F:Assets/Scripts/GameManager.cs†L63-L92】
- **In-app purchase** for buying ball packs (100 balls per purchase). 【F:Assets/Scripts/IAPManager.cs†L1-L178】【F:Assets/Scripts/GameManager.cs†L36-L84】
- **Audio controls** with mute toggle and persistent background music. 【F:Assets/Scripts/AudioManager.cs†L1-L71】【F:Assets/Scripts/MainMenu.cs†L1-L95】
- **Loading scene** with progress bar and minimum load duration. 【F:Assets/Scripts/LoadingManager.cs†L1-L90】
- **Responsive game area scaling** based on screen edges for consistent layout. 【F:Assets/Scripts/GameAreaScaler.cs†L1-L63】

## Scenes

- **LoadingScene** → loads the main menu asynchronously. 【F:Assets/Scripts/LoadingManager.cs†L8-L80】
- **0_MainMenu** → start new game, load game, settings, and info panels. 【F:Assets/Scripts/MainMenu.cs†L1-L95】
- **1_Level** → core gameplay, ball spawning, collectibles, and payout timer HUD. 【F:Assets/Scripts/BallSpawner.cs†L1-L87】【F:Assets/Scripts/LevelPayoutTimer.cs†L1-L52】
- **PaymentScene** → gem redemption and payout progress UI. 【F:Assets/Scripts/PaymentPageManager.cs†L1-L205】

## Controls

- **Mouse/touch**: tap or click to drop a ball into the playfield. 【F:Assets/Scripts/BallSpawner.cs†L41-L87】

## Tech Stack

- **Unity**: 2022.3.62f3 (LTS). 【F:ProjectSettings/ProjectVersion.txt†L1-L2】
- **Ads**: Google Mobile Ads (rewarded). 【F:Assets/Scripts/AdsManager.cs†L1-L145】
- **IAP**: Unity IAP with Unity Gaming Services initialization. 【F:Assets/Scripts/IAPManager.cs†L1-L84】

## Project Structure

- `Assets/Scenes/` — Unity scenes (main menu, gameplay, payment, loading).
- `Assets/Scripts/` — Gameplay logic, UI flow, ads, IAP, and payout systems.
- `Assets/Prefabs/` — Reusable game objects (balls, collectibles, obstacles).

