using System;
using UnityEngine;

namespace Assets._Asteroids.Logic.Weapon
{
    public interface IWeapon
    {
        public void Init(Transform launchOffset);
        public void ResetWeapon();
        public void HandleFire();
        public event Action<int> OnCountChanged;
        public event Action<float, float> OnReloadTimeChanged;
        public event Action OnShoot;
    }
}