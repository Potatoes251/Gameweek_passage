using UnityEngine;

public class LeashPull : MonoBehaviour
{
    [SerializeField]
    public Transform target;      // the thing being pulled (e.g. dog/object)
    private LineRenderer lineRenderer;
    public float leashLength = 3f;
    public float pullForce = 10f;

    public int segmentCount = 10;

    private Vector3[] segmentPositions;
    private Vector3[] segmentVelocities;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;

        segmentPositions = new Vector3[segmentCount];
        segmentVelocities = new Vector3[segmentCount];

        for (int i = 0; i < segmentCount; i++)
            segmentPositions[i] = Vector3.Lerp(transform.position, target.position, (float)i / (segmentCount - 1));
    }

    void FixedUpdate()
    {
        for (int i = 1; i < segmentCount - 1; i++)
        {
            Vector3 velocity = (segmentPositions[i] - segmentVelocities[i]) * 0.98f;
            segmentVelocities[i] = segmentPositions[i];
            segmentPositions[i] += velocity + Physics.gravity * Time.fixedDeltaTime * Time.fixedDeltaTime * 0.1f;
        }

        // Lock ends to anchor and target
        segmentPositions[0] = transform.position;
        segmentPositions[segmentCount - 1] = target.position;

        // Constrain segment distances so it doesn't stretch
        float segLength = leashLength / (segmentCount - 1);
        for (int iteration = 0; iteration < 5; iteration++)
        {
            for (int i = 0; i < segmentCount - 1; i++)
            {
                Vector3 diff = segmentPositions[i + 1] - segmentPositions[i];
                float dist = diff.magnitude;
                float error = (dist - segLength) / dist;
                Vector3 correction = diff * 0.5f * error;

                if (i != 0) segmentPositions[i] += correction;
                if (i + 1 != segmentCount - 1) segmentPositions[i + 1] -= correction;
            }
        }

        float totalDist = Vector3.Distance(transform.position, target.position);

        if (totalDist > leashLength)
        {
            Vector3 pullDir = (transform.position - target.position).normalized;
            Rigidbody rb = target.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(pullDir * pullForce * (totalDist - leashLength));
        }

        lineRenderer.SetPositions(segmentPositions);
    }
}
