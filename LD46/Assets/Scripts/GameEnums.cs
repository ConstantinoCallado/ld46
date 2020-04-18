using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnums
{
    public static bool DEBUGGING = true;

    public enum MoodStates { RageQuit = -2, Bored = -1, Neutral = 0, HavingFun = 1, OnFire = 2 };
    public enum MusicColor { Magenta = 0, Cyan = 1, Yellow = 2, White = 3 };
    public enum DancerStateNames { Inactive = 0, Created = 1, Dancing = 2, Leaving = 3 };
    public enum TurnTable { Left = 0, Right = 1 };

}