using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Blade : MonoBehaviour
{
    public float cutVelocityThreshold = 5f;
    public Vector3 Velocity { get; private set; }

    private Camera cam;
    private Vector3 lastPos;
    private TrailRenderer trail;

    private void Awake()
    {
        cam = Camera.main;

        Vector3 centerScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 worldCenter = GetScreenPosition(centerScreen);
        transform.position = lastPos = worldCenter;

        trail = GetComponentInChildren<TrailRenderer>();
        if (trail) trail.enabled = false;
    }

    private void Update()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 world = GetScreenPosition(mouseScreen);

        Velocity = (world - lastPos) / Time.deltaTime;
        transform.position = world;
        lastPos = world;

        if (trail != null)
        {
            trail.enabled = Velocity.magnitude >= cutVelocityThreshold;
        }
    }

    /** TOD: CHANGE TO USE SCREEN CAPTURE **/
    private Vector3 GetScreenPosition(Vector3 screenPosition)
    {
        screenPosition.z = Mathf.Abs(cam.transform.position.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0; // lock to 2D plane
        return worldPos;
    }
}
