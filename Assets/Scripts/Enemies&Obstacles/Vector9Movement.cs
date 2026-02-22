using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;
using TreeEditor;

public class Vector9Movement : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    public Transform[] patrolAreas;
    public float chaseDistance;
    public CanvasGroup gameOverCanvas;    
    public float fadeDuration = 2f;

    public GameObject scope;

    public Animator animator;
    //[SerializeField] private CanvasGroup gameOverCanvas;
    //[SerializeField] private AudioSource jumpscareSource;
    //[SerializeField] private AudioClip jumpscareClip;

    //public GameObject inventory,staminaAndItem,scope;

   

    public float waitTime = 2f;
    public float vectorPatrolSpeed = 2f;
    public float vectorChaseSpeed = 10f;
    //[SerializeField] float fadeDuration = 2f;

    //[SerializeField] float attackRange = 1f;
    public bool isStunned;
    public bool isSlowed;
    public float stunRange = 10f;
    int currentPatrolIndex = 0;
    bool isPlayerInRange;
    bool waiting;
    //bool gameOverTriggered;
    
    public NavMeshAgent agent;

    public GameObject stunIcon;

    [Header("Fire Zone Settings")]
    public bool isFireApplied;
    public float fireFearMinDistance = 8f;
    public float fireFearRetreatSpeed = 6f;
    public bool retreat = true;

    float fireFearUntil;

    // Save defaults of vector9's settings based on gamemode
    float defaultChaseDistance;
    float defaultChaseSpeed;
    float defaultAngularSpeed;
    float defaultAccel;

    //[SerializeField]private Collider triggerCollider;
    //[SerializeField]private Collider solidCollider;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Start()
    {
        defaultChaseDistance = chaseDistance;
        defaultChaseSpeed = vectorChaseSpeed;
        defaultAngularSpeed = agent.angularSpeed;
        defaultAccel = agent.acceleration;

        if (patrolAreas.Length > 0)
        {
            agent.speed = vectorPatrolSpeed;
            agent.destination = patrolAreas[currentPatrolIndex].position;

        }
        
    }

    private void Update()
    {
        if (isStunned)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            return;   // <-- prevents chase/patrol logic from running
        }




        if (isFireApplied && Time.time >= fireFearUntil)
        {
            EndFireZone();
        }
        float dist = Vector3.Distance(transform.position, playerPosition.position);
        if (scope != null)
        {
            if (dist <= stunRange)
                scope.SetActive(false);   
            else
                scope.SetActive(true);    
        }
        stunIcon.SetActive(dist <= stunRange);




        if (isFireApplied)
        {
            HandleFireZone();
            return;
        }


        if (dist <= chaseDistance)
        {
            isPlayerInRange = true;
            agent.speed = vectorChaseSpeed;
            agent.angularSpeed = 800f;
            agent.acceleration = 20f;
            if(dist >= 5f)
            {
                animator.speed = 5f;
            }

            agent.destination = playerPosition.position;
        }
        else 
        {
            
            animator.speed = 1f;
            agent.speed = vectorPatrolSpeed;
            agent.angularSpeed = 120f;
            agent.acceleration = 15f;

            if (patrolAreas.Length > 0)
            {
                agent.destination = patrolAreas[currentPatrolIndex].position;
            }
            Patrol();
        }
       
        
    }


    void HandleFireZone()
    {
        animator.speed = 1f;

        // Vector9 will not chase during flamethrower sequence
        if (!retreat)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            return;
        }

        agent.isStopped = false;
        agent.speed = fireFearRetreatSpeed;
        agent.angularSpeed = 720f;
        agent.acceleration = 25f;

        Vector3 awayDir = (transform.position - playerPosition.position).normalized;
        Vector3 retreatTarget = transform.position + awayDir * fireFearMinDistance;


        if (NavMesh.SamplePosition(retreatTarget, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    public void ApplyFireZone(float duration)
    {
        if (isStunned)
        {
            return;
        }

        isFireApplied = true;
        fireFearUntil = Mathf.Max(fireFearUntil, Time.time + duration);
        // Disable vector9 detection during this time
        chaseDistance = 0f;
    }

    void EndFireZone()
    {
        isFireApplied = false;

        // restore the default values for vector9 and end the flamethrower effects
        chaseDistance = defaultChaseDistance;
        vectorChaseSpeed = defaultChaseSpeed;
        agent.angularSpeed = defaultAngularSpeed;
        agent.acceleration = defaultAccel;

        agent.isStopped = false;

        if (patrolAreas.Length > 0)
        {
            agent.destination = patrolAreas[currentPatrolIndex].position;
        }
    }

    

    void Patrol()
    {
        if (patrolAreas.Length == 0 || waiting)
        {
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.3)
        {
            StartCoroutine(WaitEachPoint());
        }
    }

    IEnumerator WaitEachPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolAreas.Length;
        agent.destination = patrolAreas[currentPatrolIndex].position;
        waiting = false;
    }
    //void ChasePlayer()
    //{
    //    if (playerPosition)
    //    {
    //        agent.destination = playerPosition.position;
    //    }
    //}



    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2.5f);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (gameOverCanvas)
                gameOverCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        print("bye");
        yield return new WaitForSeconds(3f);
        print("Hi");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
    }

    public void StartFade()
    {
        StartCoroutine(GameOverSequence());
    }

    public void Stun()
    {
        
        isStunned = true;
        animator.SetTrigger("Stun");
        
        agent.isStopped = true;
        
        StartCoroutine(Recover());
    }

    
    IEnumerator Recover()
    {
        yield return new WaitForSeconds(10f);
        isStunned = false;
        agent.isStopped = false;
        animator.ResetTrigger("Stun");

        animator.Play("Walking", 0);
    }
}
