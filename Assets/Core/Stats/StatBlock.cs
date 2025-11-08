using System;
using UnityEngine;

namespace EclipseRealms.Core.Stats
{
    /// <summary>
    /// Shared stat payload for players, enemies, and scalable items.
    /// Mirrors the stat table defined in Eclipse_Realms_Combat_Progression.md.
    /// </summary>
    [Serializable]
    public struct StatBlock
    {
        [Header("Primary Attributes")]
        [Min(0)] public int Strength;
        [Min(0)] public int Intelligence;
        [Min(0)] public int Dexterity;
        [Min(0)] public int Vitality;
        [Min(0)] public int Wisdom;
        [Min(0)] public int Luck;

        public static StatBlock operator +(StatBlock lhs, StatBlock rhs)
        {
            return new StatBlock
            {
                Strength = lhs.Strength + rhs.Strength,
                Intelligence = lhs.Intelligence + rhs.Intelligence,
                Dexterity = lhs.Dexterity + rhs.Dexterity,
                Vitality = lhs.Vitality + rhs.Vitality,
                Wisdom = lhs.Wisdom + rhs.Wisdom,
                Luck = lhs.Luck + rhs.Luck
            };
        }

        public float AttackPower(float baseValue, float weaponBonus) =>
            baseValue + weaponBonus * (1f + Strength * 0.3f);

        public float MagicPower(float baseValue, float focusBonus) =>
            baseValue + focusBonus * (1f + Intelligence * 0.4f);

        public float Defense(float armor) =>
            armor * (1f + Vitality * 0.25f);

        public float CritChance(float itemModsPercent = 0f) =>
            Dexterity * 0.0015f + itemModsPercent;

        public float CritDamageMultiplier(float baseMultiplier = 1.5f) =>
            baseMultiplier + Luck * 0.01f;

        public int CharmSlots(int baseSlots = 3) =>
            Mathf.Clamp(baseSlots + Mathf.FloorToInt(Wisdom / 10f), baseSlots, 12);

        public override string ToString() =>
            $"STR:{Strength} INT:{Intelligence} DEX:{Dexterity} VIT:{Vitality} WIS:{Wisdom} LCK:{Luck}";
    }
}
