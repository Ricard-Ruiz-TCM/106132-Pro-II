using SFML.Audio;
using SFML.System;
using System.Collections.Generic;

namespace TcGame
{
    public class SoundManager
    {
        private List<Sound> sounds = new List<Sound>();

        public void Update(float deltaSeconds)
        {
            var soundsToRemove = sounds.FindAll(x => x.Status == SoundStatus.Stopped);
            soundsToRemove.ForEach(x => x.Dispose());
            sounds.RemoveAll(soundsToRemove.Contains);
        }

        public void PlaySound(string soundName, float volume = 100.0f)
        {
            SoundBuffer buffer = Resources.Sound(soundName);
            Sound sound = new Sound(buffer);
            sound.Volume = volume;
            sound.Play();
            sounds.Add(sound);
        }
    }
}
