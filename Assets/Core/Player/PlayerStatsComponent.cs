using EclipseRealms.Core.Combat;
using EclipseRealms.Core.Stats;
using UnityEngine;

namespace EclipseRealms.Core.Player
{
    /// <summary>
    /// Central point for player stat progression + realm alignment hooks.
    /// </summary>
    public class PlayerStatsComponent : MonoBehaviour
    {
        [SerializeField] private StatBlock baseStats;
        [SerializeField] private RealmAlignmentState realmAlignment = new();

        public StatBlock CurrentStats => baseStats;
        public RealmAlignmentState AlignmentState => realmAlignment;

        public float CurrentAttackPower(float baseValue, float weaponBonus) =>
            baseStats.AttackPower(baseValue, weaponBonus);

        public float CurrentMagicPower(float baseValue, float focusBonus) =>
            baseStats.MagicPower(baseValue, focusBonus);

        public int CharmSlots => realmAlignment.Alignment switch
        {
            RealmAlignment.Light => baseStats.CharmSlots() + 1,
            RealmAlignment.Shadow => baseStats.CharmSlots(),
            _ => baseStats.CharmSlots()
        };

        public void ApplyStatModifier(StatBlock modifier)
        {
            baseStats += modifier;
        }

        public void PushAlignment(RealmAlignment target, float delta) =>
            realmAlignment.PushToward(target, delta);
    }
}
