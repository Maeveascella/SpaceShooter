using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _ScoreText;
    [SerializeField]
    private Sprite[] _LiveSprites;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Text _GameOver;
    private bool _isplayerdead = false;
    [SerializeField]
    private Text _RestartText;
    [SerializeField]
    private Slider _fuelSlider;
    [SerializeField]
    private Text _fuelText;

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
    private Text _AmmoText;

    [SerializeField]
    private GameObject[] _Shields;

    

    // Start is called before the first frame update
    void Start()
    {
        _ScoreText.text = "Score: " + 0;
        _GameOver.gameObject.SetActive(false);
        _RestartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager Not Found");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _ScoreText.text = "Score: " + playerScore;ToString();
    }
    
    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _LiveSprites[currentLives];
    }

    public void UpdateAmmo(int currentAmmo)
    {
        _AmmoText.text = "Ammo: " + currentAmmo;ToString();
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
        _RestartText.gameObject.SetActive(true);
        _gameManager.Gameover();
    }

    private IEnumerator Gameoverflicker()
    {
        while (_isplayerdead == true)
        {
            _GameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _GameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void WaveCleared()
    {
        _isWaveCleared = true;
        StartCoroutine(NewWaveFlicker());
        _clearedWaveText.gameObject.SetActive(true);
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
                _Shields[1].SetActive(false);
                break;
            case 1:
                _Shields[2].SetActive(false);
                break;
            case 2:
                _Shields[3].SetActive(false);
                break;
            case 3:
                _Shields[1].SetActive(true);
                _Shields[2].SetActive(true);
                _Shields[3].SetActive(true);
                break;
        }
    }
}
