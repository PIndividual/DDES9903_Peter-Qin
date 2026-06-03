using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;



[RequireComponent(typeof(NavMeshAgent))]
public class Chase_To_Condition : MonoBehaviour
{
    public NavMeshAgent AI;
    public Transform target1;
    //public Transform target2;
    Animator Animate;//https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Animator.SetFloat.html
    public UnityEvent actionOnTarget;
    public UnityEvent actionAfter;
    public bool missionComplete;
    //Ported from Wonder_Around
    public Transform nextCentrePoint; //The center point for next path point
    public float nextRange; //The range for next path point
    private Vector3 nextLocation;
    private float timer;
    public float waitTimeBeforeStill;
    public float waitTimeAfterStill;
    private bool actionDone = false;



    [Header("Chase and Escape settings")]
    public float activateDistance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
        Animate = gameObject.GetComponent<Animator>();
        Animate.SetFloat("ReachTarget", 0);
        Animate.SetBool("missionComplete", false);
        missionComplete = false;
        timer = 0;
        activateDistance = 500;
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceToTarget = Vector3.Distance(transform.position, target1.position);
        

        if (distanceToTarget < activateDistance) {
            AI.destination = target1.position;
        }
        
        if (distanceToTarget < 0.2) {
            timer += Time.deltaTime;
            Animate.SetFloat("ReachTarget", 1);
            if (timer > waitTimeBeforeStill){
                actionOnTarget.Invoke();
                Animate.SetBool("missionComplete", true);
                missionComplete = true;
            }
            
        }

        if (missionComplete == true) {
            if ( timer > waitTimeAfterStill){
                nextLocation = nextCentrePoint.position + Random.insideUnitSphere * nextRange;//https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Random-insideUnitSphere.html
                activateDistance = 0;
                AI.SetDestination(nextLocation);
                if (!actionDone) {
                    actionAfter.Invoke();
                    actionDone = true;
                }
                
                //Debug.DrawRay(nextLocation, Vector3.up, Color.blue, 1.0f); //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Debug.DrawRay.html
            }

        }
        //Debug.Log(distanceToTarget);
        //Debug.Log(timer);
    }
}
