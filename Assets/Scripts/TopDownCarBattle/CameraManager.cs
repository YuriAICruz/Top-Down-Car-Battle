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
        public float Speed = 3;

        public Vector3 Offset;
        private Vector3 _lookPosition;

        private void Start()
        {
            if (Target == null)
                Target = FindObjectOfType<Car>();
            _position = transform.position + Target.transform.TransformPoint(Offset);
            _lookPosition = Target.transform.position +Target.transform.forward*2+ Target.transform.forward * Target.Physics.Velocity*5;
        }

        private void Update()
        {
            _position = Target.transform.TransformPoint(Offset);
            _lookPosition += (-_lookPosition + Target.transform.position +Target.transform.forward*2+ Target.transform.forward * Target.Physics.Velocity) * Time.deltaTime * Speed*2; 
            transform.LookAt(_lookPosition);

            transform.position += (-transform.position + _position) * Time.deltaTime * Speed;
        }
    }
}