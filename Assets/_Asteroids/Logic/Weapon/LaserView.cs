using Assets._Asteroids.Logic.Entities.Enemies;
using UnityEngine;

namespace Assets._Asteroids.Logic.Weapon
{
    public class LaserView : Bullet
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        private Transform _startPosition;
        private float _laserDistance;

        public void Init(Transform startPosition, float distance)
        {
            _startPosition = startPosition;
            _laserDistance = distance;
        }

        private void Update()
        {
            Vector2 origin = _startPosition.position;
            Vector2 direction = _startPosition.up;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, _laserDistance);

            if (hit.collider != null)
            {
                var enemy = hit.collider.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage();
                    SetPosition(origin, hit.point);
                }
                else
                {
                    Vector2 end = origin + direction * _laserDistance;
                    SetPosition(origin, end);
                }
            }
            else
            {
                Vector2 end = origin + direction * _laserDistance;
                SetPosition(origin, end);
            }
        }
        
        public void SetPosition(Vector3 start, Vector3 end)
        {
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }
    }
}