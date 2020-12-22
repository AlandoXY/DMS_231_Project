using Alando.Features.GamePreset;
using Alando.Features.Interfaces;
using UnityEngine;

namespace Alando.Features.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerStats playerStats;

        public int CurHealth { get; private set; }

        public int DebugHealth;

        private void Start()
        {
            CurHealth = playerStats.maxHealth;
        }


        public void DealDamage(int damage)
        {
            CurHealth -= damage;
            DebugHealth = CurHealth;
            CheckIfDead();
        }

        private void CheckIfDead()
        {
            if (CurHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}