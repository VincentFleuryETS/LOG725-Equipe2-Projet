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
    private TreeState CurrentState = TreeState.Corrupted;
    [SerializeField]
    private Color CorruptedColor;
    [SerializeField]
    private Color PurifiedColor;
    [SerializeField]
    private Color AbsorbedSpiritColor;

    private SpriteRenderer _renderer;
    private Collider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        ChangeState(CurrentState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        switch (CurrentState)
        {
            case TreeState.Corrupted:
                if (collision.TryGetComponent(out PowerProjectile projectile))
                {
                    if(projectile.powerType == PowerType.Water)
                    {
                        Destroy(projectile.gameObject);
                        ChangeState(TreeState.Purified);
                    }
                }
                break;
            case TreeState.Purified:
                
                if (collision.TryGetComponent(out Ghost ghost))
                {
                    Debug.Log("Sacred Tree absorbed Ghost.");
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
