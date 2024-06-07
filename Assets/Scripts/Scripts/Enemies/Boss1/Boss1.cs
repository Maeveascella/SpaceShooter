using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour

{
    [SerializeField]
    private GameObject _bossBombs, _rainFirePrefab, _targettedShotPrefab, _explosionPrefab;
    [SerializeField]
    private GameObject[] _enemies;
    private float _fireRate, _canFire;

    private int _bossHealth = 100;

    private bool _targettedShotReady = false;
    private bool _isBossAlive = true;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private AudioSource _explosionAudio;
    private SpawnManager _spawnManager;
    private Player _player;
    [SerializeField]
    private GameObject _enemyContainer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BombSpawnRoutine());
        StartCoroutine(TargetShotRoutine());
        StartCoroutine(ReleaseEnemyRoutine());

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager Not Found");
        }
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Not Found");
        }
        if (_explosionAudio == null)
        {
            Debug.LogError("ExplosionAudio Not Found");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }
        if (_player == null)
        {
            Debug.LogError("Player Not found");
        }

        _uiManager.RevealBossHP();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        BossFire();
    }

    void Movement()
    {
        Vector3 bossPosition = new Vector3(-0.42f, 4.8f, 0);

        transform.position = Vector3.MoveTowards(transform.position, bossPosition, 2f * Time.deltaTime);
    }

    void BossFire()
    {

        if (Time.time > _canFire && _isBossAlive == true && _targettedShotReady == true)
        {
            _fireRate = 5f;
            _canFire = Time.time + _fireRate;

            GameObject targettedLaser = Instantiate(_targettedShotPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = targettedLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
                lasers[i].EnemyHomingLaser();
            }
            _targettedShotReady = false;
        }
        else if (Time.time > _canFire && _isBossAlive == true)
        {
            _fireRate = 5f;
            _canFire = Time.time + _fireRate;
            float randomX = Random.Range(-2, 2);

            GameObject enemyLaser = Instantiate(_rainFirePrefab, new Vector3(randomX, 8, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private IEnumerator BombSpawnRoutine()
    {
        while (_isBossAlive == true)
        {
            yield return new WaitForSeconds(8);
            float randomX = Random.Range(-7f, 7f);
            float randomY = Random.Range(0, -3f);
            Instantiate(_bossBombs, new Vector3(randomX, randomY, 0), Quaternion.identity);
        }
    }

    private IEnumerator TargetShotRoutine()
    {
        while (_isBossAlive == true)
        {
            yield return new WaitForSeconds(12);
            _targettedShotReady = true;
        }
    }

    private IEnumerator ReleaseEnemyRoutine()
    {
        while (_isBossAlive == true)
        {
            yield return new WaitForSeconds(7);
            bool spawnAggressive = Random.value < .5;
            float randomX = Random.Range(-7.5f, 7.5f);
            if (spawnAggressive == true)
            {
                GameObject aggressiveEnemy = Instantiate(_enemies[0], new Vector3(randomX, 8, 0), Quaternion.identity);
                aggressiveEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                GameObject smartEnemy = Instantiate(_enemies[1], new Vector3(randomX, 8, 0), Quaternion.identity);
                smartEnemy.transform.parent = _enemyContainer.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _bossHealth = _bossHealth - 10;
            _uiManager.UpdateBossHP((int) _bossHealth);
            Destroy(other.gameObject);
            if (_bossHealth == 0)
            {
                _isBossAlive = false;
                StartCoroutine(BossDeathSequence());
            }
        }
    }

    private IEnumerator BossDeathSequence()
    {
        while (_isBossAlive == false)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _explosionAudio.Play();
            yield return new WaitForSeconds(1f);
            Instantiate(_explosionPrefab, transform.position + new Vector3(-1.5f, 1.5f, 0), Quaternion.identity);
            _explosionAudio.Play();
            yield return new WaitForSeconds(1f);
            Instantiate(_explosionPrefab, transform.position + new Vector3(2.5f, 1.4f, 0), Quaternion.identity);
            _explosionAudio.Play();
            _gameManager.WaveClear();
            _uiManager.HideBossHP();
            _spawnManager.BossWaveClear();
            Destroy(this.gameObject);
        }
    }

}
