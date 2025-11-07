# Eclipse Realms – Combat & Progression System

## ⚔️ Core Combat Philosophy
Fast, reactive ARPG combat combining Diablo II’s impact with MapleStory’s responsiveness.  
Host-authoritative to prevent exploits, deterministic hit logic.

### Design Pillars
- **Clarity** – every hit must feel weighty and readable.
- **Expression** – skills and builds enable player creativity.
- **Risk vs Reward** – high-damage builds demand precision timing.
- **Duality** – every Light ability has a Shadow counterpart.

---

## 🧩 Player Stats

| Stat | Description | Affects |
|------|--------------|---------|
| **STR** | Physical Power | Melee damage, carry weight |
| **INT** | Magical Power | Spell damage, mana regen |
| **DEX** | Agility & Accuracy | Crit chance, dodge, ranged dmg |
| **VIT** | Vitality | HP, resistances |
| **WIS** | Willpower | Charm slots, resistance to corruption |
| **LCK** | Fortune | Drop quality, crit damage |

Derived:  
- **Attack Power = Base + WeaponBonus × (1 + STR × 0.3)**  
- **Magic Power = Base + StaffBonus × (1 + INT × 0.4)**  
- **Defense = Armor × (1 + VIT × 0.25)**  
- **Crit Chance = DEX × 0.15% + ItemMods**  

---

## 🧙 Classes (Tier 1 → Tier 2)

| Base Class | Evolutions | Example Abilities |
|-------------|-------------|------------------|
| **Warden** | Crusader / Berserker | Shield Bash, Holy Guard |
| **Arcanist** | Elementalist / Chronomancer | Fire Nova, Time Distortion |
| **Rogue** | Shadow Dancer / Ranger | Smoke Bomb, Arrow Rain |
| **Acolyte** | Necrosaint / Soulbinder | Dark Pulse, Life Exchange |

---

## 🔮 Realm Alignment System

Each player gradually aligns toward **Light** or **Shadow** based on story choices, charms, and skill use.

| Alignment | Buff | Debuff |
|------------|------|--------|
| **Light‑Aligned** | +10% healing, +5% Aether regen | −10% Shadow resistance |
| **Shadow‑Aligned** | +8% damage, +5% lifesteal | −8% Light resistance |
| **Balanced (Neutral)** | +5% drop luck | −5% XP gain |

Realm alignment can unlock special Runewords and Charms.

---

## ⚔️ Combat Flow

1. **Engage** – lock on or free aim (depending on weapon).  
2. **Combo Chain** – basic → skill → finisher (3‑step cancel system).  
3. **Realm Reactions** – triggers Light or Shadow effects.  
4. **Eclipse State** – temporary dual‑realm buff (ultimate mode).

---

## 💥 Status Effects

| Effect | Source | Description |
|---------|--------|-------------|
| **Burn** | Fire | DOT scaling with INT |
| **Bleed** | Physical | Stacks, scales with DEX |
| **Freeze** | Frost | Stuns, duration by WIS |
| **Curse** | Shadow | Reduces stats |
| **Radiance** | Light | Prevents healing |
| **Corruption** | Mixed | Dual effect: buff + DOT |

---

## 🧠 Progression

- **Level Cap:** 100  
- **XP Curve:** exponential (classic ARPG)  
- **Skill Points:** +1 per level → spend in tree (4 tiers)  
- **Stat Points:** +5 per level → allocate manually  
- **Realm Affinity:** shifts every 10% threshold → visual & mechanical changes.  

---

## 🧙 Ability Tiers

| Tier | Unlock | Description |
|------|---------|-------------|
| I | Level 1 – 10 | Core combat abilities |
| II | Level 11 – 30 | Specialization paths |
| III | Level 31 – 60 | Realm‑specific powers |
| IV | Level 61 – 100 | Eclipse abilities (Light + Shadow merge) |

---

## 🏆 Endgame Loops
- Dungeon Bosses → rare Runes, Relics  
- Realm Events → temporary global buffs  
- Seasonal resets every 33 weeks (mirrors Eclipse cycle)  
- NG+ (Rebirth) → retain 1 relic, +1% XP gain stacking  

