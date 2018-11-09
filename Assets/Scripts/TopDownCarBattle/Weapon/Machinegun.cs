using System.Collections;
using System.IO;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.TopDownCarBattle.Weapon
{
    public class Machinegun : BaseWeapon
    {
        private GameObject _muzzleFXHolder;
        private GameObject _bulletFXHolder;
        private GameObject _bodyHolder;

        private ParticleSystem _muzzleFX;
        private ParticleCollisionHandler _bulletFX;
        private ParticleSystem.MainModule _main;
        private LayerMask _mask;

        protected override void LoadParticleSystems()
        {
            _mask = 1 << LayerMask.NameToLayer("HIttable");
            
            _bullet = Resources.Load<GameObject>("Particles/Mg_bullet");
            _spark = Resources.Load<GameObject>("Particles/Mg_spark");
            _body = Resources.Load<GameObject>("Weapon/Machinegun");
        }

        protected override void InstantiateParticleSystems()
        {
            _bodyHolder = Object.Instantiate(_body, _transform);
            _bodyHolder.transform.localPosition = _origin;
            
            _muzzleFXHolder = Object.Instantiate(_spark, _bodyHolder.transform);
            _muzzleFXHolder.transform.localPosition = Vector3.zero;

            _muzzleFX = _muzzleFXHolder.transform.GetChild(0).GetComponent<ParticleSystem>();
            _muzzleFX.Stop();

            _bulletFXHolder = Object.Instantiate(_bullet, _bodyHolder.transform);
            _bulletFXHolder.transform.localPosition = Vector3.zero;

            _bulletFX = _bulletFXHolder.transform.GetChild(0).GetComponent<ParticleCollisionHandler>();
            _bulletFX.OnCollision += OnParticleCollision;

            if (_bulletFX.Particle == null)
                _bulletFX.Particle = _bulletFX.GetComponent<ParticleSystem>();

            _bulletFX.Particle.Stop();
            _main = _bulletFX.Particle.main;
        }

        private void OnParticleCollision(GameObject other, Vector3 point)
        {
            Debug.DrawRay(point, (_transform.position - point), Color.magenta, 3);
        }

        public override void SetSpeed(Vector3 velocity)
        {
            base.SetSpeed(velocity);

            _main.startSpeedMultiplier = _speed;
        }

        public override void FireOn()
        {
            base.FireOn();
            
            _bulletFX.Particle.Play();
            _muzzleFX.Play();
        }

        protected override void FireUpdate()
        {
//            RaycastHit hit;
//            
//            if (Physics.Raycast(_transform.TransformPoint(_origin), _transform.forward, out hit, 40, _mask))
//            {
//                Debug.DrawRay(_transform.TransformPoint(_origin), _transform.forward * hit.distance, Color.red);
//            }else
//            {
//                Debug.DrawRay(_transform.TransformPoint(_origin), _transform.forward * 40, Color.yellow);
//            }
        }
        
        public override void FireOff()
        {
            base.FireOff();
            
            _bulletFX.Particle.Stop();
            _muzzleFX.Stop();
        }
    }
}