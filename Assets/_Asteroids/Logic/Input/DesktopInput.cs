
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

        public bool IsShootingPrimary()
        {
            return UnityEngine.Input.GetMouseButton(0);
        }

        public bool IsShootingSecondary()
        {
            return UnityEngine.Input.GetMouseButton(1);
        }
    }
}