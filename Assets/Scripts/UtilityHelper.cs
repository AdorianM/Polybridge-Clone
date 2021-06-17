using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityHelper : MonoBehaviour
{
    public class Vector2EqualityComparer : IEqualityComparer<Vector2>
    {
        public bool Equals(Vector2 v1, Vector2 v2)
        {
            
            if (Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y))
            {
                print(" v1: " + v1.x + " " + v1.y + " v2: " + v2.x + " " + v2.y);
                return true;
            }
            return false;
        }

        public int GetHashCode(Vector2 obj)
        {
            return (Mathf.RoundToInt(obj.x) + Mathf.RoundToInt(obj.y)).GetHashCode();
        }
    }
}
