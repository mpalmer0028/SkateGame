using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUpScript : MonoBehaviour
{
    //public GameObject Board;
    public GameObject PushUpPoint;
    public float PushUpForce;
    public int HoverHeight;

    //values that will be set in the Inspector
    public float RotationSpeed;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    private int HoverLayerMask;
    private RaycastHit hit;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        HoverLayerMask = ~LayerMask.GetMask("Player");
        //rb = Board.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ////find the vector pointing from our position to the target
        //_direction = (transform.position + Vector3.up).normalized;

        ////create the rotation we need to be in to look at the target
        //_lookRotation = Quaternion.LookRotation(_direction);

        ////rotate us over time according to speed until we are in the required rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
    }
}
