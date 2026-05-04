# Eclipse Realms — ULTRAPLAN
> "MapleStory's bones, Diablo II's loot, Eclipse Realms' soul."

This is the master execution plan to take the project from documentation + empty Unity shell to a playable, MapleStory-style co-op ARPG. It supersedes the high-level roadmap in `Eclipse_Realms_Dev_Blueprint.md` where they conflict, and explicitly re-targets the genre to **2D side-scrolling action-MMO** rather than 2.5D isometric ARPG.

---

## 0. Genre Pivot — Why MapleStory Style

The existing design docs already lean into MapleStory-friendly conventions (job evolutions, dual realms, sockets/runewords, auction house, seasonal resets). Pivoting to 2D side-scrolling is the cheapest, fastest, most distinctive path because:

- **Solo-dev scope:** 2D sprite art + tilemap is ~10x cheaper than 2.5D rigged 3D.
- **Identity:** the realm-shift mechanic is *visually loud* in 2D — recolor LUTs, parallax swap, lighting flip. In iso 3D it would be a quieter shader trick.
- **Networking:** 2D side-scrollers are forgiving to lag, which makes Fish-Networking + Steam P2P actually viable for 1–4p co-op.
- **Genre clarity:** "MapleStory × Diablo II" is a one-line pitch. Mood-board is instantly readable.

Anything in the existing docs that assumes iso 3D (camera, 3D physics, NavMesh) is **demoted to "future"** and replaced with the 2D analogue below.

---

## 1. Vision (One Page)

- **Pitch:** A dark-fantasy 2D side-scrolling MMO-lite where every map has a Light reflection and a Shadow reflection. Press one key to flip realms — terrain, music, mobs, loot, NPCs all change. Grind together in parties of 1–4, hunt limited drops with on-chain provenance, build with sockets + runewords, and choose a faction that re-shapes endgame.
- **Pillars:**
  1. **Side-scroll platforming combat** — jumps, double-jumps, dashes, weighty hit-stops.
  2. **Realm-shift everywhere** — every map has a dual layer; toggling is a moment-to-moment combat option, not a story beat.
  3. **Loot identity** — every legendary remembers its first owner; auctions are public; rare drops have global caps.
  4. **Friction-free co-op** — host-authoritative, Steam P2P, drop-in/drop-out parties.
  5. **Solo-dev sustainable** — small instanced maps > open world, ScriptableObject-driven content, Addressables for patching.

---

## 2. MapleStory ↔ Eclipse Realms Mapping

| MapleStory feature | Eclipse Realms equivalent | Status |
|---|---|---|
| Side-scrolling map with portals | "Realm Map" — dual-layer tilemap, portals to neighbor maps | M1 |
| Channels (parallel server instances) | Co-op sessions hosted by a player (FishNet) | M2 |
| 1st → 4th job advancement (10 / 30 / 60 / 100) | Warden / Arcanist / Rogue / Acolyte → Tier I→IV per Combat doc | M3 |
| Hotbar skills (1-9, QWERAS) | Same; abilities are SOs slotted into a `SkillBar` | M1 |
| Mob spawns + drop tables | `Spawner` MonoBehaviour driven by `MobTableSO` + `LootTableSO` | M1 |
| Equipment slots + scrolls/cubes | Equipment slots + Sockets/Runes/Runewords (already designed) | M2 |
| Free Market / Auction House | Supabase-backed Auction House (already designed) | M4 |
| Party play | FishNet party of 1–4, shared XP and loot rules | M2 |
| Town hub NPCs (shop, storage, smith) | Solari City, Nocturne Capital, Verge Crossroads NPCs | M2 |
| Pets / mounts | Charms (passive) + later, mountable Convergent companions | Stretch |
| Cash Shop cosmetics | Seasonal Tokens — purely cosmetic, no power | M5 |
| Boss raids | Eclipse Invasions, dungeon bosses with Limited Drops | M4 |

**Things we explicitly do NOT copy from MapleStory:** account-bound trade clutter, layered transparent gear with 19 enhancement systems, gacha cubes for power, P2W revival tokens. Power comes from runewords and play, cosmetics from cash.

---

## 3. Tech Stack (Revised for 2D)

