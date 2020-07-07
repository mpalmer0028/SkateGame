using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineTwist : MonoBehaviour
{
    public float Rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var hAxis = Input.GetAxis("Horizontal");

        transform.localRotation = new Quaternion(transform.localRotation.x, (hAxis * Rotation), transform.localRotation.z, transform.localRotation.w);
    }
}
