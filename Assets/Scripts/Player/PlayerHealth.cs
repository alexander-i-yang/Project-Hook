using System.Collections;
using System.Collections.Generic;
using Utils;

using UnityEngine;
using Player;

public class PlayerHealth : MonoBehaviour
{
    private PlayerCore _core;
    private PlayerDeathManager _pDM;
    private UnitHealth _playerHealth = new UnitHealth(100,100);
    
    int _currentHealth;
    int _currentMaxHealth;

    void Awake() {
        _core = GetComponent<PlayerCore>();
        _pDM = _core.DeathManager;
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _playerHealth.Health;
        _currentMaxHealth = _playerHealth.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth < 0) {
            _pDM.Die();
        }
    }

    void PlayerTakeDmg(int dmg) {
        _playerHealth.DmgUnit(dmg);
    }

    void PlayerHeal(int healing) {
        _playerHealth.HealUnit(healing);
    }
}
