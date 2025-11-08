using EclipseRealms.Core.Combat;
using EclipseRealms.Items;
using UnityEngine;

namespace EclipseRealms.Inventory.Runtime
{
    /// <summary>
    /// Subscribes to a Health component and spawns loot when it dies.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class LootDropper : MonoBehaviour
    {
        [SerializeField] private LootPickup lootPickupPrefab;
        [SerializeField] private ItemDefinition[] dropTable;

        private void Awake()
        {
            GetComponent<Health>().Died += HandleDeath;
        }

        private void OnDestroy()
        {
            if (TryGetComponent(out Health health))
            {
                health.Died -= HandleDeath;
            }
        }

        private void HandleDeath(Health _)
        {
            if (lootPickupPrefab == null || dropTable == null || dropTable.Length == 0)
            {
                return;
            }

            ItemDefinition drop = dropTable[Random.Range(0, dropTable.Length)];
            if (drop == null)
            {
                return;
            }

            LootPickup pickup = Instantiate(lootPickupPrefab, transform.position, Quaternion.identity);
            pickup.Initialize(drop);
        }
    }
}
