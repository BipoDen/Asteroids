
using UnityEngine;

namespace Assets._Asteroids.Logic.Input
{
    public class DesktopInput : IInput
    {
        public float Move()
        {
            var inputValue = UnityEngine.Input.GetAxisRaw("Vertical");
            return inputValue;
        }

        public float Rotate()
        {
            var  inputValue = UnityEngine.Input.GetAxisRaw("Horizontal");
            return inputValue;
        }

        public bool isShootingPrimary()
        {
            return UnityEngine.Input.GetKey(KeyCode.Mouse0);
        }

        public bool isShootingSecondary()
        {
            return UnityEngine.Input.GetKey(KeyCode.Mouse1);
        }
    }
}