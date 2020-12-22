
using System;
using UnityEngine;

namespace Assets.Features.Data.Scripts
{
    public enum FireType
    {
        Single, SemiAutomatic, Auto
    }
    [Serializable]
    public class WeaponData
    {
        public string weaponName;
        public Sprite weaponIcon;
        public GameObject weaponPrefab;

        [Header("Weapon Stats")]
        public int fireDamage;// 武器伤害
        public int magazineCapacity; // 弹夹容量
        public int maxAmmoCapacity; // 最大携带容量
        public float reloadTime;// 换弹时间
        public float fillingTime;// 装填时间
        public float rpm;// 每秒射速
        public FireType shootFireType;

        [Header("Weapon Recoil")]
        public float deployTime;// 后座回复时间
        public float vRecoil;// 垂直后坐
        [Tooltip("x: 左边偏移，y:右边偏移")]
        public Vector2 hRecoil;// 水平偏移

        public float zRecoil;//向后后坐力


        [Header("Bullet Scattering")]
        [Tooltip("x: 最小散射，y:最大散射")]
        public Vector2 ads; // 瞄准时，子弹散射范围
        public float hip;// 腰射时，子弹散射


        public float bulletSpeed;// 子弹速度
        public float bulletGAcceleration;// 子弹下坠速度
        public float bulletLifetime;// 子弹生命时间，单位：秒

        // 武器范围：子弹生命时间 * 子弹速度 
        // 子弹速度 *
    }
}
