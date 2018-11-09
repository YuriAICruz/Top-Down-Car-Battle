using Graphene.AutoMobileDynamics;
using Graphene.TopDownCarBattle.Weapon;
using UnityEngine;

namespace Graphene.TopDownCarBattle
{
    public class Player : MonoBehaviour
    {
        private Car _car;

        private WeaponInput _input;

        private BaseWeapon[] _weapon;
        
        private void Awake()
        {
            _car = GetComponent<Car>();

            _input = new WeaponInput();

            _input.FireOn += FireOn;
            _input.FireOff += FireOff;
            _input.SwitchWeapon += SwitchWeapon;

            _input.Init();

            _weapon = new BaseWeapon[_car.WeaponMounts.Length];
            
            for (int i = 0; i < _car.WeaponMounts.Length; i++)
            {
                _weapon[i] = new Machinegun();

                _weapon[i].SetOrigin(transform, _car.WeaponMounts[i]);
            }
        }

        private void Update()
        {
            foreach (var weapon in _weapon)
            {
                weapon.SetSpeed(_car.Physics.Velocity);
            }
        }

        private void SwitchWeapon()
        {
        }

        private void FireOff()
        {
            foreach (var weapon in _weapon)
                weapon.FireOff();
        }

        private void FireOn()
        {
            foreach (var weapon in _weapon)
                weapon.FireOn();
        }
    }
}