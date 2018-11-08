using Graphene.AutoMobileDynamics;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TopDownCarBattle
{
    public class CameraManager : MonoBehaviour
    {
        public Car Target;
        private Vector3 _position;
        public float Speed;
        public float RotationSpeed;

        public Vector3 Offset;
        private Vector3 _lookPosition;

        private void Start()
        {
            if (Target == null)
                Target = FindObjectOfType<Car>();
            _position = transform.position + Target.transform.TransformPoint(Offset);
            _lookPosition = Target.transform.position + Target.transform.forward * 2 + Target.transform.forward;
        }

        private void Update()
        {
            _position = Target.transform.TransformPoint(Offset);
            var v = Target.Physics.Velocity;
            _lookPosition +=
                (-_lookPosition + Target.transform.position + Target.Physics.Velocity.normalized * 10)
                * Time.deltaTime * RotationSpeed
                ;

            //transform.LookAt(_lookPosition);

            transform.position += (-transform.position + _position) * Time.deltaTime * Speed;
        }
    }
}