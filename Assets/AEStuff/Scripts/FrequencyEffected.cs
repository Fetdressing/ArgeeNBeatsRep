using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrequencyEffected : MonoBehaviour {

    public bool DirectEffected = true;

    List<Transform> waypoints;

	// Use this for initialization
	void Start () {
        waypoints = new List<Transform>();

        foreach (Transform Trans in transform)
        {
            waypoints.Add(Trans);
            Debug.Log("Added Trans");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!DirectEffected)
        {
            return;
        }

        
	}
}
