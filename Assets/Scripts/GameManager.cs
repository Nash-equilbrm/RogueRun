using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game
{
    using Game.GameFlow;
    using System.Collections.Generic;       
    using Tools;

    public class GameManager : Singleton<GameManager>
    {
		public Transform world;
        public GameObject _playerPrefab;
		internal Player _player;
        public float levelStartDelay = 2f;						
		public readonly int playerStartFoodPoints = 100;
        public int playerCurrentFoodPoints;
        private bool playersTurn = true;


        private BoardManager boardScript;						
		[SerializeField] private int _curLevel = 0;		
        public int CurLevel { get => _curLevel; }

        private List<Enemy> enemies;	
        public List<Enemy> Enemies { get => enemies; }

        private bool doingSetup = true;
        internal bool DoingSetup { get => doingSetup; set => doingSetup = value; }
        public bool PlayersTurn { get => playersTurn; set => playersTurn = value; }


        #region State machine
        private StateMachine<GameManager> _stateMachine;
		private GameMainMenuState _mainMenuState;
        private NewLevelLoadState _newLevelLoadState;
        private GamePlayState _gameplayState;
        private GameOverState _gameOverState;

        #endregion



        protected override void Awake()
		{
			base.Awake();
			enemies = new List<Enemy>();
			
			boardScript = GetComponent<BoardManager>();

			_stateMachine = new StateMachine<GameManager>();
			_mainMenuState = new GameMainMenuState(this);
            _newLevelLoadState = new NewLevelLoadState(this);
            _gameplayState = new GamePlayState(this);
            _gameOverState = new GameOverState(this);

            _stateMachine.Initialize(_mainMenuState);
		}



        //Initializes the game for each level.
        internal void InitGame(bool increaseLevel = false)
		{
            if(increaseLevel) { 
                _curLevel++; 
            }
           
            SpawnPlayer();
			enemies.Clear();
			boardScript.SetupScene(_curLevel);
		}
		
		internal void ResetGame()
        {
            _curLevel = 0;
            playerCurrentFoodPoints = playerStartFoodPoints;
        }


        void Update()
		{
			_stateMachine.CurrentState.LogicUpdate();
			
		}
		
		public void AddEnemyToList(Enemy script)
		{
			enemies.Add(script);
		}
		

		private void SpawnPlayer()
		{
            if (_player == null)
            {
                _player = Instantiate(_playerPrefab, world).GetComponent<Player>();
            }
            else
            {
                _player.transform.localPosition = Vector3.zero;
            }
        }
		
		


		public void ChangeState(GameState state)
		{
			switch (state)
			{
				case GameState.MainMenu:
					{
						_stateMachine.ChangeState(_mainMenuState);
						break;
					}
                case GameState.NewLevel:
                    {
                        _stateMachine.ChangeState(_newLevelLoadState);
                        break;
                    }
                case GameState.Gameplay:
                    {
                        _stateMachine.ChangeState(_gameplayState);
                        break;
                    }
                case GameState.Gameover:
                    {
                        _stateMachine.ChangeState(_gameOverState);
                        break;
                    }
                default: break;
            }
		}
	}
}

