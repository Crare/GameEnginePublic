using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Particles
{
    public static class ParticleEvents
    {
        public static event Action<int> OnParticleDestroy;
        public static void PatricleDestroy(int index) => OnParticleDestroy?.Invoke(index);

        public static event Action OnParticlesReset;
        public static void ParticlesReset() => OnParticlesReset?.Invoke();

    }
}
