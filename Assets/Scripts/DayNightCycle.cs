using System;
using UnityEditor.Rendering;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public ParticleSystem clouds; // Reference to the particle system for clouds

    [Range(0, 24)]
    public float timeOfDay = 12f; // Current time in hours (0 = midnight, 12 = noon)
    public float dayDuration = 60f; // Duration of a full day in seconds

    public Boolean isDay = true; // Flag to check if it's day or night
    private float sunInitialIntensity;
    private ParticleSystem.MainModule cloudsMain; // Reference to the particle system main module
    void Start()
    {
        sunInitialIntensity = sun.intensity;
        cloudsMain = clouds.main; // Get the main module of the particle system
        cloudsMain.startColor = new Color(0.02f, 0.02f, 0.2f); // Set initial color for clouds
    }

    void Update()
    {
        // Advance time
        timeOfDay += 24 / dayDuration * Time.deltaTime;
        if (timeOfDay >= 24f) timeOfDay = 0f;

        // Update sun and clouds
        SunUpdate();
        CloudUpdate();

        // Set isDay flag based on timeOfDay
        if (timeOfDay >= 5f && timeOfDay < 17f)
        {
            isDay = true; // Daytime
        }
        else
        {
            isDay = false; // Nighttime
        }
    }

    void SunUpdate()
    {
        // Rotate sun (0 at midnight, 180 at noon, 360 = next midnight)
        float sunAngle = timeOfDay / 24f * 360f;
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunAngle - 90f, 2f, 0f));

        // Adjust sun intensity for day/night
        if (timeOfDay >= 6f && timeOfDay < 18f) // Daytime
        {
            sun.intensity = Mathf.Lerp(0, sunInitialIntensity, (timeOfDay - 6f) / 12f); // Increase intensity
            sun.color = Color.white; // White light during the day
        }
        else // Nighttime
        {
            float nightTimeFactor = Mathf.Clamp01((timeOfDay - 18f) / 6f); // Steeper fade factor for night
            sun.intensity = Mathf.Lerp(sunInitialIntensity * 0.1f, 0f, nightTimeFactor); // More reduction at night
            sun.color = new Color(0.2f, 0.2f, 0.5f); // Blueish light at night
        }
    }
    void CloudUpdate()
    {
        // Change the cloud color based on time of day
        if (isDay)
        {
            cloudsMain.startColor = Color.white; // White color during the day
        }
        else
        {
            cloudsMain.startColor = new Color(0.2f, 0.1f, 0.2f); // Darker color at night
        }
        
    }

}

