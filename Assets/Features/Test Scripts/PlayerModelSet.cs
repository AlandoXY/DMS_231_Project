using UnityEngine;

public class PlayerModelSet : MonoBehaviour
{
    public Transform playerModel;
    // Start is called before the first frame update
    void Start()
    {
        playerModel.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerModel.hasChanged)
        {
            playerModel.localPosition = Vector3.zero;
        }
    }
}
