using UnityEngine;

/// <summary>
/// Class of mathematical and physical calculations for other scripts.
/// Simulates a static Black Hole using the Paczynski-Wiita potential.
/// </summary>
public class MathController : MonoBehaviour
{
    [SerializeField] private SettingsController settingsController;

    [Header("Calculations")]
    public bool isInEventHorizon;
    public float distance;
    public float schwarzschildRadius;
    public float distanceBySchwarzschildRadius;
    public float gravityAcceleration;
    public Vector3 gravityVelocity;
    public Vector3 gravityVelocityScaled;

    [Header("Static vars")]
    // G: Gravitational constant (m^3 kg^-1 s^-2)
    public const float G = 6.67430e-11f;
    // c: Speed of light (m/s)
    public const float c = 299792458f;
    // M: Mass of the Black Hole (kg). 1e30 is approx 0.5 Solar Masses.
    public const float M = 1e30f;

    private void Start()
    {
        // Calculate the Event Horizon radius (Schwarzschild radius)
        // Formula: Rs = 2GM / c^2
        schwarzschildRadius = (2 * G * M) / (c * c);
    }

    void Update()
    {
        Vector3 blackHolePos = settingsController.blackHole.transform.position;
        Vector3 camPos = settingsController.cam.position;

        // 1. Calculate vector and distance to the center
        Vector3 direction = blackHolePos - camPos;
        distance = direction.magnitude;

        // Clamp distance to avoid division by zero at the exact center
        distance = Mathf.Max(0.1f, distance);

        distanceBySchwarzschildRadius = distance / schwarzschildRadius;

        // 2. Physics Calculation: Paczynski-Wiita Potential
        // Unlike Newton's gravity (1/r^2), real Black Holes have an unstable orbit region.
        // We simulate this by calculating gravity based on the distance to the Horizon (Rs).
        // Formula: Gravity ~ 1 / (r - Rs)^2

        float r_g = schwarzschildRadius;
        float pseudoDistance = distance - r_g;

        // Clamp to prevent infinite gravity if we touch the horizon
        float minSafetyDistance = schwarzschildRadius * 0.05f;
        pseudoDistance = Mathf.Max(minSafetyDistance, pseudoDistance);

        // Calculate acceleration vector
        // direction.normalized gives us the direction with length 1
        Vector3 acceleration = direction.normalized * (G * M / (pseudoDistance * pseudoDistance));
        gravityAcceleration = acceleration.magnitude;

        // 3. Logic
        if (distance <= schwarzschildRadius)
        {
            // Inside the Event Horizon
            gravityVelocityScaled = Vector3.zero;
            gravityVelocity = Vector3.zero;
            isInEventHorizon = true;
        }
        else
        {
            // Outside the Horizon
            isInEventHorizon = false;

            if (settingsController.gravityIsOn)
            {
                // Integrate acceleration to get velocity (Euler method)
                gravityVelocity += acceleration * Time.deltaTime;
                gravityVelocityScaled = gravityVelocity * Time.deltaTime;

                #region Checking the horizon crossing along the trajectory
                // We calculate where the camera WILL be in the next frame
                Vector3 nextCamPos = camPos + gravityVelocityScaled;
                Vector3 intersectionPoint;

                // Check if we hit the sphere between current position and next position
                if (CheckSegmentSphereIntersection(camPos, nextCamPos, blackHolePos, schwarzschildRadius, out intersectionPoint))
                {
                    // If we hit the horizon, snap to the intersection point and stop
                    gravityVelocityScaled = intersectionPoint - camPos;
                    gravityVelocity = Vector3.zero;
                }
                #endregion
            }
        }
    }

    /// <summary>
    /// Checks if a line segment (movement) intersects a sphere (Event Horizon).
    /// Uses the Quadratic Formula (Discriminant) from school algebra!
    /// </summary>
    private bool CheckSegmentSphereIntersection(Vector3 segmentStart, Vector3 segmentEnd, Vector3 sphereCenter, float sphereRadius, out Vector3 intersectionPoint)
    {
        intersectionPoint = Vector3.zero;
        Vector3 segmentDir = segmentEnd - segmentStart;
        float segmentLength = segmentDir.magnitude;

        if (segmentLength <= 0) return false;

        Vector3 dirNormalized = segmentDir.normalized;
        Vector3 oc = segmentStart - sphereCenter;

        // We are solving the quadratic equation: at^2 + bt + c = 0
        // where 't' is the distance along the ray.

        float a = 1.0f; // Because direction is normalized (dot product of itself is 1)
        float b = 2f * Vector3.Dot(oc, dirNormalized);
        float c = Vector3.Dot(oc, oc) - sphereRadius * sphereRadius;

        // Calculate Discriminant: D = b^2 - 4ac
        float discriminant = b * b - 4 * a * c;

        // If D < 0, there are no real roots (no intersection)
        if (discriminant < 0) return false;

        // Calculate roots using the quadratic formula: t = (-b +/- sqrt(D)) / 2a
        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float twoA = 2 * a;
        float t1 = (-b - sqrtDiscriminant) / twoA;
        float t2 = (-b + sqrtDiscriminant) / twoA;

        // We need the closest valid intersection (t must be between 0 and segmentLength)
        float t = Mathf.Min(t1, t2);
        if (t < 0) t = Mathf.Max(t1, t2);

        if (t < 0 || t > segmentLength) return false;

        intersectionPoint = segmentStart + dirNormalized * t;
        return true;
    }
}