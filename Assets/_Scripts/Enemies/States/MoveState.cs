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

	//여기 끝에 base가 MoveState에 있는 변수들을 초기화 해줌 저 base에 있는 변수는 부모상속에서 들고 오는거 만약 없는 변수가 있다면
	//여기 밑에처럼 따로 또 추가로 초기화ㄱㄱ																											
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
		base.Enter(); //부모클래스의 Enter함수 실행
					  //속력 설정
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
