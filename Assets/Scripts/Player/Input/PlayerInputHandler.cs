using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput {  get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormalInputX { get; private set; }
    public int NormalInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    public bool[] AttackInputs { get; private set; }


    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float dashInputSartTime;
    private float jumpInputStartTime;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];

        cam = Camera.main;
	}

	private void Update()
	{
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
	}

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if(context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
		}
    }
    
    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
		if (context.started)
		{
			AttackInputs[(int)CombatInputs.secondary] = true;
		}

		if (context.canceled)
		{
			AttackInputs[(int)CombatInputs.secondary] = false;
		}
	}


	public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        //게임패트 인식이 float -1~1인데 이 모든 값을 -1, 0, 1형식으로 바꿈
        NormalInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormalInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if(context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GrabInput = true;
        }
       
        if(context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputSartTime = Time.time;
        }
        else if(context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();
        
        if(playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput)
                                    - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputSartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary
}
