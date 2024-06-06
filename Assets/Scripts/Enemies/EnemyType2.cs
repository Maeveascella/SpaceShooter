using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : MonoBehaviour
{
    private float _speed = 2.5f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _spreadShotPrefab;

    private float _randomX;
    private float _fireRate;
    private float _canFire;
    private bool _isEnemyDead = false;

    private Player _player;
    private SpawnManager _spawnManager;
    private AudioSource _explosionAudio;
    private Transform _playerPos;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private bool _spreadShotisReady;
    [SerializeField]
    private bool _willDodge;
    private float _dodgeDistance;

    // Start is called before the first frame update
    void Start()
    {
        _randomX = Random.Range(-1, 2);
        _dodgeDistance = Random.Range(-4, 4f);

        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        StartCoroutine(SpreadShotTimer());

        if(_player == null)
        {
            Debug.LogError("Player Not Found");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }

        if (_explosionAudio == null)
        {
            Debug.LogError("Explosion Audio Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GunnerMovement();
        //GunnerFire();
    }

    void GunnerMovement()
    {

        transform.Translate(new Vector3(_randomX, -1, 0) * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 3.5f, 10), 0);
        if (transform.position.x >= 7.8f)
        {
            _randomX = _randomX * -1;
        }
        else if (transform.position.x <= -7.8f)
        {
            _randomX = _randomX * -1;
        }
    }

    private void GunnerFire()
    {

        if (Time.time > _canFire && _isEnemyDead == false && _spreadShotisReady == true)
        {
            _fireRate = 1f;
            _canFire = Time.time + _fireRate;
            GameObject spreadShot = Instantiate(_spreadShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            _spreadShotisReady = false;
            Laser[] lasers = spreadShot.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
        else if (Time.time > _canFire && _isEnemyDead == false)
        {
            _fireRate = 1f;
            _canFire = Time.time + _fireRate;
            
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            GunnerDeath();
        }
    }
    
    private void GunnerDeath()
    {
        _isEnemyDead = true;
        _player.AddScore(15);
        _spawnManager.EnemyKilled();
        _explosionAudio.Play();
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, .1f);
    }

    private IEnumerator SpreadShotTimer()
    {
        while (_isEnemyDead == false)
        {
            yield return new WaitForSeconds(3f);
            _spreadShotisReady = true;
        }
    }

    public void AlertShot()
    {
        float xDistance = _playerPos.position.x - transform.position.x;
        if(xDistance <= 2)
        {
            _willDodge = Random.value < 1f;
        }
        if (_willDodge == true)
        {
            float lowrange = Random.Range(-3, -2f);
            float highrange = Random.Range(2, 3f);
            _speed = _speed * 2;
            _randomX = _randomX + Random.Range(lowrange, highrange);
            StartCoroutine(DodgeChance());
        }
    }

    private IEnumerator DodgeChance()
    {
        while (_willDodge == true)
        {
            yield return new WaitForSeconds(.3f);
            _randomX = Random.Range(-1, 2);
            _speed = _speed * .5f;
            _willDodge = false;
        }
        
    }

}
