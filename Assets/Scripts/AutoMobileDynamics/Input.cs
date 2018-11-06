using System;
using Graphene.InputManager;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    public class Input : InputSystem
    {
        private Vector2 _leftStickDirection;
        private Vector2 _rightStickDirection;
        public event Action<Vector2> RightStick;
        public event Action<Vector2> LeftStick;

        public event Action GasOn;
        public event Action GasOff;
        public event Action BrakeOn;
        public event Action BrakeOff;

        protected override void ExecuteCombo(int id)
        {
            switch (id)
            {
                case 10:
                    GasOn?.Invoke();
                    break;
                case 11:
                    GasOff?.Invoke();
                    break;
                case 15:
                    BrakeOn?.Invoke();
                    break;
                case 16:
                    BrakeOff?.Invoke();
                    break;
            }
        }

        protected override void GetInputs()
        {
            base.GetInputs();

            _leftStickDirection.x = UnityEngine.Input.GetAxis("Horizontal");
            _leftStickDirection.y = UnityEngine.Input.GetAxis("Vertical");
            _rightStickDirection.x = UnityEngine.Input.GetAxis("Horizontal");
            _rightStickDirection.y = UnityEngine.Input.GetAxis("Vertical");

            if (LeftStick != null) LeftStick(_leftStickDirection);
            if (RightStick != null) RightStick(_leftStickDirection);
        }
    }
}