| Layer | Choice | Why |
|---|---|---|
| **Engine** | Unity **2022.3 LTS** (bump from 2021.3) | URP 2D Renderer matured here; FishNet supports it cleanly |
| **Rendering** | URP **2D Renderer** + 2D Lights | Realm-shift = renderer feature toggle + LUT swap |
| **Input** | New Input System | Rebindable, controller-ready |
| **Physics** | Physics2D + Tilemap colliders | Standard side-scroller setup |
| **Camera** | Cinemachine 2D | Smooth follow, room-bounds confiner |
| **Animation** | 2D Animation + PSD Importer (skeletal) | Cheap to author, easy to swap costumes |
| **Tilemaps** | Rule Tile + Super Tilemap (eval) | Light/Shadow tile swapping |
| **Networking** | **FishNet** (free) | Host-authoritative, Steam P2P transport |
| **Backend** | Supabase (Postgres + Edge Functions + Auth) | Free tier viable to alpha |
| **Distribution** | Steam P2P + Cloudflare R2 (Addressables) | No relay cost |
| **CI** | GitHub Actions + GameCI | Headless build matrix (Win/Mac/Linux) |
| **Audio** | Unity audio + FMOD (later) | Realm-shift crossfade is a stretch goal |
| **Analytics/Errors** | Sentry + Supabase logs | Defer to M5 |

### Packages to add to `Packages/manifest.json`
- `com.unity.render-pipelines.universal` (URP)
- `com.unity.2d.animation`
- `com.unity.2d.psdimporter`
- `com.unity.2d.tilemap.extras`
- `com.unity.2d.pixel-perfect`
- `com.unity.cinemachine`
- `com.unity.inputsystem`
- `com.unity.addressables`
- `com.unity.nuget.newtonsoft-json`
- FishNet (manual import or via OpenUPM)

### Packages to remove (deadweight for a 2D game)
- `com.unity.modules.vehicles`, `com.unity.modules.cloth`, `com.unity.modules.terrain*`, `com.unity.modules.vr`, `com.unity.modules.xr`, `com.unity.visualscripting`

---

## 4. Asset Pipeline (Sprite-First)

- **Resolution target:** 320×180 internal, scaled. Pixel-perfect camera. Final render 1920×1080 / 2560×1440.
- **Sprite specs:** 32×32 tile units, 48–64px characters, 8-direction → reduced to L/R only (MapleStory style).
- **Folder convention:** `Assets/Art/{Characters|Mobs|Tilesets|VFX|UI}/{Realm}/{Subject}/...`
- **Realm variants:** every art asset that differs by realm has `_light` and `_shadow` siblings + a shared LUT. Tilemaps store the same world geometry; renderer swaps the spritesheet ref.
- **Naming:** `mob_solari_acolyte_idle.png`, `tile_umbral_obsidian_floor.asset`.
- **Authoring tools:** Aseprite (sprites), Tiled (optional — re-export to Unity Tilemap), Audacity (SFX).
- **Placeholder strategy:** ship M0 with **Kenney free tilesets + colored capsules**. Real art arrives at M5.

---

## 5. Core Loops

### Moment-to-moment (seconds)
Move → attack combo → spend a skill → dodge / realm-shift → loot.

### Map session (minutes)
Enter map → clear waves of mobs → mini-boss → portal to next map or back to town.

### Daily (per session)
Quest stack → 2–3 dungeon clears → AH check → realm-shift events → log out.

### Weekly
Eclipse Invasion world boss → faction reputation tier → seasonal milestone.

### Seasonal (33 weeks, mirroring lore)
Reset → new Limited Drops → new runewords pool → legacy items become "Echo" cosmetics.

---

## 6. Systems Spec

### 6.1 Player Controller (M0)
- Side-scrolling: WASD move, Space jump (double-jump unlock), Shift dash (i-frames), J basic, K skill1, L skill2, U/I/O/P potions/utility.
- Realm-shift: hold `Tab` to preview both realms, tap to commit. Cooldown 6s.
- States: Idle / Run / Jump / Fall / Attack / Dash / Hurt / Dead. FSM via lightweight state classes.
- Stats from `Eclipse_Realms_Combat_Progression.md` — STR/INT/DEX/VIT/WIS/LCK; derived AP/MP/Def/Crit calculated in `StatsComponent`.

### 6.2 Combat (M0 → M1)
- Hitboxes via `BoxCollider2D` triggers spawned per attack frame.
- Damage formula from Combat doc; crits roll on hit; floating combat numbers (object-pooled).
- Status effects (Burn / Bleed / Freeze / Curse / Radiance / Corruption) implemented as `StatusEffectSO` applied to a `StatusController` on every combatant.
- Host validates damage; clients receive `OnDamage(victimId, amount, type)` RPC.

