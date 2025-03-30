using UnityEngine;

public class FirePower : Power
{
    [SerializeField]
    private PowerProjectile FireProjectile;
    [SerializeField]
    private float ProjectileLifespan = 0.7f;
    [SerializeField]
    private float ProjectileSpeed = 20.0f;
    [SerializeField]
    private float SlowdownDuration = 0.7f; // Durée du ralentissement

    public override void Cast(Vector2 direction)
    {
        if (Charges > 0)
        {
            // Jouer le son de lancement
            AudioManager.GetSingleton().PlaySFX(CastSound);

            Vector2 tempDirection;
            if (direction.magnitude < 0.1f) // Si la direction est trop faible, prendre la direction du joueur
            {
                tempDirection = playerMovementController.GetFacingDirection();
            }
            else
            {
                tempDirection = direction;
            }

            tempDirection.Normalize();

            // Calcul de la rotation pour aligner le projectile
            var rotationFromDefault = Quaternion.FromToRotation(Vector2.right, tempDirection);

            // Création du projectile
            var projectile = Instantiate(FireProjectile, transform.position, rotationFromDefault);

            // Appliquer une vitesse initiale
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = tempDirection * ProjectileSpeed;

            // Empêcher la collision avec le joueur
            playerMovementController.IgnoreCollision(projectile.GetComponent<Collider2D>());

            // Début du ralentissement progressif
            projectile.StartCoroutine(SlowDownAndDestroy(rb));

            Charges--;
        }
    }

    private System.Collections.IEnumerator SlowDownAndDestroy(Rigidbody2D rb)
    {
        float elapsedTime = 0f;
        Vector2 initialVelocity = rb.velocity;

        while (elapsedTime < SlowdownDuration)
        {
            float t = elapsedTime / SlowdownDuration; // Progression du ralentissement (0 à 1)
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t); // Réduction progressive
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero; // Assurer que la vitesse est nulle à la fin
        Destroy(rb.gameObject, ProjectileLifespan - SlowdownDuration); // Détruire après la durée restante
    }
}
