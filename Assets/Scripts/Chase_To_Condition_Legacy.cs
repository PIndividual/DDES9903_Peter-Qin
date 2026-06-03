using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;



[RequireComponent(typeof(NavMeshAgent))]
public class Chase_To_Condition_Legacy : MonoBehaviour
{
    public NavMeshAgent AI;
    public Transform target1;
    //public Transform target2;
    Animator Animate;//https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Animator.SetFloat.html
    public UnityEvent actionOnTarget;
    //public bool missionComplete;




    [Header("Chase and Escape settings")]
    public float activateDistance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
        Animate = gameObject.GetComponent<Animator>();
        Animate.SetFloat("ReachTarget", 0);
        //Animate.SetBool("missionComplete", false);
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceToTarget = Vector3.Distance(transform.position, target1.position);

        if (distanceToTarget < activateDistance) {
            AI.destination = target1.position;
        }
        
        if (distanceToTarget < 0.2) {

            Animate.SetFloat("ReachTarget", 1.0f);
            actionOnTarget.Invoke();
            //Animate.SetBool("missionComplete", true);

        }


        //Debug.Log(distanceToTarget);

    }
}
