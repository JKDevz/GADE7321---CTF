using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region VARIABLES
    [Header("--- UI manager references")]
    public GameObject UICanvas;
    public TMP_Text playerScore, aiScore;
    public TMP_Text currentRound;
    [Space]
    public Animator toastAnimator;
    public TMP_Text uiToastText;
    public Color blue;
    public Color red;

    [Header("--- Game Over Screens")]
    public GameObject gameOverCanvas;
    public TMP_Text victoryText;
    public TMP_Text playerEndScore;
    public TMP_Text aiEndScore;

    private static UIManager _instance;

    #endregion

    #region SINGLETON STUFF

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UIManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("UIManager");
                    _instance = singleton.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
        private set { }
    }

    #endregion

    #region ENABLE/DISABLE

    private void OnEnable()
    {
        GameManager.onScoreUpdated += UpdateHUD;
        GameManager.onGameFinished += OnGameFinish;
        Flag.onFlagPickup += FlagToast;
    }

    private void OnDisable()
    {
        GameManager.onScoreUpdated -= UpdateHUD;
        GameManager.onGameFinished -= OnGameFinish;
        Flag.onFlagPickup -= FlagToast;
    }

    private void Start()
    {
        UpdateHUD();
    }

    #endregion

    #region METHODS

    public void UpdateHUD()
    {
        playerScore.text = GameManager.Instance.playerScore.ToString();
        aiScore.text = GameManager.Instance.aiScore.ToString();
        currentRound.text = "Round " + GameManager.Instance.currentRound.ToString();
    }

    public void OnGameFinish()
    {
        UICanvas.SetActive(false);
        gameOverCanvas.SetActive(true);

        aiEndScore.text = GameManager.Instance.aiScore.ToString();
        playerEndScore.text = GameManager.Instance.playerScore.ToString();

        if (GameManager.Instance.playerScore > GameManager.Instance.aiScore)
        {
            victoryText.text = "Blue player Wins!";
        }
        else
        {
            victoryText.text = "Red player Wins!";
        }
    }

    private void FlagToast(FlagType type)
    {
        toastAnimator.SetTrigger("OnToast");

        if (type == FlagType.Red)
        {
            uiToastText.text = "Red Flag was Picked Up!";
            uiToastText.color = red;
        }
        else if (type == FlagType.Blue)
        {
            uiToastText.text = "Blue Flag was Picked Up!";
            uiToastText.color = blue;
        }
    }

    #endregion
}
