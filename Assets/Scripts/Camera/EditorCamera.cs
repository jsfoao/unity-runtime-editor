using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    [SerializeField] private float PanSpeedKeys;
    [SerializeField] private float PanSpeedMouse;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float ScrollSpeed;
    
    private float xRotation;
    private float yRotation;
    private void Move(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void DoRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * RotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * RotationSpeed;
        
        xRotation += mouseY;
        yRotation -= mouseX;

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0f) * -1;
    }
    
    
    private void Update()
    {
        // Key Camera Movement
        if (Input.GetKey(KeyCode.W))
        {
            Move(transform.forward, PanSpeedKeys);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(-transform.right, PanSpeedKeys);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(-transform.forward, PanSpeedKeys);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(transform.right, PanSpeedKeys);
        }
        
        // Mouse Camera Pan
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 directionX = transform.right * -mouseX;
            Vector3 directionY = transform.up * -mouseY;
            
            Move(directionX + directionY, PanSpeedMouse);
        }
        
        // Camera Rotation
        if (Input.GetMouseButton(1))
        {
            DoRotation();
        }
        
        // Scroll movement
        Move(transform.forward * Input.mouseScrollDelta.y, ScrollSpeed);
    }
}
