using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false; // 是否已经命中
    public float bounceForce = 5f;
    public float bounceMultiplier = 1f;
    public int bounceTimes = 2;
    private CapsuleCollider CapsuleCollider;
    public TrailerSource TrailerSource;

    public AudioClip colli;
    public AudioClip hitting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 让箭矢的方向始终等于其速度方向
        if (!hasHit && rb.velocity.sqrMagnitude > 0.01f) // 只有在箭矢未命中且速度不为0时调整方向
        {
            transform.forward = rb.velocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (!hasHit)
        {
            hasHit = true;
            rb.velocity = Vector3.zero; // 停止箭矢运动
            rb.isKinematic = true; // 让箭矢固定不动
            transform.parent = collision.transform; // 让箭附着在命中的物体上
        }*/

        if (collision.collider.CompareTag("Button") && bounceTimes == collision.collider.GetComponent<ButtonTrigger>().bounceTimes)
        {
            collision.collider.GetComponent<ButtonTrigger>().triggerTarget(); 
            hasHit = true;
            GetComponent<AudioSource>().clip = hitting;
            GetComponent<AudioSource>().Play();
            rb.isKinematic = true;
            return;
        }

        if (bounceTimes > 0 && !hasHit)
        {

            TrailerSource.changeColor(bounceTimes-1);

            // 获取碰撞点法线方向
            Vector3 normal = collision.contacts[0].normal;

            // 计算反射方向
            Vector3 bounceDirection = Vector3.Reflect(rb.velocity, normal);

            // 施加反弹力，增加向上的偏移
            bounceDirection += Vector3.up * bounceForce;

            // 应用新的速度
            rb.velocity = bounceDirection * bounceMultiplier;

            bounceTimes--;

            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();

        } else
        {
            //CapsuleCollider.enabled = false;
            rb.isKinematic = true;
            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();
        }
    }
}
