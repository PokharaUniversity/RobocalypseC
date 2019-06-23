using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{
    public List<GameObject> SurroundWaypoints = new List<GameObject>();
    public List<float> Distance = new List<float>();
    public List<bool> jumpRequired = new List<bool>();
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < SurroundWaypoints.Count; i++) {
            Distance.Add( Vector3.Distance(transform.position, SurroundWaypoints[i].transform.position));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < SurroundWaypoints.Count; i++) {
            if(SurroundWaypoints[i]!=null)
            Gizmos.DrawLine(transform.position, SurroundWaypoints[i].transform.position);
        }
    }
}
