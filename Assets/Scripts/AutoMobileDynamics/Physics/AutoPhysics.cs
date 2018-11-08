using System;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.AutoMobileDynamics.Physics
{
    [Serializable]
    public class AutoPhysics
    {
        [HideInInspector] public BoxCollider CarMass;
        [HideInInspector] public Vector3[] Axes;
        public Vector3 Velocity;

        [Header("Body")] public float Mass;
        public float FrontWheelSize = 0.2f;
        public float BackWheelSize = 0.2f;

        [Header("Handling Attributes")] public float TopSpeed = 3;
        public float Acceleration = 2;
        [Range(0, 1)] public float Handling = 0.01f;
        [Range(0.01f, 0.99f)] public float Drag = 0.2f;
        [Space] public float Traction = 1;
        public float DriftAngle;
        public float AngleDrag = 10;
        public float Angle;

        public Vector3 MassShift;

        private Vector3 _position;
        private float _lastGasTime;
        private float _lastBrakeTime;
        private Transform _transform;
        private float _angleVelocity;
        private float _lastAngle;
        private float _speedRatio;

        void Awake()
        {
            Velocity = Vector3.zero;
        }

        public void SetPosition(Transform transform)
        {
            _position = transform.position;
            _transform = transform;
        }

        public Vector3 Update(float dir, float gas)
        {
            CalculatePosition_M2(dir, gas);
            //CalculatePosition_M1(dir);

            return _position;
        }

        private void CalculatePosition_M2(float dir, float gas)
        {
            // v = at
            // F = ma
            // I = Ft => I = mat => I = mv
            // a = dv / t = 2v/t = 2v / (pi / v) = (2/pi) pow(v, 2) / R
            // a = pow(v,2) / R
            // F = m pow(v,2) / R
            // Torque = T => T = Fd
            // W = fd
            // W = Fd = mad = ma(1/2 a pow(t,2) ) = 1/2 m pow(at,2) = 1/2 m pow(v, 2)
            // P = mgh
            // Weight = Wd => Wd = Fh / R

            if (gas < 0 && Vector3.Dot(Velocity.normalized, _transform.forward) > 0)
                gas = -2;

            var fwd = _transform.forward;
            var vRatio = Mathf.Abs(Velocity.magnitude / TopSpeed);
            _speedRatio = vRatio;
            dir *= (1 - vRatio);

            var _a = Acceleration * (1 - Mathf.Pow(vRatio, 0.5f)) * gas;

            Velocity += (fwd).normalized * _a * Time.deltaTime - Velocity.normalized * Drag * TopSpeed * Time.deltaTime;

            if (Velocity.magnitude < 0.2f)
                Velocity = Vector3.zero;

            MassShift = new Vector3(-dir * vRatio, 0, -(_a / Acceleration));

            var rearAxis = _transform.TransformPoint(Axes[2] + (Axes[3] - Axes[2]) * 0.5f);
            var frontAxis = _transform.TransformPoint(Axes[0] + (Axes[1] - Axes[0]) * 0.5f);

            var backslip = _transform.right; // Quaternion.AngleAxis(MassShift.x, _transform.up) * _transform.right; 

            var offset = Vector3.zero;
            if (dir > 0)
            {
                offset = CalculateSlip(dir, backslip, Axes[0], Axes[2], Axes[1]);
            }
            else if (dir < 0)
            {
                offset = CalculateSlip(dir, backslip, Axes[1], Axes[3], Axes[0]);
            }

            var moveDir = Vector3.Angle(Velocity.normalized, fwd) > 90 ? -fwd : fwd;

            var directionVelocityAngle = Vector3.SignedAngle(Velocity.normalized, moveDir, _transform.up);

#if UNITY_EDITOR
            Debug.DrawRay(_position, Velocity.normalized * 10, Color.blue);
            Debug.DrawRay(_position, Quaternion.AngleAxis(directionVelocityAngle, _transform.up) * Velocity.normalized * 10, Color.green);
#endif

            Velocity = Quaternion.AngleAxis(directionVelocityAngle * Handling * (Mathf.Pow(1 - _speedRatio, 4)), _transform.up) *
                       (Velocity - Velocity.normalized * Vector3.Angle(Velocity.normalized, moveDir) / 90);

            _position += Velocity * Time.deltaTime; //+ offset;
        }

        Vector3 CalculateSlip(float dir, Vector3 backslip, Vector3 a, Vector3 b, Vector3 c)
        {
            var right = Vector3.Cross(Velocity.normalized, _transform.up); //_transform.right
            var offset = Vector3.zero;
            a = _transform.TransformPoint(a);
            b = _transform.TransformPoint(b);
            c = _transform.TransformPoint(c);

            var axisdir = Quaternion.AngleAxis(dir * Angle, _transform.up) * right;
            Vector3 v;
            if (a.Intersection(axisdir, b, backslip, out v))
            {
                var last = _transform.position;
                _transform.RotateAround(
                    v,
                    Vector3.up,
                    dir * Angle * _speedRatio
                );
                offset = _transform.position - last;

#if UNITY_EDITOR
                Debug.DrawLine(a, v, Color.red);
                Debug.DrawLine(b, v, Color.red);
                Debug.DrawLine(c, v, Color.red);
#endif
            }

            return offset;
        }

        public void OnCollisionEnter(Collision other)
        {
            _position -= other.contacts[0].normal * other.contacts[0].separation;
            
            Velocity = Vector3.Reflect(Velocity.normalized, other.contacts[0].normal) * Velocity.magnitude;
        }

        public void OnCollisionEnter(Collider other)
        {
#if UNITY_EDITOR
            Debug.Log("Trigger Collision: " + other.transform.gameObject);
#endif

            Velocity = Vector3.zero;
        }

        public void OnCollisionStay(Collision other)
        {
            _position -= other.contacts[0].normal * other.contacts[0].separation;
        }
    }
}