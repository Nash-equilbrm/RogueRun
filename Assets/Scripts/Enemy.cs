using UnityEngine;
using System.Collections;

namespace Game
{
	public class Enemy : MovingObject
	{
		public int playerDamage; 							//The amount of food points to subtract from the player when attacking.
		public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
		public AudioClip attackSound2;                      //Second of two audio clips to play when attacking the player.

		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		
		
		protected override void Start ()
		{
			GameManager.Instance.AddEnemyToList (this);
			
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			
			base.Start ();
		}
        private void Update()
		{
			animator.SetBool("enemyMoving", isMoving);
		}

        protected override void AttemptMove <T> (int xDir, int yDir)
		{
			if(skipMove)
			{
				skipMove = false;
				return;
				
			}
			
			base.AttemptMove <T> (xDir, yDir);
			
			skipMove = true;
		}
		
		
		public void MoveEnemy ()
		{
			int xDir = 0;
			int yDir = 0;
			
			if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
				
				yDir = target.position.y > transform.position.y ? 1 : -1;
			
			else
				xDir = target.position.x > transform.position.x ? 1 : -1;
			
			AttemptMove <Player> (xDir, yDir);
            spriteRenderer.flipX = (xDir > 0) ? false : true;
        }


        protected override void OnCantMove <T> (T component)
		{
			Player hitPlayer = component as Player;
			
			animator.SetTrigger ("enemyAttack");

			StartCoroutine(nameof(AttackPlayerCoroutine), hitPlayer);
			
			AudioManager.Instance.RandomizeSfx (attackSound1, attackSound2);
		}

		private float _timer = 0f;
        private float _attackAnimDuration = 0.3f;

        private IEnumerator AttackPlayerCoroutine(Player hitPlayer)
		{
			while(_timer < _attackAnimDuration)
			{
				_timer += Time.deltaTime;
				yield return null;
			}
			_timer = 0f;
            hitPlayer.LoseFood(playerDamage);
        }

    }
}
