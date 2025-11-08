# Eclipse Realms – Unity Setup Playbook

You already have the code scaffolding (URP + FishNet + gameplay scripts). Follow these steps inside the **Unity Editor** to get the first playable slice running. Keep this file open in your IDE and check items off as you go.

---

## 1. Prep & Verification

1. **Open** the project in Unity 2023 LTS. Wait for packages to import (FishNet + URP).  
2. In the top menu run `Edit → Render Pipeline → Universal Render Pipeline → Upgrade Project Materials to URP`.  
3. Press `Ctrl+S` to save any open scenes, then continue.

> _Screenshot cue:_ Grab a screenshot of the Package Manager showing `FishNet` + `Universal RP` in the Installed tab if you need progress proof.

---

## 2. Create the Prototype Scene

1. In the Project window right–click `Assets/Scenes → Create → Scene` and name it **PrototypeHub**.  
2. Double–click the scene to open it and remove the default skybox fog if desired.  
3. Add a basic ground plane (`GameObject → 3D Object → Plane`) and a Directional Light (URP default is fine).  
4. Save the scene (`Ctrl+S`).  

> Screenshot cue: Scene view with plane + light and the hierarchy showing `PrototypeHub`.

---

## 3. Network Bootstrap

1. From the Hierarchy choose `Fish-Networking → NetworkManager`.  
2. Add the `FishNetBootstrapper` component (found in `Assets/Net/Bootstrap`).  
3. (Optional) Create UI buttons to call `StartHost`, `StartClient`, `StopAll` via OnClick events later.

---

## 4. Player Prefab

1. In the Hierarchy create `GameObject → 3D Object → Capsule`; rename to `PlayerPrefab`.  
2. Add components:  
   - `CharacterController`  
   - `PlayerStatsComponent` (`Assets/Core/Player`)  
   - `TopDownMover` (`Assets/Core/Player`)  
3. Set `TopDownMover.FacingProxy` to the capsule mesh (or add a child transform called `Pivot`).  
4. Adjust stats in `PlayerStatsComponent` (e.g., STR 8, DEX 10, VIT 12, others 5).  
5. Drag the object into `Assets/Core/Player` to create a prefab, then delete the scene copy (it will spawn via FishNet).  
6. Select the `NetworkManager`, scroll down to the **ServerManager** component, expand `Settings`, and drag the prefab into `Default Player Prefab`.

> Screenshot cue: Inspector of `PlayerPrefab` showing CharacterController + scripts.

---

## 5. Enemy Prefab & Spawner

1. Duplicate the capsule, rename to `EnemyPrefab`, add:  
   - `CharacterController`  
   - `SimpleEnemyBrain` (`Assets/Core/Combat`)  
2. Save it to `Assets/World/Spawners`.  
3. Create an empty GameObject `EnemySpawner`, add the `EnemySpawner` script, assign the prefab, set `Initial Count` (e.g., 3) and `Spawn Radius` (e.g., 6). Place it near the arena.

---

## 6. HUD Overlay

1. Add `GameObject → UI → Canvas (Screen Space - Overlay)` and `UI → Text - TextMeshPro`.  
2. Create an empty object `HUDController`, add `PlayerHudController` (`Assets/UI/HUD`).  
3. Assign the text field to `Stats Label`. During play mode the script fills it with STR/DEX/etc.  
4. Drag the player prefab from the Project window into the `Player` reference field so the HUD knows what to read.

---

## 7. ScriptableObject Samples

Create a handful of data assets so inventory/economy systems have content:

| Asset Type | Path | Menu |
|------------|------|------|
| Base item | `Assets/Items` | `Create → Eclipse Realms → Items → Item Definition` |
| Rune | `Assets/Items/Runes` | `Create → Eclipse Realms → Items → Rune` |
| Runeword | `Assets/Items/Runewords` | `Create → Eclipse Realms → Items → Runeword` |
| Charm | `Assets/Items/Charms` | `Create → Eclipse Realms → Items → Charm` |
| Inventory template | `Assets/Inventory/Data` | `Create → Eclipse Realms → Inventory → Player Inventory Template` |

Fill in IDs, tiers, and text per the design docs.

---

## 8. Playtest Loop

1. Hit **Play**; open the `NetworkManager` component and click `Start Host` (or use UI buttons later).  
2. The player should spawn via FishNet, move with WASD, and enemies should chase.  
3. Open the Game view and take a screenshot once the loop works (for dev logs / documentation).

---

## 9. Next Targets

- Hook FishNet UI buttons (Start Host/Client) onto an in‑scene canvas.  
- Add drop logic: when an enemy dies, instantiate a ScriptableObject reference from your sample item list.  
- Begin realm shift visuals by duplicating the scene lighting and toggling via `RealmAlignmentState`.  
- When stable, commit everything except `Library/`, `Temp/`, `Logs/`, `obj/`.

You can extend this playbook—add checkboxes or notes as you iterate. Ping Codex whenever a step needs code changes (new scripts, addressable setup, Supabase hooks, etc.).
