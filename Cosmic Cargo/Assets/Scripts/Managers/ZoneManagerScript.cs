using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagerScript : MonoBehaviour
{
    private HashSet<GameObject> shipAttackers = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D collider)
    {
    if (collider.gameObject.GetComponent<StationAttackerScript>() != null)
        {
            shipAttackers.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
    if (collider.gameObject.GetComponent<StationAttackerScript>() != null)
        {
            shipAttackers.Remove(collider.gameObject);
        }
    }

    void Update()
    {
        // Remove any destroyed objects from the HashSet
        Debug.Log(shipAttackers.Count);
        shipAttackers.RemoveWhere(obj => obj == null);
    }

    public int GetShipsInZone()
    {
        return shipAttackers.Count;
    }

    public float GetShipsInZonePercent()
    {
        return Mathf.Clamp01((float)shipAttackers.Count / 10); // Normalized 0-1
    }
}
