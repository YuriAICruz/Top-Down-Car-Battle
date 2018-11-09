using UnityEngine;

namespace Graphene.TopDownCarBattle.Weapon
{
    public class Machinegun : BaseWeapon
    {
        private GameObject _muzzleFXHolder;
        private GameObject _bulletFXHolder;

        private ParticleSystem _muzzleFX;
        private ParticleSystem _bulletFX;
        private ParticleSystem.MainModule _main;

        protected override void LoadParticleSystems()
        {
            _bullet = Resources.Load<GameObject>("Particles/Mg_bullet");
            _spark = Resources.Load<GameObject>("Particles/Mg_spark");
        }

        protected override void InstantiateParticleSystems()
        {
            _muzzleFXHolder = Object.Instantiate(_spark, _transform);
            _muzzleFXHolder.transform.localPosition = _origin;

            _muzzleFX = _muzzleFXHolder.transform.GetChild(0).GetComponent<ParticleSystem>();
            _muzzleFX.Stop();

            _bulletFXHolder = Object.Instantiate(_bullet, _transform);
            _bulletFXHolder.transform.localPosition = _origin;

            _bulletFX = _bulletFXHolder.transform.GetChild(0).GetComponent<ParticleSystem>();
            _bulletFX.Stop();

            _main = _bulletFX.main;
        }

        public override void SetSpeed(Vector3 velocity)
        {
            base.SetSpeed(velocity);

            _main.startSpeedMultiplier = _speed;
        }

        public override void FireOn()
        {
            base.FireOn();
            
            _bulletFX.Play();
            _muzzleFX.Play();
        }

        public override void FireOff()
        {
            base.FireOff();
            
            _bulletFX.Stop();
            _muzzleFX.Stop();
        }
    }
}