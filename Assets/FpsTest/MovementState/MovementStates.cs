using System;
using Unity.VisualScripting;
using UnityEngine;

namespace FpsTest
{
    /// <summary>
    /// 各ステートを管理するクラス
    /// 各ステートの Update / FixedUpdate の挙動はここから呼び出している
    /// 現在のステートを知っているのもこのクラス
    /// </summary>
    public class MovementStates : MonoBehaviour
    {
        public IMovementState CurrentState { get; private set; }

        public MovementStateFactory Factory { get; private set; }
        
        public Action<IMovementState> OnStateChanged;
        
        public void OnNewStateChanged(IMovementState newState)
        {
            CurrentState = newState;
            
            Debug.Log($"State changed to {newState.GetStateName()}");
            OnStateChanged?.Invoke(newState);
        }

        private void Awake()
        {
            Factory = new MovementStateFactory(this);

            // 最初は Default ステートで初期化
            CurrentState = Factory.Default();
            CurrentState.EnterState();
        }

        private void Update()
        {
            CurrentState.UpdateState();
        }

        private void FixedUpdate()
        {
            CurrentState.FixedUpdateState();
        }
    }
}