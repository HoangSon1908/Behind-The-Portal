using UnityEngine.UI;
using UnityEngine;

//Lệnh cho xử lý thanh máu UI (Trong healthbar của PlayerUI)
public class HealthSystem : MonoBehaviour
{
    private float health;//máu hiện tại
    private float lerpTimer;//biến tính thời gian
    public float maxHealth = 100f;
    public float chipSpeed = 2f;//độ nhanh chậm của hiệu ứng(càng cao càng chậm)
    public RectTransform frontHealthBar;
    public RectTransform backHealthBar;
    public Image colorHealthBar;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        health=Mathf.Clamp(health,0,maxHealth);//khiến máu không thể dưới 0 hoặc trên maxHealth
        UpdateHealthUI();
    }

    public void UpdateHealthUI()//Tạo hiệu ứng trừ hoặc hồi máu
    {
        float fillF = frontHealthBar.localScale.y;
        float fillB = backHealthBar.localScale.y;
        float hFraction=health/maxHealth;
        if (fillB > hFraction)
        {
            colorHealthBar.color = Color.yellow;
            frontHealthBar.localScale = new Vector3(1f, hFraction, 1f);
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;//tạo hiệu ứng nhanh dần khi gần hết hiệu ứng
            float smooth=Mathf.Lerp(fillB,hFraction,percentComplete);
            backHealthBar.localScale=new Vector3(1f,smooth,1f);
        }
        if (fillF < hFraction)
        {
            colorHealthBar.color = Color.green;
            backHealthBar.localScale = new Vector3(1f, hFraction, 1f);
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            float smooth = Mathf.Lerp(fillF, hFraction, percentComplete);
            frontHealthBar.localScale = new Vector3(1f, smooth, 1f);
        }
    }


    public void Damaged(float Health)
    {
        health = Health;
        lerpTimer = 0f;
    }

    public void Restored(float Heal)
    {
        health = Heal;
        lerpTimer = 0f;
    }
}
