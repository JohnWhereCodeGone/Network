using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputs.IInputsActions //0
{

    private PlayerInputs inputs;
    public event UnityAction<Vector2> MoveEvent = delegate { }; //1
    public event UnityAction EmoteEvent = delegate { }; //1
    public event UnityAction ShootEvent = delegate { };

    public void OnEmote(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EmoteEvent.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context) //2
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootEvent.Invoke();
        }
    }

    private void OnEnable()
    {
        if (inputs == null)
        {
            inputs = new PlayerInputs();
            inputs.Inputs.SetCallbacks(this);
            inputs.Inputs.Enable();
        }
    }


}
