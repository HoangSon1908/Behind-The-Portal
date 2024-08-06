using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public Vector3 DamageLocation;
    public Transform PlayerTransform;
    public Transform DamageImage;
    
    public CanvasGroup DamageCanva;
    public float FadeTime = 1f;
    public float FadeDelay = 1f;
    private float maxFadeTime;
    // Start is called before the first frame update
    void Start()
    {
        maxFadeTime = FadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(FadeDelay > 0)
        {
            FadeDelay -= Time.deltaTime;
        }
        else
        {
            FadeTime -= Time.deltaTime;
            DamageCanva.alpha = FadeTime / maxFadeTime;
            if(FadeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        DamageLocation.y = PlayerTransform.position.y;
        Vector3 Direction = (DamageLocation - PlayerTransform.position).normalized;
        float Angle = Vector3.SignedAngle(Direction, PlayerTransform.forward, Vector3.up);
        DamageImage.localEulerAngles = new Vector3(0, 0, Angle);
    }
}
