using UnityEngine;


public class AirPower : Power
{
    
    public float AirDashForce = 20.0f;
    public float AirDashDuration = 0.2f;

    public override void Cast(Vector2 direction)
    {
        if(Charges > 0)
        {
            Debug.Log("Air Power used!");
            AudioManager.GetSingleton().PlaySFX(CastSound);
            //If the direction is basically zero, use the direction the Player is facing instead.
            if (direction.x < 0.1f && direction.x > -0.1f && direction.y < 0.1f && direction.y > -0.1f)
            {
                playerMovementController.LockVelocity(playerMovementController.GetFacingDirection() * AirDashForce, AirDashDuration);
            }
            else
            {
                direction.Normalize();
                playerMovementController.LockVelocity(direction * AirDashForce, AirDashDuration);
            }
            Charges--;
        }
        
    }
}
