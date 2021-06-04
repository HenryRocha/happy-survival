using UnityEngine;
using UnityEngine.AI;

public class hr_ZombieController : MonoBehaviour
{
    [Header("Stats settings")]
    [SerializeField] private float health = 100;

    [Header("AI settings")]
    [SerializeField] private bool enableWander = true;
    [SerializeField] private LayerMask allMasks;
    [SerializeField] private float fov = 120.0f;
    [SerializeField] private float viewDistance = 10.0f;
    [SerializeField] private float wanderRadius = 7.0f;
    [SerializeField] private float loseThreshold = 10.0f;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isAware = false;
    private bool isDetecting = false;
    private float loseTimer = 0.0f;
    private Vector3 wanderPoint = Vector3.zero;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        player = GameObject.Find("Player");
        wanderPoint = RandomWanderPoint();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (isAware)
        {
            agent.SetDestination(player.transform.position);

            if (!isDetecting)
            {
                loseTimer += Time.deltaTime;
                if (loseTimer >= loseThreshold)
                {
                    isAware = false;
                    loseTimer = 0.0f;
                }
            }
        }
        else
        {
            loseTimer = 0.0f;
            if (enableWander)
            {
                Wander();
            }
        }

        SearchForPlayer();
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    private void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fov / 2.0f)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, player.transform.position, out hit, allMasks))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    else
                    {
                        isDetecting = false;
                    }
                }
                else
                {
                    isDetecting = false;
                }
            }
            else
            {
                isDetecting = false;
            }
        }
        else
        {
            isDetecting = false;
        }
    }

    public void OnAware()
    {
        isAware = true;
        isDetecting = true;
        hr_AudioManager.instance.PlayScaredShout();
    }

    public void TakeDamage(float amount = 10.0f)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, allMasks);
        return new Vector3(hit.position.x, transform.position.y, hit.position.z);
    }

    public void Wander()
    {
        if (Vector3.Distance(transform.position, wanderPoint) < 1.5f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else
        {
            agent.SetDestination(wanderPoint);
        }
    }
}
