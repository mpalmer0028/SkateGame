using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkateMovementScript : MonoBehaviour
{
    public float ResetAngle = 89;
    public float ResetRotationSpeed = .01f;
    public GameObject Skater;

    public GameObject Rudder;
    public GameObject JumpPoint;
    public GameObject[] Wheels;    
    public GameObject[] TrainingWheels;    
    public GameObject[] LightTrainingWheels;    

    public float HoverHeight;
    public float UpForce;
    public float AntiBottomOutForce;
    public float AntiBottomOutDistance;
    /// <summary>
    /// For going down when grinding
    /// </summary>
    public float DownForce;
    public float JumpForce;
    public float PushSpeed;
    public float TurnSpeed;
    public float SnapVelocity;

    private int HoverLayerMask;
    private bool JumpCooldown;
    private Rigidbody rb;
    private float initDrag;
    private float initAngDrag;
    public bool shouldResetRotation;
    private Quaternion resetRotation;
    private float resetRotationI = 0;
    private Quaternion startResetRotation;
    private bool hitGround;
    private float timeCount;
    


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
            var wheelHeights = NormalizeData(Wheels.AsQueryable().Select(x => (double)x.transform.position.y).ToArray<double>(), 0, UpForce);
            for (var i = 0; i < Wheels.Length; i++)
            {

                var currentWheel = Wheels[i];
                //Debug.DrawRay(currentWheel.transform.position, currentWheel.transform.up *1000, Color.green);
                if (Physics.Raycast(currentWheel.transform.position, -currentWheel.transform.up, out hit, HoverHeight, HoverLayerMask))
                {
                    // Add more force to prevent bottoming out
                    if (hit.distance > AntiBottomOutDistance)
                    {
                        rb.AddForceAtPosition(currentWheel.transform.up * UpForce * (1f - (hit.distance / HoverHeight)),
                        currentWheel.transform.position);
                        Debug.DrawRay(currentWheel.transform.position, -currentWheel.transform.up * hit.distance, Color.yellow);
                    }
                    else
                    {
                        rb.AddForceAtPosition(currentWheel.transform.up * AntiBottomOutForce, currentWheel.transform.position);
                        Debug.DrawRay(currentWheel.transform.position, -currentWheel.transform.up * hit.distance, Color.black);
                    }

                    //Debug.DrawRay(currentWheel.transform.position, currentWheel.transform.TransformDirection(-currentWheel.transform.up) * hit.distance, Color.yellow);
                    allHit.Add(true);
                }
                else
                {
                    allHit.Add(false);                  

                    //rb.AddForceAtPosition(currentWheel.transform.up * ((UpForce * .5f) - (float)wheelHeights[i]) / 50,
                    //        currentWheel.transform.position);
                }
            }

            // Training Wheels
            if ((Quaternion.Angle(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up)) > ResetAngle && Physics.Raycast(transform.position, -Vector3.up, out hit, HoverHeight/2, HoverLayerMask)) 
                || shouldResetRotation)
            {
                shouldResetRotation = true;
                
                //find the vector pointing from our position to the target
                var direction = (transform.position + Vector3.up).normalized;

                //create the rotation we need to be in to look at the target
                var lookRotation = Quaternion.LookRotation(transform.forward, Vector3.up) ;
                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, timeCount);
                timeCount = timeCount + Time.deltaTime;
                //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * SnapVelocity);
                
                if (timeCount > 1)
                {
                    Debug.DrawRay(transform.position, Vector3.up * 10, Color.green);
                    shouldResetRotation = false;
                    timeCount = 0;
                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.up * 10, Color.blue);
                }
            }

            // Light Training Wheels
            //for (var i = 0; i < LightTrainingWheels.Length; i++)
            //{
            //    var currentWheel = LightTrainingWheels[i];
            //    //Debug.DrawRay(currentWheel.transform.position, currentWheel.transform.up *1000, Color.green);
            //    if (Physics.Raycast(currentWheel.transform.position, -currentWheel.transform.up, out hit, HoverHeight, HoverLayerMask))
            //    {
            //        // Add more force to prevent bottoming out
            //        if (hit.distance > AntiBottomOutDistance)
            //        {
            //            rb.AddForceAtPosition(currentWheel.transform.up * UpForce*2 * (1f - (hit.distance / HoverHeight)),
            //            currentWheel.transform.position);
            //            Debug.DrawRay(currentWheel.transform.position, -currentWheel.transform.up * hit.distance, Color.yellow);
            //        }
            //        else
            //        {
            //            rb.AddForceAtPosition(currentWheel.transform.up * AntiBottomOutForce, currentWheel.transform.position);
            //            Debug.DrawRay(currentWheel.transform.position, -currentWheel.transform.up * hit.distance, Color.black);
            //        }

            //    }

            //}
        }

        // Push
        var turnForce = transform.rotation * new Vector3(-Input.GetAxis("Horizontal") * TurnSpeed, 0, 0);
        
        if (!grindbtnKey)
        {
            var push = transform.rotation * new Vector3(0, 0, -Input.GetAxis("Vertical") * PushSpeed);
            if (!allHit.AsQueryable().All(x => x))
            {
                //push /= 2;
            }
            rb.AddForce(push);
            rb.AddForce(turnForce / 10);
            rb.AddForceAtPosition(turnForce, Rudder.transform.position);
            Debug.DrawLine(
                Rudder.transform.position,
                Rudder.transform.position + turnForce, 
                Color.green
            );

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
            //for (var i = 0; i < Wheels.Length; i++)
            //{
            //    var currentWheel = Wheels[i];
            //    //rb.AddForceAtPosition(currentWheel.transform.up * JumpForce, currentWheel.transform.position);
            //    rb.AddForceAtPosition(Vector3.up * JumpForce, currentWheel.transform.position);
            //}

            rb.AddForce(transform.up * JumpForce);
        }


        //if(!allHit.AsQueryable().All(x => x) && Vector3.Angle(Vector3.down, -transform.up) > ResetAngle //&&
        //    /*Physics.Raycast(transform.position, -Vector3.up, out hit, HoverHeight *2, HoverLayerMask)*/)
        //{
        //    //rb.angularVelocity = rb.angularVelocity * .5f;

        //    shouldResetRotation = hitGround;
        //    resetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        //    startResetRotation = transform.rotation;
        //    //var targetDirection = (transform.position - transform.position).normalized;
        //    //var targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime);
        //}
        //if (allHit.AsQueryable().All(x => x))
        //{
        //    hitGround = true;
        //}
        //if (shouldResetRotation)
        //{
        //    //Debug.DrawLine(transform.position, transform.position + (Vector3.up * 10), Color.white);
        //    //transform.rotation = Quaternion.Lerp(startResetRotation, resetRotation, resetRotationI);
        //    transform.rotation = Quaternion.Lerp(startResetRotation, resetRotation, resetRotationI);
        //    rb.angularVelocity = Vector3.zero;
        //    resetRotationI += .005f;
        //    if (resetRotationI > 1)
        //    {
        //        shouldResetRotation = false;
        //        hitGround = false;
        //    }
        //}
        if (Vector3.Angle(Vector3.down, -transform.up) > 100 &&
            Physics.Raycast(transform.position, -Vector3.up, out hit, HoverHeight / 2, HoverLayerMask))
        {
            Crash();
        }
    }


    public void Crash()
    {
        Skater.transform.parent = null;

    }

    public static double[] NormalizeData(IEnumerable<double> data, double min, double max)
    {
        double dataMax = data.Max();
        double dataMin = data.Min();
        double range = dataMax - dataMin;

        return data
            .Select(d => (d - dataMin) / range)
            .Select(n => (double)((1 - n) * min + n * max))
            .ToArray();
    }
}


