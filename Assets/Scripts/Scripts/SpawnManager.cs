using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private float _spawnrate = 2.5f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private int _enemiesKilled;
    [SerializeField]
    private int _toClear = 3;
    [SerializeField]
    private int _spawnCount;


    [SerializeField]
    private GameObject _gunnerPrefab;
    [SerializeField]
    private GameObject _aggressivePrefab;
    [SerializeField]
    private GameObject _smartEnemyPrefab;

    [SerializeField]
    private GameObject _boss1;

    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _ammo;
    [SerializeField]
    private GameObject _lifeUp;

    private GameManager _gameManager;
    private UIManager _uiManager;


    



    private bool _isPlayerDead = false;
    [SerializeField]
    private bool _isWaveClear = false;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if ( _gameManager == null )
        {
            Debug.LogError("Game Manager Not Found");
        }

        if ( _uiManager == null)
        {
            Debug.LogError("UI Manager Not Found");
        }
    }

     public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(AmmoSpawnRoutine());
        StartCoroutine(LifeUpSpawnRoutine());
        StartCoroutine(RarePowerupSpawnRoutine());
        StartCoroutine(GunnerSpawnRoutine());
        StartCoroutine(AggressiveEnemyRoutine());
        StartCoroutine(SmartEnemyRoutine());
    }


    private IEnumerator EnemySpawnRoutine()
    {
        while (_isPlayerDead == false && _spawnCount < _toClear)
        {
            yield return new WaitForSeconds(2.5f);
            UpdateSpawnCount();
            Vector3 posToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnrate);
        }
    }

    private IEnumerator PowerupSpawnRoutine()
    {
        while (_isPlayerDead == false && _isWaveClear == false)
        {
            yield return new WaitForSeconds(2.5f);

            float powerupSpawnrate = Random.Range(3f, 7f);
            Vector3 posToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            int randomPowerup = Random.Range(0, 4);
            GameObject newPowerup = Instantiate(_powerUps[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(powerupSpawnrate);
        }
    }

    private IEnumerator AmmoSpawnRoutine()
    {
        while (_isPlayerDead == false && _isWaveClear == false)
        {
            yield return new WaitForSeconds(5f);

            float ammoSpawnrate = Random.Range(5f, 10f);
            Vector3 posToSpawn = new Vector3(Random.Range(-7.5f, 7.5f), 8f, 0);
            Instantiate(_ammo, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(ammoSpawnrate);
        }
    }

    private IEnumerator LifeUpSpawnRoutine()
    {
        while (_isPlayerDead == false && _isWaveClear == false)
        {
            yield return new WaitForSeconds(Random.Range(25f, 40f));

            float lifeUpSpawnrate = Random.Range(25, 40f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), Random.Range(-3.8f, 0), 0);
            Instantiate(_lifeUp, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(lifeUpSpawnrate);
        }
    }

    private IEnumerator RarePowerupSpawnRoutine()
    {
       while(_isPlayerDead == false && _isWaveClear == false)
        {
            yield return new WaitForSeconds(Random.Range(20f, 25f));

            float rapidFireSpawnRate = Random.Range(20f, 25f);
            Vector3 posToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            int randomRarePowerup = Random.Range(4, 6);
            GameObject newPowerup = Instantiate(_powerUps[randomRarePowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(rapidFireSpawnRate);
        }
    }

    private IEnumerator GunnerSpawnRoutine()
    {
        while(_isPlayerDead == false && _spawnCount < _toClear)
        {
            yield return new WaitForSeconds(Random.Range(20f, 35f));
            UpdateSpawnCount();
            Vector3 posToSpawn = new Vector3(Random.Range(-7.5f, 7.5f), 7.5f, 0);
            Instantiate(_gunnerPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }

    }

    private IEnumerator AggressiveEnemyRoutine()
    {
        while(_isPlayerDead == false && _spawnCount < _toClear)
        {
            yield return new WaitForSeconds(10f);
            UpdateSpawnCount();
            Vector3 posToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            Instantiate(_aggressivePrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10f, 15f));
        }
    }

    private IEnumerator SmartEnemyRoutine()
    {
        while (_isPlayerDead == false && _spawnCount < _toClear)
        {
            yield return new WaitForSeconds(20f);
            UpdateSpawnCount();
            Vector3 posToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            Instantiate(_smartEnemyPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10f, 15f));
        }
    }

    public void PlayerDeath()
    {
        _isPlayerDead = true;
    }

    void WaveClear()
    {
        _isWaveClear = true;
        _uiManager.WaveCleared();
        StopAllCoroutines();
    }

    public void BossWaveClear()
    {
        _isWaveClear = true;
        StopAllCoroutines();
    }


    public void EnemyKilled()
    {
        _enemiesKilled++;
        if (_enemiesKilled == _toClear)
        {
            WaveClear();
            _gameManager.WaveClear();
        }
    }

    public void NewWave()
    {
        _spawnCount = 0;
        _enemiesKilled = 0;
        _toClear = _toClear + 5;
        _isWaveClear = false;
    }

    private void UpdateSpawnCount()
    {
        _spawnCount++;
        if (_spawnCount == _toClear)
        {
            StopCoroutine(EnemySpawnRoutine());
            StopCoroutine(AggressiveEnemyRoutine());
            StopCoroutine(GunnerSpawnRoutine());
            StopCoroutine(SmartEnemyRoutine());
        }
    }

    public void WaveThreeBoss()
    {
        Instantiate(_boss1, new Vector3(-.42f, 11, 0), Quaternion.identity);
        _isWaveClear = false;
        StartCoroutine(AmmoSpawnRoutine());
    }
    

}
