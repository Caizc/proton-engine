using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 管理器
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _healthText;

    [HideInInspector] public SyncedHealth PlayerHealth;

    void Update()
    {
        if (PlayerHealth != null)
        {
            _healthText.text = "Health: " + PlayerHealth.Health;
        }
    }
}