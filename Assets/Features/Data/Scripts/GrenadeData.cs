using System;
using UnityEngine;

namespace Assets.Features.Data.Scripts
{
    [Serializable]
    public class GrenadeData
    {
        public string grenadeName;
        public Sprite grenadeImage;
        public int equipCapacity;
        public int maxCarryCapacity;

        [Header("Damage Data")]
        public int maxDamge;
        public float dmageRange;
    }
}