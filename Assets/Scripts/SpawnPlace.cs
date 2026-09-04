using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlace : MonoBehaviour
{
    private bool occupied = false;

    public void SetOccupied(bool occupancy)
    {
        occupied = occupancy;
    }

    public bool IsOccupied()
    {
        return occupied;
    }
}
