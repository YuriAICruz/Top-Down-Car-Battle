using System;
using Graphene.InputManager;
using UnityEngine;

namespace Graphene.TopDownCarBattle.Weapon
{
    internal class WeaponInput : InputSystem
    {
        public event Action FireOn, FireOff, SwitchWeapon;

        protected override void ExecuteCombo(int id)
        {
            switch (id)
            {
                case 40:
                    SwitchWeapon?.Invoke();
                    break;
                case 50:
                    FireOn?.Invoke();
                    break;
                case 51:
                    FireOff?.Invoke();
                    break;
            }
        }
    }
}