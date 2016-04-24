using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    private int thisCheckpoint;

    void Start() {
        thisCheckpoint = int.Parse(name);
    }

	private void OnTriggerEnter(Collider carCollider) {
        if (carCollider.tag == "CheckpointCollider") {
            Transform car = carCollider.gameObject.transform.root;
            if (car.GetComponent<CarRaceLogic>().lastCheckpoint == thisCheckpoint - 1) {
                car.GetComponent<CarRaceLogic>().lastCheckpoint = thisCheckpoint;
            } else {
                Debug.Log("Turn around you are travelling in the wrong direction.");
            }
            Debug.Log(car.GetComponent<CarRaceLogic>().lastCheckpoint);
        }
    }
}
