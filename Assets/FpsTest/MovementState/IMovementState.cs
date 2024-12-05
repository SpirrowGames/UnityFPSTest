namespace FpsTest
{
    // State パターン
    // https://unity.com/resources/level-up-your-code-with-game-programming-patterns PDF の 69 ページ
    // https://github.com/Unity-Technologies/game-programming-patterns-demo/tree/main/Assets/UnityTechnologies/_DesignPatterns/5_State
    public interface IMovementState
    {
        /// <summary>
        /// 状態に入ったイベント
        /// </summary>
        public void EnterState();

        /// <summary>
        /// その状態での Update イベント
        /// </summary>
        public void UpdateState();

        /// <summary>
        /// その状態での FixedUpdate イベント
        /// </summary>
        public void FixedUpdateState();

        /// <summary>
        /// 状態から出たイベント
        /// </summary>
        public void ExitState();

        /// <summary>
        /// その状態から他の状態に遷移できれば遷移する
        /// </summary>
        public void ChangeState();

        public string GetStateName();
    }
}