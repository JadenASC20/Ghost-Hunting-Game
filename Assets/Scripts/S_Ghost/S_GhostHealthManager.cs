
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GhostHealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public float vacuumTickRate = 1f;
    public float vacuumDamage = 10f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DecreaseHealth()
    {
        
//        currentHealth -= vacuumDamage;
        Debug.Log($"Health decreased! Current health: {currentHealth}");

        if (currentHealth >= 0)
        {
            currentHealth -= vacuumDamage;
        }
        else
        {
            Debug.Log("Ghost is Dead RIP!");
        }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }
}
