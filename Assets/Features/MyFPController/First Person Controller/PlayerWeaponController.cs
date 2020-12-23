using Alando.Features.Interfaces;
using Alando.Features.Weapon;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Assets.Features.BattleField;
using TMPro;
using UnityEngine;

namespace Alando.Features.MyFPController
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Title("Weapons Control")]
        public List<WeaponData> weapons = new List<WeaponData>();
        private WeaponData curWeapon;
        public CameraController cameraController;
        [SerializeField] private TMP_Text AmmoText;
        [SerializeField] private LayerMask hitLayer;
        [SerializeField] private BulletControl bullet;
        [SerializeField] private Transform shootTransform;

        [Title("Weapon Recoil")]
        [SerializeField] private float maxRecoilDistance = 2f;
        [SerializeField] private float recoilSharpness = 50f;
        [SerializeField] private float recoilRestitutionSharpness = 10f;
        [SerializeField] private Transform weaponHandle;

        [Title("Sway")]
        [SerializeField] private float intensity = 1f;
        [SerializeField] private float smooth = 10f;

        private Quaternion originSwayRotation;
        private PlayerInfoData infoData;




        private float lastTime = Mathf.NegativeInfinity;
        public int curAmmo { get; private set; }
        private int restAmmo;
        private Vector3 weaponAccumulatedRecoil;
        private Vector3 weaponOrigin;
        private int fillingNum;
        private bool isFilling = false;
        private bool isReload = false;
        private bool isFire = false;


        private void Start()
        {
            curWeapon = weapons[0];
            curAmmo = curWeapon.weapon.magazineCapacity;
            restAmmo = curWeapon.weapon.maxAmmoCapacity;
            weaponOrigin = weaponHandle.localPosition;
            originSwayRotation = weaponHandle.localRotation;
            infoData = GetComponent<PlayerInfoData>();
            //weaponAccumulatedRecoil = weaponHandle.localPosition;
        }

        private void Update()
        {
            if (CanShoot() && Input.GetMouseButtonDown(0))
            {
                WeaponShoot();
            }
            WeaponFilling();
            WeaponReload();
            UpdateWeaponRecoil();
            //AmmoText.text = curWeapon.GetAmmo();

            UpdateSway();
        }







        /// <summary>
        /// 武器是否可以发射,当武器不在装填/不在换弹/子弹大于零的时候可以发射
        /// </summary>
        /// <returns></returns>
        private bool CanShoot()
        {
            return !(isFilling && isReload && !(curAmmo > 0));
        }

        /// <summary>
        /// 武器开火
        /// </summary>
        private void WeaponShoot()
        {
            switch (curWeapon.weapon.shootFireType)
            {
                case FireType.Single:
                    TryShoot(1);
                    break;
                case FireType.SemiAutomatic:
                    TryShoot(3);
                    break;
                case FireType.Auto:
                    TryShoot(curWeapon.weapon.magazineCapacity);
                    break;
            }

        }

        /// <summary>
        /// 武器后坐力
        /// </summary>
        private void UpdateWeaponRecoil()
        {
            if (weaponHandle.localPosition.z >= weaponAccumulatedRecoil.z * 0.99f) // 后坐力
            {
                weaponHandle.localPosition = Vector3.Lerp(weaponHandle.localPosition, weaponAccumulatedRecoil, recoilSharpness * Time.deltaTime);
            }
            else // 后坐回正力
            {
                weaponHandle.localPosition = Vector3.Lerp(weaponHandle.localPosition, weaponOrigin, recoilRestitutionSharpness * Time.deltaTime);
                weaponAccumulatedRecoil = weaponHandle.localPosition;
            }
        }

        /// <summary>
        /// 武器装填
        /// </summary>
        /// <param name="filling">是否可以装填</param>
        private void WeaponFilling()
        {
            if (isReload) isFilling = false;
            if (isFilling && curAmmo < curWeapon.weapon.magazineCapacity)
            {
                float fillingTime = curWeapon.weapon.fillingTime > 0 ? curWeapon.weapon.fillingTime : 0.001f;
                float radius = (Time.time - lastTime) / fillingTime;
                if ((1.0f - radius) < 0.01f)
                {
                    // 装填动画
                    isFilling = false;
                    fillingNum = 0;
                    lastTime = Time.time;
                }

            }
        }

        /// <summary>
        /// 武器换弹
        /// </summary>
        /// <param name="reload">是否可以换弹</param>
        private void WeaponReload()
        {
            if ((isReload || curAmmo == 0) && restAmmo != 0)
            {
                float reloadTime = curWeapon.weapon.reloadTime > 0 ? curWeapon.weapon.reloadTime : 0.001f;
                float radius = (Time.time - lastTime) / reloadTime;
                Debug.Log("Reload");
                if ((1.0f - radius) < 0.01f)
                {
                    // 换弹动画
                    fillingNum = 0;
                    ResetAmmo();
                    lastTime = Time.time;
                    isReload = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>返回当前子弹/剩余子弹</returns>
        public string GetAmmo()
        {
            return $"{curAmmo}/{restAmmo}";
        }



        private void TryShoot(int num)
        {
            float rps = 1 / (curWeapon.weapon.rpm / 60.0f); // 从RPM换算成一发子弹发射的时间
            if (rps < (Time.time - lastTime) && curAmmo > 0)
            {
                if (fillingNum < num)
                {
                    HandleShoot();
                    curAmmo--;
                    fillingNum++;
                    weaponAccumulatedRecoil += Vector3.back * curWeapon.weapon.zRecoil;
                    weaponAccumulatedRecoil = Vector3.ClampMagnitude(weaponAccumulatedRecoil, maxRecoilDistance);
                }
                if (fillingNum == num)
                {
                    isFilling = true;
                }
            }
        }



        private void WeaponAim()
        {

        }

        private void KickBack(float vRecoil, Vector2 hRecoil, float deployTime, int num)
        {
            float vKick = vRecoil + (num - 1) / 100.0f;
            float hKick;
            int randomNum = UnityEngine.Random.Range(1, num + 2);
            if (randomNum % 2 != 0)
            {
                hKick = -hRecoil.x;
            }
            else
            {
                hKick = hRecoil.y;
            }
            //Debug.Log($"vKick:{vKick}, hKick:{hKick}");
            cameraController.AddWeaponRecoil(new Vector2(hKick, vKick));
        }


        private void WeaponHit()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 200f, hitLayer))
            {
                IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
                Debug.Log($"{hitInfo.collider.transform.parent.name}:{hitInfo.collider.name}");
                //damageable?.DealDamage(weapon.fireDamage);
                damageable?.DealDamage(20);
            }
        }

        private void HandleShoot()
        {
            KickBack(curWeapon.weapon.vRecoil, curWeapon.weapon.hRecoil, curWeapon.weapon.deployTime, fillingNum);
            //WeaponHit();
            lastTime = Time.time;
            Vector3 pos = shootTransform.position;
            BulletControl newBullet = Instantiate(bullet,pos,shootTransform.rotation);
            bullet.bulletSide = infoData.BattleSide;
        }

        private void ResetAmmo()
        {
            if (restAmmo > curWeapon.weapon.magazineCapacity)
            {
                curAmmo = curWeapon.weapon.magazineCapacity;
                restAmmo -= curAmmo;
            }
            else
            {
                curAmmo = restAmmo;
                restAmmo = 0;
            }
        }


        private void UpdateSway()
        {
            Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * mouseInput.x, Vector3.up);
            Quaternion t_y_adj = Quaternion.AngleAxis(intensity * mouseInput.y, Vector3.right);
            Quaternion target_rotation = originSwayRotation * t_x_adj * t_y_adj;

            weaponHandle.localRotation = Quaternion.Lerp(weaponHandle.localRotation, target_rotation, smooth * Time.deltaTime);
        }


    }
}