using Alando.Features.Player;
using Assets.Features.GameManagers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Features.BattleField
{
    public class PlayerInfoData : MonoBehaviour
    {
        [SerializeField] [EnumToggleButtons] private BattleSide battleSide;
        public BattleSide BattleSide => battleSide;
        public PlayerHealth health;


        private void Start()
        {
            BattlefieldManager.instance.AddPlayer(this);
        }

        public void SetBattleSide(BattleSide bs)
        {
            battleSide = bs;
        }
    }
}