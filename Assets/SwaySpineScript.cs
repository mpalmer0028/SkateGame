using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwaySpineScript : MonoBehaviour
{
    public GameObject Target;
    public float Push;
    public float Rotation;
    private float initPosY;
    public float initPosX;

    // Start is called before the first frame update
    void Start()
    {
        initPosY = transform.position.y;
        initPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        var hAxis = Input.GetAxis("Horizontal");
        var tt = Target.transform;
        
        transform.localPosition = new Vector3(initPosX + hAxis * Push, transform.localPosition.y, transform.localPosition.z);
        transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, (hAxis * Rotation), transform.localRotation.w);
    }
}
