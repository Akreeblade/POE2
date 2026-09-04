using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedPath
{
    public Transform spawner;
    public Transform tower;

    public List<Vector3> waypoints = new List<Vector3>();
}