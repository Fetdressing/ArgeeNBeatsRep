using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // If we're not local
        if (!isLocalPlayer)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal") * Time.deltaTime* 150f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 500.0f;

        Rigidbody rigbody = gameObject.GetComponent<Rigidbody>();

        rigbody.AddForce(transform.forward * z);

        transform.Rotate(0, x, 0);
        //transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    // Needs to have Cmd infront to be recognized
    [Command]
    void CmdFire()
    {
        // Create bullet
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        // Spawn on clients
        NetworkServer.Spawn(bullet);

        // Destroy after two seconds
        Destroy(bullet, 2.0f);

    }
}
