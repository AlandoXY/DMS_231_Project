using Assets.Features.Data.Scripts;
using Assets.Features.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Features.GameManagers
{
    public class ChooseWeaponUI : MonoBehaviour
    {
        public static ChooseWeaponUI instance;

        private void Awake() => instance = this;

        public List<Button> armsButtons = new List<Button>();// Commando, Medical, Sniper, Support
        public List<WeaponUI> weaponButtons = new List<WeaponUI>();// Main, Secondary, Melee, Grenade
        public List<Button> battleSideToggle = new List<Button>();// Blue, Red
        public GameObject chooseWeaponPanel;
        public GameObject weaponListPanel;
        public GameObject battleSidePanel;
        public Transform weaponListTransform;


        private void OnEnable()
        {
            UpdatePlayerWeapon();
        }

        private void Start()
        {

        }

        private void UpdatePlayerWeapon()
        {
            int armNum = ChooseWeaponManager.instance.playerStatus.arms.GetHashCode();
            armsButtons[armNum].Select();

            WeaponData mainWeapon = ChooseWeaponManager.instance.playerStatus.mainWeapon;
            weaponButtons[0].weaponName.text = mainWeapon.weaponName;
            weaponButtons[0].weaponImage.sprite = mainWeapon.weaponIcon;
            weaponButtons[0].equipAmmo.text = mainWeapon.magazineCapacity.ToString();
            weaponButtons[0].carryAmmo.text = mainWeapon.maxAmmoCapacity.ToString();

            WeaponData secondaryWeapon = ChooseWeaponManager.instance.playerStatus.secondaryWeapon;
            weaponButtons[1].weaponName.text = secondaryWeapon.weaponName;
            weaponButtons[1].weaponImage.sprite = secondaryWeapon.weaponIcon;
            weaponButtons[1].equipAmmo.text = secondaryWeapon.magazineCapacity.ToString();
            weaponButtons[1].carryAmmo.text = secondaryWeapon.maxAmmoCapacity.ToString();

            MeleeData melee = ChooseWeaponManager.instance.playerStatus.melee;
            weaponButtons[2].weaponName.text = melee.meleeName;
            weaponButtons[2].weaponImage.sprite = melee.meleeImage;
            weaponButtons[2].equipAmmo.text = "";
            weaponButtons[2].carryAmmo.text = "";

            GrenadeData grenade = ChooseWeaponManager.instance.playerStatus.grenade;
            weaponButtons[3].weaponName.text = grenade.grenadeName;
            weaponButtons[3].weaponImage.sprite = grenade.grenadeImage;
            weaponButtons[3].equipAmmo.text = grenade.equipCapacity.ToString();
            weaponButtons[3].carryAmmo.text = grenade.maxCarryCapacity.ToString();

            int battleSideNum = ChooseWeaponManager.instance.playerStatus.battleSide.GetHashCode();
            battleSideToggle[battleSideNum].Select();
        }




    }
}
