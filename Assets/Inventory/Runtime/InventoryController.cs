using System.Collections.Generic;
using EclipseRealms.Inventory.Data;
using EclipseRealms.Items;
using UnityEngine;

namespace EclipseRealms.Inventory.Runtime
{
    /// <summary>
    /// Runtime inventory instance derived from a ScriptableObject template.
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private PlayerInventory template;

        private readonly List<InventorySlot> runtimeSlots = new();

        private void Awake()
        {
            if (template != null)
            {
                foreach (InventorySlot slot in template.Slots)
                {
                    runtimeSlots.Add(new InventorySlot());
                }
            }
        }

        public bool TryAddItem(ItemDefinition item)
        {
            if (item == null)
            {
                return false;
            }

            foreach (InventorySlot slot in runtimeSlots)
            {
                if (slot.IsEmpty)
                {
                    slot.AssignItem(item);
                    return true;
                }
            }

            return false;
        }
    }
}
