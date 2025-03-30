using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DestructiblePlant : MonoBehaviour
{
    [SerializeField]
    private AudioClip destroySound; // Son joué lors de la destruction

    private void Awake()
    {
        // Assure que le collider est en mode "Trigger" pour détecter les collisions sans bloquer physiquement
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet qui entre en collision est un projectile
        PowerProjectile projectile = other.GetComponent<PowerProjectile>();
        if (projectile != null)
        {
            // Vérifie si le projectile est de type "Fire"
            if (projectile.powerType == PowerType.Fire)
            {
                // Joue un son de destruction (si défini)
                if (destroySound != null)
                {
                    AudioManager.GetSingleton().PlaySFX(destroySound);
                }

                // Détruit la plante
                Destroy(gameObject);

                // Détruit aussi le projectile pour qu'il ne continue pas sa trajectoire
                Destroy(projectile.gameObject);
            }
        }
    }
}