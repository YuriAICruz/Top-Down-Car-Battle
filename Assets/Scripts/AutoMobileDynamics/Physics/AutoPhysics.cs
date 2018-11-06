using System;
using UnityEngine;

namespace Graphene.AutoMobileDynamics.Physics
{
    [Serializable]
    public class AutoPhysics
    {
        [HideInInspector] public BoxCollider CarMass;
        [HideInInspector] public Vector3[] Axes;
        [HideInInspector] public float Velocity;

        [Header("Body")] public float Mass;
        public float FrontWheelSize = 0.2f;
        public float BackWheelSize = 0.2f;

        [Header("Handling Attributes")] public float TopSpeed = 3;
        public float Acceleration = 2;
        [Space] public float Traction = 1;
        public float DriftAngle;
        public float AngleDrag = 10;

        private Vector3 _position;
        private float _lastGasTime;
        private float _lastBrakeTime;
        private float _gas;
        private Transform _transform;
        private float _angleVelocity;
        private float _lastAngle;
        private float _speedRatio;

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
            _gas = -1f;
        }

        public void BrakeOff()
        {
            _gas = 0;
        }

        public Vector3 Update(float dir)
        {
            CalcVelocity();

            var mAxis = _transform.TransformPoint(Axes[1] + (Axes[0] - Axes[1]) / 2);

            var currentDir = _transform.forward;

            _speedRatio = Mathf.Sin(Velocity / TopSpeed * Mathf.PI * 0.5f);

            var angle = dir * _speedRatio * Traction * Time.deltaTime * Mathf.PI;
            var angleOffset = angle;
            _lastAngle = angle;

            _angleVelocity += angleOffset * Time.deltaTime * Velocity * AngleDrag;
            ClampAngleVelocity();

            var last = _transform.position;
            _transform.RotateAround(
                mAxis,
                Vector3.up,
                angle
            );

            currentDir.y = 0;
            currentDir.Normalize();

            var translationDirection = Quaternion.AngleAxis(_angleVelocity, Vector3.up) * currentDir;

            Debug.DrawRay(
                mAxis,
                Quaternion.AngleAxis(_angleVelocity * 10, Vector3.up) * currentDir * 10,
                Mathf.Abs(_angleVelocity) > DriftAngle ? Color.red : Color.magenta,
                1
            );


            if (Mathf.Abs(_angleVelocity) > DriftAngle)
            {
                _transform.RotateAround(
                    mAxis,
                    Vector3.up,
                    _angleVelocity * 0.5f
                );

                Velocity = Mathf.Max(0, Mathf.Abs(Velocity) - Time.deltaTime*0.2f) * Mathf.Sign(Velocity);
            }

            var offset = _transform.position - last;
            _position += offset + (translationDirection.normalized) * Velocity;

            return _position;
        }

        private void CalcVelocity()
        {
            Velocity += _gas * Time.deltaTime * Acceleration;
            Velocity = Mathf.Min(Velocity, TopSpeed);
            ClampVelocity();
        }

        private void ClampVelocity()
        {
            Velocity = Mathf.Max(0, Mathf.Abs(Velocity) - Time.deltaTime) * Mathf.Sign(Velocity);
        }

        private void ClampAngleVelocity()
        {
            _angleVelocity = Mathf.Max(0, Mathf.Abs(_angleVelocity) - Time.deltaTime * (1 - _speedRatio + 1) * 20) * Mathf.Sign(_angleVelocity);
            _angleVelocity = Mathf.Min(10, Mathf.Abs(_angleVelocity)) * Mathf.Sign(_angleVelocity);
        }
    }
}