using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUpScript : MonoBehaviour
{
    public GameObject Board;
    public GameObject PushUpPoint;
    public float PushUpForce;
    public int HoverHeight;    
    private int HoverLayerMask;
    private RaycastHit hit;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        HoverLayerMask = ~LayerMask.GetMask("Player");
        rb = Board.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {            
            if (Physics.Raycast(PushUpPoint.transform.position, -Vector3.up, out hit, HoverHeight, HoverLayerMask))
            {
                rb.AddForceAtPosition(Vector3.up * PushUpForce, PushUpPoint.transform.position, ForceMode.Impulse);
            }
        }
    }
}
