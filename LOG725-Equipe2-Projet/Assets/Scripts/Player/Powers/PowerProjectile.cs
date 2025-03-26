using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PowerProjectile : MonoBehaviour
{
    public PowerType powerType;
    
    [SerializeField]
    private AudioClip CollisionSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.GetSingleton().PlaySFX(CollisionSound);
        Destroy(gameObject);
    }

    public void ActivateGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale = 5.0f;
    }
}
