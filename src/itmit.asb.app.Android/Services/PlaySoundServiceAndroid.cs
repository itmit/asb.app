using System.Threading;
using Android.Content.Res;
using Android.Media;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlaySoundServiceAndroid))]
namespace itmit.asb.app.Droid.Services
{
	public class PlaySoundServiceAndroid : IPlaySoundService
	{
		private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
		private AssetFileDescriptor _assetFileDescriptor;

		public void PlayAudio(string path)
		{
			PlayAudio(path, false);
		}

		public void PlayAudio(string path, bool isLooping)
		{
			StopAudio();
			_assetFileDescriptor = Android.App.Application.Context.Assets.OpenFd("alert.mp3");

			_mediaPlayer.SetDataSource(_assetFileDescriptor.FileDescriptor, _assetFileDescriptor.StartOffset, _assetFileDescriptor.Length);
			
			_mediaPlayer.Looping = isLooping;
			_mediaPlayer.Prepare();
			_mediaPlayer.Start();
		}

		public void StopAudio()
		{
			if(_mediaPlayer.IsPlaying)
			{
				_mediaPlayer.Stop();
			}

			_mediaPlayer.Reset();
		}
	}
}
