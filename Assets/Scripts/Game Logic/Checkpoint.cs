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
                car.GetComponent<RaceInfo>().lastCheckpoint = thisCheckpoint;

                // Quick and dirty way to update the UI counter - NEEDS REFACTORING.
                //GameObject.Find("CheckpointCount").GetComponent<Text>().text = "" + car.GetComponent<RaceInfo>().lastCheckpoint;
            } 
            else if (lastCheckpoint == GameObject.Find("Checkpoints").transform.childCount - 1 && thisCheckpoint == 0)
            {
                // The car has completed a lap.
                car.GetComponent<RaceInfo>().lap++;
                car.GetComponent<RaceInfo>().lastCheckpoint = thisCheckpoint;

                // Quick and dirty way to update the UI counters - NEEDS REFACTORING.
                //GameObject.Find("LapCount").GetComponent<Text>().text = "" + car.GetComponent<RaceInfo>().lap;
                //GameObject.Find("CheckpointCount").GetComponent<Text>().text = "" + car.GetComponent<RaceInfo>().lastCheckpoint;
            } 
            else
            {
                Debug.Log("Turn around you are travelling in the wrong direction.");
            }
            Debug.Log(car.GetComponent<RaceInfo>().lastCheckpoint);
        }
    }
}