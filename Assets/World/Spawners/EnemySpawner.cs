using EclipseRealms.Core.Combat;
using UnityEngine;

namespace EclipseRealms.World.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private SimpleEnemyBrain enemyPrefab;
        [SerializeField] private int initialCount = 3;
        [SerializeField] private float spawnRadius = 8f;

        private void Start()
        {
            for (int i = 0; i < initialCount; i++)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("EnemySpawner has no prefab assigned.", this);
                return;
            }

            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 position = new(transform.position.x + offset.x, transform.position.y, transform.position.z + offset.y);
            Instantiate(enemyPrefab, position, Quaternion.identity, transform);
        }
    }
}
