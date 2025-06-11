using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Transform cam;
    Vector2 camStartPos;
    float distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 0.6f)]
    public float parallaxSpeed;

    // Width of each background element, assumed to be the same for all elements
    public float backgroundWidth;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int BackCount = transform.childCount;
        mat = new Material[BackCount];
        backSpeed = new float[BackCount];
        backgrounds = new GameObject[BackCount];

        for (int i = 0; i < BackCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        backSpeedCalculate(BackCount);
    }

    void backSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            float backgroundZ = backgrounds[i].transform.position.z - cam.position.z;
            if (backgroundZ > farthestBack)
            {
                farthestBack = backgroundZ;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            float backgroundZ = backgrounds[i].transform.position.z - cam.position.z;
            backSpeed[i] = 1 - (backgroundZ / farthestBack);
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance * speed, 0));
        }

        // Generate new background if needed
        GenerateNewBackgrounds();
    }

    void GenerateNewBackgrounds()
    {
        float camLeftEdge = cam.position.x - (Camera.main.orthographicSize * Camera.main.aspect);
        float camRightEdge = cam.position.x + (Camera.main.orthographicSize * Camera.main.aspect);

        foreach (GameObject background in backgrounds)
        {
            float backgroundRightEdge = background.transform.position.x + backgroundWidth / 2;
            float backgroundLeftEdge = background.transform.position.x - backgroundWidth / 2;

            // Check if the background is completely out of the left side of the camera's view
            if (backgroundRightEdge < camLeftEdge)
            {
                // Find the rightmost background
                GameObject rightmostBackground = backgrounds[0];
                foreach (GameObject bg in backgrounds)
                {
                    if (bg.transform.position.x > rightmostBackground.transform.position.x)
                    {
                        rightmostBackground = bg;
                    }
                }

                // Move the current background to the rightmost position plus its width
                background.transform.position = new Vector3(
                    rightmostBackground.transform.position.x + backgroundWidth,
                    background.transform.position.y,
                    background.transform.position.z
                );
            }
        }
    }
}


