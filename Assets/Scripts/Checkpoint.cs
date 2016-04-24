using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    private int thisCheckpoint;

    void Start() {
        thisCheckpoint = int.Parse(name);
    }

	private void OnTriggerEnter(Collider carCollider) {
        if (carCollider.tag == "CheckpointCollider") {
            Transform car = carCollider.gameObject.transform.root;

            // Enforce that the car travels in the correct direction.
            if (car.GetComponent<CarRaceInfo>().lastCheckpoint == thisCheckpoint - 1) {
                car.GetComponent<CarRaceInfo>().lastCheckpoint = thisCheckpoint;

                // Quick and dirty way to update the UI counter - NEEDS REFACTORING.
                GameObject.Find("CheckpointCount").GetComponent<Text>().text = "" + car.GetComponent<CarRaceInfo>().lastCheckpoint;
            } else if (car.GetComponent<CarRaceInfo>().lastCheckpoint == 7 && thisCheckpoint == 0) {
                // The car has completed a lap.
                car.GetComponent<CarRaceInfo>().lap++;
                car.GetComponent<CarRaceInfo>().lastCheckpoint = thisCheckpoint;

                // Quick and dirty way to update the UI counters - NEEDS REFACTORING.
                GameObject.Find("LapCount").GetComponent<Text>().text = "" + car.GetComponent<CarRaceInfo>().lap;
                GameObject.Find("CheckpointCount").GetComponent<Text>().text = "" + car.GetComponent<CarRaceInfo>().lastCheckpoint;
            } else {
                Debug.Log("Turn around you are travelling in the wrong direction.");
            }
            Debug.Log(car.GetComponent<CarRaceInfo>().lastCheckpoint);
        }
    }
}