### 6.3 Items / Inventory / Sockets / Runewords (M1 → M2)
- All items are `ItemSO` ScriptableObjects + per-instance `ItemInstance` records (rolls, sockets, provenance).
- Inventory: 32 slots, drag-drop, equipment panel, stash (60 slots, account-bound).
- Sockets: 0–3 per item, fillable with `RuneSO`. `RunewordSO` defines required sequence + grants effects.
- Drops: `LootTableSO` with weighted entries by rarity (Mortal → Singular). Limited drops query Supabase before granting.

### 6.4 Realm-Shift (M1)
- Two `Tilemap` layers per map: `Tilemap_Light`, `Tilemap_Shadow`. Only one is active.
- A `RealmManager` toggles active layer + spawner table + ambient music + URP 2D renderer asset (Light/Shadow LUT).
- Mobs spawned in one realm despawn (or convert to "echo") on shift.
- Some platforms exist *only* in one realm — puzzles built around this.

### 6.5 Maps & Portals (M1)
- Each map = scene OR addressable prefab loaded into a persistent `World` scene.
- `Portal` triggers transfer player to target map at named spawn point.
- Town hubs: `solari_plaza`, `nocturne_market`, `verge_crossroads` (neutral, has stash + AH terminal).

### 6.6 Skills & Jobs (M3)
- `SkillSO` defines cost, cooldown, damage formula, animation, FX.
- `JobSO` defines available skills per tier, evolution requirements (level + faction reputation + quest flag).
- Skill Bar: 8 slots, drag SO into slot, persisted per character.

### 6.7 Quests (M4)
- JSON-backed quest definitions loaded via Addressables; binding to NPC dialogue via Yarn Spinner (later) or simple branching SO trees first.
- Player quest state stored in Supabase `player_quests`.

### 6.8 Auction House (M4)
- Listing → server holds item, deletes from inventory, inserts into `auctions`.
- Buy → SQL transaction: deduct gold, transfer item, append provenance row, 5% tax to faction fund.
- Anti-dupe: server-side only; client never moves items between players.

### 6.9 Limited Drops (M4)
- Supabase Edge Function `claim_drop(drop_id, player_id)` increments `claimed_count` atomically; returns "claimed" or "exhausted". Host calls it before granting.
- Provenance: every transfer appends `{timestamp, from, to, reason}` to the item's provenance array.

### 6.10 Networking (M2)
- FishNet host-authoritative. Steam P2P transport (or Tugboat for dev).
- Authoritative on host: position, hits, RNG drops, status, mob HP.
- Clients send: input intents, attack commands, skill activations.
- Snapshot rate: 20 Hz; interpolation 100 ms.
- Anti-cheat: server-side stat caps, rate limits on craft/trade/AH RPCs.

---

## 7. Project Folder Layout (Assets/)

```
Assets/
├── Art/                    # sprites, tilesets, vfx, ui textures
│   ├── Characters/
│   ├── Mobs/
│   ├── Tilesets/
│   ├── UI/
│   └── VFX/
├── Audio/
├── Data/                   # ScriptableObject assets (content)
│   ├── Items/
│   ├── Runes/
│   ├── Runewords/
│   ├── Charms/
│   ├── Mobs/
│   ├── Skills/
│   ├── Jobs/
│   ├── LootTables/
│   ├── Quests/
│   └── StatusEffects/
├── Prefabs/
│   ├── Player/
│   ├── Mobs/
│   ├── Pickups/
│   ├── UI/
│   └── World/
├── Scenes/
│   ├── Boot.unity
│   ├── Town_VergeCrossroads.unity
│   └── Test_Sandbox.unity
├── Scripts/
│   ├── Core/               # FSM, services, events
│   ├── Combat/
│   ├── Stats/
│   ├── Items/
│   ├── Inventory/
│   ├── Skills/
│   ├── Jobs/
│   ├── World/              # maps, portals, realm-shift
│   ├── Net/                # FishNet glue
│   ├── Backend/            # Supabase client
│   ├── UI/
│   ├── Quests/
│   └── Utils/
├── Settings/               # URP assets, input actions, render features
└── Addressables/
```

Each folder gets a `.asmdef` to keep compile times fast; `Net` and `Backend` are last to compile so iteration on combat is snappy.

---

## 8. Roadmap — Milestones with Deliverables and Acceptance Criteria

> **Cadence assumption:** ~10–15 hrs/week solo. Slip is expected; ship at the milestone, not the date.

