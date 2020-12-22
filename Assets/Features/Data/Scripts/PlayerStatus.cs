using Assets.Features.BattleField;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Features.Data.Scripts
{
    public enum ArmsStatus
    {
        Commando, Medical, Sniper, Support
    }

    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "Alando/Data/PlayerStatus", order = 0)]
    public class PlayerStatus : ScriptableObject
    {

        [EnumToggleButtons] public BattleSide battleSide;
        [EnumToggleButtons] public ArmsStatus arms = ArmsStatus.Commando;
        public WeaponData mainWeapon;
        public WeaponData secondaryWeapon;
        public MeleeData melee;
        public GrenadeData grenade;
    }
}