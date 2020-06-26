using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkateMovementScript : MonoBehaviour
{
    public GameObject Skater;

    public GameObject Rudder;
    public GameObject JumpPoint;
    public GameObject[] Wheels;    

    public float HoverHeight;
    public float UpForce;
    /// <summary>
    /// For going down when grinding
    /// </summary>
    public float DownForce;
    public float JumpForce;
    public float PushSpeed;
    public float TurnSpeed;

    private int HoverLayerMask;
    private bool JumpCooldown;
    private Rigidbody rb;
    private float initDrag;
    private float initAngDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        HoverLayerMask = ~LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody>();
        initDrag = rb.drag;
        initAngDrag = rb.angularDrag;
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
            rb.angularDrag = initAngDrag;
            for (var i = 0; i < Wheels.Length; i++)
            {
                
                var currentWheel = Wheels[i];
                Debug.DrawRay(currentWheel.transform.position, currentWheel.transform.up *1000, Color.green);
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
                        //rb.AddForceAtPosition(currentWheel.transform.up * UpForce/10 * (1f - (hit.distance / HoverHeight)),
                        //currentWheel.transform.position);
                    }
                    else
                    {
                        rb.AddForceAtPosition(-currentWheel.transform.up * UpForce/100 * (1f - (hit.distance / HoverHeight)),
                        currentWheel.transform.position);
                    }

                }
            }
        }
        else
        {
            rb.drag = initDrag/4;
            rb.angularDrag = 0;
            for (var i = 0; i < Wheels.Length; i++)
            {
                var currentWheel = Wheels[i];
                rb.AddForceAtPosition(-currentWheel.transform.up * DownForce,
                    currentWheel.transform.position);
            }
        }

        // Push
        var turnForce = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * TurnSpeed, 0, 0);
        
        if (!grindbtnKey)
        {
            var push = transform.rotation * new Vector3(0, 0, -Input.GetAxis("Vertical") * PushSpeed);
            if (!allHit.AsQueryable().All(x => x))
            {
                push /= 2;
            }
            rb.AddForce(push);
            rb.AddForce(turnForce / 10);
            rb.AddForceAtPosition(turnForce, Rudder.transform.position);
            
        }
        else
        {
            rb.AddForce(turnForce);
            rb.AddForce(transform.rotation * new Vector3(0, -Input.GetAxis("Vertical") * PushSpeed / 4), 0);
        }
        // Jump
        if (Input.GetKey(KeyCode.Space) && allHit.AsQueryable().All(x=> x) && !grindbtnKey)
        {

            //rb.AddForce(transform.up * JumpForce);
            for (var i = 0; i < Wheels.Length; i++)
            {
                var currentWheel = Wheels[i];
                rb.AddForceAtPosition(currentWheel.transform.up * JumpForce, currentWheel.transform.position);
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
            //rb.angularVelocity = rb.angularVelocity * .5f;

            var targetDirection = (transform.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
        }

        if(Vector3.Angle(Vector3.down, -transform.up)>90 && 
            Physics.Raycast(transform.position, -Vector3.up, out hit, HoverHeight/2, HoverLayerMask))
        {
            Crash();
        }
    }


    public void Crash()
    {
        Skater.transform.parent = null;

    }
}


