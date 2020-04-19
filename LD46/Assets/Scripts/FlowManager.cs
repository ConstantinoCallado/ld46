using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public GameEnums.FlowStates currentState = GameEnums.FlowStates.TitleScreen;
    //    public enum FlowStates { TitleScreen = 0, Preparing = 1, Tutorial=2, Playing = 3 };

    private GameEnums.FlowStates nextScene = GameEnums.FlowStates.TitleScreen;

    void Awake()
    {
        currentState = GameEnums.FlowStates.TitleScreen;
        nextScene = currentState;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStateStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeScene(GameEnums.FlowStates nextState)
    {
        currentState = nextState;
        OnStateStart();
    }

    void OnStateStart()
    {
        if (currentState == GameEnums.FlowStates.TitleScreen)
        {
            OnTitleScreenStart();
        }
        else if (currentState == GameEnums.FlowStates.Preparing)
        {
            OnPreparingStart();
        }
        else if (currentState == GameEnums.FlowStates.Tutorial)
        {
            OnTutorialStart();
        }
        else if (currentState == GameEnums.FlowStates.Playing)
        {
            OnPlayingStart();
        }
    }

    void OnTitleScreenStart()
    {

    }

    void OnPreparingStart()
    {

    }

    void OnTutorialStart()
    {

    }

    void OnPlayingStart()
    {

    }
    
}
