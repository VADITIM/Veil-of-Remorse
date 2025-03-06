using UnityEngine;

public class FleeBehaviour : SteeringBehaviour
{
    //Removed override GetVelocity

    public override (float[] interest, float[] danger) GetSteering(out float[] desirability, float[] interest, float[] danger, AIData aiData)
    {
        if (aiData.target == null)
        {
            desirability = new float[8];
            return (interest, danger);
        }

        Vector2 directionFromTarget = (transform.position - aiData.target.position).normalized;

        for (int i = 0; i < 8; i++)
        {
            float dot = Vector2.Dot(directionFromTarget, Directions.eightDirections[i]);
            if (dot > 0)
            {
                interest[i] = dot * weight;
            }
        }
        desirability = interest;
        return (interest, danger);
    }
}