using UnityEngine;

namespace Assets.Features.BehaviorTrees.Nodes
{
    public class IsOccupyPoint : Node
    {
        private Transform target;
        private Transform origin;

        public IsOccupyPoint(Transform target, Transform origin)
        {
            this.target = target;
            this.origin = origin;
        }

        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(target.position, origin.position);
            if (distance < 0.2f)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
}