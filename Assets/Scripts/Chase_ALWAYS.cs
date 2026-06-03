using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Chase_ALWAYS : MonoBehaviour
{
    public NavMeshAgent AI;
    public Transform target;

    

    [Header("Chase and Escape settings")]
    public float activateDistance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < activateDistance) {
            AI.destination = target.position;
        }
        
        Debug.Log(distanceToTarget);

    }
}
