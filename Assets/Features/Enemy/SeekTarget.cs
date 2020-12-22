using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Features.Enemy
{
    public class SeekTarget : MonoBehaviour
    {
        public NavMeshAgent agent;
        //public Transform testTarget;

        private EnemyAnimation enemyAnimation;

        [Title("DEBUG")]
        [ReadOnly] public string target;
        [ReadOnly] public Vector3 TargetVector3;
        [ReadOnly] public float Distance;

        private void Start()
        {
            enemyAnimation = GetComponent<EnemyAnimation>();
        }

        private void Update()
        {
            var aTarget = agent.destination;
            if (aTarget != null)
            {
                agent.isStopped = false;

                /*
                var position = target.position;
                float disatance = Vector3.Distance(position, transform.position);
                
                if (disatance < 10f)
                {
                    agent.speed = 2f;
                    enemyAnimation.moveState = EnemyMoveState.Walk;
                }
                else if (disatance > 10f)
                {
                    agent.speed = 6f;
                    enemyAnimation.moveState = EnemyMoveState.Run;
                }
                */

                enemyAnimation.moveState = EnemyMoveState.Run;

                //agent.SetDestination(testTarget.position);

                float dist = Vector3.Distance(transform.position, TargetVector3);
                Distance = dist;
                if (dist < 0.2f)
                {
                    enemyAnimation.moveState = EnemyMoveState.Idle;
                    agent.isStopped = true;
                }
            }


        }


        public void SetAgentTarget(Transform target)
        {
            this.target = target.name;
            Vector3 tPos = target.position;
            Vector3 pos = new Vector3(tPos.x, tPos.y, tPos.z);
            pos += new Vector3(Random.Range(0f, 10f), 1f, Random.Range(0f, 10f));
            pos.y = 0.2f;
            TargetVector3 = pos;
            agent.SetDestination(pos);
        }
    }
}