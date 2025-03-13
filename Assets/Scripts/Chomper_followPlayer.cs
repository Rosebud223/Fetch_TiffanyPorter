using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 4f;
    public float runThreshold = 10f;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private bool isFollowing = true;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFollowing && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            bool isMoving = distance > stopDistance;
            bool isRunning = distance >= runThreshold;

            if (isMoving)
            {
                m_Agent.SetDestination(player.position);
                m_Animator.SetBool("Walking", isMoving);
            }
            else
            {
                m_Agent.ResetPath();
            }

            m_Animator.SetBool("Walking", isMoving);
            m_Animator.SetBool("Running", isRunning);
        }
    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
        m_Agent.ResetPath();
        m_Animator.SetBool("Walking", false);
        m_Animator.SetBool("Running", false);
    }
}
