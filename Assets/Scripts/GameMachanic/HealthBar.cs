using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealth;
    [SerializeField] private Image curHealth;

    private void Start()
    {
        totalHealth.fillAmount = playerHealth.curHealth / 10;
    }

    private void Update()
    {
        curHealth.fillAmount = playerHealth.curHealth / 10;
    }
}
