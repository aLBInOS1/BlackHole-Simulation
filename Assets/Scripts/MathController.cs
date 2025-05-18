using UnityEngine;

/// <summary>
/// Class of mathematical and physical calculations for other scripts
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
    public const float G = 6.67430e-11f;
    public const float c = 299792458f;
    public const float M = 1e30f;

    private void Start()
    {
        schwarzschildRadius = (2 * G * M) / (c * c);
    }

    void Update()
    {
        Vector3 blackHolePos = settingsController.blackHole.transform.position;
        Vector3 camPos = settingsController.cam.position;
        Vector3 direction = blackHolePos - camPos;
        distance = direction.magnitude;
        distance = Mathf.Max(0.1f, distance);

        distanceBySchwarzschildRadius = distance / schwarzschildRadius;

        //float forceMagnitude = G * (M * settingsController.camWeight) / (predictedDistance * predictedDistance);
        //Vector3 acceleration = direction * (forceMagnitude / settingsController.camWeight);
        Vector3 acceleration = direction.normalized * (G * M / (distance * distance));
        gravityAcceleration = acceleration.magnitude;

        if (distance <= schwarzschildRadius)
        {
            // Stopping gravity
            // on Schwarzschild radius (also Event horizon for for a non-rotating (static) Schwarzschild black hole)
            gravityVelocityScaled = Vector3.zero;
            gravityVelocity = Vector3.zero;
            isInEventHorizon = true;
            //Debug.Log("В радиусе Шварцшильда");
        }
        else
        {
            // Calculation gravity
            isInEventHorizon = false;

            if (settingsController.gravityIsOn)
            {
                // Calculation velocity
                direction.Normalize();

                gravityVelocity += acceleration * Time.deltaTime;
                gravityVelocityScaled = gravityVelocity * Time.deltaTime;

                #region Checking the horizon crossing along the trajectory
                Vector3 nextCamPos = camPos + gravityVelocityScaled;
                Vector3 intersectionPoint;
                if (CheckSegmentSphereIntersection(camPos, nextCamPos, blackHolePos, schwarzschildRadius, out intersectionPoint))
                {
                    // Adjusting the speed and movement to the intersection point
                    gravityVelocityScaled = intersectionPoint - camPos;
                    gravityVelocity = Vector3.zero;
                    //Debug.Log("Пересек Радиус Шварцшильда");
                }
                #endregion
            }
        }
    }

    /// <summary>
    /// When flying through the Schwarzschild radius,
    /// the distance check may not work (We can fly over the entire black hole in one frame),
    /// so we additionally check it here
    /// </summary>
    private bool CheckSegmentSphereIntersection(Vector3 segmentStart, Vector3 segmentEnd, Vector3 sphereCenter, float sphereRadius, out Vector3 intersectionPoint)
    {
        intersectionPoint = Vector3.zero;
        Vector3 segmentDir = segmentEnd - segmentStart;
        float segmentLength = segmentDir.magnitude;
        if (segmentLength <= 0) return false;

        Vector3 dirNormalized = segmentDir.normalized;
        Vector3 oc = segmentStart - sphereCenter;
        float a = 1.0f; // Vector3.Dot(dirNormalized, dirNormalized) = 1 always
        float b = 2f * Vector3.Dot(oc, dirNormalized);
        float c = Vector3.Dot(oc, oc) - sphereRadius * sphereRadius;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0) return false;

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float twoA = 2 * a;
        float t1 = (-b - sqrtDiscriminant) / (twoA);
        float t2 = (-b + sqrtDiscriminant) / (twoA);

        float t = Mathf.Min(t1, t2);
        if (t < 0) t = Mathf.Max(t1, t2);
        if (t < 0 || t > segmentLength) return false;

        intersectionPoint = segmentStart + dirNormalized * t;
        return true;
    }
}