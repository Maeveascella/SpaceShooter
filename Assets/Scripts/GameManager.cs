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

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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
            _spawnManager.NewWave();
            _spawnManager.StartSpawning();
            _isWaveClear=false;
        }
    }

    public void Gameover()
    {
        _isGameOver=true;
    }

    public void WaveClear()
    {
        _isWaveClear=true;
    }
}
