using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaterCameraScript : MonoBehaviour
{
    public GameObject LocationTarget;
    public GameObject AimTarget;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = LocationTarget.transform.rotation;
        transform.position = LocationTarget.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(AimTarget.transform);
        
        transform.position = new Vector3(
            Mathf.Lerp(LocationTarget.transform.position.x, transform.position.x, .1f),
            Mathf.Lerp(LocationTarget.transform.position.y, transform.position.y, .05f),
            Mathf.Lerp(LocationTarget.transform.position.z, transform.position.z, .1f));
    }
}
