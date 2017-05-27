using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]


public class NPCFollowPlayer : MonoBehaviour {





	public Transform target;			
	public float moveSpeed = 8.0f;
	public float turnSpeed = 2.0f;
	public float rayDistance = 2.0f;
    public float angularRayDistance = 1.0f;
	public enum NPC {Idle, FreeRoam, Chasing, RunningAway, Waypointing};
	public NPC myState;
	public 	float minRange = 4.0f;
	public	float maxRange = 45.0f;
	public bool isNpcChasing = true;
    public float heightChange = 0.75f;
    public float centerRayDist = 1.0f;

	private float minimumRangeSqr;
	private float maximumRangeSqr;

	private Transform myTransform;		
	private Rigidbody myRigidbody;
	private Vector3 desiredVelocity;
	private bool isGrounded = false;

	private float freeRoamTimer = 0.0f;
	private float freeRoamTimerMax = 5.0f;
	private float freeRoamTimerMaxRange = 1.5f;
	private float freeRoamTimerMaxAdjusted = 5.0f;

	Vector3 calcDir;







	void Start () {
		minimumRangeSqr = minRange * minRange;
		maximumRangeSqr = maxRange * maxRange;

		myTransform = transform;
		myRigidbody = GetComponent<Rigidbody>();
		myRigidbody.freezeRotation = true;

		calcDir = Random.onUnitSphere;
		calcDir.y = 0.0f; //myTransform.forward.y;   no required as myTransform.forward.y is also 0 on the y axis
	}




	void Update() {


		switch (myState) 
		{

		case NPC.Idle :
			desiredVelocity = new Vector3(0, myRigidbody.velocity.y, 0);
			break;

		case NPC.FreeRoam :
			freeRoamTimer += Time.deltaTime;

			if(freeRoamTimer > freeRoamTimerMaxAdjusted )
			{
				freeRoamTimer = 0.0f;
				freeRoamTimerMaxAdjusted = freeRoamTimerMax + Random.Range( -freeRoamTimerMaxRange, freeRoamTimerMaxRange);

				calcDir = Random.onUnitSphere;  //Ask for a random vector 3
				calcDir.y = 0.0f; //myTransform.forward.y;
			}

			Moving (calcDir);
			break;

		case NPC.Chasing :
			Moving ((target.position - myTransform.position).normalized);
			break;

		case NPC.RunningAway :
			Moving ((myTransform.position - target.position).normalized);
			break;
		}



        //Moving ();
    }





	void Moving (Vector3 lookDirection) {

		//Rotation
		//Vector3 lookDirection = (target.position - myTransform.position).normalized;

		RaycastHit hit;

		float shoulderMultiplyer = 0.2f;
		Vector3 leftRayPos = myTransform.position - (myTransform.right * shoulderMultiplyer);
		Vector3 rightRayPos = myTransform.position + (myTransform.right * shoulderMultiplyer);
        Vector3 heightAdj = new Vector3(0, heightChange, 0);
        
        //Left ray
        if (Physics.Raycast(leftRayPos + heightAdj, myTransform.forward,out hit, rayDistance))
		{ if (hit.collider.gameObject.tag != "Player" )
			{
                if (hit.collider.gameObject.tag != "Room")
                {
                    Debug.DrawLine(leftRayPos, hit.point, Color.red);
                    lookDirection += hit.normal * 20.0f;
                }
			}
		}
        //right Ray
		else if (Physics.Raycast(rightRayPos + heightAdj, myTransform.forward,out hit, rayDistance))
		{ if (hit.collider.gameObject.name != "Player")
			{
                if (hit.collider.gameObject.tag != "Room")
                {
                    Debug.DrawRay(rightRayPos, myTransform.forward * rayDistance, Color.red);
                    lookDirection += hit.normal * 20.0f;
                }
			}
		}
        //center ray
        else if (Physics.Raycast(myTransform.position + heightAdj, (myTransform.forward * rayDistance) * centerRayDist, out hit, rayDistance))
        {
            if (hit.collider.gameObject.name != "Player")
            {
                if (hit.collider.gameObject.tag != "Room")
                {
                    Debug.DrawRay(rightRayPos, myTransform.forward * rayDistance, Color.red);
                    lookDirection += hit.normal * 55.0f;
                }
            }
        }
        //right 45
        else if (Physics.Raycast(rightRayPos + heightAdj, (myTransform.forward + myTransform.right) * angularRayDistance, out hit, angularRayDistance))
        {
            if (hit.collider.gameObject.name != "Player")
            {
                if (hit.collider.gameObject.tag != "Room")
                {
                    Debug.DrawRay(rightRayPos, (myTransform.forward + myTransform.right) * angularRayDistance, Color.red);
                    lookDirection += hit.normal * 10.0f;
                }
            }
        }
        //left 45
        else if (Physics.Raycast(leftRayPos + heightAdj, (myTransform.forward - myTransform.right) * angularRayDistance, out hit, angularRayDistance))
        {
            if (hit.collider.gameObject.name != "Player")
            {
                if (hit.collider.gameObject.tag != "Room")
                {
                    Debug.DrawRay(leftRayPos, (myTransform.forward - myTransform.right) * angularRayDistance, Color.red);
                    lookDirection += hit.normal * 10.0f;
                }
            }
        }
        else 
		{
            Debug.DrawRay(rightRayPos + heightAdj, (myTransform.forward + myTransform.right) * angularRayDistance, Color.green);
            Debug.DrawRay(leftRayPos + heightAdj, (myTransform.forward - myTransform.right) * angularRayDistance, Color.green);
            Debug.DrawRay(myTransform.position + heightAdj, (myTransform.forward * rayDistance) * centerRayDist, Color.green);
            Debug.DrawRay(leftRayPos + heightAdj, myTransform.forward * rayDistance, Color.green);
			Debug.DrawRay(rightRayPos + heightAdj, myTransform.forward * rayDistance, Color.green);
		}


		Quaternion lookRot = Quaternion.LookRotation (lookDirection);

		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, lookRot, turnSpeed * Time.deltaTime);

		//Movement
		//myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
		desiredVelocity = myTransform.forward * moveSpeed;
		desiredVelocity.y = myRigidbody.velocity.y;
	}

	void FixedUpdate() {
		//if (isGrounded) {
			myRigidbody.velocity = desiredVelocity;
		//}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.name == "Floor") {
			isGrounded = true;
		}
	}
	void OnCollisionStay(Collision collision) {
		if (collision.collider.gameObject.name == "Floor") {
			isGrounded = true;
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.collider.gameObject.name == "Floor") {
			isGrounded = false;
		}
	}




	void MakeDecisions() {
		float sqrDist = (target.position - myTransform.position).sqrMagnitude;

		if (sqrDist > maximumRangeSqr) {
			if (isNpcChasing) 
			{
				myState = NPC.Chasing;
			} 
			else 
			{
				myState = NPC.FreeRoam;
			}
		} 
		else if (sqrDist < minimumRangeSqr) 
		{
			if ( isNpcChasing )
			{
				myState = NPC.Idle;
			}

			else 
			{
				myState = NPC.RunningAway;
			}
		}

		else
		{
			if ( isNpcChasing )
			{
				myState = NPC.Chasing;
			}

			else 
			{
				myState = NPC.RunningAway;
			}
		}

	}


}
