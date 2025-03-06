using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    public float weight = 1;  // How much this behavior influences the final movement.
    public Vector2 desiredVelocity;

    public abstract (float[] interest, float[] danger) GetSteering(out float[] desirability, float[] interest, float[] danger, AIData aiData);

    // Helper function for gizmos, if you want to visualize the desired velocity.
    public virtual void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + desiredVelocity);
        }
    }
}