using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 战斗控制器
/// </summary>
public class BattleController : MonoBehaviour
{
    // 角色 prefab 列表
    public GameObject[] characterPrefabs;

    // 敌人 prefab 列表
    [SerializeField] private GameObject[] hazards;

    [SerializeField] private Vector3 spawnValues;
    [SerializeField] private int hazardCount;
    [SerializeField] private float spawnWait;
    [SerializeField] private float waveWait;
    [SerializeField] private float startWait;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text restartText;

    private bool _isGameOver;
    private bool _isRestart;
    private int _score;

    void Start()
    {
        DontDestroyOnLoad(this);

        _isGameOver = false;
        _isRestart = false;
        gameOverText.enabled = false;
        restartText.enabled = false;

        _score = 0;

        UpdateScore();

        // 开始刷怪
//        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (_isRestart)
        {
            // TODO: 这里可不能重新加载场景，要改为还原现场，或者说重置所有对象的状态

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Online");
            }

#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount != 0)
            {
                SceneManager.LoadScene("Online");
            }
#endif
        }
    }

    /// <summary>
    /// 一波波地刷怪
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y,
                    spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];

                Instantiate(hazard, spawnPosition, spawnRotation);

                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);

            if (_isGameOver)
            {
#if UNITY_EDITOR
                restartText.text = "Press 'R' to Restart";
#endif
#if UNITY_ANDROID || UNITY_IOS
                restartText.text = "Touch Screen to Restart";
#endif
                restartText.enabled = true;
                _isRestart = true;
                break;
            }
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        UpdateScore();
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOverText.enabled = true;
        _isGameOver = true;
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + _score;
    }
}