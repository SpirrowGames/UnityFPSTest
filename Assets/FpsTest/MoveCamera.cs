using UnityEngine;

namespace FpsTest
{
    public class MoveCamera : MonoBehaviour
    {
        [Tooltip("head をカメラ位置とする"), SerializeField]
        private Transform head;

        private void Update() => transform.position = head.transform.position;
    }
}