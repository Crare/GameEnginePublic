using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Audio
{
    public sealed class AudioManager
    {
        private static AudioManager instance = null; // singleton
        private static readonly object padlock = new();
        public static AudioManager Instance
        {
            get
            {
                // locks the Singleton instance to be once created by locking one of its objects.
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AudioManager();
                    }
                    return instance;
                }
            }
        }

        public Dictionary<int, SoundEffect> SoundEffects = new();

        public AudioManager() { }

        public void Init(Dictionary<int, SoundEffect> soundEffects)
        {
            SoundEffects = soundEffects;
        }

        public void PlaySound(int index)
        {
            var soundInstance = SoundEffects[index].CreateInstance();
            soundInstance.Play();
        }
    }
}
