
using UnityEngine;

public class WaterPower : Power
{
    [SerializeField]
    private PowerProjectile WaterProjectile;
    [SerializeField]
    private float ProjectileLifespan = 1.5f;
    [SerializeField]
    private float ProjectileSpeed = 10.0f;

    public override void Cast(Vector2 direction)
    {
        
        if (Charges > 0)
        {
            //Debug.Log("Water Power used!");
            AudioManager.GetSingleton().PlaySFX(CastSound);
            Vector2 tempDirection;
            //If the direction is basically zero, use the direction the Player is facing instead.
            if (direction.x < 0.1f && direction.x > -0.1f && direction.y < 0.1f && direction.y > -0.1f)
            {
                tempDirection = playerMovementController.GetFacingDirection();
            }
            else
            {
                tempDirection = direction;
            }

            tempDirection.Normalize();

            //We get the rotation between the default, and the shooting direction.
            var rotationFromDefault = Quaternion.FromToRotation(Vector2.right, tempDirection);
            //We create the projectile.
            var projectile = Instantiate(WaterProjectile, transform.position, rotationFromDefault);
            //We set it's velocity.
            projectile.GetComponent<Rigidbody2D>().velocity = tempDirection * ProjectileSpeed;
            //We set it to not collide with the creator.
            playerMovementController.IgnoreCollision(projectile.GetComponent<Collider2D>());
            //Set gravity after a percentage of the lifespan.
            projectile.Invoke(nameof(projectile.ActivateGravity), ProjectileLifespan * 0.2f);
            //Destroy the projectile after X seconds.
            Destroy(projectile.gameObject, ProjectileLifespan);

            Charges--;
        }
    }
}
