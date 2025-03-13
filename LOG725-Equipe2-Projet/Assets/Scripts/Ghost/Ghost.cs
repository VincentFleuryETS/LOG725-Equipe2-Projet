using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float passiveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatFrequency = 1f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;

    private Transform player;
    private bool isHostile = false;
    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Chercher le joueur s’il n’est pas encore trouvé
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogWarning("Player reference is still null! Waiting for player...");
                return; // Sortir si le joueur n’est pas trouvé
            }
            else
            {
                Debug.Log("Player found at: " + player.position);
            }
        }

        CheckPlayerDetection();

        if (isHostile)
        {
            ChasePlayer();
        }
        else
        {
            PassiveFloat();
        }
    }

    private void CheckPlayerDetection()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Distance to player: {distanceToPlayer}, Detection Radius: {detectionRadius}");
        isHostile = distanceToPlayer <= detectionRadius;
        Debug.Log($"Ghost State: {(isHostile ? "Hostile" : "Passive")}");
    }

    private void PassiveFloat()
    {
        float floatY = Mathf.Sin(Time.time * floatFrequency + timeOffset) * floatAmplitude;
        Vector3 newPosition = startPosition + new Vector3(
            Mathf.Cos(Time.time * floatFrequency + timeOffset) * floatAmplitude,
            floatY,
            0f
        );
        transform.position = Vector3.MoveTowards(
            transform.position,
            newPosition,
            passiveSpeed * Time.deltaTime
        );
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
        Debug.Log($"Chasing player! Direction: {direction}, New Position: {transform.position}");

        if (direction.x > 0 && !facingRight())
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight())
        {
            Flip();
        }
    }

    private bool facingRight()
    {
        return transform.localScale.x < 0;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isHostile ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}