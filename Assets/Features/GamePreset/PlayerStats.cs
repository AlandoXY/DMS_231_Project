using UnityEngine;

namespace Alando.Features.GamePreset
{
    [CreateAssetMenu(fileName = "Player Stat", menuName = "Alando/Player/Stats", order = 0)]
    public class PlayerStats : ScriptableObject
    {
        public string playerName;
        public int maxHealth;
    }
}