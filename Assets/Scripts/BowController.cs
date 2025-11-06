using UnityEngine;

public class BowController : MonoBehaviour
{
    public Transform playerCamera; // 玩家摄像机
    public Transform arrowSpawnPoint; // 箭的生成位置
    public GameObject arrowPrefab; // 箭的预制体
    public float arrowSpeed = 10f; // 箭的速度

    public float followSpeed = 5f; // Lerp 速度（控制弓旋转平滑度）
    public float positionFollowSpeed = 5f; // Lerp 速度（控制弓位置平滑度）
    public Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f); // 弓相对于摄像机的位置偏移量

    void Update()
    {
        FollowPlayerView();

        if (Input.GetMouseButtonDown(0)) // 按下左键射箭
        {
            ShootArrow();
        }
    }

    void FollowPlayerView()
    {
        // **让弓的位置平滑跟随摄像机**
        Vector3 targetPosition = playerCamera.position + playerCamera.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionFollowSpeed * Time.deltaTime);

        // **让弓的方向平滑跟随摄像机**
        Quaternion targetRotation = Quaternion.LookRotation(playerCamera.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }

    void ShootArrow()
    {
        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        Rigidbody arrowRb = arrowInstance.GetComponent<Rigidbody>();
        arrowRb.velocity = arrowSpawnPoint.forward * arrowSpeed;
    }
}
