using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines properties and behaviour of a bar element
public class Bar : MonoBehaviour
{
    public float maxLength = 1f;                    // Length of a bar
    public Vector2 StartPosition;                   // Defined by first click on the canvas
    public SpriteRenderer barSpriteRenderer;        // Used to actually draw the bar (dynamic drawing)
    public BoxCollider2D boxCollider;
    public HingeJoint2D StartJoint;                 
    public HingeJoint2D EndJoint;

    float StartJointCurrentLoad = 0;
    float EndJointCurrentLoad = 0;
    MaterialPropertyBlock propBlock;                //Individually change material

    // Creates bar on user click
    public void UpdateCreatingBar(Vector2 ToPosition)
    {
        transform.position = (ToPosition + StartPosition) / 2;     // Position is in the middle of the two points

        // Determine the Angle, Direction, rotation and length of the bar.

        Vector2 dir = ToPosition - StartPosition;
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        float length = dir.magnitude;

        // Render sprite and collider using new data

        barSpriteRenderer.size = new Vector2(length, barSpriteRenderer.size.y);
        boxCollider.size = barSpriteRenderer.size;
    }

    public void UpdateMaterial()
    {
        if (StartJoint != null)
        {
            StartJointCurrentLoad = StartJoint.reactionForce.magnitude / StartJoint.breakForce;
        }
        if (EndJoint != null)
        {
            EndJointCurrentLoad = EndJoint.reactionForce.magnitude / EndJoint.breakForce;
        }

        float maxLoad = Mathf.Max(StartJointCurrentLoad, EndJointCurrentLoad);

        propBlock = new MaterialPropertyBlock();
        barSpriteRenderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_Load", maxLoad);
        barSpriteRenderer.SetPropertyBlock(propBlock);
    }

    private void Update()
    {
        if(Time.timeScale == 1)
        {
            UpdateMaterial();
        }
    }
}
