using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IDamage
{
    public int healthLevel = 10;
    public int MaxHealth;
    public int currentHealth;

    public int maxStamina;
    public int currentStamina;
    public int StaminaLevel = 10;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    AnimatorManager animatorManager;
    CinemaCameraManager cinemaCameraManager;
    private void Awake() {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        cinemaCameraManager = FindObjectOfType<CinemaCameraManager>();
    }
     void Start() {
        MaxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);

        maxStamina = SetMaxHealthFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    private int SetMaxHealthFromHealthLevel(){
        MaxHealth = healthLevel  * 10;
        return MaxHealth;
    }


    private int SetMaxHealthFromStaminaLevel(){
        maxStamina = StaminaLevel  * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage){
        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);
        animatorManager.PlayTargetAnimation("Damage_02",true,true);
        cinemaCameraManager.ShakeCamera();
        if(currentHealth <=0){
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Dead_01",true,true);
        }
    }

    public void TakeStaminaDamage(int damage){
        currentStamina = currentStamina - damage;
        if(currentStamina <=0){
            currentStamina = 0;
        }
        staminaBar.SetCurrentStamina(currentStamina);
    }
}
