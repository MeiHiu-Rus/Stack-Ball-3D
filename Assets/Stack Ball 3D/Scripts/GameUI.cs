using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI;
    public GameObject allbuttons;

    private bool buttons;

    [Header("PreGame")]
    public Button soundButton;
    public Sprite soundOns, soundOff;

    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevetImg;
    public Image nextLevelImg;
    public Text currentLevelText, nextLevelText;

    [Header("Finish")]
    public Text finishLevelText;

    [Header("GameOver")]
    public Text gameOverScoreText;
    public Text gameOverBestText;

    private Material ballMat;
    private Ball ball;

    private void Awake()
    {
        ballMat = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        ball = FindObjectOfType<Ball>();
        levelSlider.transform.parent.GetComponent<Image>().color = ballMat.color + Color.gray;
        levelSlider.color = ballMat.color;
        currentLevetImg.color = ballMat.color;
        nextLevelImg.color = ballMat.color;

        soundButton.onClick.AddListener(() => SoundManager.instance.SoundOff());
    }

    private void Start()
    {
        currentLevelText.text = FindObjectOfType<LevelSpawner>().level.ToString();
        nextLevelText.text = FindObjectOfType<LevelSpawner>().level + 1 + "";
    }
    // Update is called once per frame
    void Update()
    {
        if(ball.ballState == Ball.BallState.Prepare)
        {
            if (SoundManager.instance.sound && soundButton.GetComponent<Image>().sprite != soundOns)
                soundButton.GetComponent<Image>().sprite = soundOns;
            else if (!SoundManager.instance.sound && soundButton.GetComponent<Image>().sprite != soundOff)
                soundButton.GetComponent<Image>().sprite = soundOff;
        }
        
        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && ball.ballState == Ball.BallState.Prepare)
        {
            ball.ballState = Ball.BallState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            finishUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        if(ball.ballState == Ball.BallState.Finish){
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level1 " + FindObjectOfType<LevelSpawner>().level;


        }

        if (ball.ballState == Ball.BallState.Died){
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = ScoreManager.instance.score.ToString();
            gameOverBestText.text = PlayerPrefs.GetInt("HightScore").ToString();

            if(Input.GetMouseButtonDown(0))
            {
                ScoreManager.instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void Setting()
    {
        buttons = !buttons;
        allbuttons.SetActive(buttons);
    }
}
