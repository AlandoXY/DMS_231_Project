using UnityEngine;

public class MenuBotAnimatorSet : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool aim;
    [SerializeField] private bool run;
    [SerializeField] private bool fire;
    [SerializeField] private bool crouch;


    private void Update()
    {
        animator.SetBool("aim", aim);
        animator.SetBool("run", run);
        animator.SetBool("fire", fire);
        animator.SetBool("crouch", crouch);
    }
}
