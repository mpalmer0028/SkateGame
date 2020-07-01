using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideBoardAnimationScript : MonoBehaviour
{
    public GameObject Board;
    public GameObject Spine;
    public GameObject Camera;
    public float HeightOfLookPoint;
    public float LookPointRotX;
    public float LookPointRotY;
    public float LookPointRotZ;
    public float LookPointRotW;
    private Quaternion InitSpineRotation;

    // Start is called before the first frame update
    void Start()
    {
        InitSpineRotation = Spine.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var spineT = Spine.transform;
        var boardT = Board.transform;
        var cameraT = Camera.transform;
        var initE = InitSpineRotation.eulerAngles;

        //var rotation = spineT.rotation;
        //var e = new Vector3(boardT.rotation.eulerAngles.z, boardT.rotation.y, boardT.rotation.z);
        //rotation.eulerAngles = e;
        //spineT.rotation = rotation;

        //spineT.LookAt(((cameraT.position - boardT.position) * -1 + boardT.position) + new Vector3(0,HeightOfLookPoint,0), Vector3.up);
        //spineT.rotation *= new Quaternion(LookPointRotX, LookPointRotY, LookPointRotZ, LookPointRotW);
    }
}
