using Graphene.AutoMobileDynamics;
using Graphene.TopDownCarBattle.Weapon;
using UnityEngine;

namespace Graphene.TopDownCarBattle
{
    public class Player : MonoBehaviour
    {
        public Vector3[] WeaponMounts;

        private Car _car;
        
        private WeaponInput _input;

        private BaseWeapon _weapon;

        private void Awake()
        {
            _input = new WeaponInput();
            
            _input.FireOn += FireOn;
            _input.FireOff += FireOff;
            _input.SwitchWeapon += SwitchWeapon;
            
            _input.Init();
            
            _weapon = new Machinegun();
            _weapon.SetOrigin(transform, WeaponMounts[0]);

            _car = GetComponent<Car>();
        }

        private void Update()
        {
            _weapon.SetSpeed(_car.Physics.Velocity);
        }

        private void SwitchWeapon()
        {
            
        }

        private void FireOff()
        {
            _weapon.FireOff();
        }

        private void FireOn()
        {
            _weapon.FireOn();
        }
    }
}