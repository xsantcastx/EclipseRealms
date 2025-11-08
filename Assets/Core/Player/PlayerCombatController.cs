using EclipseRealms.Core.Combat;
using UnityEngine;

namespace EclipseRealms.Core.Player
{
    /// <summary>
    /// Handles simple melee attacks driven by the default input axes.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private float attackCooldown = 0.35f;
        [SerializeField] private float attackRange = 2.25f;
        [SerializeField] private int attackDamage = 15;
        [SerializeField] private LayerMask targetMask = ~0;

        private float lastAttackTime;

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (Time.time < lastAttackTime + attackCooldown)
            {
                return;
            }

            lastAttackTime = Time.time;

            Vector3 origin = transform.position + Vector3.up;
            Vector3 forward = transform.forward;
            if (Physics.SphereCast(origin, 0.5f, forward, out RaycastHit hitInfo, attackRange, targetMask, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.collider.TryGetComponent(out Health health))
                {
                    health.ApplyDamage(attackDamage);
                }
            }
        }
    }
}
