using UnityEngine;
using UnityEngine.AI;

namespace Assets.Features.Enemy
{
    public enum EnemyMoveState
    {
        Idle, Walk, Run
    }

    public enum EnemyAimState
    {
        Aim, NotAim
    }
    public class EnemyAnimation : MonoBehaviour
    {
        [HideInInspector] public EnemyMoveState moveState;
        [HideInInspector] public EnemyAimState aimState;
        public Animator animator;
        private NavMeshAgent agent;
        private EnemyMoveState preMoveState;
        private EnemyAimState preAimState;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }


        private void FixedUpdate()
        {
            SetMove();
            SetAim();
        }

        private void ResetAnimator()
        {
            animator.SetBool("jump", false);
            animator.SetBool("crouch", false);
            animator.SetBool("aim", false);
            animator.SetBool("run", false);
            animator.SetBool("fire", false);
            animator.SetFloat("posY", 0);
            animator.SetFloat("posX", 0);

        }

        private void SetMove()
        {
            if (preMoveState == moveState) return;
            switch (moveState)
            {
                case EnemyMoveState.Idle:
                    ResetAnimator();
                    break;
                case EnemyMoveState.Walk:
                    ResetAnimator();
                    animator.SetFloat("posY", 1f);
                    break;
                case EnemyMoveState.Run:
                    ResetAnimator();
                    animator.SetBool("run", true);
                    animator.SetFloat("posY", 1f);
                    break;
            }
            preMoveState = moveState;
        }

        private void SetAim()
        {
            if (preAimState == aimState) return;
            animator.SetBool("aim", (aimState == EnemyAimState.Aim));
        }
    }
}