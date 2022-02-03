using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject scoreObject, startButton, endPanel,circle;

    [SerializeField]
    private TMP_Text scoreText, scoreEndText, highScoreText, highScoreEndText;

    private int score, highScore;

    [SerializeField] private GameObject linePrefab;

    [SerializeField] Vector3 startPos;

    [SerializeField] private float offset;

    private Vector3 currentDirection;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        scoreObject.SetActive(false);
        endPanel.SetActive(false);
        startButton.SetActive(true);
    }

    private void Start()
    {
        score = 0;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();

        currentDirection = Vector3.right;
        SpawnStart();
        InvokeRepeating("SpawnObstacle", 0f, 1f);
    }

    void SpawnStart()
    {
        var currentObstacle = Instantiate(linePrefab);
        currentObstacle.transform.position = startPos + currentDirection * offset * 0.5f;
        startPos += currentDirection * offset;
        currentObstacle = Instantiate(linePrefab);
        currentObstacle.transform.position = startPos + currentDirection * offset * 0.5f;
        startPos += currentDirection * offset;

    }

    void SpawnObstacle()
    {
        var currentObstacle = Instantiate(linePrefab);
        var rotation = Quaternion.AngleAxis(Random.Range(-3, 4) * 15f,Vector3.forward);
        currentObstacle.transform.rotation = rotation;
        float currentAngle = Mathf.PI * rotation.eulerAngles.z / 180f;
        currentDirection = new Vector3(Mathf.Cos(currentAngle),Mathf.Sin(currentAngle),0);
        currentObstacle.transform.position = startPos + currentDirection * offset * 0.5f;
        startPos += currentDirection * offset;
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void GameStart()
    {
        startButton.SetActive(false);
        scoreObject.SetActive(true);
        circle.GetComponent<Player>().GameStart();
    }

    public void GameEnd()
    {
        circle.GetComponent<Player>().GameOver();

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        scoreObject.SetActive(true);
        endPanel.SetActive(true);

        scoreEndText.text = score.ToString();
        highScoreEndText.text = highScore.ToString();
    }


    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
