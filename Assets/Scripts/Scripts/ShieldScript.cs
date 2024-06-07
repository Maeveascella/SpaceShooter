using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    private int _shieldLives = 3;
    private UIManager _uiManager;
    private SpriteRenderer _shieldSprite;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shieldSprite = GetComponent<SpriteRenderer>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShieldCollected()
    {
        _shieldLives = 3;
        _uiManager.UpdateShield(_shieldLives);
        this._shieldSprite.enabled = true;
    }

    public void ShieldDamage()
    {
        if(_shieldLives <= 1)
        {
            this._shieldSprite.enabled = false;
            _player.ShieldDeactivate();
        }
        _shieldLives = _shieldLives - 1;
        _uiManager.UpdateShield(_shieldLives);

    }
}
