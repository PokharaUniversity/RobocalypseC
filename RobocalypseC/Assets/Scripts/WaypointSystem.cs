using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public List<GameObject> waypoints=new List<GameObject>();
    public gameManager manager;
    public int nearestWaypointToPlayer=0;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = manager.Player;
        calculateNearestWaypointToPlayer();
    }

    void calculateNearestWaypointToPlayer() {
        int index=nearestWaypointToPlayer;
        float shortstDistance=Vector3.Distance(player.transform.position,waypoints[index].transform.position);
        for (int i = 0; i < waypoints.Count; i++) {
            if (shortstDistance > Vector3.Distance(player.transform.position, waypoints[i].transform.position)) {
                index = i;
                shortstDistance = Vector3.Distance(player.transform.position, waypoints[i].transform.position);
            }
        }
        nearestWaypointToPlayer = index;
    }

    int calculateNearestWaypointToPoint(Vector3 position) {
        int index = 0;
        float shortstDistance = Vector3.Distance( position, waypoints[index].transform.position);
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (shortstDistance > Vector3.Distance(position, waypoints[i].transform.position))
            {
                index = i;
                shortstDistance = Vector3.Distance(position, waypoints[i].transform.position);
            }
        }
        return index;
    }

  
}
