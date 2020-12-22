using Assets.Features.BattleField;
using Assets.Features.GameManagers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private EnemyAI ai;

    public ChaseNode(Transform target, NavMeshAgent agent, EnemyAI ai)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.yellow);
        if (target == null)
        {
            List<PlayerInfoData> playerNearMe = BattlefieldManager.instance.GetPlayerNearMe(ai.playerInfo, ai.WeaponController.weapon.fireRange);
            target = playerNearMe[Random.Range(0, playerNearMe.Count - 1)].transform;
        }
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }


}
