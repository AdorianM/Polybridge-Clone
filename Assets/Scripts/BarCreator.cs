using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 
public class BarCreator : MonoBehaviour, IPointerDownHandler
{
    public GameObject RoadBar;                  // Prefabs
    public GameObject WoodBar;
    bool BarCreationStarted = false;            // Tells if we're in the process of creating a bar
    public Bar CurrentBar;
    public GameObject BarToInstantiate;         // New bar GameObject
    public Transform barParent;                 // Helps instantiate BarToInstantiate
    public Point CurrentStartPoint;             // Start point of the CurrentBar
    public Point CurrentEndPoint;               // End point of the CurrentBar
    public GameObject PointToInstantiate;       // New point GameObject
    public Transform PointParent;               // Helps instantiate PointToInstantiate

    // On Click events
    public void OnPointerDown(PointerEventData eventData)
    {
        // One Left click to start creation
        // Another Left click to finish creation
        // A right click to cancel creation

        if (BarCreationStarted == false)
        {
            BarCreationStarted = true;
            StartBarCreation(Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(eventData.position)));
        }
        else
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                FinishBarCreation();
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                BarCreationStarted = false;
                DeleteCurrentBar();
            }
        }
    }

    // Instantiate bar and starting point
    void StartBarCreation(Vector2 StartPosition)
    {
        // Instantiate and initialize current bar
        CurrentBar = Instantiate(BarToInstantiate, barParent).GetComponent<Bar>();
        CurrentBar.StartPosition = StartPosition;

        foreach (KeyValuePair<Vector2, Point> kvp in GameManager.AllPoints)
        {
            print(string.Format("Key = {0}, KeyType = {1}, Value = {2}", kvp.Key, kvp.Key.GetType(), kvp.Value));
        }

        print("Current key: " + StartPosition + " Contains? " + GameManager.AllPoints.ContainsKey(StartPosition));

        // If Point already exists, use it. If not, instantiate another point.
        if (GameManager.AllPoints.ContainsKey(StartPosition))
        {
            //print("aici");
            CurrentStartPoint = GameManager.AllPoints[StartPosition];
        }
        else
        {
            //print("Nu exista start");
            CurrentStartPoint = Instantiate(PointToInstantiate, StartPosition, Quaternion.identity, PointParent).GetComponent<Point>();
            GameManager.AllPoints.Add(StartPosition, CurrentStartPoint);
        }
        
        // No info about end point. Instantiate at start position.
        CurrentEndPoint = Instantiate(PointToInstantiate, StartPosition, Quaternion.identity, PointParent).GetComponent<Point>();
    }


    void FinishBarCreation()
    {
        /**/
        // If endpoint exist, use that. If not, create a new point.

        if (GameManager.AllPoints.ContainsKey(CurrentEndPoint.PointID))
        {
            Destroy(CurrentEndPoint.gameObject);
            CurrentEndPoint = GameManager.AllPoints[(Vector2)CurrentEndPoint.PointID];
        }
        else
        {
            GameManager.AllPoints.Add(CurrentEndPoint.PointID, CurrentEndPoint);
        }

        // Tell the points that the bar is connected to them
        CurrentStartPoint.ConnectedBars.Add(CurrentBar);
        CurrentEndPoint.ConnectedBars.Add(CurrentBar);

        // Anchor bar to start and end point.
        CurrentBar.StartJoint.connectedBody = CurrentStartPoint.rbd;
        CurrentBar.StartJoint.anchor = CurrentBar.transform.InverseTransformPoint(CurrentBar.StartPosition);
        CurrentBar.EndJoint.connectedBody = CurrentEndPoint.rbd;
        CurrentBar.EndJoint.anchor = CurrentBar.transform.InverseTransformPoint(CurrentEndPoint.transform.position);

        // Start creation of a new bar. (Creation continues until right click)
        StartBarCreation(CurrentEndPoint.transform.position);
    }

    // Visually update the bar in creation
    private void Update()
    {
        if(BarCreationStarted == true)
        {
            Vector2 EndPosition = (Vector2)Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 Dir = EndPosition - CurrentBar.StartPosition;
            Vector2 ClampedPosition = CurrentBar.StartPosition + Vector2.ClampMagnitude(Dir, CurrentBar.maxLength);
            CurrentEndPoint.transform.position = (Vector2)Vector2Int.FloorToInt(ClampedPosition);
            CurrentEndPoint.PointID = CurrentEndPoint.transform.position;
            CurrentBar.UpdateCreatingBar(CurrentEndPoint.transform.position);
        }
    }

    // Remove bar and connected points if they are not static
    void DeleteCurrentBar()
    {
        Destroy(CurrentBar.gameObject);
        if (CurrentStartPoint.ConnectedBars.Count == 0 && CurrentStartPoint.Runtime == true) Destroy(CurrentStartPoint.gameObject);
        if (CurrentEndPoint.ConnectedBars.Count == 0 && CurrentEndPoint.Runtime == true) Destroy(CurrentEndPoint.gameObject);
    }
}
