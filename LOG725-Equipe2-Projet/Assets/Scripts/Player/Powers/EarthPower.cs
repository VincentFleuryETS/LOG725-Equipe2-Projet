using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthPower : Power
{
    public override void Cast(Vector2 direction)
    {
        Debug.Log("Earth Power used!");
    }
}
