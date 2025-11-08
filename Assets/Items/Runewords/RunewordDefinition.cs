using System.Collections.Generic;
using EclipseRealms.Items.Runes;
using UnityEngine;

namespace EclipseRealms.Items.Runewords
{
    [CreateAssetMenu(menuName = "Eclipse Realms/Items/Runeword")]
    public class RunewordDefinition : ScriptableObject
    {
        [SerializeField] private string runewordId;
        [SerializeField] private ItemSlot targetSlot;
        [SerializeField] private List<RuneDefinition> sequence = new();
        [SerializeField, TextArea] private string effectDescription;

        public string RunewordId => runewordId;
        public ItemSlot TargetSlot => targetSlot;
        public IReadOnlyList<RuneDefinition> Sequence => sequence;
        public string EffectDescription => effectDescription;
    }
}
