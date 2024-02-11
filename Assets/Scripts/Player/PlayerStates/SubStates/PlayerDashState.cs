using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
	public bool CanDash { get; private set; }
	public bool isHolding;
	private bool dashInputStop;

	private float lastDashTime;

	private Vector2 dashDirection;
	private Vector2 dashDirectionInput;
	private Vector2 lastAfterImagePos;

	public PlayerDashState(Player player, PlayerStateMachine stateMachine,
		PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		CanDash = false;
		player.InputHandler.UseDashInput();

		isHolding = true;
		dashDirection = Vector2.right * player.FacingDirection;

		Time.timeScale = playerData.holdTimeScalse;
		startTime = Time.unscaledTime;

		player.DashDirectionIndicator.gameObject.SetActive(true);
	}

	public override void Exit()
	{
		base.Exit();

		if(player.CurrentVelocity.y > 0 )
		{
			player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(!isExitingState)
		{
			player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
			player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));


			if(isHolding)
			{
				dashDirectionInput = player.InputHandler.DashDirectionInput;
				dashInputStop = player.InputHandler.DashInputStop;

				if(dashDirectionInput != Vector2.zero)
				{
					dashDirection = dashDirectionInput;
					dashDirection.Normalize();
				}

				float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
				player.DashDirectionIndicator.rotation = Quaternion.Euler(0, 0, angle - 45f);

				if(dashInputStop || Time.unscaledTime >= startTime + playerData.maxHlodTime)
				{
					isHolding = false;
					Time.timeScale = 1f;
					startTime = Time.time;
					player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
					player.RB.drag = playerData.darag;
					player.SetVelocity(playerData.dashVelocity, dashDirection);
					player.DashDirectionIndicator.gameObject.SetActive(false);
					PlaceAfterImage();
				}
			}
			else
			{
				player.SetVelocity(playerData.dashVelocity, dashDirection);
				CheckIfShouldPlaceAfterImage();

				if (Time.time >= startTime + playerData.dashTime)
				{
					player.RB.drag = 0f;
					isAbilityDone = true;
					lastDashTime = Time.time;
				}
			}
		}
	}

	private void CheckIfShouldPlaceAfterImage()
	{
		if(Vector2.Distance(player.transform.position, lastAfterImagePos) >= playerData.distanceBetweenAfterImages)
		{
			PlaceAfterImage();
		}
	}

	private void PlaceAfterImage()
	{
		PlayerAfterImagePool.Instance.GetFromPool();
		lastAfterImagePos = player.transform.position;
	}


	public bool CheckIfCanDash()
	{
		return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
	}

	public void ResetCanDash() => CanDash = true;

	
}
