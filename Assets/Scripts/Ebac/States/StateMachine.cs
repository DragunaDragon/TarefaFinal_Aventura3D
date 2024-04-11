using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Ebac.StateMachine
{

    public class StateMachine<T> where T : System.Enum
    {

        public Dictionary<T, StateBase> dicionaryState;

        private StateBase _currentState;
        public float timeToStartGame = 1f;


        public StateBase CurrentState
        {
            get { return _currentState; }
        }


        public void Init()
        {
            dicionaryState = new Dictionary<T, StateBase>();
        }



        public void RegisterStates(T typeEnum, StateBase state)
        {
            dicionaryState.Add(typeEnum, state);

        }


        public void SwitchState(T state, params object[] objs)
        {
            if (_currentState != null) _currentState.OnStateExit();

            _currentState = dicionaryState[state];

            _currentState.OnStateEnter(objs);

        }


        public void Update()
        {
            if (_currentState != null) _currentState.OnStateStay();


        }
    }


}

