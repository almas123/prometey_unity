using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -5);
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lookAtTarget = true;
    
    [Header("Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minZ = -50f;
    [SerializeField] private float maxZ = 50f;
    
    private void Start()
    {
        // If target not set, find player via utility (DRY principle)
        if (target == null)
        {
            target = GameObjectFinder.FindPlayerTransform();
        }

        // Set initial camera position
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate desired camera position
        Vector3 desiredPosition = target.position + offset;

        // Apply bounds if enabled
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);
        }

        // Smoothly move camera to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Rotate camera towards target if enabled
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }

    // Method to change target during gameplay
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Method to change camera offset
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
