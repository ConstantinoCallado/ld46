using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnums
{
    public static bool DEBUGGING = true;

    public enum FlowStates { TitleScreen = 0, Preparing = 1, Tutorial=2, Playing = 3 };

    public enum MoodStates { RageQuit = -2, Bored = -1, Neutral = 0, HavingFun = 1, OnFire = 2 };
    public enum MusicColor { Magenta = 0, Cyan = 1, Yellow = 2, White = 3 };
    public enum DancerStateNames { Inactive = 0, Created = 1, MoodActive = 2, Dancing = 3, Leaving = 4 };
    public enum TurnTable { Left = 0, Right = 1 };
    public enum PartyStatus { Dead = 0, WarmingUp = 1, Super = 2, PartyHard = 3};
    public enum MusicStatus { Blocked = 0, TooSoon = 1, Perfect = 2, TooLate = 3 };

}