### M0 — Walkable Prototype (Weeks 1–3)
- [ ] Unity bumped to 2022.3 LTS; URP 2D + 2D Animation + Cinemachine + Input System added; deadweight modules removed.
- [ ] `Assets/` skeleton + asmdefs created.
- [ ] `Boot.unity` and `Test_Sandbox.unity` scenes exist.
- [ ] Player prefab walks/jumps/dashes on a Kenney tilemap.
- [ ] One enemy slime prefab patrols and damages on contact.
- [ ] HUD shows HP/MP placeholder bars driven by `StatsComponent`.
- **Acceptance:** standalone Win build runs; sandbox playable for 60 seconds without errors.

### M1 — Combat & Realm-Shift Vertical Slice (Weeks 4–6)
- [ ] Basic-attack hitbox + 3 enemy types (melee, ranged, caster).
- [ ] One Tier-I skill per future class (4 total) usable on debug character.
- [ ] `RealmManager` swaps tilemap layer + LUT + spawner; press `Tab` to flip.
- [ ] `LootTableSO` drops `ItemSO` instances; pickup → inventory.
- [ ] Inventory UI (32 slots) drag-drop.
- **Acceptance:** clear a 3-room sandbox in both realms, equip a dropped sword, see stats change.

### M2 — Co-op + Hub Town (Weeks 7–9)
- [ ] FishNet integrated; host can start a session, 1 client connects via direct IP (Steam P2P swap later).
- [ ] Position/animation/damage/loot all replicate.
- [ ] Verge Crossroads town scene with: spawn point, vendor NPC (sells potions), stash NPC, AH stub NPC.
- [ ] Portal system between town and sandbox dungeon.
- **Acceptance:** two clients fight a slime in the same dungeon, both see drops, both can return to town.

### M3 — Jobs, Skills, Progression (Weeks 10–13)
- [ ] 4 base classes selectable at character creation (`Warden / Arcanist / Rogue / Acolyte`).
- [ ] Level 1–30 progression with per-level stat/skill points.
- [ ] 2nd-job evolution at Lv30 (one quest gate per class).
- [ ] 6 skills per class (3 Tier-I, 3 Tier-II).
- [ ] Skill Bar UI with cooldowns, mana costs, hotkeys.
- **Acceptance:** create a Warden, level to 30 in test dungeon, evolve to Crusader, slot Tier-II skill.

### M4 — Backend, Persistence, Economy (Weeks 14–17)
- [ ] Supabase project provisioned; schema from blueprint deployed.
- [ ] Auth (email + Steam OpenID) → `players` row.
- [ ] Inventory/character persistence on logout / interval save.
- [ ] Auction House MVP: list, browse, buy, 5% tax.
- [ ] Limited Drops Edge Function with global cap enforcement.
- [ ] One Mythic drop added to a boss to validate provenance flow.
- **Acceptance:** kill boss in client A, sell on AH, client B buys, both see provenance trail in tooltip.

### M5 — Content & Polish for Closed Alpha (Weeks 18–22)
- [ ] 3 dungeons × 2 realms = 6 map variants.
- [ ] 12 mobs, 3 mini-bosses, 1 raid boss (Eclipse Invasion stub).
- [ ] First-pass real art on player + 3 mobs + 1 tileset.
- [ ] Audio pass: 6 SFX, 2 music tracks (Light + Shadow stems).
- [ ] Settings menu (resolution, key rebind, audio).
- [ ] Sentry crash reporting wired.
- **Acceptance:** invite 5 friends to play 60 min, < 3 critical bugs.

### M6 — Steam Page + Closed Beta (Weeks 23–28)
- [ ] Steam page live, capsule art, 30s trailer.
- [ ] Steamworks integrated (achievements, P2P transport, friends invite).
- [ ] Closed beta key distribution via Discord bot.
- [ ] Telemetry dashboard (DAU, deaths/map, AH GMV).
- **Acceptance:** 50 beta users, ≥ 30% Day-1 retention.

### M7 — Early Access Launch (Weeks 29+)
- [ ] One full faction storyline (Solari OR Nocturne, not both) playable end-to-end Lv1→60.
- [ ] Seasonal infrastructure live (token, leaderboard, reset job).
- [ ] Cosmetics shop (Seasonal Tokens earned in-game; optional cash purchase).
- **Acceptance:** ship to Steam EA, ≥ 70% positive reviews in first 100.

---

## 9. Backend Schema Deltas (additions to blueprint)

