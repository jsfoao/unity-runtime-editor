using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float PanSpeedKeys;
    [SerializeField] private float PanSpeedMouse;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float ScrollSpeed;
    
    private float xRotation;
    private float yRotation;

    public void MoveRight()
    {
        Move(transform.right, PanSpeedKeys);
    }
    public void MoveLeft()
    {
        Move(-transform.right, PanSpeedKeys);
    }
    public void MoveForward()
    {
        Move(transform.forward, PanSpeedKeys);
    }
    public void MoveBackward()
    {
        Move(-transform.forward, PanSpeedKeys);
    }

    public void DoMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Transform _transform = transform;
        Vector3 directionX = _transform.right * -mouseX;
        Vector3 directionY = _transform.up * -mouseY;
            
        Move(directionX + directionY, PanSpeedMouse);
    }
    
    private void Move(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void DoMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * RotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * RotationSpeed;
        
        xRotation += mouseY;
        yRotation -= mouseX;

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0f) * -1;
    }
    
    private void Update()
    {
        Move(transform.forward * Input.mouseScrollDelta.y, ScrollSpeed);
    }
}
