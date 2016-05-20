using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private int thisCheckpoint;

    void Start()
    {
        thisCheckpoint = int.Parse(name.Substring(10));
    }

    private void OnTriggerEnter(Collider carCollider)
    {
        if (carCollider.tag == "CheckpointCollider")
        {
            Transform car = carCollider.gameObject.transform.root;
            int lastCheckpoint = car.GetComponent<RaceInfo>().lastCheckpoint;

            // Enforce that the car travels in the correct direction.
            if (lastCheckpoint == thisCheckpoint - 1)
            {
                // Update the checkpoint.
                car.GetComponent<RaceInfo>().lastCheckpoint = thisCheckpoint;
                
                // Update UI.
            } 
            else if (lastCheckpoint == GameObject.Find("Checkpoints").transform.childCount - 1 && thisCheckpoint == 0)
            {
                // The car has completed a lap.
                car.GetComponent<RaceInfo>().lap++;
                car.GetComponent<RaceInfo>().lastCheckpoint = thisCheckpoint;

                car.GetComponent<RaceInfo>().SaveLapTime();

                // Update UI.
            } 
            else
            {
                // Travelling in the wrong direction.
                Debug.Log("Turn around you are travelling in the wrong direction.");
            }

            // For debugging.
            Debug.Log("Lap: " + car.GetComponent<RaceInfo>().lap + " " +
                      "Checkpoint: " + car.GetComponent<RaceInfo>().lastCheckpoint + " " +
                      "Time: " + car.GetComponent<RaceInfo>().lapTimer);
        }
    }
}