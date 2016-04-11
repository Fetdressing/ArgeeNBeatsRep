using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public Vector2 yMaxMin;
    private float rotationX;
    private float rotationY;

    public float turnSpeedMultiplier = 1.0f;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float dy = -1 * Input.GetAxis("Mouse Y");
        rotationY += dy * turnSpeedMultiplier;
        if (rotationY > yMaxMin.x)
        {
            rotationY = yMaxMin.x;
        }
        else if (rotationY < yMaxMin.y)
        {
            rotationY = yMaxMin.y;
        }

        Quaternion quater = Quaternion.AngleAxis(rotationY, transform.right);
        //transform.Rotate(new Vector3(dy, 0, 0));
        transform.rotation = quater * transform.root.rotation;
    }
}
