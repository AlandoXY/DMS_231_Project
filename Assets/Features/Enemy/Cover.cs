using Assets.Features.GameManagers;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] private Transform[] coverSpots;

    private void Start()
    {
        foreach (Transform spot in coverSpots)
        {
            BattlefieldManager.instance.SignCoverSpot(spot);
        }
    }

    public Transform[] GetCoverSpots()
    {
        return coverSpots;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Transform spot in coverSpots)
        {
            Gizmos.DrawSphere(spot.position, 0.5f);
        }
    }
}
