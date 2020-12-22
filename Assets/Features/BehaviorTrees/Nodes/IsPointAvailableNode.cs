using Assets.Features.BattleField;
using Assets.Features.GameManagers;

namespace Assets.Features.BehaviorTrees.Nodes
{
    public class IsPointAvailableNode : Node
    {
        private BattlePoint targetPoint;
        private EnemyAI ai;

        public IsPointAvailableNode(BattlePoint _target, EnemyAI _ai)
        {
            targetPoint = _target;
            ai = _ai;
        }
        public override NodeState Evaluate()
        {
            BattlePoint bp = FindBattlePoint();
            ai.SetBestBattlePoint(bp);
            return bp != null ? NodeState.SUCCESS : NodeState.FAILURE;
        }


        private BattlePoint FindBattlePoint()
        {
            if (ai.GetCurBattlePoint() != null)
            {
                return ai.GetCurBattlePoint();
            }
            BattlePoint bp = BattlefieldManager.instance.GetBattlePoint(ai.transform, ai.playerInfo);
            while (bp == ai.GetCurBattlePoint())
            {
                bp = BattlefieldManager.instance.GetBattlePoint(ai.transform, ai.playerInfo);
            }
            return bp;
        }


    }
}