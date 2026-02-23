namespace Assets._Asteroids.Logic.Input
{
    public interface IInput
    {
        public float Move();
        public float Rotate();

        public bool isShootingPrimary();
        public bool isShootingSecondary();
    }
}