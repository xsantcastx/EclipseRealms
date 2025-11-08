using EclipseRealms.Core.Player;
using UnityEngine;

namespace EclipseRealms.Core.Combat
{
    /// <summary>
    /// Lightweight AI used for the M0 prototype target.
    /// Chases the first PlayerStatsComponent found in scene and performs contact damage.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Health))]
    public class SimpleEnemyBrain : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private int damagePerSecond = 5;

        private CharacterController controller;
        private PlayerStatsComponent target;
        private Health targetHealth;
        private Health selfHealth;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            selfHealth = GetComponent<Health>();
        }

        private void Start()
        {
            target = FindObjectOfType<PlayerStatsComponent>();
            if (target != null)
            {
                targetHealth = target.GetComponent<Health>();
            }
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            Vector3 toPlayer = (target.transform.position - transform.position);
            Vector3 planar = new(toPlayer.x, 0f, toPlayer.z);

            if (planar.sqrMagnitude > attackRange * attackRange)
            {
                Vector3 step = planar.normalized * moveSpeed * Time.deltaTime;
                controller.Move(step);
                transform.rotation = Quaternion.LookRotation(planar);
            }
            else
            {
                if (targetHealth != null)
                {
                    int damage = Mathf.CeilToInt(damagePerSecond * Time.deltaTime);
                    targetHealth.ApplyDamage(damage);
                }
            }
        }
    }
}
