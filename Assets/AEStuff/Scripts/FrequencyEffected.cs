using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class FrequencyEffected : MonoBehaviour {

    public bool DirectEffected = true;
    public Transform RemoteTransform;
    public float range = 0;

    Transform transToCheck;
    Transform objectTrans;
    Rigidbody objectRigid;
    List<Transform> waypoints;

    // 0 is 0->1 , 1 is 1->2
    public int currWaypoint;

	// Use this for initialization
	void Start () {
        waypoints = new List<Transform>();

        bool first = true;
        foreach (Transform Trans in transform)
        {
            // Save first as object
            if (first)
            {
                objectTrans = Trans;
                objectRigid = objectTrans.GetComponent<Rigidbody>();
                first = false;
            }
            else
            {
                waypoints.Add(Trans);
            }
        }

        if (waypoints.Count < 2)
        {
            Debug.LogError("Less then two waypoints on a frq effected object: " + waypoints.Count);
        }

        if (DirectEffected)
        {
            transToCheck = transform;
        }
        else
        {
            transToCheck = RemoteTransform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!NetworkServer.active)
        {
            return;
        }

        float totalFreq = 0.0f;
        // For each player
        foreach(GameObject player in PlayerManager.playerList)
        {
            // Check if inside range
            if (Vector3.Distance(player.transform.position, transToCheck.position) < range)
            {
                // Add their frequency
                totalFreq += player.GetComponent<SoundInput>().GetCurrentFrequency();
            }
        }

        // When we done here yo, we move object according
        float endFreq = Mathf.Min(totalFreq, 1.0f);

        MoveByFrequency(endFreq);
	}

    public void MoveByFrequency(float inFrequency)
    {
        // borde vara längden från start till obj, och längd från start till waypoint
        // om obj längd är större, så borde jag sätta den till nästa waypoint pos,
        // öka sedan nästa waypoint index
        float lengthToWaypoint1 = Vector3.Distance(objectTrans.position, waypoints[currWaypoint + 1].position);
        float lengthToWaypoint2 = Vector3.Distance(objectTrans.position, waypoints[currWaypoint].position);
        float lengthWaypoints = Vector3.Distance(waypoints[currWaypoint].position, waypoints[currWaypoint + 1].position);

        Vector3 objDir;
        if (inFrequency > 0.1f)
        {
            objDir = waypoints[currWaypoint + 1].position - objectTrans.position;
        }
        else
        {
            objDir = waypoints[currWaypoint].position - objectTrans.position;
            inFrequency = 0.3f;
        }

        objectRigid.AddForce(objDir * inFrequency*Time.deltaTime* 100.0f);

        if (objectRigid.velocity.magnitude > 10)
        {
            objectRigid.velocity = objectRigid.velocity.normalized * 10.0f;
        }


        if (lengthToWaypoint2 >= lengthWaypoints &&
    Vector3.Dot(objectRigid.velocity.normalized, (waypoints[currWaypoint + 1].position - objectTrans.position).normalized) < -0.5f)
        {
            objectRigid.velocity = Vector3.zero;
            objectTrans.position = waypoints[currWaypoint + 1].position;

            currWaypoint++;
            currWaypoint = Mathf.Min(currWaypoint, waypoints.Count - 2);
            
            Debug.Log("Entered first " + currWaypoint);
        }
        else if (lengthToWaypoint1 >= lengthWaypoints &&
            Vector3.Dot(objectRigid.velocity.normalized, (waypoints[currWaypoint].position - objectTrans.position).normalized) < -0.5f)
        {
            objectRigid.velocity = Vector3.zero;
            objectTrans.position = waypoints[currWaypoint].position;

            currWaypoint--;
            currWaypoint = Mathf.Max(currWaypoint, 0);

            Debug.Log("Entered second" + currWaypoint);
        }

    }
}
