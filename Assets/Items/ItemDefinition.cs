using EclipseRealms.Core.Stats;
using UnityEngine;

namespace EclipseRealms.Items
{
    public enum ItemSlot
    {
        Weapon,
        Offhand,
        Helmet,
        Chest,
        Gloves,
        Boots,
        Charm,
        Relic
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    [CreateAssetMenu(menuName = "Eclipse Realms/Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string itemId;
        [SerializeField] private ItemSlot slot;
        [SerializeField] private ItemRarity rarity;
        [SerializeField] private StatBlock statBonuses;
        [SerializeField, Range(0, 6)] private int sockets;

        public string ItemId => itemId;
        public ItemSlot Slot => slot;
        public ItemRarity Rarity => rarity;
        public StatBlock StatBonuses => statBonuses;
        public int Sockets => sockets;
    }
}
