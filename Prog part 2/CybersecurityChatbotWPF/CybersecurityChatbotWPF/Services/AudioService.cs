using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace CybersecurityChatbotWPF.Services
{
    public class AudioService
    {
        public async Task PlayGreetingAsync()
        {
            string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "greeting.wav");
            
            if (File.Exists(audioPath))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        using (var soundPlayer = new SoundPlayer(audioPath))
                        {
                            soundPlayer.PlaySync();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error playing audio: {ex.Message}");
                    }
                });
            }
        }
        
        public async Task PlaySoundAsync(string soundPath)
        {
            if (File.Exists(soundPath))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        using (var soundPlayer = new SoundPlayer(soundPath))
                        {
                            soundPlayer.Play();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error playing sound: {ex.Message}");
                    }
                });
            }
        }
    }
}