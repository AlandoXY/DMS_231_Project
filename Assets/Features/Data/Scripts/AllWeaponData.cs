using Assets.Features.BattleField;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Data.Scripts
{
    [CreateAssetMenu(fileName = "AllWeaponData", menuName = "Alando/Data/AllWeaponData", order = 0)]
    public class AllWeaponData : ScriptableObject
    {
        [EnumToggleButtons] public BattleSide battleside;
        public Arms commando = new Arms();
        public Arms medical = new Arms();
        public Arms sniper = new Arms();
        public Arms support = new Arms();
    }

    [Serializable]
    public class Arms
    {
        public Sprite armsIcon;
        public List<WeaponData> mainWeapons = new List<WeaponData>();
        public List<WeaponData> secondaryWeapons = new List<WeaponData>();
        public List<MeleeData> melees = new List<MeleeData>();
        public List<GrenadeData> grenades = new List<GrenadeData>();
    }

}