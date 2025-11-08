using EclipseRealms.Items;
using UnityEngine;

namespace EclipseRealms.Inventory.Runtime
{
    [RequireComponent(typeof(Collider))]
    public class LootPickup : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 45f;
        [SerializeField] private ItemDefinition item;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }

        public void Initialize(ItemDefinition definition)
        {
            item = definition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (item == null)
            {
                return;
            }

            if (other.TryGetComponent(out InventoryController controller))
            {
                if (controller.TryAddItem(item))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
