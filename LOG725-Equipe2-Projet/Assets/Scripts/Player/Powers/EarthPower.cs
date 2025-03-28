using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class EarthPower : Power
{
    [SerializeField]
    private float PowerDuration = 5.0f;
    [SerializeField]
    private Color PowerColor;

    private bool _powerActive = false;
    private List<EarthMovableObject> _movableObjects;
    private SpriteRenderer _spriteRenderer;

    


    private void Awake()
    {
        _powerActive = false;
        _movableObjects = new List<EarthMovableObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsPowerActive()
    {
        return _powerActive;
    }

    public void AddMovableObject(EarthMovableObject movableObject)
    {
        _movableObjects.Add(movableObject);
        movableObject.StartMoving();
    }

    public void RemoveMovableObject(EarthMovableObject movableObject)
    {
        _movableObjects.Remove(movableObject);
        movableObject.StopMoving();
    }

    private void EndPower()
    {
        _powerActive = false;
        foreach(var movableObject in _movableObjects)
        {
            movableObject.StopMoving();
        }
        _movableObjects.Clear();
        _spriteRenderer.color = Color.white;
    }

    public override void Cast(Vector2 direction)
    {

        if (Charges > 0 && !IsPowerActive())
        {
            //Debug.Log("Earth Power used!");
            AudioManager.GetSingleton().PlaySFX(CastSound);
            _powerActive = true;
            _spriteRenderer.color = PowerColor;
            Invoke(nameof(EndPower), PowerDuration);
            Charges--;
        }
    }
}
