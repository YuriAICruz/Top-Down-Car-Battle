using Graphene.AutoMobileDynamics;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TopDownCarBattle
{
    public class CameraManager : MonoBehaviour
    {
        public Transform Target;
        private Vector3 _position;
        public float Speed = 3;

        private void Start()
        {
            if (Target == null)
                Target = FindObjectOfType<Car>().transform;
            _position = transform.position;
        }

        private void Update()
        {
            _position.x = Target.position.x;
            _position.z = Target.position.z;

            transform.position += (-transform.position + _position) * Time.deltaTime * Speed;
        }
    }
}