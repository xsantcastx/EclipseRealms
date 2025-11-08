using UnityEngine;

namespace EclipseRealms.Items.Runes
{
    public enum RuneAlignment
    {
        Light,
        Shadow,
        Neutral
    }

    [CreateAssetMenu(menuName = "Eclipse Realms/Items/Rune")]
    public class RuneDefinition : ScriptableObject
    {
        [SerializeField] private string runeId;
        [SerializeField] private RuneAlignment alignment;
        [SerializeField, Range(1, 10)] private int tier;
        [SerializeField, TextArea] private string effectSummary;

        public string RuneId => runeId;
        public RuneAlignment Alignment => alignment;
        public int Tier => tier;
        public string EffectSummary => effectSummary;
    }
}
