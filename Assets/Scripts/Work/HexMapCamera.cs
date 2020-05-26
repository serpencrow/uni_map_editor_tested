using UnityEngine;

public class HexMapCamera : MonoBehaviour
{
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom, moveSpeedMaxZoom;
    public float rotationSpeed;
    public Transform swivel, stick;
    public HexGrid grid;
    public float zoomDeltaP = 0f;
    public float rotationDeltaP = 0f;
    public float xDeltaP = 0f;
    public float zDeltaP = 0f;
    float zoom = 1f;
    float rotationAngle;
    static HexMapCamera instance;
    public static bool Locked
    {
        set
        {
            instance.enabled = !value;
        }
        get
        {
            return !instance.enabled;
        }
    }
    public static void ValidatePosition()
    {
        instance.AdjustPosition(0f, 0f);
    }
    void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }
    void OnEnable()
    {
        instance = this;
    }
    public void Zooming()
    {
        if (zoomDeltaP == 0)
            zoomDeltaP = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDeltaP != 0f)
        {
            AdjustZoom(zoomDeltaP);
            zoomDeltaP = 0;
        }
    }
    public void Moving()
    {
        if (xDeltaP == 0)
            xDeltaP = Input.GetAxis("Horizontal");
        if (zDeltaP == 0)
            zDeltaP = Input.GetAxis("Vertical");
        if (xDeltaP != 0f || zDeltaP != 0f)
        {
            AdjustPosition(xDeltaP, zDeltaP);
            xDeltaP = 0;
            zDeltaP = 0;
        }
    }
    public void Rotation()
    {
        if (rotationDeltaP == 0)
            rotationDeltaP = Input.GetAxis("Rotation");
        if (rotationDeltaP != 0f)
        {
            AdjustRotation(rotationDeltaP);
            rotationDeltaP = 0;
        }
    }
    void Update()
    {
        Zooming();
        Moving();
        Rotation();
    }
    void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);
        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);
        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
    void AdjustRotation(float delta)
    {
        rotationAngle += delta * rotationSpeed * Time.deltaTime;
        if (rotationAngle < 0f)
        {
            rotationAngle += 360f;
        }
        else if (rotationAngle >= 360f)
        {
            rotationAngle -= 360f;
        }
        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }
    void AdjustPosition(float xDelta, float zDelta)
    {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * damping * Time.deltaTime;
        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }
    Vector3 ClampPosition(Vector3 position)
    {
        float xMax = (grid.cellCountX - 0.5f) * (2f * HexMetrics.innerRadius);
        position.x = Mathf.Clamp(position.x, 0f, xMax);
        float zMax = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius);
        position.z = Mathf.Clamp(position.z, 0f, zMax);
        return position;
    }
}
