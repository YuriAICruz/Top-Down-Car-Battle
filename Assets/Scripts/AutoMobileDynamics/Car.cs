using Graphene.AutoMobileDynamics.Physics;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour
    {
        public AutoPhysics Physics;
        public Steering Steering;

        public Vector3[] WeaponMounts;
        
        private Input _input;

        private void Awake()
        {
            Physics.SetPosition(transform);
            Steering.Physics = Physics;
            Steering.Transform = transform;
            
            _input = new Input();

            _input.LeftStick += Mootion;
            
            _input.Init();
        }

        private void Mootion(Vector2 dir)
        {
            Steering.Steer(dir.x);

            transform.position = Physics.Update(dir.x, dir.y);
        }

        private void OnTriggerEnter(Collider other)
        {
            Physics.OnCollisionEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            Physics.OnCollisionEnter(other);
        }

        private void OnCollisionStay(Collision other)
        {
            Physics.OnCollisionStay(other);
        }

        private void OnCollisionExit(Collision other)
        {
            
        }
    }
}