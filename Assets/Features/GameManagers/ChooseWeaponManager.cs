using Assets.Features.Data.Scripts;
using UnityEngine;

namespace Assets.Features.GameManagers
{
    public class ChooseWeaponManager : MonoBehaviour
    {
        public static ChooseWeaponManager instance;
        private void Awake() => instance = this;

        public AllWeaponData weaponData;
        public PlayerStatus playerStatus;




    }
}