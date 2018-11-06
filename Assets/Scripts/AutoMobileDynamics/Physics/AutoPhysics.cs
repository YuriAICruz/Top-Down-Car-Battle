using System;
using UnityEngine;

namespace Graphene.AutoMobileDynamics.Physics
{
    [Serializable]
    public class AutoPhysics
    {
        public float Mass;
        public float FrontWheelSize = 0.2f;
        public float BackWheelSize = 0.2f;

        [HideInInspector] public BoxCollider CarMass;
        [HideInInspector] public Vector3[] Axes;

        public float TopSpeed;
        private Vector3 _position;
        private float _lastGasTime;
        private float _lastBrakeTime;
        private float _gas;

        public float Acceleration = 2;
        private float _velocity;
        private Transform _transform;

        public void SetPosition(Transform transform)
        {
            _position = transform.position;
            _transform = transform;
        }

        public void GasOn()
        {
            _lastGasTime = Time.time;
            _gas = 1;
        }

        public void GasOff()
        {
            _gas = 0;
        }

        public void BrakeOn()
        {
            _lastBrakeTime = Time.time;
            _gas = -1;
        }

        public void BrakeOff()
        {
            _gas = 0;
        }

        public Vector3 Update(float dir)
        {
            _velocity += _gas * Time.deltaTime * Acceleration;
            _velocity = Mathf.Min(_velocity, TopSpeed);
            _velocity = Mathf.Max(0, _velocity - Time.deltaTime);


            _position = _position + (_transform.forward + _transform.TransformDirection(Vector3.right * dir)).normalized * _velocity;

            return _position;
        }

    }
}