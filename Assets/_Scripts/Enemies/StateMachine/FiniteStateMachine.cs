using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    //현재 상태
    public State currentState { get; private set; }
    //초기화
    public void Initialize(State startingState)
    {
        //상태 초기화
        currentState = startingState;
        //상태 진입
        currentState.Enter();
    }
    //상태바꾸기
    public void ChangeState(State newState)
    {
        //상태나오고
        currentState.Exit();
        //상태 다시 초기화
        currentState = newState;
        //상태 진입
        currentState.Enter();
    }
}
