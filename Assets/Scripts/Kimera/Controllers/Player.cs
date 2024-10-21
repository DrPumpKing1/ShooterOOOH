using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Features;

public class Player : Controller, KineticEntity, TerrainEntity
{
    //Kinetic
    public Vector3 currentSpeed {get; set;}
    //Terrain
    public bool onGround {get; set;}

    public void OnLookAround(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>();
            CallFeature<LookAround>(new Setting("Mouse Delta", mouseDelta, Setting.ValueType.Vector2));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            CallFeature<Movement>(new Setting("Move Input", moveInput, Setting.ValueType.Vector2));
        }

        else if(context.canceled)
        {
            CallFeature<Movement>(new Setting("Move Input", Vector2.zero, Setting.ValueType.Vector2));
        }
    }
}
