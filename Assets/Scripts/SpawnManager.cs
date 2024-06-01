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
    private GameObject[] Powerups;
    [SerializeField]
    private GameObject _Ammo;
    [SerializeField]
    private GameObject _LifeUp;

    private GameManager _gameManager;

    



    private bool isPlayerDead = false;
    [SerializeField]
    private bool isWaveClear = false;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (_enemiesKilled == _toClear)
        {
            WaveClear();
            _gameManager.WaveClear();
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(AmmoSpawnRoutine());
        StartCoroutine(LifeUpSpawnRoutine());
        StartCoroutine(RapidFireSpawnRoutine());
    }


    private IEnumerator EnemySpawnRoutine()
    {
        while (isPlayerDead == false && _spawnCount < _toClear)
        {
            yield return new WaitForSeconds(2.5f);
            _spawnCount++;
            Vector3 postospawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, postospawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnrate);
        }
    }

    private IEnumerator PowerupSpawnRoutine()
    {
        while (isPlayerDead == false && isWaveClear == false)
        {
            yield return new WaitForSeconds(2.5f);

            float _PowerupSpawnrate = Random.Range(3f, 7f);
            Vector3 PosToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(Powerups[randomPowerup], PosToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_PowerupSpawnrate);
        }
    }

    private IEnumerator AmmoSpawnRoutine()
    {
        while (isPlayerDead == false && isWaveClear == false)
        {
            yield return new WaitForSeconds(5f);

            float _ammoSpawnrate = Random.Range(5f, 10f);
            Vector3 PosToSpawn = new Vector3(Random.Range(-7.5f, 7.5f), 8f, 0);
            Instantiate(_Ammo, PosToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_ammoSpawnrate);
        }
    }

    private IEnumerator LifeUpSpawnRoutine()
    {
        while (isPlayerDead == false && isWaveClear == false)
        {
            yield return new WaitForSeconds(Random.Range(25f, 40f));

            float _lifeUPspawnrate = Random.Range(25, 40f);
            Vector3 PosTospawn = new Vector3(Random.Range(-8f, 8f), Random.Range(-3.8f, 0), 0);
            Instantiate(_LifeUp, PosTospawn, Quaternion.identity);
            yield return new WaitForSeconds(_lifeUPspawnrate);
        }
    }

    private IEnumerator RapidFireSpawnRoutine()
    {
       while(isPlayerDead == false && isWaveClear == false)
        {
            yield return new WaitForSeconds(Random.Range(20f, 25f));

            float _rapidFirespawnrate = Random.Range(20f, 25f);
            Vector3 PosToSpawn = new Vector3(Random.Range(-10.5f, 10.5f), 7.5f, 0);
            GameObject newPowerup = Instantiate(Powerups[3], PosToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_rapidFirespawnrate);
        }
    }
    
    public void PlayerDeath()
    {
        isPlayerDead = true;
    }

    void WaveClear()
    {
        isWaveClear = true;
        StopAllCoroutines();
    }

    private IEnumerator WaveCountDown()
    {
        yield return new WaitForSeconds(3f);
    }

    public void EnemyKilled()
    {
        _enemiesKilled++;
    }

    public void NewWave()
    {
        StartCoroutine(WaveCountDown());
        _spawnCount = 0;
        _enemiesKilled = 0;
        _toClear++;
        isWaveClear = false;
    }

}
