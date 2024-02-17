using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	private CollisionSenses CollisionSenses
	{ get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private CollisionSenses collisionSenses;


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
		if(CollisionSenses)
		{
			isDetectingLedge = CollisionSenses.LedgeVertical;
			isDetectingWall = CollisionSenses.WallFront;
		}
		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
		isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
	}

	public override void Enter()
	{
		base.Enter(); //�θ�Ŭ������ Enter�Լ� ����
					  //�ӷ� ����
		Movement?.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		Movement?.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
