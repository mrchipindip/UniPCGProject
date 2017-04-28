using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{


    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    public Transform follow;
    private Vector3 targetPosition;

    public static int health = 10;

    //private float distance = 10;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {

    }

    void LateUpdate()
    {


        //set the target position to be on the correct offset from the object
        //targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;


        Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
        Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
        Debug.DrawRay(follow.position, targetPosition, Color.magenta);


        //make a smooth transition between the current position an the position it should be in

        transform.position = new Vector3(follow.position.x, follow.position.y + distanceUp, follow.position.z - distanceAway); //Follows from a stationary position
                                                                                                                               //transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * smooth);  //Follows behind the player




        //make camera look in correct direction
        transform.LookAt(follow);

    }
}
