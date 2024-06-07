using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    private bool _isWaveClear = false;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private int _waveCount = 1;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Main Game Scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.F) && _isWaveClear == true)
        {
            _waveCount++;
            _uiManager.UpdateWave(_waveCount);
            _isWaveClear = false;
            if (_waveCount == 3)
            {
                _spawnManager.WaveThreeBoss();
            }
            else
            {
                _spawnManager.NewWave();
                _spawnManager.StartSpawning();
            }
        }
    }

    public void Gameover()
    {
        _isWaveClear=false;
        _isGameOver=true;
    }

    public void WaveClear()
    {
        _isWaveClear=true;
        _uiManager.WaveCleared();
    }
}
