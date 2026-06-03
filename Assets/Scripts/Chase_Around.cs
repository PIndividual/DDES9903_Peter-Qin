using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Chase_Around : MonoBehaviour
{
    public NavMeshAgent AI;
    public Transform target;

    

    [Header("Chase and Escape settings")]
    public float activateDistance;
    public float escapeDistance;
    public float escapeStopDistance;//run until this far away

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        //AI.destination = target.position;
        if (distanceToTarget < activateDistance && escapeDistance<distanceToTarget)//close, but not that close
        {

            AI.isStopped = false;//https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AI.NavMeshAgent-isStopped.html
            AI.destination = target.position;
        }
        else if (distanceToTarget>activateDistance)//too far
        {
            
            AI.isStopped = true;
           
        }
        else if (distanceToTarget < escapeDistance)//too close
        {
            AI.isStopped = true;
            //Vector3 finalStop = target.position + target.forward * escapeStopDistance;
            //AI.destination = finalStop;
        }
        Debug.Log(distanceToTarget);

    }
}
