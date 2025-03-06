using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player; // Reference to the player

    public float panSpeed = 3f;  // Speed of camera panning
    public Vector2 panLimits = new Vector2(3f, 2f); // Maximum panning range

    private CinemachineFramingTransposer framingTransposer;
    private Vector3 originalOffset;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Cinemachine Virtual Camera is not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player is not assigned!");
            return;
        }

        // Get the Framing Transposer component
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (framingTransposer == null)
        {
            Debug.LogError("Cinemachine Framing Transposer not found! Make sure your virtual camera's 'Body' is set to 'Framing Transposer'.");
            return;
        }

        originalOffset = framingTransposer.m_TrackedObjectOffset;
    }

    void Update()
    {
        if (framingTransposer == null || player == null) return;

        // Get mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ensure we ignore Z depth

        // Get offset from player to mouse position
        Vector3 mouseOffset = mouseWorldPos - player.position;

        // Normalize the offset to keep panning within limits
        Vector3 targetOffset = new Vector3(
            Mathf.Clamp(mouseOffset.x, -panLimits.x, panLimits.x),
            Mathf.Clamp(mouseOffset.y, -panLimits.y, panLimits.y),
            0
        );

        // Smoothly move the camera offset
        framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
            framingTransposer.m_TrackedObjectOffset, targetOffset, panSpeed * Time.deltaTime
        );
    }
}
