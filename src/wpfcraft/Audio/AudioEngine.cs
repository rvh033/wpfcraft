using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace wpfcraft.Audio
{
    internal class AudioEngine
    {
        static WaveOutEvent OutputDevice;
        static AudioFileReader AudioFile;
        static string WorkingDir = Directory.GetCurrentDirectory();

        public static void Play(string file)
        {
            OutputDevice = new WaveOutEvent();
            OutputDevice.PlaybackStopped += OnStopped;
            AudioFile = new AudioFileReader($"{WorkingDir}/Sound/{file}");
            OutputDevice.Init(AudioFile);
            OutputDevice.Play();
        }

        static void OnStopped(object sender, EventArgs a)
        {
            OutputDevice?.Stop();
        }
    }
}
