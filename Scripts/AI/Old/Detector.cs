using UnityEngine;
using System.Collections.Generic;

public abstract class Detector : MonoBehaviour
{
    public abstract void Detect(AIData aiData);
}