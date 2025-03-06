using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    public override (float[] interest, float[] danger) GetSteering(out float[] desirability, float[] interest, float[] danger, AIData aiData)
    {
        desirability = new float[8]; // Initialize desirability

        foreach (Transform obstacle in aiData.obstacles)
        {
            //get the relative position to the obstacle
            Vector2 directionToObstacle = obstacle.position - transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance, the closer the obstacle the higher the weight
            //weight should be 0 if the obstacle is far, and 1 if we are on top of it
            float weight = distanceToObstacle > aiData.detectionRadius ? 0 : (1 - distanceToObstacle / aiData.detectionRadius);

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //loop through each direction and set the danger for each based on the dot product with the directionToObstacleNormalized
            for (int i = 0; i < 8; i++)
            {
                float dot = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                //accept only directions at the same side of the obstacle at 90 degree
                if (dot > 0)
                {
                    danger[i] = Mathf.Max(danger[i], weight * dot);
                }
            }
        }

        return (interest, danger);
    }
}