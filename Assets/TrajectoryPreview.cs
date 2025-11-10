using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPreview : MonoBehaviour
{
    [Header("Arrow Settings")]
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10f;

    [Header("Trail Settings")]
    public TrailRenderer trailSegmentPrefab;
    public int previewPoints = 1000;       // Increased for smoother long curve
    public float timeStep = 0.05f;

    [Header("Tip Arrow")]
    public float tipArrowLength = 0.5f;
    public Material tipMaterial;
    public float tipWidth = 0.05f;

    [Header("Bounce Settings")]
    public float bounceForce = 5f;
    public float bounceMultiplier = 1f;

    private List<TrailRenderer> segmentTrails = new List<TrailRenderer>();
    private GameObject tipObject;

    void Update()
    {
        if (!arrowSpawnPoint || !trailSegmentPrefab) return;

        // Clean up old visuals
        foreach (var seg in segmentTrails)
            if (seg) Destroy(seg.gameObject);
        segmentTrails.Clear();

        if (tipObject)
        {
            Destroy(tipObject);
            tipObject = null;
        }

        Vector3 startPos = arrowSpawnPoint.position;
        Vector3 velocity = arrowSpawnPoint.forward * arrowSpeed;

        Vector3 prevPoint = startPos;
        Vector3 lastDir = velocity.normalized;
        bool hasBounced = false;

        for (int i = 1; i < previewPoints; i++)
        {
            // Calculate next position using gravity
            float t = timeStep * i;
            Vector3 nextPos = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
            Vector3 dir = nextPos - prevPoint;
            float dist = dir.magnitude;
            if (dist <= Mathf.Epsilon) break;

            // Check collision between prev and next point
            if (Physics.Raycast(prevPoint, dir.normalized, out RaycastHit hit, dist))
            {
                // Draw up to collision
                CreateSegment(prevPoint, hit.point);

                // Compute bounce direction
                Vector3 bounceVel = Vector3.Reflect(velocity + Physics.gravity * t, hit.normal);
                bounceVel += Vector3.up * bounceForce;
                bounceVel *= bounceMultiplier;

                // Short indicator line only
                DrawTip(hit.point, bounceVel.normalized);

                hasBounced = true;
                break;
            }

            // No hit ¡ú continue drawing
            CreateSegment(prevPoint, nextPos);
            lastDir = dir.normalized;
            prevPoint = nextPos;
        }

        // If no bounce happened, draw tip at end
        if (!hasBounced)
            DrawTip(prevPoint, lastDir);
    }

    void CreateSegment(Vector3 start, Vector3 end)
    {
        TrailRenderer seg = Instantiate(trailSegmentPrefab);
        seg.transform.position = start;
        seg.Clear();
        seg.AddPosition(start);
        seg.AddPosition(end);
        segmentTrails.Add(seg);
    }

    void DrawTip(Vector3 start, Vector3 dir)
    {
        tipObject = new GameObject("TrajectoryTip");
        LineRenderer lr = tipObject.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, start + dir * tipArrowLength);
        lr.widthMultiplier = tipWidth;
        lr.numCapVertices = 2;
        lr.material = tipMaterial ? tipMaterial : new Material(Shader.Find("Sprites/Default"));
    }
}
