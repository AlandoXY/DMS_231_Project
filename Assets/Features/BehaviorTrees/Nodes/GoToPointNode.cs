using Assets.Features.BattleField;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Features.BehaviorTrees.Nodes
{
    public class GoToPointNode : Node
    {
        private NavMeshAgent agent;
        private EnemyAI ai;

        public GoToPointNode(NavMeshAgent agent, EnemyAI ai)
        {
            this.agent = agent;
            this.ai = ai;
        }

        public override NodeState Evaluate()
        {
            BattlePoint point = ai.GetCurBattlePoint();
            if (point == null)
                return NodeState.FAILURE;
            Debug.Log($"GoToPointNode: {point.name}");
            Vector3 targetPointPosition = point.transform.position + new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), 0.5f);
            float distance = Vector3.Distance(targetPointPosition, agent.transform.position);
            if (distance > 0.2f)
            {
                agent.isStopped = false;
                agent.SetDestination(targetPointPosition);
                return NodeState.RUNNING;
            }
            else
            {
                agent.isStopped = true;
                return NodeState.SUCCESS;
            }
        }
    }
}