namespace itmit.asb.app.Services
{
	public interface IPlaySoundService
	{
		void PlayAudio(string path);
		void PlayAudio(string alarmTone, bool isLooping);
		void StopAudio();
	}
}
