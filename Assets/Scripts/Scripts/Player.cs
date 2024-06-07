using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f, _firerate = 0.5f;
    private float _boostRecharge, _boostMultiplier = 1.5f, _thrusterFuel = 100f, _nextfire = -1f, _speedCountdown = 5f, _tripleShotCounter = 5f;

    [SerializeField]
    private GameObject _laserPrefab, _tripleshotPrefab, _shieldPrefab, _leftEngine, _rightEngine, _thruster;

    private int _ammo = 6, _lives = 3, _score = 0, _magnetUses = 1, _magnetTimer = 20;

    private bool _isBurstFireActive = false;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private bool _homingLaserIsActive = false;
    private UIManager _uiManager;
    private ShieldScript _shield;
    private AudioSource _audioSource, _explodeAudio;
    private SpawnManager _spawnManager;
    private Animator _cameraShake;
    private GameObject[] _powerUps;
    private GameObject _ammoCollectible, _lifeCollectible;

    private Powerup _basicPowerups;
    private Ammo _ammoPickup;
    private Life _lifePickup;
    private EnemyType2 _gunnerEnemy;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, 0, 0);
        _audioSource = GetComponent<AudioSource>();
        _explodeAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        _shield = GameObject.Find("Shield").GetComponent<ShieldScript>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<Animator>();



        if (_spawnManager == null)
        { 
            Debug.LogError("Spawn Manager Not Found");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI manager Not Found");
        }

        if ( _audioSource == null)
        {
            Debug.LogError("Player Audio Not Found");
        }

        if (_explodeAudio == null)
        {
            Debug.LogError("Explode Audio Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.C) && _magnetUses >= 1)
        {
            PowerUpMagnet();
            _magnetUses--;
            _magnetTimer = 19;
            StartCoroutine(MagnetText());
            StartCoroutine(MagnetRecharge());
        }

        if(Input.GetKey(KeyCode.Space) && Time.time > _nextfire &&_isBurstFireActive == true)
        {
            Firelaser();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextfire && _ammo >= 1)
        {
            Firelaser();
            _ammo = _ammo - 1;
            _uiManager.UpdateAmmo(_ammo);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterFuel >= 0f)
        {
            transform.Translate(direction * _speed * _boostMultiplier * Time.deltaTime);
            _thruster.gameObject.SetActive(true);
            _thrusterFuel = _thrusterFuel - 20 * Time.deltaTime;
            _uiManager.UpdateBoost((int) _thrusterFuel);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
            _thruster.gameObject.SetActive(false);
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _boostRecharge = Time.time + 2.5f;
            }
            if (Time.time > _boostRecharge && _thrusterFuel < 100)
            {
                _thrusterFuel = _thrusterFuel + 10 * Time.deltaTime;
                _uiManager.UpdateBoost((int) _thrusterFuel);
            }
        }

        //if y => 0 clamp to 0
        //else if y =< -3.8f clamp to -3.8f
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

    }

    void Firelaser()
    {
            _nextfire = Time.time + _firerate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
        }

        else
        {
            GameObject homingLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
            if(_homingLaserIsActive == true)
            {
                Laser[] lasers = homingLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignHomingLaser();
                }
            }
        }

        _gunnerEnemy = GameObject.Find("Gunner Enemy").GetComponent<EnemyType2>();
        _audioSource.Play();
        if (_gunnerEnemy != null)
        {
            _gunnerEnemy.AlertShot();
        }
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shield.ShieldDamage();
        }
        else
        {
            _lives = _lives - 1;
            _uiManager.UpdateLives(_lives);
            UpdateEngines();
            _cameraShake.SetTrigger("OnPlayerDamage");
        }

        if (_lives < 1)
        {
            _spawnManager.PlayerDeath();
            _uiManager.PlayerDeath();
            _explodeAudio.Play();
            Destroy(this.gameObject);
        }
    }

    public void TripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCountdown());
    }

    private IEnumerator TripleShotCountdown()
    {
        yield return new WaitForSeconds(_tripleShotCounter);
        _isTripleShotActive = false;
    }

    public void SpeedPower()
    {
        _speed = 6f;
        StartCoroutine(SpeedCountdown());
    }

    private IEnumerator SpeedCountdown()
    {
        yield return new WaitForSeconds(_speedCountdown);
        _speed = 4f;
    }

    public void ShieldActivated()
    {
        _isShieldActive = true;
        _shield.ShieldCollected();
    }

    public void ShieldDeactivate()
    {
        _isShieldActive = false;
    }

    public void AddScore(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
    }

    public void AddAmmo(int ammo)
    {
        _ammo = _ammo + ammo;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void BurstFire()
    {
        _isBurstFireActive = true;
        _firerate = 0.1f;
        StartCoroutine(RapidFire());
    }

    public void AddLife()
    {
        if (_lives < 3)
        {
            _lives = _lives + 1;
            _uiManager.UpdateLives(_lives);
            UpdateEngines();
        }
    }

    private IEnumerator RapidFire()
    {
        while (_isBurstFireActive == true)
        { 
            yield return new WaitForSeconds(5f);
            _isBurstFireActive = false;
        }
        _firerate = 0.5f;
    }

    private void UpdateEngines()
    {
        switch (_lives)
        {
            case 1:
                _rightEngine.SetActive(true);
                break;
            case 2:
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(false);
                break;
            case 3:
                _leftEngine.SetActive(false);
                break;
        }
    }
    
    public void ReduceBoost()
    {
        _thrusterFuel = _thrusterFuel *.5f;
        _uiManager.UpdateBoost((int)_thrusterFuel);
    }

    private void PowerUpMagnet()
    {
        _powerUps = GameObject.FindGameObjectsWithTag("Powerup");
        _ammoCollectible = GameObject.FindGameObjectWithTag("AmmoCollectible");
        _lifeCollectible = GameObject.FindGameObjectWithTag("LifeCollectible");

        if (_powerUps != null )
        {
            for(int i = 0; i < _powerUps.Length; i++)
            {
                _basicPowerups = _powerUps[i].GetComponent<Powerup>();
                _basicPowerups.PlayerMagnet();
            }
        }

        if (_ammoCollectible != null )
        {
            _ammoPickup = _ammoCollectible.GetComponent<Ammo>();
            _ammoPickup.PlayerMagnet();
        }

        if ( _lifeCollectible != null )
        {
            _lifePickup = _lifeCollectible.GetComponent<Life>();
            _lifePickup.PlayerMagnet();
        }
    }

    public void HomingLaser()
    {
        _homingLaserIsActive = true;
        StartCoroutine(HomingLaserCountDown());
    }

    private IEnumerator HomingLaserCountDown()
    {
        while (_homingLaserIsActive == true)
        {
            yield return new WaitForSeconds(5f);
            _homingLaserIsActive=false;
        }
    }

    private IEnumerator MagnetRecharge()
    {
        while (_magnetUses < 1)
        {
            yield return new WaitForSeconds(20);
            _magnetUses++;
        }
    }

    private IEnumerator MagnetText()
    {
        while (_magnetUses < 1)
        {
            _uiManager.UpdateMagnetText((int)_magnetTimer);
            _magnetTimer--;
            yield return new WaitForSeconds(1);
        }
    }
}

