using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedSpikes : MonoBehaviour
{
    public Transform pointA; // Startpunkt
    public Transform pointB; // Endpunkt
    public Transform platform; // Die Plattform, die sich bewegt

    public float speed = 1.5f; // Geschwindigkeit der Bewegung
    private float journeyLength;
    private float startTime;
    private bool isMovingToB = true;

    private void Start()
    {
        // Berechne die Länge des Weges
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
        startTime = Time.time;
    }

    private void Update()
    {
        // Berechne den Fortschritt basierend auf der Zeit
        float distanceCovered = (Time.time - startTime) * speed;
        float journeyFraction = distanceCovered / journeyLength;

        // Berechne den sanften Übergang
        float smoothStepValue = Mathf.SmoothStep(0f, 1f, journeyFraction);

        if (isMovingToB)
        {
            // Bewege die Plattform von Punkt A zu Punkt B
            platform.position = Vector3.Lerp(pointA.position, pointB.position, smoothStepValue);
        }
        else
        {
            // Bewege die Plattform von Punkt B zu Punkt A
            platform.position = Vector3.Lerp(pointB.position, pointA.position, smoothStepValue);
        }

        // Wenn die Plattform das Ende erreicht hat, setze die Startzeit zurück und tausche die Richtung
        if (distanceCovered >= journeyLength)
        {
            startTime = Time.time;
            isMovingToB = !isMovingToB;

            // Berechne die neue Weglänge basierend auf der aktuellen Richtung
            if (isMovingToB)
            {
                journeyLength = Vector3.Distance(pointA.position, pointB.position);
            }
            else
            {
                journeyLength = Vector3.Distance(pointB.position, pointA.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Nur für Debug-Zwecke
        if (platform != null && pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.1f);
            Gizmos.DrawWireSphere(pointB.position, 0.1f);
        }
    }
}
