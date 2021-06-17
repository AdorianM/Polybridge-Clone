using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Point : MonoBehaviour
{
    public bool Runtime = true;         // Adds the ability to have static points when not at runtime
    public Rigidbody2D rbd;             // Used to add physics to the point
    public Vector2 PointID;             // Define point's ID using its position
    public List<Bar> ConnectedBars;

    // At start make predefined dots static (immovable)
    private void Start()
    {
        if(Runtime == false)
        {
            rbd.bodyType = RigidbodyType2D.Static;
            PointID = transform.position;
            //PointID.x = Mathf.Round(transform.position.x);
            //PointID.y = Mathf.Round(transform.position.y);
            if (GameManager.AllPoints.ContainsKey(PointID) == false)
            {
                GameManager.AllPoints.Add(PointID, this);
            }
            
        }
    }

    // Position change for static points
    private void Update()
    {
        if(Runtime == false)
        {
            if (transform.hasChanged == true)
            {
                transform.hasChanged = false;
                transform.position = Vector3Int.RoundToInt(transform.position);
            }
        }
    }
}
