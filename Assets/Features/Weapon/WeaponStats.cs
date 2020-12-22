using UnityEngine;

namespace Alando.Features.Weapon
{
    /// <summary>
    /// 武器射击模式: Single:单发, SemiAuto:半自动, Auto:全自动
    /// 单发：鼠标点击一下->发射一颗子弹->手动装填 【循环】
    /// 半自动：鼠标点击一下->发射一颗子弹 【循环】
    /// 自动：鼠标按下->持续发射子弹
    /// </summary>
    public enum FireType
    {
        Single, SemiAutomatic, Auto
    }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "Alando/Weapon/New Weapon", order = 0)]
    public class WeaponStats : ScriptableObject
    {
        public string gunName;
        public GameObject weaponPrefab;

        [Header("Weapon Stats")]
        public int fireDamage;// 武器伤害
        public int magazineCapacity; // 弹夹容量
        public int maxAmmoCapacity; // 最大携带容量
        public float reloadTime;// 换弹时间
        public float fillingTime;// 装填时间
        public float rpm;// 每秒射速
        public FireType shootFireType;
        public float fireRange;//武器射程

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