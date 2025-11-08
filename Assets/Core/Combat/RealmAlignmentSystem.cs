using System;
using UnityEngine;

namespace EclipseRealms.Core.Combat
{
    public enum RealmAlignment
    {
        Light,
        Neutral,
        Shadow
    }

    /// <summary>
    /// Tracks light / shadow affinity and exposes the blueprint buffs & debuffs.
    /// </summary>
    [Serializable]
    public class RealmAlignmentState
    {
        [Range(-100f, 100f)]
        [Tooltip("-100 = Shadow, 0 = Neutral, 100 = Light")]
        public float AlignmentScore;

        public RealmAlignment Alignment =>
            AlignmentScore switch
            {
                > 25f => RealmAlignment.Light,
                < -25f => RealmAlignment.Shadow,
                _ => RealmAlignment.Neutral
            };

        public float HealingBonus => Alignment == RealmAlignment.Light ? 0.10f : 0f;
        public float AetherRegenBonus => Alignment == RealmAlignment.Light ? 0.05f : 0f;
        public float ShadowResistancePenalty => Alignment == RealmAlignment.Light ? -0.10f : 0f;

        public float DamageBonus => Alignment == RealmAlignment.Shadow ? 0.08f : 0f;
        public float LifeStealBonus => Alignment == RealmAlignment.Shadow ? 0.05f : 0f;
        public float LightResistancePenalty => Alignment == RealmAlignment.Shadow ? -0.08f : 0f;

        public float DropLuckBonus => Alignment == RealmAlignment.Neutral ? 0.05f : 0f;
        public float XpPenalty => Alignment == RealmAlignment.Neutral ? -0.05f : 0f;

        public void PushToward(RealmAlignment target, float magnitude)
        {
            AlignmentScore = target switch
            {
                RealmAlignment.Light => Mathf.Clamp(AlignmentScore + magnitude, -100f, 100f),
                RealmAlignment.Shadow => Mathf.Clamp(AlignmentScore - magnitude, -100f, 100f),
                _ => Mathf.MoveTowards(AlignmentScore, 0f, magnitude)
            };
        }
    }
}
