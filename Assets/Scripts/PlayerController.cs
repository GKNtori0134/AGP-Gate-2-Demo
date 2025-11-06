using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        // 锁定鼠标光标，使其不可见并固定在屏幕中心
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");  // A, D 左右移动
        float moveZ = Input.GetAxis("Vertical");    // W, S 前后移动

        // 计算移动方向
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection *= moveSpeed;

        // 让 Rigidbody 处理物理移动
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 旋转玩家水平视角
        transform.Rotate(Vector3.up * mouseX);

        // 旋转摄像机垂直视角
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // 限制上下视角范围
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
