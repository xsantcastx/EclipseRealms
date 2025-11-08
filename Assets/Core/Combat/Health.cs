using System;
using UnityEngine;

namespace EclipseRealms.Core.Combat
{
    /// <summary>
    /// Lightweight health container shared by players, enemies, and interactables.
    /// </summary>
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private bool destroyOnDeath = false;

        private int currentHealth;
        private bool initialized;

        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;
        public bool IsDead => CurrentHealth <= 0;

        public event Action<Health> Died;
        public event Action<int, Health> Damaged;
        public event Action<int, Health> Healed;

        private void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            currentHealth = Mathf.Max(1, maxHealth);
            initialized = true;
        }

        public void ApplyDamage(int amount)
        {
            if (!initialized)
            {
                ResetHealth();
            }

            if (amount <= 0 || IsDead)
            {
                return;
            }

            currentHealth = Mathf.Max(0, currentHealth - amount);
            Damaged?.Invoke(amount, this);

            if (currentHealth == 0)
            {
                HandleDeath();
            }
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead)
            {
                return;
            }

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            Healed?.Invoke(amount, this);
        }

        private void HandleDeath()
        {
            Died?.Invoke(this);
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
    }
}
