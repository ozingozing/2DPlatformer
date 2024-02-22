using Ozing.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    //
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    protected Core core;

    public float startTime { get; protected set; }

    protected string animBoolName;

    public State(Entity entity, FiniteStateMachine sateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = sateMachine;
        this.animBoolName = animBoolName;
        core = entity.Core;
    }
    //상태 진입
    public virtual void Enter()
    {
        //시작시간 저장
        startTime = Time.time;
        //애니메이션 bool값 설정
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }
    //상태 탈출
    public virtual void Exit()
    {
		entity.anim.SetBool(animBoolName, false);
	}

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }



}
