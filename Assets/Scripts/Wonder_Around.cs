
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Wonder_Around : MonoBehaviour
{

    public NavMeshAgent AI;
    public Transform nextCentrePoint; //The center point for next path point
    public float nextRange; //The range for next path point
    private Vector3 nextLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AI.remainingDistance <= AI.stoppingDistance)
        {
            // Gebnnerate a new random location within the range around the centre point (in a sphere)
            nextLocation = nextCentrePoint.position + Random.insideUnitSphere * nextRange;//https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Random-insideUnitSphere.html

            AI.SetDestination(nextLocation);

          
           
        }
    }
}
