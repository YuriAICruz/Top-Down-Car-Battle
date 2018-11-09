using System.Collections;
using System.Collections.Generic;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.TopDownCarBattle.Weapon
{
    public abstract class BaseWeapon
    {
        protected GameObject _bullet, _spark, _body;
        protected Vector3 _origin;
        protected Transform _transform;
        protected float _speed;
        private bool _fire;

        public BaseWeapon()
        {
            LoadParticleSystems();
        }

        public virtual void FireOn()
        {
            _fire = true;
            GlobalCoroutineManager.Instance.StartCoroutine(FireUpdateRoutine());
        }

        public virtual void FireOff()
        {
            _fire = false;
        }

        private IEnumerator FireUpdateRoutine()
        {
            while (_fire)
            {
                FireUpdate();
                yield return null;
            }
        }

        protected virtual void FireUpdate()
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