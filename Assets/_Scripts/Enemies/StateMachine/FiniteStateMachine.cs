using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    //���� ����
    public State currentState { get; private set; }
    //�ʱ�ȭ
    public void Initialize(State startingState)
    {
        //���� �ʱ�ȭ
        currentState = startingState;
        //���� ����
        currentState.Enter();
    }
    //���¹ٲٱ�
    public void ChangeState(State newState)
    {
        //���³�����
        currentState.Exit();
        //���� �ٽ� �ʱ�ȭ
        currentState = newState;
        //���� ����
        currentState.Enter();
    }
}
