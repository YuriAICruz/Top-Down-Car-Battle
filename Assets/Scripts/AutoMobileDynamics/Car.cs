using Graphene.AutoMobileDynamics.Physics;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    [RequireComponent(typeof(BoxCollider))]
    public class Car : MonoBehaviour
    {
        public AutoPhysics Physics;
        public Steering Steering;

        private Input _input;

        private void Awake()
        {
            Physics.SetPosition(transform);
            
            _input = new Input();

            _input.LeftStick += Steer;
            _input.GasOn += Physics.GasOn;
            _input.GasOff += Physics.GasOff;
            _input.BrakeOn += Physics.BrakeOn;
            _input.BrakeOff += Physics.BrakeOff;
            
            _input.Init();
        }

        private void Steer(Vector2 dir)
        {
            Steering.Steer(dir.x);

            transform.position = Physics.Update(dir.x);
        }
    }
}