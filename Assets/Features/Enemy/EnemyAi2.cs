using Assets.Features.BattleField;
using Assets.Features.GameManagers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Enemy
{
    public class EnemyAi2 : MonoBehaviour
    {
        public PlayerInfoData aiInfo;
        [SerializeField] private List<Material> materials = new List<Material>();
        [SerializeField] private MeshRenderer render;

        private BattlePoint curBattlePoint;
        private PlayerInfoData curShootingTarget;
        private SeekTarget seekTarget;

        [Title("DEBUG")]
        [ReadOnly, SerializeField]
        private string curBattlePointName;

        private void Start()
        {
            aiInfo = GetComponent<PlayerInfoData>();
            seekTarget = GetComponent<SeekTarget>();
        }


        private void Update()
        {
            SetMaterial();
            CheckIfNull();
            if (curBattlePoint.CurBattlePointPercent == 1f)
            {
                curBattlePoint = BattlefieldManager.instance.GetBattlePoint(transform, aiInfo);
                SetTargetPoint();
            }

            /*
            if (Vector3.Distance(curShootingTarget.transform.position, transform.position) > 100f)
            {
                var targets = BattlefieldManager.instance.GetPlayerNearMe(aiInfo, 100f);
                curShootingTarget = targets[Random.Range(0, targets.Count - 1)];
            }
            */
            UpdateDebugInfo();
        }


        private void SetTargetPoint()
        {
            seekTarget.SetAgentTarget(curBattlePoint.transform);
        }


        private void UpdateDebugInfo()
        {
            curBattlePointName = curBattlePoint.name;
        }

        private void CheckIfNull()
        {
            if (curBattlePoint == null)
            {
                curBattlePoint = BattlefieldManager.instance.GetBattlePoint(transform, aiInfo);
                SetTargetPoint();
            }

            /*
            if (curShootingTarget == null)
            {
                var targets = BattlefieldManager.instance.GetPlayerNearMe(aiInfo, 100f);
                curShootingTarget = targets[Random.Range(0, targets.Count - 1)];
            }
            */
        }

        private void SetMaterial()
        {
            render.material = materials[aiInfo.BattleSide.GetHashCode()];
        }
    }
}