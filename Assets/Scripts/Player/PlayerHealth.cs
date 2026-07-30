using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;


    [Header("Player Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float curHealth;
    private float lerpTimer;
    [SerializeField] private float chipSpeed = 2f;

    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
        HealthBarChip();
        if (curHealth <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    void HealthBarChip()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = curHealth / maxHealth;
        float percentComplete = lerpTimer / chipSpeed;
        percentComplete = percentComplete * percentComplete;
        healthText.text = Mathf.Round(curHealth) + " ";
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bounds"))
        {
            TakeDamage(20f);
            playerTransform.position = respawnPoint.transform.position;
            Debug.Log("Outbounds");
        }
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        lerpTimer = 0f;
    }
}
