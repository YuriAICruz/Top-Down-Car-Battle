using Graphene.UiGenerics;

namespace Graphene.AutoMobileDynamics.Presentation
{
    public class CarVelocityFeedback : ImageView
    {
        private Car _car;

        private void Setup()
        {
            _car = FindObjectOfType<Car>();
        }

        private void Update()
        {
            Image.fillAmount = _car.Physics.Velocity.magnitude / _car.Physics.TopSpeed;
        }
    }
}