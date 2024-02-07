using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;
using Tools;
using System;

namespace Game
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
		public float loadNewLevelDelay = 1f;		//Delay time in seconds to restart level.
		public int pointsPerFood = 10;				//Number of points to add to player food points when picking up a food object.
		public int pointsPerSoda = 20;				//Number of points to add to player food points when picking up a soda object.
		public int wallDamage = 1;					//How much damage a player does to a wall when chopping it.
		public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip eatSound1;					//1 of 2 Audio clips to play when player collects a food object.
		public AudioClip eatSound2;					//2 of 2 Audio clips to play when player collects a food object.
		public AudioClip drinkSound1;				//1 of 2 Audio clips to play when player collects a soda object.
		public AudioClip drinkSound2;				//2 of 2 Audio clips to play when player collects a soda object.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
		
		[SerializeField] private Animator animator;                 //Used to store a reference to the Player's animator component.
		[SerializeField] private SpriteRenderer spriteRenderer;

        private int food;                           //Used to store player food points total during level.

        public int Food { get => food; }
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif



        private void OnEnable()
        {
            food = GameManager.Instance.playerCurrentFoodPoints;
            this.Register(EventID.StartLevel, InitPlayer);
        }

        private void OnDisable ()
		{
            this.Unregister(EventID.StartLevel, InitPlayer);
            GameManager.Instance.playerCurrentFoodPoints = food;
		}
		
		
		private void Update ()
		{
			animator.SetBool("playerMoving", isMoving);
			if(!GameManager.Instance.PlayersTurn || isMoving) return;
			
			int horizontal = 0;  	
			int vertical = 0;		
			
#if UNITY_STANDALONE || UNITY_WEBPLAYER
			
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			vertical = (int) (Input.GetAxisRaw ("Vertical"));
			
			if(horizontal != 0)
			{
				vertical = 0;
			}
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			if (Input.touchCount > 0)
			{
				Touch myTouch = Input.touches[0];
				
				if (myTouch.phase == TouchPhase.Began)
				{
					touchOrigin = myTouch.position;
				}
				
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					Vector2 touchEnd = myTouch.position;
					
					float x = touchEnd.x - touchOrigin.x;
					
					float y = touchEnd.y - touchOrigin.y;
					
					touchOrigin.x = -1;
					
					if (Mathf.Abs(x) > Mathf.Abs(y))
						horizontal = x > 0 ? 1 : -1;
					else
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif 
			if(horizontal != 0 || vertical != 0)
			{
				if(horizontal != 0) spriteRenderer.flipX = (horizontal > 0) ? false : true;

                AttemptMove<Wall> (horizontal, vertical);
			}
		}
        private void InitPlayer(object data = null)
        {
            isMoving = false;
            food = GameManager.Instance.playerCurrentFoodPoints;
            this.Broadcast(EventID.OnFoodChange, "Food: " + food);
        }

        protected override void AttemptMove <T> (int xDir, int yDir)
		{
			food--;
			this.Broadcast(EventID.OnFoodChange, "Food: " + food);

			base.AttemptMove <T> (xDir, yDir);
			
			RaycastHit2D hit;
			
			if (Move (xDir, yDir, out hit)) 
			{
				AudioManager.Instance.RandomizeSfx (moveSound1, moveSound2);
			}
			
			CheckIfGameOver ();
			
			GameManager.Instance.PlayersTurn = false;
		}
		
		
		protected override void OnCantMove <T> (T component)
		{
			Wall hitWall = component as Wall;
			
			hitWall.DamageWall (wallDamage);
			
			animator.SetTrigger ("playerChop");
		}
		
		
		private void OnTriggerEnter2D (Collider2D other)
		{
			if(other.tag == "Exit")
			{
				Invoke ("LoadNewLevel", loadNewLevelDelay);
			}
			
			else if(other.tag == "Food")
			{
				food += pointsPerFood;
				
				this.Broadcast(EventID.OnFoodChange, "+" + pointsPerFood + " Food: " + food);

				AudioManager.Instance.RandomizeSfx (eatSound1, eatSound2);
				
				other.gameObject.SetActive (false);
			}
			
			else if(other.tag == "Soda")
			{
				food += pointsPerSoda;
				
				this.Broadcast(EventID.OnFoodChange, "+" + pointsPerSoda + " Food: " + food);

                AudioManager.Instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				other.gameObject.SetActive (false);
			}
		}
		
		
		private void LoadNewLevel()
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
			this.Broadcast(EventID.NewLevel);
		}
		
		
		public void LoseFood (int loss)
		{
			animator.SetTrigger ("playerHit");
			
			food -= loss;
			
			this.Broadcast(EventID.OnFoodChange, "-" + loss + " Food: " + food);

			CheckIfGameOver ();
		}
		
		
		private void CheckIfGameOver ()
		{
			if (food <= 0) 
			{
				AudioManager.Instance.PlaySingle (gameOverSound);
				
				AudioManager.Instance.musicSource.Stop();
				
				this.Broadcast(EventID.GameOver);
			}
		}
	}
}

