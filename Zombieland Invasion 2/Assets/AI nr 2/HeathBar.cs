using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;

    public Camera cam;

    


    public void UpdateHeathBar(float maxHealth, float currentHealth)
    {
        _healthbarSprite.fillAmount = currentHealth / maxHealth;

    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

}
