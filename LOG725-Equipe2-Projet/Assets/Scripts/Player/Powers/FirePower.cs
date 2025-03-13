using UnityEngine;

public class FirePower : Power
{
    public override void Cast(Vector2 direction)
    {
        Debug.Log("Fire Power used!");
    }
}
