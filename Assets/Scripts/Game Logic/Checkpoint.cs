using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private int thisCheckpoint;
    private GameManager gm;

    void Start()
    {
        thisCheckpoint = int.Parse(name.Substring(10));
        GameObject gmObject = GameObject.Find("GameManager");

        // Disable this script if no GameManager in scene.
        if (gmObject == null)
        {
            return;
        }
        gm = gmObject.GetComponent<GameManager>();
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
            } else if (lastCheckpoint == GameObject.Find("Checkpoints").transform.childCount - 1 && thisCheckpoint == 0)
            {
                // The car has completed a lap.
                car.GetComponent<RaceInfo>().lap++;
                car.GetComponent<RaceInfo>().lastCheckpoint = thisCheckpoint;
                car.GetComponent<RaceInfo>().SaveLapTime();

                // Update UI.
            } else
            {
                // Travelling in the wrong direction.
                Debug.Log("Turn around you are travelling in the wrong direction.");
            }

            // For debugging.
            Debug.Log("Lap: " + car.GetComponent<RaceInfo>().lap + " " +
                      "Checkpoint: " + car.GetComponent<RaceInfo>().lastCheckpoint + " " +
                      "Time: " + car.GetComponent<RaceInfo>().lapTimer);

            // Check if the car has finished the race.
            if (thisCheckpoint == 0 && car.GetComponent<RaceInfo>().lap == gm.numberOfLaps + 1)
            {
                // Car has finished the race, add it to the finishing order.
                gm.finalPositions.Add(car.gameObject);
            }
        }
    }
}