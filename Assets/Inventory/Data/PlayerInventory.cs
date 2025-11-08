using System.Collections.Generic;
using UnityEngine;

namespace EclipseRealms.Inventory.Data
{
    [CreateAssetMenu(menuName = "Eclipse Realms/Inventory/Player Inventory Template")]
    public class PlayerInventory : ScriptableObject
    {
        [SerializeField] private List<InventorySlot> slots = new();

        public IReadOnlyList<InventorySlot> Slots => slots;

        public InventorySlot GetOrCreateSlot(int index)
        {
            while (slots.Count <= index)
            {
                slots.Add(new InventorySlot());
            }

            return slots[index];
        }
    }
}
