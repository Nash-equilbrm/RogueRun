using Game.UI;
using Tools;
using UnityEngine;


namespace Game.GameFlow
{
    public class NewLevelLoadState : State<GameManager>
    {
        private float _levelStartDelay = 2f;
        private float _timer = 0f;
        public NewLevelLoadState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _timer = 0f;
            _context.DoingSetup = true;

            _context.InitGame(increaseLevel: true);

            _context.playerCurrentFoodPoints = _context._player.Food;
            UIManager.Instance.ShowScreen<LevelChangeScreen>(data: _context.CurLevel, forceShowData: true);
        }

        public override void LogicUpdate()
        {
            if(_timer < _levelStartDelay)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _context.ChangeState(GameState.Gameplay);
                _timer = 0f;
            }
        }

        public override void Exit()
        {
            if(_context.CurLevel == 1)
            {
                _context.playerCurrentFoodPoints = _context.playerStartFoodPoints;
            }
            HideLevelScreen();
            _context.Broadcast(EventID.StartLevel);
            base.Exit();
        }


        void HideLevelScreen()
        {
            UIManager.Instance.HideAllScreens();
            _context.DoingSetup = false;
        }
    }
}

