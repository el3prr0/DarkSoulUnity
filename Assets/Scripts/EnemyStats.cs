using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour,IDamage
{
    public int healthLevel = 10;
    public int MaxHealth;
    public int currentHealth;

    //public HealthBar healthBar;
    Animator animator;

    CinemaCameraManager cinemaCameraManager;
    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        cinemaCameraManager = FindObjectOfType<CinemaCameraManager>();
    }
     void Start() {
        MaxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = MaxHealth;
        //healthBar.SetMaxHealth(MaxHealth);
    }

    private int SetMaxHealthFromHealthLevel(){
        MaxHealth = healthLevel  * 10;
        return MaxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth = currentHealth - damage;
        //healthBar.SetCurrentHealth(currentHealth);
        animator.Play("Damage_02");
        cinemaCameraManager.ShakeCamera();
        if(currentHealth <=0){
            currentHealth = 0;
            animator.Play("Dead_01");
        }
    }

    
    public void TakeStaminaDamage(int damage){
       
    }
}
