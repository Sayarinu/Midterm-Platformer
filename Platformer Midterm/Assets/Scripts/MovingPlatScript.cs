using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatScript : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float range = .1f;
    [SerializeField]
    GameObject points;
    [SerializeField]
    GameObject platform;
    List<Vector3> waypointsList;
    Vector3 currTarget;
    int currInd = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypointsList = new List<Vector3>();
        foreach (Transform c in points.transform) {
            waypointsList.Add(c.position);
        }
        currTarget = waypointsList[currInd];
    }

    // Update is called once per frame
    void Update()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currTarget, speed * Time.deltaTime);
        if (Vector3.Distance(currTarget, platform.transform.position) <= range) {
            currInd++;
            if (currInd >= waypointsList.Count) {
                currInd = 0;
            }
            currTarget = waypointsList[currInd];
        }
    }
}
