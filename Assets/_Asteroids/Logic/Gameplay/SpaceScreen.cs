using UnityEngine;

namespace Assets._Asteroids.Logic.Gameplay
{
    public class SpaceScreen
    {
        private const float OFFSET = 0.5f;
        private const float RANDOM_SPREAD = .3f;
        
        public Vector2 UpLeftBorder { get; private set; }
        public Vector2 DownRightBorder { get; private set; }
        
        private float _width;
        private float _height;

        public SpaceScreen(Camera camera)
        {
            float cameraHeight = camera.orthographicSize;
            float cameraWidth = cameraHeight * camera.aspect;
            
            Vector3 camPos = camera.transform.position;
            
            UpLeftBorder = new Vector2(camPos.x - cameraWidth, camPos.y + cameraHeight);
            DownRightBorder = new Vector2(camPos.x + cameraWidth, camPos.y - cameraHeight);
            
            _width = DownRightBorder.x - UpLeftBorder.x;
            _height = UpLeftBorder.y - DownRightBorder.y;
        }

        public Vector2 WrapPosition(Vector2 position)
        {
            Vector2 newPosition = position;

            if (position.x > DownRightBorder.x + OFFSET)
                newPosition.x = UpLeftBorder.x - OFFSET;
            else if (position.x < UpLeftBorder.x - OFFSET)
                newPosition.x = DownRightBorder.x + OFFSET;

            if (position.y > UpLeftBorder.y + OFFSET)
                newPosition.y = DownRightBorder.y - OFFSET;
            else if (position.y < DownRightBorder.y - OFFSET)
                newPosition.y = UpLeftBorder.y + OFFSET;
            
            return newPosition;
        }

        public Vector2 GetRandomSpawnPosition()
        {
            int randomSide = Random.Range(0, 4);

            switch (randomSide)
            {
                case 0:
                    return new Vector2(
                        Random.Range(UpLeftBorder.x, DownRightBorder.x),
                        UpLeftBorder.y + OFFSET);
                case 1:
                    return new Vector2(
                        Random.Range(UpLeftBorder.x, DownRightBorder.x),
                        DownRightBorder.y - OFFSET);
                case 2:
                    return new Vector2(
                        DownRightBorder.x + OFFSET,
                        Random.Range(DownRightBorder.y, UpLeftBorder.y));
                case 3:
                    return new Vector2(
                        UpLeftBorder.x - OFFSET,
                        Random.Range(DownRightBorder.y, UpLeftBorder.y));
            }
            return Vector2.zero;
        }

        public Vector2 GetRandomDirection(Vector2 position)
        {
            Vector2 center = Vector2.zero;
            
            Vector2 direction = (center - position).normalized;
            direction += Random.insideUnitCircle * RANDOM_SPREAD;
            
            return direction.normalized;
        }
        
        public Vector2 GetRandomFragmentDirection()
        {
            return Random.insideUnitCircle.normalized;
        }
    }
}