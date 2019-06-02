namespace Nirvana
{
    public partial class NParticleSystem
    {
        public enum SimulationSpace
        {
            Local = 1,
            World = 0
        }

        public enum EmissionShape
        {
            Sphere,
            Edge
        }

        public enum ColorMode
        {
            Random,
            OverLifetime
        }

        public enum SizeMode
        {
            Random,
            OverLifetime
        }
    }
}