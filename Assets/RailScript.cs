using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailScript : MonoBehaviour
{
    private LinkedList<GameObject> Points = new LinkedList<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i < transform.childCount; i++)
        {
            Points.AddLast(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        if(Points.Count == 0)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Points.AddLast(transform.GetChild(i).gameObject);
            }
        }
        Gizmos.color = Color.green;
        var p = Points.First;
        while (p.Next != null)
        {
            Gizmos.DrawWireSphere(p.Value.transform.position, 0.1f);
            Gizmos.DrawLine(
                p.Value.transform.position,
                p.Next.Value.transform.position
            );
            p = p.Next;

        }
        Gizmos.DrawWireSphere(p.Value.transform.position, 0.1f);
    }
}
