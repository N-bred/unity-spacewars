using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public GameObject paneMenu;
    public GameObject panePlay;
    public GameObject paneLevelCompleted;
    public GameObject paneGameOver;
    GameObject _currentBall;
    GameObject _currentLevel;
    public GameObject[] levels;
    public Text scoreText;
    public Text levelText;
    public Text remainingText;
    public Text highscore;
    bool _isSwitchingState;

    public static GameManager Instance { get; set; }

    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    State _state;

    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value; scoreText.text = $"SCORE: {_score}"; }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value; levelText.text = $"LEVEL: {_level}"; }
    }

    private int _remaining;

    public int Remaining
    {
        get { return _remaining; }
        set { _remaining = value; remainingText.text = $"REMAINING: {_remaining}"; }
    }



    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }



    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (_currentBall == null)
                {
                    if (Remaining >= 0)
                    {
                        _currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }

                if (_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    public void playClick()
    {
        SwitchState(State.INIT);
    }

    public void SwitchState(State newState, float delay = 0f)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay = 0f)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        setState(newState, true);

    }

    void EndState()
    {
        setState(_state, false);

    }

    void setState(State state, bool value)
    {
        switch (state)
        {
            case State.MENU:
                Cursor.visible = true;
                highscore.text = $"HIGHSCORE: {PlayerPrefs.GetInt("Highscore")}";
                paneMenu.SetActive(value);
                break;
            case State.INIT:


                if (value)
                {
                    Cursor.visible = false;
                    panePlay.SetActive(value);
                    Score = 0;
                    Level = 0;
                    Remaining = 3;
                    if (_currentLevel != null)
                    {
                        Destroy(_currentLevel);
                    }
                    Instantiate(playerPrefab);
                    SwitchState(State.LOADLEVEL);
                }

                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(_currentBall);
                Destroy(_currentLevel);
                paneLevelCompleted.SetActive(value);

                Level++;
                SwitchState(State.LOADLEVEL, 2f);

                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("Highscore"))
                {
                    PlayerPrefs.SetInt("Highscore", Score);
                }
                if (!value) panePlay.SetActive(value);
                paneGameOver.SetActive(value);
                break;
        }
    }
}
