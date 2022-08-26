using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float Sensitivity = 100f;

    float xRot;

    public bool canCamMoveOnX;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        // Inputs
        float mx = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

        xRot -= my;
        xRot = Mathf.Clamp(xRot, -90, 90);

        if (!canCamMoveOnX) { mx = 0;}

        // Rotation
        transform.localRotation = Quaternion.Euler(xRot,0,0);
        transform.parent.Rotate(Vector3.up*mx);
    }
}
