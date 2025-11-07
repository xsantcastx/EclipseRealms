# Eclipse Realms – Development Blueprint

## 🧩 Overview
Solo‑dev plan for **Eclipse Realms**, a dark‑fantasy ARPG inspired by Diablo II, MapleStory, and RuneScape.  
Focus: deep loot, realm‑shift mechanic, player‑driven economy, and co‑op sessions (1–4 players).

---

## ⚙️ Tech Stack (Cheap + Scalable)

| Layer | Choice | Notes |
|-------|---------|-------|
| **Engine** | Unity 2023 LTS + URP | 2.5D isometric, Shader Graph, Addressables |
| **Networking** | Fish‑Networking | Free, supports Host/Client, good perf |
| **Auth / Backend** | Supabase (Postgres + Edge Functions) | Free tier, SQL power, realtime feeds |
| **Distribution** | Steam P2P + Addressables CDN (Cloudflare) | No relay cost |
| **Audio** | FMOD (optional) | Realm‑shift crossfades |
| **Analytics** | Supabase + Grafana/Sentry (later) | Errors + economy metrics |

---

## 🧱 Architecture

### Unity Project Layout
```
/Core          – state machines, abilities, combat logic
/Items         – ScriptableObjects: Items, Runes, Runewords, Charms, Relics
/Inventory     – equipment, sockets, codex, crafting
/Economy       – drop tables, rarity, auction client
/World         – biomes, spawners, realm‑shift layers
/Net           – FishNet prefabs, session manager
/UI            – HUD, inventory, codex, auction
/Server        – headless config (for later dedicated tests)
/Scripts/Utils – helpers, extensions
/Addressables  – icons, VFX, audio, data JSON
```

### Backend Schema (Supabase / Postgres)

**players**
```sql
id uuid primary key,
handle text unique not null,
created_at timestamptz default now()
```

**item_catalog**
```sql
id text primary key,
slot text not null,
base_stats jsonb not null,
sockets int not null default 0,
rarity text not null
```

**rune_catalog**
```sql
id text primary key,
alignment text check (alignment in ('Light','Shadow','Neutral')),
tier int not null
```

**runewords**
```sql
id text primary key,
sequence text[] not null,
constraints jsonb,
effects jsonb
```

**player_items**
```sql
instance_id uuid primary key,
owner uuid references players(id),
catalog_id text references item_catalog(id),
rolls jsonb not null,
sockets text[] default '{}',
provenance jsonb default '[]',
season text,
bound boolean default false
```

**limited_drops**
```sql
drop_id text primary key,
max_count int not null,
claimed_count int not null default 0
```

**auctions**
```sql
id uuid primary key,
item_instance uuid references player_items(instance_id),
seller uuid references players(id),
price_bigint bigint not null,
expires_at timestamptz,
status text check (status in ('active','sold','expired')) default 'active'
```

---

## 🧠 Core Systems

### 1. Runewords
- Socketed items store Rune IDs.
- Host validates sequence → applies Runeword effects.
- Light + Shadow mix = “Unstable” variants.

### 2. Charms / Soul Slots
- Passive buffs stored in player’s Codex.
- 3–9 slots; realm alignment flips effects on shift.
- Ethereal charms expire after real‑time.

### 3. Limited Drops & Provenance
- Supabase ledger tracks global counts.
- Tooltip displays first owner + history.

### 4. Auction House
- SQL transactions prevent dupes.
- Tax % configurable via environment var.

### 5. Realm Shift
- Two URP renderers (Light/Shadow) toggled by shader keyword.
- Spawners, loot tables, music stem swap per realm.

---

## 🛠️ Authoritative Loop (Host = Server)
1. Client inputs → host validates (movement, hits, RNG drops).  
2. Host sends snapshots to peers.  
3. Crafting & limited drops verified in Supabase Edge Function.  
4. Persist item instance → Postgres.

---

## 🔐 Anti‑Cheat Basics
- All item creation via host validation + signed hash.  
- Server‐side drop tables.  
- Rate‑limit RPCs (craft, trade).  
- Sanity checks for impossible rolls.

---

## 💰 Monthly Estimate
| Item | Cost |
|------|------|
| Steam P2P | €0 |
| Supabase Pro | €25–€49 |
| Cloudflare R2 (CDN) | €0–€5 |
| Domain/Email | €2–€5 |
| **≈ Total** | **€30–€60/mo max** |

---

## 🗓️ Roadmap (Solo Dev)

| Phase | Weeks | Goal | Deliverables |
|-------|--------|------|---------------|
| **M0 – Prototype** | 0‑2 | Core combat loop | Movement, 1 enemy, inventory, SO items |
| **M1 – Co‑op** | 3‑4 | FishNet host/peer, realm shift | 1–4 players, 2 Runewords, 3 Charms |
| **M2 – Backend** | 5‑6 | Supabase integration | Auth, inventory save, limited drops ledger |
| **M3 – Economy** | 7‑8 | Auction House + Season flags | Trade, tax, rarity pools |
| **M4 – Playtest** | 9‑10 | Steam demo | Feedback loop, balance pass |
| **M5 – Content Push** | 11‑12+ | Lore, codex, UI polish | First public build |

---

## 🧩 Key Principles
- **Authoritative host:** Combat, loot, crafting.  
- **Supabase validates:** limited drops, auctions.  
- **Addressables:** rapid content patching.  
- **Small scope:** instanced dungeons > open world.  
- **Player legacy:** every rare drop tells a story.

---

## 🚀 Next Step
- Initialize Unity project (URP + FishNet).  
- Create ScriptableObjects for base items, runes, runewords, charms.  
- Connect Supabase (auth + PostgREST).  
- Implement minimal dungeon + realm shift + loot drop.  
- Prepare Steam P2P integration for early access.
