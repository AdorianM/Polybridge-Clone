using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures that we do not repeat points.
// Adds testing functionality
public class GameManager : MonoBehaviour
{
    private static UtilityHelper.Vector2EqualityComparer equalityComparer = new UtilityHelper.Vector2EqualityComparer();
    public static Dictionary<Vector2, Point> AllPoints = new Dictionary<Vector2, Point>(equalityComparer);

    private void Awake()
    {
        AllPoints.Clear();
        Time.timeScale = 0;
    }
}
