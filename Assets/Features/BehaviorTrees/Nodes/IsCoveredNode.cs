using UnityEngine;

public class IsCoveredNode : Node
{
    private Transform target;
    private Transform origin;

    public IsCoveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, target.position - origin.position, out hit))
        {
            if (hit.collider.transform != target)
            {
                //Debug.Log($"IsCoveredNode: {hit.collider.transform.name} | Is Covered");
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
