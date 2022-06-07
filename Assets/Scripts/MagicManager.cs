using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    public float MagicSpeed = 50f;

    public int MagicDamage = 10;

    public GameObject DestroyAnimation;
    Rigidbody magicRidigBody;
    private void Awake() {
        magicRidigBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        CreateMagic();
    }
    private void OnTriggerEnter(Collider other) {
        IDamage damage = other.GetComponent<IDamage>();
        if(damage != null){
            damage.TakeDamage(MagicDamage);
        }
        
        if(DestroyAnimation != null){
            DestroyAnimation.transform.position = gameObject.transform.position;
            Instantiate(DestroyAnimation);
        }
         Destroy(gameObject);
    }
    private void CreateMagic(){
        if(magicRidigBody != null){
            magicRidigBody.velocity = transform.forward * MagicSpeed;
        }
         
    }
}
