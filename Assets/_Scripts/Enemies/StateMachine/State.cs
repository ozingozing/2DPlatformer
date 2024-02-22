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
    //���� ����
    public virtual void Enter()
    {
        //���۽ð� ����
        startTime = Time.time;
        //�ִϸ��̼� bool�� ����
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }
    //���� Ż��
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
