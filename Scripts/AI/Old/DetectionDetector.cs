using UnityEngine;
using System.Collections.Generic;

public class DetectionDetector : Detector
{
    public float detectionRadius = 5f;
    public float detectionDelay = 0.15f; //Delay
    public float angle = 360;
    [Range(1, 30)] public int rays = 8;

    public override void Detect(AIData aiData)
    {
        //Generate rays in a circle
        var deltaAngle = angle / rays;
        for (int i = 0; i < rays; i++)
        {
            //Calculate direction of ray
            var currentAngle = -angle / 2 + deltaAngle / 2 + deltaAngle * i;
            var direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

            //Draw ray and store obstacle data
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(direction), detectionRadius, aiData.playerMask);
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * detectionRadius, Color.red);
                aiData.target = hit.transform;
                return; // Exit as soon as the player is found
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * detectionRadius, Color.green);
            }
        }

        // Player not found
        aiData.target = null;
    }
}