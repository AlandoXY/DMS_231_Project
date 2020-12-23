using System;
using System.Collections;
using Alando.Features.Interfaces;
using Alando.Features.Player;
using Assets.Features.BattleField;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Alando.Features.Weapon
{
    public class BulletControl : MonoBehaviour
    {
        public float speed = 20f;
        private float lifeTime = 10f;
        private float spawnTime;
        [SerializeField,ReadOnly] private float DebugLiveTime;
        public BattleSide bulletSide;

        private void Start()
        {
            spawnTime = Time.time;
        }

        private void Update()
        {
            transform.localPosition += transform.forward * (speed * Time.deltaTime);
            DebugLiveTime = Time.time - spawnTime;
            if (Time.time - spawnTime > lifeTime)
            {
                Destroy(gameObject);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AI"))
            {
                IDamageable damage = other.GetComponent<IDamageable>();
                PlayerInfoData infoData = other.GetComponent<PlayerInfoData>();
                if (infoData.BattleSide != bulletSide)
                {
                    damage.DealDamage(20);
                    Debug.Log($"Hit {other.transform.name}");
                }
            }
            Destroy(gameObject);
        }
    }
}