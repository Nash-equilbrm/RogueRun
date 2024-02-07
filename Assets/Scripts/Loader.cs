using UnityEngine;
using System.Collections;

namespace Game
{	
	public class Loader : MonoBehaviour 
	{
		public Transform managersParent;
		public GameObject gameManager;			//GameManager prefab to instantiate.
		public GameObject soundManager;			//SoundManager prefab to instantiate.
		
		
		void Awake ()
		{
			//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
			if (GameManager.Instance == null)
				
				//Instantiate gameManager prefab
				Instantiate(gameManager, managersParent);
			
			//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
			if (AudioManager.Instance == null)
				
				//Instantiate SoundManager prefab
				Instantiate(soundManager, managersParent);
		}
	}
}