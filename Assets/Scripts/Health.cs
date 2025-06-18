using UnityEngine;
using System;

public class Health : CoreComponent 
{
    public float CurrentHealth { get; private set; }
    [SerializeField] private float maxHealth;//temp

    public event Action onPlayerDeath;

    //Temp
    private void Start()
    {
        CurrentHealth = maxHealth;
        Debug.Log($"Health: {CurrentHealth} / {maxHealth}");

    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            onPlayerDeath?.Invoke();
        }
        Debug.Log($"Health: {CurrentHealth} / {maxHealth}");
    }

    public void Heal(float heal)
    {
        CurrentHealth += heal;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / maxHealth;
    }

    public void LogicUpdate()
    {
        //if(CurrentHealth <= 0)
        //{
        //    onPlayerDeath?.Invoke();
        //}
    }
}
