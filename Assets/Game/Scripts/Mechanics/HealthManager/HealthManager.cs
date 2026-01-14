using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IHealthManager
{
    public float currentHealth { get; private set; }
    public int maxHealth = 100;

    public bool isPaused = false;

    public Action OnHealthEnded { get; set; }

    private void Update()
    {
        if (isPaused) return;
        
        currentHealth -= 2 * Time.deltaTime;

        EndCheck();
    }

    public void HealthReset()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(HitResult hitResult)
    {
        switch (hitResult)
        {
            case HitResult.Perfect:
                currentHealth += 10;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                break;
            case HitResult.Good:
                currentHealth += 5;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                break;
            case HitResult.Miss:
                currentHealth -= 15;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                EndCheck();
                break;
            default:
                break;
        }
    }

    private void EndCheck()
    {
        if (currentHealth <= 0)
        {
            OnHealthEnded?.Invoke();
        }
    }


}
