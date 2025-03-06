using UnityEngine;
using System.Collections.Generic;

public class ObstacleDetector : Detector
{
    public float detectionRadius = 2f;
    [Range(1, 30)] public int rays = 8;
    public float angle = 90;

    //Removed target

    public override void Detect(AIData aiData)
    {
        //Obstacles are no longer also the target

        //Generate rays in a circle
        var deltaAngle = angle / rays;
        for (int i = 0; i < rays; i++)
        {
            //Calculate direction of ray
            var currentAngle = -angle / 2 + deltaAngle / 2 + deltaAngle * i;
            var direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

            //Draw ray and store obstacle data
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(direction), detectionRadius, aiData.obstacleMask);
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * detectionRadius, Color.red);
                //Store obstacle data to danger array
                if (hit.collider.transform != null) //Add this check
                {
                    aiData.obstacles.Add(hit.collider.transform); // Add this line
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * detectionRadius, Color.green);
                //aiData.obstacles.Remove(hit.collider.transform); Remove this.
            }        }
    }
}