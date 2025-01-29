using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MatchScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float periodDuration;
    [SerializeField] private GameObject mainCamera;

    private static bool isArrows, isAI, isSound = true;
    public bool IsArrows { get { return isArrows; } set { isArrows = value; } }
    public bool IsAI { get { return isAI; } set { isAI = value; } }
    public bool IsSound { get { return isSound; } set { isSound = value; } }

    [Header("Objects")]
    [SerializeField] private GameObject playerBlue;
    [SerializeField] private GameObject playerRed;
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform blueGoalColor, redGoalColor;
    [SerializeField] private Collider blueGoal, redGoal;
    [SerializeField] private SpriteRenderer blueArrow, redArrow; 

    [Header("UI")]
    [SerializeField] private GameObject goalParticles;
    [SerializeField] private GameObject goalPanel;
    [SerializeField] private GameObject halfTimePanel;
    [SerializeField] private GameObject changeGoalsPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject outPanel;
    [SerializeField] private GameObject blueWins, redWins, drawWins;

    private static int blueScore, redScore;
    private float currentTime;
    public float CurrentTime { get { return currentTime; } }
    private int periodCount = 1;
    public float PeriodCount { get { return periodCount; } }
    private bool isGoal, isHalfTime, isEnd;
    private bool isHold; // Для остановки игры (не пауза). Переключает движения игроков и коллайдеры ворот

    [Header("Start Positions")]
    [SerializeField] private Transform blueTargetStartPosition;
    [SerializeField] private Transform redTargetStartPosition;
    [SerializeField] private Transform sideTriggertStartPosition;
    [SerializeField] private Transform blueGoalStartPosition, redGoalStartPosition;
    [SerializeField] private Transform blueGoalColorStartPosition, redGoalColorStartPosition;
    [SerializeField] private Transform playerBlueStartPosition, playerRedStartPosition, BallStartPosition;

    public int BlueScore { get { return blueScore; } }
    public int RedScore { get { return redScore; } }

    [Header("Audio")]
    [SerializeField] private AudioClip startWhistle;
    [SerializeField] private AudioClip halftimeWistle;
    [SerializeField] private AudioClip winCheers, draw;
    [SerializeField] private AudioClip goalWhistle, goalCheers;

    private AudioSource audioSource;

    private void Start()
    {
        isHold = false;
        currentTime = periodDuration;
        blueScore = 0;
        redScore = 0;
        audioSource = GetComponent<AudioSource>();

        if (isSound == true) mainCamera.GetComponent<AudioSource>().volume = 0.6f;
        else mainCamera.GetComponent<AudioSource>().volume = 0;

        ChooseRedControl();
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        if (!isHold && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= 0) EndOfPeriod();        
    }

    public void GoalCoroutine(string color) { StartCoroutine(Goal(color)); }

    public IEnumerator Goal(string color) //Запускает события, связанные с голом
    {
        isGoal = true;
        HoldSwitch();

        audioSource.PlayOneShot(goalWhistle);
        audioSource.PlayOneShot(goalCheers);
        goalPanel.SetActive(true);
        if (color == "Red") redScore++;
        if (color == "Blue") blueScore++;
        goalParticles.SetActive(true);

        yield return new WaitForSeconds(3);
        goalParticles.SetActive(false);

        goalPanel.SetActive(false);

        yield return new WaitForSeconds(1);

        StartCoroutine(SetToStartThenStart());
        isGoal = false;
    }

    private IEnumerator SetToStartThenStart() //Возвращает игроков и мяч в исходное положение
    {
        playerBlue.transform.position = playerBlueStartPosition.transform.position;
        playerBlue.transform.rotation = playerBlueStartPosition.transform.rotation;
        playerRed.transform.position = playerRedStartPosition.transform.position;
        playerRed.transform.rotation = playerRedStartPosition.transform.rotation;
        sideTriggertStartPosition.transform.position = sideTriggertStartPosition.transform.position;
        
        GameObject.Find("TargetBlue").transform.rotation = blueTargetStartPosition.transform.rotation;
        GameObject.Find("TargetRed").transform.rotation = redTargetStartPosition.transform.rotation;        
        GameObject.Find("SideTriggers").transform.rotation = sideTriggertStartPosition.transform.rotation;

        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        // Рандомное отклонение мяча в начале
        Vector3 ballOffset = new Vector3(Random.Range(-0.02f, 0.02f), 0, Random.Range(-0.02f, 0.02f));
        ball.transform.position = BallStartPosition.transform.position + ballOffset;

        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(startWhistle);
        HoldSwitch();
    }

    public void HoldSwitch() //Переключает режим остановки игры (после свистка)
    {
        isHold = !isHold;

        redGoal.GetComponent<BoxCollider>().enabled = !isHold;
        blueGoal.GetComponent<BoxCollider>().enabled = !isHold;

        playerBlue.GetComponent<PlayerMovement>().enabled = !isHold;

        playerBlue.GetComponent<Animator>().SetBool("isRun", !isHold);
        playerRed.GetComponent<Animator>().SetBool("isRun", !isHold);

        if (playerRed.GetComponent<PlayerMovement>()) { playerRed.GetComponent<PlayerMovement>().enabled = !isHold; }        
        if (playerRed.GetComponent<AIScript>()) { playerRed.GetComponent<AIScript>().enabled = !isHold; }

        blueArrow.enabled = !isHold;
        if (isArrows) redArrow.enabled = !isHold;
    }

    public void OutStart()
    {
        if (!isGoal && !isEnd && !isHalfTime) StartCoroutine(OutCoroutine());
    }

    private IEnumerator OutCoroutine()
    {
        audioSource.PlayOneShot(goalWhistle);
        outPanel.gameObject.SetActive(true);
        HoldSwitch();
        yield return new WaitForSeconds(3);
        StartCoroutine(SetToStartThenStart());
        outPanel.gameObject.SetActive(false);

        OutScript outScript = new OutScript();
        outScript.IsOut = false;
        Destroy(outScript);
    }

    private void EndOfPeriod()
    {
        if (periodCount == 1 && !isHalfTime) StartCoroutine(HalfTime());
        if (periodCount == 2 && !isEnd) StartCoroutine(GameOver());
        playerBlue.GetComponent<Animator>().SetBool("isRun", false);
        playerRed.GetComponent<Animator>().SetBool("isRun", false);
    }

    private IEnumerator HalfTime()
    {
        isHalfTime = true;
        HoldSwitch();
        audioSource.PlayOneShot(halftimeWistle);
        halfTimePanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        halfTimePanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        changeGoalsPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SwapSides();
        changeGoalsPanel.gameObject.SetActive(false);
        currentTime = periodDuration;
        periodCount++;
        StartCoroutine(SetToStartThenStart());
        isHalfTime = false;
    }

    private IEnumerator GameOver()
    {
        isEnd = true;
        HoldSwitch();
        audioSource.PlayOneShot(halftimeWistle);
        yield return new WaitForSeconds(1);
        WhoWin();
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(4);
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void WhoWin()
    {
        mainCamera.GetComponent<AudioSource>().Stop();
        if (blueScore > redScore)
        {
            audioSource.PlayOneShot(winCheers);
            blueWins.SetActive(true);
            goalParticles.SetActive(true);
        }
        if (blueScore < redScore) 
        { 
            audioSource.PlayOneShot(winCheers);
            redWins.SetActive(true);
            goalParticles.SetActive(true);
        }
        if (blueScore == redScore)
        {
            audioSource.PlayOneShot(draw);
            drawWins.SetActive(true);
        }
    }

    private void ChooseRedControl()
    {
        if (isArrows)
        {
            Destroy(playerRed.GetComponent<AIScript>());
            playerRed.GetComponent<PlayerMovement>().enabled = true;
        }

        if (isAI)
        {
            Destroy(playerRed.GetComponent<PlayerMovement>());
            playerRed.GetComponent<AIScript>().enabled = true;
            redArrow.enabled = false;
        }
    }

    private void SwapSides()
    {
        //Смена  ворот
        blueGoal.transform.position = redGoalColorStartPosition.transform.position;
        redGoal.transform.position = blueGoalColorStartPosition.transform.position;

        //Смена стартовых положений игроков
        Transform tempPlayerStartposition = playerBlueStartPosition;
        playerBlueStartPosition = playerRedStartPosition;
        playerRedStartPosition = tempPlayerStartposition;

        // Поворот таргетов игроков
        blueTargetStartPosition.Rotate(0, 180, 0);
        redTargetStartPosition.Rotate(0, 180, 0);

        // Поворот триггеров для AI
        sideTriggertStartPosition.Rotate(0, 180, 0);
    }
}