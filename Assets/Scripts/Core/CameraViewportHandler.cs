using ClumsyWizard.Utilities;
using UnityEngine;

public class CameraViewportHandler : Persistant<CameraViewportHandler>
{
    public Color wireColor = Color.white;
    [field: SerializeField] public float UnitsSize { get; private set; } // size of your scene in unity units

    public bool update = false;

    public static float Width;
    public static float Height;

    // helper points:
    public static Vector3 BottomLeft;
    public static Vector3 BottomCenter;
    public static Vector3 BottomRight;
    public static Vector3 MiddleLeft;
    public static Vector3 MiddleCenter;
    public static Vector3 MiddleRight;
    public static Vector3 TopLeft;
    public static Vector3 TopCenter;
    public static Vector3 TopRight;

    protected override void Awake()
    {
        base.Awake();
        ComputeResolution();
    }

    private void ComputeResolution()
    {
        float leftX, rightX, topY, bottomY;
        //CameraHelper.Camera.orthographicSize = UnitsSize / 2f;
        CameraHelper.Camera.orthographicSize = 1f / CameraHelper.Camera.aspect * UnitsSize / 2.0f;

        Height = 2f * CameraHelper.Camera.orthographicSize;
        Width = Height * CameraHelper.Camera.aspect;

        float cameraX, cameraY;
        cameraX = CameraHelper.Camera.transform.position.x;
        cameraY = CameraHelper.Camera.transform.position.y;

        leftX = cameraX - Width / 2;
        rightX = cameraX + Width / 2;
        topY = cameraY + Height / 2;
        bottomY = cameraY - Height / 2;

        //*** bottom
        BottomLeft = new Vector3(leftX, bottomY, 0);
        BottomCenter = new Vector3(cameraX, bottomY, 0);
        BottomRight = new Vector3(rightX, bottomY, 0);
        //*** middle
        MiddleLeft = new Vector3(leftX, cameraY, 0);
        MiddleCenter = new Vector3(cameraX, cameraY, 0);
        MiddleRight = new Vector3(rightX, cameraY, 0);
        //*** top
        TopLeft = new Vector3(leftX, topY, 0);
        TopCenter = new Vector3(cameraX, topY, 0);
        TopRight = new Vector3(rightX, topY, 0);
    }

    private void OnValidate()
    {
        if (update)
        {
            update = false;
            ComputeResolution();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = wireColor;

        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        if (CameraHelper.Camera.orthographic)
        {
            float spread = CameraHelper.Camera.farClipPlane - CameraHelper.Camera.nearClipPlane;
            float center = (CameraHelper.Camera.farClipPlane + CameraHelper.Camera.nearClipPlane) * 0.5f;
            Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(CameraHelper.Camera.orthographicSize * 2 * CameraHelper.Camera.aspect, CameraHelper.Camera.orthographicSize * 2, spread));
        }
        else
        {
            Gizmos.DrawFrustum(Vector3.zero, CameraHelper.Camera.fieldOfView, CameraHelper.Camera.farClipPlane, CameraHelper.Camera.nearClipPlane, CameraHelper.Camera.aspect);
        }
        Gizmos.matrix = temp;
    }

    protected override void CleanUp()
    {

    }
}