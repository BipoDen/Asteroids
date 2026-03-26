using System;

namespace Assets._Asteroids.Logic.Gameplay
{
    [Serializable]
    public class SaveData
    {
        public int MaxScore { get; set; }
        public bool IsAdDisabled { get; set; }
        public long LastModified { get; set; }
    }
}