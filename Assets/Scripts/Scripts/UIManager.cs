using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOver;
    private bool _isplayerdead = false;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Slider _fuelSlider;
    [SerializeField]
    private Text _fuelText;
    [SerializeField]
    private Slider _bossHealthBar;
    [SerializeField] 
    private Text _magnetText;

    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _newWaveText;
    [SerializeField]
    private Text _clearedWaveText;
    private bool _isWaveCleared = false;

    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private GameObject[] _shields;

    

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager Not Found");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;ToString();
    }
    
    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }

    public void UpdateBossHP(int currentBossHP)
    {
        _bossHealthBar.value = currentBossHP;
    }

    public void RevealBossHP()
    {
        _bossHealthBar.gameObject.SetActive(true);
    }

    public void HideBossHP()
    {
        _bossHealthBar.gameObject.SetActive(false);
    }

    public void UpdateAmmo(int currentAmmo)
    {
        _ammoText.text = "Ammo: " + currentAmmo;ToString();
    }

    public void UpdateBoost(int currentBoost)
    {
        _fuelSlider.value = currentBoost;
        _fuelText.text =  "" + currentBoost;ToString();
    }

    public void PlayerDeath()
    {
        _isplayerdead = true;
        StartCoroutine(Gameoverflicker());
        _restartText.gameObject.SetActive(true);
        _gameManager.Gameover();
    }

    private IEnumerator Gameoverflicker()
    {
        while (_isplayerdead == true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void WaveCleared()
    {
        _isWaveCleared = true;
        StartCoroutine(NewWaveFlicker());
    }

    public void UpdateWave(int currentwave)
    {
        _isWaveCleared = false;
        _clearedWaveText.gameObject.SetActive(false);
        _waveText.text = "Wave: " + currentwave;ToString();
    }

    private IEnumerator NewWaveFlicker()
    {
        while (_isWaveCleared == true)
        {
            _clearedWaveText.gameObject.SetActive(true);
            _newWaveText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _newWaveText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void UpdateShield(int shieldLives)
    {
        switch (shieldLives)
        {
            case 0:
                _shields[1].SetActive(false);
                break;
            case 1:
                _shields[2].SetActive(false);
                break;
            case 2:
                _shields[3].SetActive(false);
                break;
            case 3:
                _shields[1].SetActive(true);
                _shields[2].SetActive(true);
                _shields[3].SetActive(true);
                break;
        }
    }

    public void UpdateMagnetText(int magnetTimer)
    {
        _magnetText.text = "Magnet: " + (magnetTimer);

        if (magnetTimer <= 0)
        {
            _magnetText.text = "Magnet: Ready";
        }
    }
}
