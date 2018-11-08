using System;
using Graphene.AutoMobileDynamics.Physics;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    [Serializable]
    public class Steering
    {
        public AutoPhysics Physics;
        public Transform Transform;
        public Transform[] FrontWheels;
        public Transform[] AllWheels;

        public TrailRenderer[] Trails;

        public float WheelSpeed;

        private float _lastRot;

        public void Steer(float dirX)
        {
            var rot = Physics.Angle * dirX;
            foreach (var wheel in FrontWheels)
            {
                wheel.Rotate(Transform.up, rot - _lastRot, Space.World);
            }
            var i = 0;
            foreach (var wheel in AllWheels)
            {
                wheel.Rotate(Vector3.right, Physics.Velocity.magnitude * WheelSpeed * (i < 2 ? Physics.FrontWheelSize : Physics.BackWheelSize), Space.Self);
                i++;
            }
            _lastRot = rot;

            if (
                Physics.MassShift.z < -0.62f ||
                Physics.MassShift.z > 1f ||
                Vector3.Angle(Physics.Velocity.normalized, Vector3.Angle(Physics.Velocity.normalized, Transform.forward) > 90 ? -Transform.forward : Transform.forward) > 8
            )
            {
                foreach (var trail in Trails)
                {
                    trail.emitting = true;
                }
            }
            else
            {
                foreach (var trail in Trails)
                {
                    trail.emitting = false;
                }
            }
        }
    }
}