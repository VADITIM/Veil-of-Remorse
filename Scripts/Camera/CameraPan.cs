using UnityEngine;
using Cinemachine;
using UnityEngine.U2D;

public class CameraPan : CameraLogic
{
    public Transform player;
    public float panSpeed = 5f;
    public Vector2 panLimits = new Vector2(2f, 2f);

    private CinemachineFramingTransposer framingTransposer;
    private Vector3 screenCenter;

    public override void Initialize(CinemachineVirtualCamera camera, PixelPerfectCamera pixelCamera)
    {
        base.Initialize(camera, pixelCamera);
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Awake()
    {
        PixelPerfectCamera pixelCamera = FindObjectOfType<PixelPerfectCamera>();
        Initialize(virtualCamera, pixelCamera);

        // Initialize screenCenter based on the screen resolution
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }

    void Update()
    {
        if (player == null || framingTransposer == null || pixelPerfectCamera == null) return;

        if (FindObjectOfType<DivinePoint>()?.isPaused == true)
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.zero;
            return;
        }

        // Get mouse position in screen space
        Vector3 mouseScreenPos = Input.mousePosition;

        // Adjust screenCenter dynamically in case resolution changes (optional)
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // Calculate mouse offset relative to screen center
        Vector3 mouseOffset = (mouseScreenPos - screenCenter);

        // Normalize the offset based on screen size to make it resolution-independent
        Vector2 normalizedOffset = new Vector2(
            mouseOffset.x / (Screen.width / 2f),
            mouseOffset.y / (Screen.height / 2f)
        );

        // Apply pan limits and calculate target offset
        Vector3 targetOffset = new Vector3(
            Mathf.Clamp(normalizedOffset.x * panLimits.x, -panLimits.x, panLimits.x),
            Mathf.Clamp(normalizedOffset.y * panLimits.y, -panLimits.y, panLimits.y),
            0
        );

        // Smoothly adjust the framing offset
        framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
            framingTransposer.m_TrackedObjectOffset,
            targetOffset,
            panSpeed * Time.deltaTime
        );
    }
}