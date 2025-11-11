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

    // 💥 三种不同的弹跳特效
    [Header("Bounce VFX Prefabs (from first to last bounce)")]
    public GameObject[] bounceVFXPrefabs; // 0 = first bounce, 1 = second bounce, 2 = third bounce

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 让箭矢的方向始终等于其速度方向
        if (!hasHit && rb.velocity.sqrMagnitude > 0.01f)
        {
            transform.forward = rb.velocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 命中按钮的特殊逻辑
        if (collision.collider.CompareTag("Button") && bounceTimes == collision.collider.GetComponent<ButtonTrigger>().bounceTimes)
        {
            collision.collider.GetComponent<ButtonTrigger>().triggerTarget();
            hasHit = true;
            GetComponent<AudioSource>().clip = hitting;
            GetComponent<AudioSource>().Play();
            rb.isKinematic = true;
            return;
        }

        // 普通反弹逻辑
        if (bounceTimes > 0 && !hasHit)
        {
            TrailerSource.changeColor(bounceTimes - 1);

            // 获取碰撞点法线方向
            Vector3 normal = collision.contacts[0].normal;

            // 计算反射方向
            Vector3 bounceDirection = Vector3.Reflect(rb.velocity, normal);

            // 施加反弹力，增加向上的偏移
            bounceDirection += Vector3.up * bounceForce;

            // 应用新的速度
            rb.velocity = bounceDirection * bounceMultiplier;

            // 播放音效
            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();

            // 💥 播放对应该弹次数的特效
            int vfxIndex = Mathf.Clamp(bounceTimes - 1, 0, bounceVFXPrefabs.Length - 1);
            SpawnBounceVFX(collision.contacts[0].point, normal, vfxIndex);

            bounceTimes--;
        }
        else
        {
            // 最后一次碰撞
            rb.isKinematic = true;
            GetComponent<AudioSource>().clip = colli;
            GetComponent<AudioSource>().time = 0.2f;
            GetComponent<AudioSource>().Play();

            // 💥 最后一击的小特效
            SpawnBounceVFX(collision.contacts[0].point, collision.contacts[0].normal, bounceVFXPrefabs.Length - 1);
        }
    }

    // 💨 生成特定弹跳特效
    void SpawnBounceVFX(Vector3 position, Vector3 normal, int index)
    {
        if (bounceVFXPrefabs != null && index >= 0 && index < bounceVFXPrefabs.Length && bounceVFXPrefabs[index] != null)
        {
            GameObject vfx = Instantiate(bounceVFXPrefabs[index], position, Quaternion.LookRotation(normal));

            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(vfx, 2f);
            }
        }
    }
}
