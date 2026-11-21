using System;
using System.Windows.Media;

namespace WpfApp2;

public class AudioService
{
    private readonly MediaPlayer _musicPlayer = new();
    private readonly MediaPlayer _effectPlayer = new();

    public void PlayMusic(string fileName, bool loop = true)
    {
        _musicPlayer.Stop();
        _musicPlayer.Open(new Uri($"Media/Music/{fileName}", UriKind.Relative));
        _musicPlayer.MediaEnded -= MusicPlayer_MediaEnded;

        if (loop)
            _musicPlayer.MediaEnded += MusicPlayer_MediaEnded;

        _musicPlayer.Play();
    }

    private void MusicPlayer_MediaEnded(object? sender, EventArgs e)
    {
        _musicPlayer.Position = TimeSpan.Zero;
        _musicPlayer.Play();
    }

    public void PlayEffect(string fileName)
    {
        _effectPlayer.Stop();
        _effectPlayer.Open(new Uri($"Media/Sounds/{fileName}", UriKind.Relative));
        _effectPlayer.Volume = 0.7;
        _effectPlayer.Play();
    }

    public void StopAll()
    {
        _musicPlayer.Stop();
        _effectPlayer.Stop();
    }
}
