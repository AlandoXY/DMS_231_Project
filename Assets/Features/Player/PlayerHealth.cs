using Alando.Features.GamePreset;
using Alando.Features.Interfaces;
using UnityEngine;

namespace Alando.Features.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerStats playerStats;

        public int CurHealth => curHealth;

        [SerializeField]private int curHealth;

        private void Start()
        {
            curHealth = playerStats.maxHealth;
        }


        public void DealDamage(int damage)
        {
            curHealth -= damage;
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