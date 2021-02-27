using System;

namespace SceneManagement
{
    [Serializable]
    public class SceneProperties
    {
        public enum Topic
        {
            Mass, Force, Speed, Velocity, Acceleration, KineticEnergy, Momentum, PotentialEnergy
        }

        public string Name;
        public Topic[] Topics;
    }
}
