using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkateMovementScript : MonoBehaviour
{
    public GameObject Rudder;
    public GameObject JumpPoint;
    public GameObject[] Wheels;    

    public float HoverHeight;
    public float UpForce;
    public float JumpForce;
    public float PushSpeed;
    public float TurnSpeed;

    private int HoverLayerMask;
    private bool JumpCooldown;
    private Rigidbody rb;
    private float initDrag;

    // Start is called before the first frame update
    void Start()
    {
        HoverLayerMask = ~LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody>();
        initDrag = rb.drag;
    }

    void FixedUpdate()
    {
        var grindbtnKey = Input.GetKey(KeyCode.LeftControl);
        // Hover
        RaycastHit hit;
        var allHit = new List<bool>();
        if (!grindbtnKey)
        {
            rb.drag = initDrag;
            for (var i = 0; i < Wheels.Length; i++)
            {
                var currentWheel = Wheels[i];
                if (Physics.Raycast(currentWheel.transform.position, -currentWheel.transform.up, out hit, HoverHeight, HoverLayerMask))
                {
                    rb.AddForceAtPosition(currentWheel.transform.up * UpForce * (1f - (hit.distance / HoverHeight)),
                        currentWheel.transform.position);
                    Debug.DrawRay(currentWheel.transform.position, -currentWheel.transform.up * hit.distance, Color.yellow);
                    //Debug.DrawRay(currentWheel.transform.position, currentWheel.transform.TransformDirection(-currentWheel.transform.up) * hit.distance, Color.yellow);
                    allHit.Add(true);
                }
                else
                {
                    allHit.Add(false);
                    if (transform.position.y > currentWheel.transform.position.y)
                    {
                        rb.AddForceAtPosition(currentWheel.transform.up * UpForce,
                        currentWheel.transform.position);
                    }
                    else
                    {
                        //rb.AddForceAtPosition(-currentWheel.transform.up * UpForce/10,
                        //currentWheel.transform.position);
                    }

                }
            }
        }
        else
        {
            rb.drag = initDrag/4;
            for (var i = 0; i < Wheels.Length; i++)
            {
                var currentWheel = Wheels[i];
                rb.AddForceAtPosition(-currentWheel.transform.up * UpForce,
                    currentWheel.transform.position);
            }
        }

        // Push
        var turnForce = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * TurnSpeed, 0, 0);
        
        if (!grindbtnKey)
        {
            rb.AddForce(transform.rotation * new Vector3(0, -Input.GetAxis("Vertical") * PushSpeed, 0));
            rb.AddForce(turnForce / 10);
            rb.AddForceAtPosition(turnForce, Rudder.transform.position);
        }
        else
        {
            rb.AddForce(transform.rotation * new Vector3(0, -Input.GetAxis("Vertical") * PushSpeed/4, 0));
        }
        // Jump
        if (Input.GetKey(KeyCode.Space) && allHit.AsQueryable().All(x=> x) && !grindbtnKey)
        {
            
            //rb.AddForce(transform.forward * JumpForce);
            for (var i = 0; i < Wheels.Length; i++)
            {
                var currentWheel = Wheels[i];
                rb.AddForceAtPosition(transform.forward * JumpForce, currentWheel.transform.position);
            }
                
            //rb.AddForceAtPosition(transform.forward * JumpForce, Wheels[3].transform.position);
        }

        //if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(Vector3.up, Vector3.forward)) > 90)
        //{
        //    rb.angularVelocity = -rb.angularVelocity/2;
        //    rb.MoveRotation(Quaternion.LookRotation(Vector3.up, Vector3.forward));
        //}
        if(!allHit.AsQueryable().All(x => x))
        {
            rb.angularVelocity = rb.angularVelocity * .5f;
        }

        if(Quaternion.Angle(Quaternion.LookRotation(Vector3.forward, Vector3.down), transform.rotation)<30)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            transform.position += Vector3.up * 2;
        }
    }
}
