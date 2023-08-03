using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public void Init(Dictionary<int, string> soundEffects, ContentManager contentManager)
        {
            foreach(var keyValuePair in soundEffects)
            {
                try {
                    var soundEffect = contentManager.Load<SoundEffect>(keyValuePair.Value);
                    SoundEffects.Add(keyValuePair.Key, soundEffect);
                } catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw new Exception($"couldn't load file with filePathName: '{keyValuePair.Value}'.");
                }
            }
        }

        public void Init(Dictionary<int, SoundEffect> soundEffects)
        {
            SoundEffects = soundEffects;
        }

        /// <summary>
        /// Plays sound once
        /// </summary>
        public void PlaySound(int index)
        {
            var soundInstance = SoundEffects[index].CreateInstance();
            soundInstance.Play();
        }

        /// <summary>
        /// Plays continuesly sound.
        /// returns the instance that is playing. stop the instance when done.
        /// </summary>
        public SoundEffectInstance StartPlaying(int index)
        {
            var soundInstance = SoundEffects[index].CreateInstance();
            soundInstance.IsLooped = true;
            soundInstance.Play();
            return soundInstance;
        }
    }
}
