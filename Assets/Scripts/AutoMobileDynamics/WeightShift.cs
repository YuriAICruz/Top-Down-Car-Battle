using Graphene.AutoMobileDynamics;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    public class WeightShift : MonoBehaviour
    {
        private Quaternion _rotation;
        public Car Car;

        public float ShiftForce;

        private void Update()
        {
            CalcRotation();
            transform.localRotation = _rotation;
        }

        private void CalcRotation()
        {
            _rotation = Quaternion.Euler(
                Car.Physics.MassShift.z * ShiftForce,
                transform.localEulerAngles.y,
                -Car.Physics.MassShift.x * ShiftForce
            );
        }
    }
}