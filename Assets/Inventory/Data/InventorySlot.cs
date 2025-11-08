using System;
using EclipseRealms.Items;
using EclipseRealms.Items.Charms;
using EclipseRealms.Items.Runes;
using EclipseRealms.Items.Runewords;
using UnityEngine;

namespace EclipseRealms.Inventory.Data
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private ItemDefinition item;
        [SerializeField] private RuneDefinition[] socketedRunes = Array.Empty<RuneDefinition>();
        [SerializeField] private RunewordDefinition appliedRuneword;
        [SerializeField] private CharmDefinition slottedCharm;

        public ItemDefinition Item => item;
        public RuneDefinition[] SocketedRunes => socketedRunes;
        public RunewordDefinition Runeword => appliedRuneword;
        public CharmDefinition Charm => slottedCharm;

        public bool IsEmpty => item == null && slottedCharm == null;

        public void AssignItem(ItemDefinition newItem, RuneDefinition[] runes = null)
        {
            item = newItem;
            socketedRunes = runes ?? Array.Empty<RuneDefinition>();
        }

        public void Clear()
        {
            item = null;
            socketedRunes = Array.Empty<RuneDefinition>();
            appliedRuneword = null;
            slottedCharm = null;
        }
    }
}