```sql
-- characters: a player can have multiple
create table characters (
  id uuid primary key,
  player uuid references players(id),
  name text unique not null,
  job text not null,           -- 'warden','crusader',...
  level int not null default 1,
  xp bigint not null default 0,
  stats jsonb not null,         -- STR/INT/DEX/VIT/WIS/LCK + allocations
  alignment_light int not null default 0,
  alignment_shadow int not null default 0,
  faction text,
  created_at timestamptz default now()
);

create table player_quests (
  character uuid references characters(id),
  quest_id text not null,
  state text not null,          -- 'active','completed','failed'
  flags jsonb default '{}'::jsonb,
  primary key (character, quest_id)
);

create table sessions (
  id uuid primary key,
  host uuid references characters(id),
  realm text,
  started_at timestamptz default now(),
  ended_at timestamptz
);

-- provenance is already on player_items.provenance jsonb[]
```

Edge Functions:
- `claim_limited_drop(drop_id, character_id)` — atomic increment
- `place_auction(...)`, `buy_auction(...)` — transactional
- `evolve_job(character_id, target_job)` — gated on level/faction/quest

---

## 10. Live-Ops, Monetization, Anti-Toxicity

- **Free-to-play with cosmetic-only purchases.** No power.
- **Seasonal Tokens** earned in-game also purchasable; spend on cosmetics, name changes, stash tabs.
- **Account-bound stash; tradeable inventory.** Most items can trade once before binding (MapleStory-style scroll-of-vega tradeability rule). Mythics bind on equip; their provenance trail still public.
- **Auction tax** scales with server economy (3% → 8%) per Economy doc.
- **Anti-bot:** server-side rate limit on map-clear xp gain; behavior-anomaly flagging on AH listings.
- **Reporting:** in-game report → Supabase moderation queue.

---

## 11. Risks & Cuts

| Risk | Mitigation |
|---|---|
| FishNet + Steam P2P NAT fails for some players | Fallback: a single $5/mo VPS running headless host as relay option |
| 2D art bottleneck stalls M5 | Commission one freelancer for player + 3 mobs; everything else placeholder for alpha |
| Supabase free tier outgrown | Pro plan budgeted (€25/mo); shard reads via PostgREST cache |
| Realm-shift complexity explodes scenes | Constraint: dungeon designers must build **both layers in the same Tilemap component**, just on different sub-layers |
| Solo-dev scope creep | Hard cap: no new system after M4 until alpha ships |

**Explicit cuts (postpone past EA):** mounts, pets that fight, guild halls, PvP, cross-platform, mobile.

---

## 12. Immediate Next Actions (Top 10 PRs)

1. **Bump Unity to 2022.3 LTS** — edit `ProjectSettings/ProjectVersion.txt` + `Packages/manifest.json`.
2. **Add URP 2D + 2D Animation + Cinemachine + Input System + Addressables** to manifest; remove deadweight modules.
3. **Create `Assets/` folder skeleton + asmdefs** as in §7.
4. **Boot scene + Test_Sandbox scene** with pixel-perfect camera.
5. **`StatsComponent` + `PlayerController` + Input Actions asset** — walk, jump, dash, basic attack.
6. **`ItemSO`, `RuneSO`, `RunewordSO`, `CharmSO`, `MobSO`, `LootTableSO`** ScriptableObject base classes.
7. **`RealmManager` + dual-layer Tilemap demo** in sandbox scene.
8. **Spawner + first slime mob** with patrol/aggro/attack/die/loot.
9. **Inventory data model + UI panel** (32 slots, drag-drop, equip).
10. **`README.md` and `CONTRIBUTING.md`** at repo root with how-to-run + branch policy.

Each PR should be small, branch-named `feat/<area>-<verb>`, merged into `claude/continue-game-project-zTeR9` until that branch is promoted.

---

## 13. Definition of Done (per feature)

A feature is "done" only when **all** of the following are true:
- It compiles with zero errors and zero new warnings.
- It runs in `Test_Sandbox.unity` without throwing in 60 seconds of normal play.
- Its public API has at least one EditMode or PlayMode test (when Test Framework matures around M3).
- Its data lives in a ScriptableObject, not hard-coded constants.
- A ten-word changelog entry is added to `CHANGELOG.md`.

---

## 14. Open Questions (decide before M0 starts)

1. Unity version: **bump to 2022.3 LTS now** (recommended) or stay on 2021.3?
2. Networking package: **FishNet via OpenUPM** or manual import?
3. First playable class for M0/M1: **Warden** (simple melee, easiest to validate combat) or your preference?
4. Hub town for M2: **Verge Crossroads** (neutral) or pick one realm first?
5. Art strategy: **Kenney placeholder through M4** then commission, or pay for art earlier?

Answer these and we move to PR #1.
