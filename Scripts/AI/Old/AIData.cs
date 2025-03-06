using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AIData", menuName = "AI/AIData")]
public class AIData : ScriptableObject
{
    public float detectionRadius = 5f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public Transform target;
    public List<Transform> obstacles;

    //This function is called when the scriptable object is loaded.
    public void OnEnable()
    {
        obstacles = new List<Transform>();
    }
}