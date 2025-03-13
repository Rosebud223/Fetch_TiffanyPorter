using UnityEngine;
using UnityEngine.AI;

public class ChomperPlayFetch : MonoBehaviour
{
    public Transform player;
    public float pickupDistance = 1.2f;
    public float dropDistance = 2.0f;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private GameObject thrownBall;
    private bool hasBall = false;
    private bool isFetching = false;
    private FollowPlayer followScript;
    

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        followScript = GetComponent<FollowPlayer>();
    }

    void Update()
    {
        if (isFetching)
        {
            m_Animator.SetBool("Walking", true);
            m_Animator.SetBool("Running", true);
            
            if (!hasBall) // Chomper needs to pick up the ball
            {
                m_Agent.SetDestination(thrownBall.transform.position);

                float distanceToBall = Vector3.Distance(transform.position, thrownBall.transform.position);
                if (distanceToBall <= pickupDistance) // Chomper is close enough to pick up the ball
                {
                    PickUpBall();
                }
            }
            else // Chomper has the ball, heading back to the player
            {
                m_Agent.SetDestination(player.position);

                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= dropDistance) // Chomper is close enough to drop the ball
                {
                    DropBall();
                }
            }
        }
        else // Chomper is not fetching
        {
            // Restart follow script
            if (followScript != null)
            {
                followScript.StartFollowing();
            }
            m_Animator.SetBool("Walking", false);
            m_Animator.SetBool("Running", false);
        }
    }

       
        



    public void StartFetch(GameObject ball)
    {
        thrownBall = ball;
        isFetching = true;
        m_Agent.SetDestination(thrownBall.transform.position);
        
        if (followScript != null)
        {
            followScript.StopFollowing();
        }
    }

    public void StopFetch()
    {
        isFetching = false;
        thrownBall = null;
        m_Agent.ResetPath();
        
        if (followScript != null)
        {
            followScript.StartFollowing();
        }

        m_Animator.SetBool("Walking", false);
        m_Animator.SetBool("Running", false);
    }

    private void PickUpBall()
    {
        if (thrownBall != null)
        {
            Rigidbody ballRb = thrownBall.GetComponent<Rigidbody>();
            if (ballRb)
            {
                ballRb.isKinematic = true;
                thrownBall.transform.SetParent(transform);
                thrownBall.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
            }

            m_Agent.SetDestination(player.position);
            hasBall = true;
        }
    }

    public void DropBall()
    {
        if (thrownBall != null)
        {
            Rigidbody ballRb = thrownBall.GetComponent<Rigidbody>();
            if (ballRb)
            {
                ballRb.isKinematic = false;
            }

            thrownBall.transform.SetParent(null);
            thrownBall = null; 
            isFetching = false; 
            hasBall = false;

            if (followScript != null)
            {
                followScript.StartFollowing();
            }
        }
    }
}