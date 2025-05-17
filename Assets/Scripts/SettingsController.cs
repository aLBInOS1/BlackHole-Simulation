using UnityEngine;

/// <summary>
/// Class for managing simulation and storing links
/// </summary>
public class SettingsController : MonoBehaviour
{
    [Header("Settings")]
    public float camWeight = 100;
    public bool gravityIsOn = false;

    [Header("Links")]
    public MathController mathController;
    public BlackHole blackHole;
    public Transform cam;

    private Vector3 startCamPos;
    private Quaternion startCamRot;

    void Start()
    {
        Application.targetFrameRate = 45;
        startCamPos = cam.position;
        startCamRot = cam.rotation;
    }

    public void OnGravityChanged(bool value)
    {
        gravityIsOn = value;
    }

    public void OnTimeScaleChanged(float value)
    {
        Time.timeScale = value;
    }

    public void OnResetCamPos()
    {
        cam.position = startCamPos;
        cam.rotation = startCamRot;
    }
} 
