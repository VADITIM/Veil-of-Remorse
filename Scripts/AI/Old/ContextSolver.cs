using UnityEngine;
using System.Collections.Generic;

public class ContextSolver : MonoBehaviour
{
    [SerializeField] private bool showGizmos = true;

    private List<SteeringBehaviour> steeringBehaviours = new List<SteeringBehaviour>();
    private List<Detector> detectors = new List<Detector>();

    private Vector2 _resultDirection = Vector2.zero;
    public Vector2 ResultDirection { get => _resultDirection; }

    private AIData aiData;
    private float[] interest;
    private float[] danger;

    private void Start()
    {
        steeringBehaviours = new List<SteeringBehaviour>(GetComponents<SteeringBehaviour>());
        detectors = new List<Detector>(GetComponents<Detector>());
        aiData = GetComponent<EnemyBase>().aiData; // Assuming AIData is on the EnemyBase
        interest = new float[8];
        danger = new float[8];
    }

    private void Update()
    {
        // 1. Run Detectors
        foreach (var detector in detectors)
        {
            detector.Detect(aiData);
        }

        // 2. Reset interest and danger arrays
        for (int i = 0; i < 8; i++)
        {
            interest[i] = 0;
            danger[i] = 0;
        }

        // 3. Calculate interest and danger for each steering behavior
        foreach (var steering in steeringBehaviours)
        {
            (interest, danger) = steering.GetSteering(out var desirability, interest, danger, aiData);
        }
        
        //4. Combine interest and danger arrays, and get only directions where danger == 0
        _resultDirection = Vector2.zero;
        for (int i = 0; i < 8; i++)
        {
            if (danger[i] > 0)
            {
                continue;
            }
            _resultDirection += Directions.eightDirections[i] * interest[i];
        }
        _resultDirection.Normalize();

        //5. Move Agent
        GetComponent<EnemyBase>().Move(_resultDirection);
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        // Visualize interest and danger
        if (Application.isPlaying && interest != null && danger != null)
        {
            Gizmos.color = Color.green;
            for(int i = 0; i < 8; i++)
            {
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + Directions.eightDirections[i] * interest[i] * 2);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < 8; i++)
            {
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + Directions.eightDirections[i] * danger[i] * 2);
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _resultDirection * 2);
        }

    }
}