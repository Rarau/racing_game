﻿using UnityEngine;
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
            if (car.GetComponent<CarRaceLogic>().lastCheckpoint == thisCheckpoint - 1) {
                car.GetComponent<CarRaceLogic>().lastCheckpoint = thisCheckpoint;
            } else if (car.GetComponent<CarRaceLogic>().lastCheckpoint == 7 && thisCheckpoint == 0) {
                // The car has completed a lap.
                car.GetComponent<CarRaceLogic>().lap++;
                car.GetComponent<CarRaceLogic>().lastCheckpoint = thisCheckpoint;
            } else {
                Debug.Log("Turn around you are travelling in the wrong direction.");
            }
            Debug.Log(car.GetComponent<CarRaceLogic>().lastCheckpoint);
        }
    }
}
