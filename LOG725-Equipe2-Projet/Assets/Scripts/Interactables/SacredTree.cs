using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeState
{
    Corrupted,
    Purified,
    AbsorbedSpirit
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SacredTree : MonoBehaviour
{
    [SerializeField]
    public TreeState CurrentState = TreeState.Corrupted; // Changé en public

    [Header("----- Color Settings -----")]
    [SerializeField]
    private Color CorruptedColor;
    [SerializeField]
    private Color PurifiedColor;
    [SerializeField]
    private Color AbsorbedSpiritColor;

    [Header("----- Audio -----")]
    [SerializeField]
    private AudioClip PurifySound;
    [SerializeField]
    private AudioClip AbsorbSound;

    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        ChangeState(CurrentState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void CheckCollision(Collider2D collision)
    {
        switch (CurrentState)
        {
            case TreeState.Corrupted:
                if (collision.TryGetComponent(out PowerProjectile projectile))
                {
                    if (projectile.powerType == PowerType.Water)
                    {
                        Debug.Log("Sacred Tree purified.");
                        AudioManager.GetSingleton().PlaySFX(PurifySound);
                        Destroy(projectile.gameObject);
                        ChangeState(TreeState.Purified);
                    }
                }
                break;
            case TreeState.Purified:
                if (collision.TryGetComponent(out Ghost ghost))
                {
                    Debug.Log("Sacred Tree absorbed Ghost.");
                    AudioManager.GetSingleton().PlaySFX(AbsorbSound);
                    Destroy(ghost.gameObject);
                    ChangeState(TreeState.AbsorbedSpirit);
                }
                break;
            case TreeState.AbsorbedSpirit:
                break;
        }
    }

    private void ChangeState(TreeState state)
    {
        CurrentState = state;
        switch (state)
        {
            case TreeState.Corrupted:
                _renderer.color = CorruptedColor;
                break;
            case TreeState.Purified:
                _renderer.color = PurifiedColor;
                break;
            case TreeState.AbsorbedSpirit:
                _renderer.color = AbsorbedSpiritColor;
                break;
        }
    }
}