using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PassingPlatforms : NetworkBehaviour {
    private Transform thisTransform;

    public Transform startTransform;
    public Transform endTransform;

    public Transform[] spawnObjects;
    public float objectForce = 100;

    public float offsetPos = 100.0f;
    public float respawn_Time = 1.0f;
    public float lifeTime = 5.0f;

    private Vector3 startToEndVec;
    private Vector3 endToStartVec;
	// Use this for initialization
	void Start () {
        startToEndVec = endTransform.position - startTransform.position;
        endToStartVec = startTransform.position - endTransform.position;

        startToEndVec.Normalize();
        endToStartVec.Normalize();

        thisTransform = this.transform;

        StartCoroutine(Run());

    }

    IEnumerator Run()
    {
        while(this != null)
        {
            if (!NetworkServer.active)
            {
                yield return new WaitForSeconds(5.0f);
                continue;
            }

            GameObject platform;

            platform = Instantiate(spawnObjects[0].gameObject, startTransform.position, thisTransform.rotation) as GameObject;
            platform.transform.LookAt(endTransform.position);
            Vector3 sideVec = platform.transform.right;
            float offset = Random.Range(-offsetPos, offsetPos);
            platform.transform.position += sideVec * offset;
            //platform.GetComponent<Rigidbody>().AddForce(startToEndVec * objectForce, ForceMode.Impulse);
            platform.GetComponent<Rigidbody>().velocity = startToEndVec * objectForce;

            NetworkServer.Spawn(platform);

            Destroy(platform.gameObject, lifeTime);

            yield return new WaitForSeconds(respawn_Time);
        }
    }
}
