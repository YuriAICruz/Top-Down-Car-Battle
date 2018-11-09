using UnityEngine;

namespace Graphene.TopDownCarBattle.Weapon
{
    public abstract class BaseWeapon
    {
        protected GameObject _bullet, _spark;
        protected Vector3 _origin;
        protected Transform _transform;
        protected float _speed;

        public BaseWeapon()
        {
            LoadParticleSystems();
        }

        public virtual void FireOn()
        {
            
        }

        public virtual void FireOff()
        {
            
        }

        public void SetOrigin(Transform transform, Vector3 origin)
        {
            _transform = transform;
            _origin = origin;
            InstantiateParticleSystems();
        }

        public virtual void SetSpeed(Vector3 velocity)
        {
            _speed = velocity.magnitude;
        }

        protected virtual void LoadParticleSystems()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void InstantiateParticleSystems()
        {
            throw new System.NotImplementedException();
        }
    }
}