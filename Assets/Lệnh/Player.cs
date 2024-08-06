using System.Collections;
using UnityEngine;

//Lệnh để xử lý health, energy, hiệu ứng và GodMode của player (Trong Player)
public class Player : MonoBehaviour
{
    public static Player instance;

    public HealthSystem health;

    public TakeDameEffect dameEffect;
    
    public int HealthAmount=100;
    private int currentHealth;

    public int EnergyAmount=100;
    public float currentEnergy;

    public Movement movement;

    public GameObject effect;
    public Loading loading;

    public bool isGodmode;

    public DamageIndicator indicator;
    void Start()
    {
        isGodmode = false;
        instance=this;
        currentHealth = HealthAmount;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentEnergy == EnergyAmount && !isGodmode)
        {
            BurstMode();
            StartCoroutine(NormalMode());
        }
    }

    public float GetCurrentEnergy()
    {
        return (float)currentEnergy/EnergyAmount;
    }

    private void BurstMode()
    {
            effect.SetActive(true);
            movement.speedMultiplier = 2f;
            isGodmode = true;
    }

    IEnumerator NormalMode()
    {
        while (currentEnergy > 0)
        {
            currentEnergy -= 10 * Time.deltaTime;

            currentEnergy = Mathf.Max(currentEnergy, 0f);

            yield return null;
        }
        movement.speedMultiplier = 1f;
        effect.SetActive(false);
        isGodmode = false;
    }

    public void TakeDamage(int damage,Vector3 enemyLocation)
    {
        indicator.DamageLocation = enemyLocation;
        GameObject gameObject = Instantiate(indicator.gameObject, indicator.transform.position, indicator.transform.rotation,indicator.transform.parent);
        gameObject.SetActive(true);

        if (currentHealth - damage > 0)
        currentHealth -= damage;
        else
        {
            currentHealth = 0;
        }
        dameEffect.StartCoroutine(dameEffect.Effect());
        health.Damaged(currentHealth);

        if (currentHealth <= 0 && !PlayerUI.isPaused) Invoke(nameof(YouLose), 0.5f);
    }
    public void RestoreHealth(int heal)
    {
        if (currentHealth + heal < 100)
            currentHealth += heal;
        else
        {
            currentHealth = 100;
        }
        health.Restored(currentHealth);
    }

    public void YouLose()
    {
        PlayerUI.instance.youLose();
    }
}
