using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController _playerController;

    [Range(1, 50)]
    public float sens;

    public Transform body;
    public float maxRangeX = 60,
        minRangeX = 60,
        xRot = 0f;

    public bool includeCurseLocked, lockRotate = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraMovement();
    }


    void CameraMovement()
    {
        if (_playerController.canMove)
        {
            float rotX = _playerController.inputController.mouseLookV2Direction.x * sens * Time.deltaTime;
            float rotY = _playerController.inputController.mouseLookV2Direction.y * sens * Time.deltaTime;

            xRot -= rotY;
            xRot = Mathf.Clamp(xRot, -minRangeX, maxRangeX);

            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            body.Rotate(Vector3.up * rotX);
        }
    }
}
