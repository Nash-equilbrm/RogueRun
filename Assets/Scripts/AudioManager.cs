using UnityEngine;
using System.Collections;
using Tools;

namespace Game
{
	public class AudioManager : Singleton<AudioManager> 
	{
		public AudioSource sfxSource;					//Drag a reference to the audio source which will play the sound effects.
		public AudioSource musicSource;					//Drag a reference to the audio source which will play the music.
		public float lowPitchRange = .95f;				//The lowest a sound effect will be randomly pitched.
		public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

        private void OnEnable()
        {
			this.Register(EventID.OnMusicChanged, OnMusicChanged);
            this.Register(EventID.OnSFXChanged, OnSFXChanged);

        }

        private void OnDisable()
        {
            this.Unregister(EventID.OnMusicChanged, OnMusicChanged);
            this.Unregister(EventID.OnSFXChanged, OnSFXChanged);
        }

        private void OnSFXChanged(object data)
        {
            if(data != null)
			{
				float value = (float)data;
				sfxSource.volume = value;
            }
        }

        private void OnMusicChanged(object data)
        {
            if (data != null)
            {
                float value = (float)data;
                musicSource.volume = value;
            }
        }


        public void PlaySingle(AudioClip clip)
		{
			sfxSource.clip = clip;
			
			sfxSource.Play ();
		}
		
		
		public void RandomizeSfx (params AudioClip[] clips)
		{
			int randomIndex = Random.Range(0, clips.Length);
			
			float randomPitch = Random.Range(lowPitchRange, highPitchRange);
			
			sfxSource.pitch = randomPitch;
			
			sfxSource.clip = clips[randomIndex];
			
			sfxSource.Play();
		}
	}
}
