using System;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    [Serializable]
    public class Steering
    {
        public Transform[] FrontWheels;

        public float MaxAngle = 60;

        public void Steer(float dirX)
        {
            foreach (var wheel in FrontWheels)
            {
                wheel.transform.rotation = Quaternion.Euler(0, MaxAngle * dirX, 0);
            }
        }
    }
}