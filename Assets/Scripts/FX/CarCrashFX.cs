using UnityEngine;
using System.Collections;

public class CarCrashFX : MonoBehaviour
{
    CarController carController;
    ParticleSystem particles;

    public int numParticles = 25;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        carController = transform.root.GetComponent<CarController>();
    }

	void OnEnable () 
    {
        carController.crashEvent += OnCarCrash;
	}

    void OnCarCrash(Collision other)
    {
        if (carController.currentGear>2)
        {
            particles.Emit(numParticles);
            gameObject.SetActive(true);
            transform.position = other.contacts[0].point;
        }

    }

    void OnDisable()
    {
        // carController.crashEvent -= OnCarCrash;
    }
}
