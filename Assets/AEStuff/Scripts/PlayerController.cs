using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float forceMulti;
    public float jumpForce;
    public float jumpVectorCheck;
    public float downAccel = 0.75f;
    private Vector3 velocity = Vector3.zero;
    private Rigidbody trans;
    private bool isTimeToJump = false;
    public float jumpTimer = 5f;
    private float timeJumped = 0;
    private LayerMask playerLayer;

    public float turnSpeedMultiplier = 1.3f;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        playerLayer = ~(1 << LayerMask.NameToLayer("Player")); // ignore collisions with layerX
        trans = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Jump();
        Move();
        Turn();



    }

    private void Jump()
    {
        ///JUMP
        Vector3 downVector = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, downVector, out hit, jumpVectorCheck, playerLayer))
        {
            trans.drag = 2;
            //Debug.Log(hit.collider.ToString());
            //if (Physics.Raycast(transform.position, downVector, jumpVectorCheck))
            if (Input.GetKeyDown(KeyCode.Space))
            {
                trans.velocity = new Vector3(trans.velocity.x, 0, trans.velocity.z);
                //trans.position = new Vector3(trans.position.x, tran, trans.position.z);
                trans.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
        else
        {
            trans.drag = 0.75f;
        }
    }

    private void Turn()
    {
        float dx = Input.GetAxis("Mouse X");
        dx *= turnSpeedMultiplier;
        transform.RotateAround(transform.root.position, new Vector3(0, 1, 0), dx);
    }

    private void Move()
    {
        Vector3 totalVelocity = new Vector3(0, /*trans.velocity.y*/0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            totalVelocity += this.transform.forward * forceMulti;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVelocity += this.transform.right * forceMulti;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalVelocity += -this.transform.right * forceMulti;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalVelocity += -this.transform.forward * forceMulti;
        }

        trans.AddForce(totalVelocity * Time.deltaTime);
    }
}
