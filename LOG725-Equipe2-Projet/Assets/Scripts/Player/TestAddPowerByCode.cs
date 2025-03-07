using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAddPowerByCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //We must deactivate the gameObject first, otherwise the OnEnable function of the Power class will trigger before we set the input.
        gameObject.SetActive(false);
        var airPower = gameObject.AddComponent<AirPower>();
        var playerInput = gameObject.GetComponent<PlayerInput>();
        airPower.PowerInput = InputActionReference.Create(playerInput.actions["Air"]);
        gameObject.SetActive(true);
    }

}
