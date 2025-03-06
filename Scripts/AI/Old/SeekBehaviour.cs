using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    //Removed override GetVelocity

    public override (float[] interest, float[] danger) GetSteering(out float[] desirability, float[] interest, float[] danger, AIData aiData)
    {
        if (aiData.target == null)
        {
            desirability = new float[8];
            return (interest, danger);
        }

        //Get the direction to the target
        Vector2 directionToTarget = (aiData.target.position - transform.position).normalized;

        //loop through each direction and set the interest for each based on the dot product with the direction to the target
        for (int i = 0; i < 8; i++)
        {
            float dot = Vector2.Dot(directionToTarget, Directions.eightDirections[i]);

            //accept only directions at the same side of the target at 90 degree
            if (dot > 0)
            {
                interest[i] = dot * weight;
            }
        }
        desirability = interest;
        return (interest, danger);
    }
}