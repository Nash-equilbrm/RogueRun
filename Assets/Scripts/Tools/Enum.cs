using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public enum EventID
    {
        StartGame,
        StartLevel,
        NewLevel,
        GameOver,
        BackToMenu,
        OnFoodChange,
        OnMusicChanged,
        OnSFXChanged
    }

    public enum UIType
    {
        Unknown,
        Notify,
        Popup,
        Screen,
        Overlap
    }

    public enum GameState
    {
        MainMenu,
        NewLevel,
        Gameplay,
        Gameover
    }
}
