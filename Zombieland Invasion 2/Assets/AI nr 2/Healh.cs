using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healh : MonoBehaviour
{
    public float maxHealth; // masksimaliu gyvybiu skai?ius
    public float currentHealth; // dabartiniu gyvybiu skaicius
  
    public GameObject body; // prieso kuno objektas
    public float blinkIntensity; // mirksejimo stiprumas
    public float blinkDuration; // mirksejimo laikas
    public float blinkTimer; // laikmatis mirksejimo
    public Material oldmaterial; // sena spalva
    [SerializeField] private HeathBar heathBar; // pasiekimas prieso gyvybiu eilutei
    
    void Start()
    {        
        currentHealth = maxHealth; // nustatomos dabartin?s gyvybes

        heathBar.UpdateHeathBar(maxHealth, currentHealth); // atnaujinama gyvybiu eilute
        var rigidBodies = GetComponentsInChildren<Rigidbody>();     
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime; // laikas mazeja
        heathBar.UpdateHeathBar(maxHealth, currentHealth); // atnaujinama gyvybiu eilute
        if (blinkTimer > 0)
        {
            body.GetComponent<Renderer>().material.color = new Color(0, 204, 102); // pakeiciama slapva
        }
        if (blinkTimer <= 0)
        {
            body.GetComponent<Renderer>().material.color = oldmaterial.color; // sugrazinama spalva
        }

        if (currentHealth <= 0) 
        {
            Die();
        }
    }
    private void Die()
    {
        
        Destroy(gameObject); // sunaikinamas objektas
    }


}
