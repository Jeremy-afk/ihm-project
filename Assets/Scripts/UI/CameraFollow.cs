using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minimumZoom = 2f;
    [SerializeField] private float maximumZoom = 8f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float moveFactor = 2f;

    private Camera mainCamera;
    private Transform primaryTarget;
    private Transform secondaryTarget;
    private bool following = false;
    private bool dynamicMode = false;
    private float baseSize;

    private void Start()
    {
        mainCamera = Camera.main;
        baseSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        if (following && primaryTarget)
        {
            Vector3 targetPosition;

            if (!dynamicMode)
                targetPosition = primaryTarget.position;
            else
                targetPosition = (primaryTarget.position + secondaryTarget.position) / 2;

            targetPosition.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveFactor);
        }

        if (dynamicMode)
        {
            // Modulate the orthographic size of the camera based on the distance between the two targets
            float distance = Vector3.Distance(primaryTarget.position, secondaryTarget.position);
            float size = distance * 0.5f;

            size = Mathf.Clamp(size, minimumZoom, maximumZoom);

            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, size, Time.deltaTime * zoomSpeed);
        }
    }

    public void ToogleFollow(bool activate)
    {
        following = activate;
    }

    public void SetPrimaryTarget(Transform transformTarget)
    {
        primaryTarget = transformTarget;
    }

    public void ToogleDynamicMode(bool activate, Transform secondaryTarget = null)
    {
        dynamicMode = activate;

        if (!activate)
            Camera.main.orthographicSize = baseSize;
        else
        {
            if (secondaryTarget != null)
            {
                this.secondaryTarget = secondaryTarget;
            }
            else if (this.secondaryTarget == null)
            {
                dynamicMode = false;
                Debug.LogError("Attempted to activate dynamic mode without a secondary target.");
            }
        }
    }
}
