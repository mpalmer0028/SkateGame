using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DustPlaneScript : MonoBehaviour
{
    public GameObject Board;
    public int HoverHeight;
    public ParticleSystem ps;
    private int HoverLayerMask;
    private Vector3 InitScale;
    private RaycastHit hit;



    // Start is called before the first frame update
    void Start()
    {
        HoverLayerMask = ~LayerMask.GetMask("Player");
        InitScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var shape = ps.shape;
        //shape = 
        if (Physics.Raycast(Board.transform.position, -Board.transform.up, out hit, HoverHeight))
        {
            Debug.DrawRay(hit.point, Board.transform.up, Color.magenta);
            transform.rotation = Board.transform.rotation;
            transform.position = hit.point;
            transform.localScale = InitScale;
        }
        else
        {
            transform.position = Vector3.zero;
            transform.localScale = Vector3.zero;
            Debug.DrawRay(Board.transform.position, Board.transform.up, Color.black);
        }
    }
}
