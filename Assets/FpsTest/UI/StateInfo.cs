using TMPro;
using UnityEngine;

namespace FpsTest
{
    public class StateInfo : MonoBehaviour
    {
        public MovementStates states;
        public TextMeshProUGUI currentStateText;

        private void Awake()
        {
            states.OnStateChanged += UpdateStateText;
        }

        private void UpdateStateText(IMovementState state)
        {
            currentStateText.text = state.GetStateName();
        }
    }
}