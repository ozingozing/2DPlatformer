using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
	protected D_MoveState stateData;

	protected bool isDetectingWall;
	protected bool isDetectingLedge;
	protected bool isPlayerInMinAgroRange;
	protected bool isPlayerInMaxAgroRange;

	//���� ���� base�� MoveState�� �ִ� �������� �ʱ�ȭ ���� �� base�� �ִ� ������ �θ��ӿ��� ��� ���°� ���� ���� ������ �ִٸ�
	//���� �ؿ�ó�� ���� �� �߰��� �ʱ�ȭ����																											
	public MoveState(Entity entity, FiniteStateMachine sateMachine, string animBoolName, D_MoveState stateData) : base(entity, sateMachine, animBoolName)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();
		isDetectingLedge = core.CollisionSenses.LedgeVertical;
		isDetectingWall = core.CollisionSenses.WallFront;
		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
		isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
	}

	public override void Enter()
	{
		base.Enter(); //�θ�Ŭ������ Enter�Լ� ����
					  //�ӷ� ����
		core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
