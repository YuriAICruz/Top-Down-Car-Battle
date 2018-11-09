using System.ComponentModel.Design.Serialization;
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
        private float rotation;

        private Vector3 _noUp = new Vector3(1, 0, 1);

        private void Start()
        {
            if (Target == null)
                Target = FindObjectOfType<Car>();
            _position = Target.transform.TransformPoint(Offset);

            rotation = Target.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                rotation,
                transform.eulerAngles.z
            );
        }

        private void Update()
        {
            _position = Target.transform.TransformPoint(Offset);

            rotation = (-rotation + Target.transform.eulerAngles.y) * Time.deltaTime * RotationSpeed;

            transform.Rotate(
                Vector3.up,
                Vector3.SignedAngle(
                    Vector3.Scale(Target.transform.forward, _noUp).normalized,
                    Vector3.Scale(transform.up, _noUp).normalized,
                    -Vector3.up
                ) * Time.deltaTime * RotationSpeed,
                Space.World);

            transform.position += (-transform.position + _position) * Time.deltaTime * Speed;
        }
    }
}