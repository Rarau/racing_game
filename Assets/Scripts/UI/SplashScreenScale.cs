using UnityEngine;
using System.Collections;

public class SplashScreenScale : MonoBehaviour
{
    SpriteRenderer splashScreen;

    // Use this for initialization.
    void Start()
    {
        splashScreen = GetComponent<SpriteRenderer>();
        ResizeToResolution();
    }

    // Resize the splash screen to fit the camera.
    private void ResizeToResolution()
    {
        Vector3 scale = new Vector3(1, 1, 1);

        float width = splashScreen.sprite.bounds.size.x;
        float height = splashScreen.sprite.bounds.size.y;

        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        scale.x = (float)worldScreenWidth / width;
        scale.y = (float)worldScreenHeight / height;

        transform.localScale = scale;
    }
}
