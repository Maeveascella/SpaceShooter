using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _boostMultiplier = 1.5f;
    private float _thrusterFuel = 100f;
    private float _boostRecharge;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _firerate = 0.5f;
    private float _nextfire = -1f;
    private int _ammo = 6;
    private bool _isBurstFireActive = false;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _TripleshotPrefab;
    [SerializeField]
    private float _TripleShotCounter = 5f;

    [SerializeField]
    private float _SpeedCountdown = 5f;
    

    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _ShieldPrefab;
    private ShieldScript _shield;

    [SerializeField]
    private int _Score = 0;
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _Left_Engine, _Right_Engine, _thruster;

    private AudioSource _audioSource;
    private AudioSource _ExplodeAudio;

    private Animator _cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        transform.position = new Vector3(0, 0, 0);
        _audioSource = GetComponent<AudioSource>();
        _ExplodeAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
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

        if (_ExplodeAudio == null)
        {
            Debug.LogError("Explode Audio Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Input.GetKey(KeyCode.Space) && Time.time > _nextfire &&_isBurstFireActive == true)
        {
            Firelaser();
        }
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextfire && _ammo >= 1)
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
            Instantiate(_TripleshotPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
        }
        _audioSource.Play();
        
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
            _ExplodeAudio.Play();
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
        yield return new WaitForSeconds(_TripleShotCounter);
        _isTripleShotActive = false;
    }

    public void SpeedPower()
    {
        _speed = 6f;
        StartCoroutine(SpeedCountdown());
    }

    private IEnumerator SpeedCountdown()
    {
        yield return new WaitForSeconds(_SpeedCountdown);
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
        _Score = _Score + points;
        _uiManager.UpdateScore(_Score);
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
                _Right_Engine.SetActive(true);
                break;
            case 2:
                _Left_Engine.SetActive(true);
                _Right_Engine.SetActive(false);
                break;
            case 3:
                _Left_Engine.SetActive(false);
                break;
        }
    }
    
    public void ReduceBoost()
    {
        _thrusterFuel = _thrusterFuel *.5f;
        _uiManager.UpdateBoost((int)_thrusterFuel);
    }

}
