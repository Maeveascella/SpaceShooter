using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : MonoBehaviour
{
    private float _speed = 2.5f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _randomX;
    private float _fireRate;
    private float _canFire;
    private bool _isEnemyDead = false;

    private Player _player;
    private SpawnManager _spawnManager;
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _randomX = Random.Range(-1, 2);

        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunnerMovement();
        GunnerFire();
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

        if (Time.time > _canFire && _isEnemyDead == false)
        {
            _fireRate = Random.Range(1f, 3f);
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

}
