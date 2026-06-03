
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Wonder_To_Target : MonoBehaviour
{

    public NavMeshAgent AI;
    public Transform nextCentrePoint; //The center point for next path point
    public float nextRange; //The range for next path point
    private Vector3 nextLocation;
    Animator Animate;
    public Transform finalLocation;
    //public float activateDistance;
    private bool stage2;
    private bool clapFlag = false;
    public float flexDistance;//Leaving a space so the NPC won't get too close
    public UnityEvent clap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
        Animate = gameObject.GetComponent<Animator>();
        Animate.SetBool("Clap", false);
        stage2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log (stage2);
        if (stage2 == false)
        {

                if (AI.remainingDistance <= AI.stoppingDistance)
                {
                    
                    nextLocation = nextCentrePoint.position + Random.insideUnitSphere * nextRange;//https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Random-insideUnitSphere.html
                
                    AI.SetDestination(nextLocation);

                }
        }
        else if (stage2 == true)
        {
            AI.destination = finalLocation.position;
            
            float distanceToTarget = Vector3.Distance(transform.position, finalLocation.position);
            //Debug.Log (distanceToTarget);
            if (distanceToTarget< flexDistance)
            {
                AI.isStopped = true;
                Animate.SetBool("Clap", true);

                if(!clapFlag)//This prevents the function to keep getting called every frame, causing audio issue
                {
                    clap.Invoke();
                  
                    clapFlag = true;
                }
                
            }
        }


    }

    public void NewTarget()
    {
        stage2 = true;   
    }
}
