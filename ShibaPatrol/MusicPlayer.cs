using NAudio.Wave;

namespace ShibaPatrol
{
    public class MusicPlayer
    //Claude AI was used extensively in the creation of the music player in trying to understand it.
    {
        private static AudioFileReader _reader;
        private static WaveOutEvent _output;
        private static bool _looping = true;
        
            public static void Play(string filename)
            {
                _looping = true;

                if (_output != null)
                {
                    _output.PlaybackStopped -= OnPlaybackStopped;
                    _output.Stop();
                    try
                    {
                        _output.Dispose();
                    }
                    catch (InvalidOperationException) { }
                    finally
                    {
                        _output = null;
                    }
                }

                _reader?.Dispose();
                _reader = null;  

                _reader = new AudioFileReader(filename);
                _output = new WaveOutEvent();
                _output.Init(_reader);
                _output.PlaybackStopped += OnPlaybackStopped;
                _output.Play();
            }

     private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
{
    if (_looping && _reader != null && _output != null)
    {
        try
        {
            _reader.Position = 0;
            _output.Init(_reader);
            _output.Play();
        }
        catch (ObjectDisposedException) { }
        catch (InvalidOperationException) { }
    }
}

        public static void MusicStop()
        {
            _looping = false;
            _output?.Stop();
        }
    }
}