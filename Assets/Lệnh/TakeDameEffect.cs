using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

//Lệnh này cho hiệu ứng chảy máu của nhân vật(Trong TakeDameEffect)
public class TakeDameEffect : MonoBehaviour
{
    public float intensity = 0f;

    public PostProcessVolume volume;
    private Vignette vignette;

    void Start()
    {
        volume.profile.TryGetSettings<Vignette>(out vignette);
    }

    public IEnumerator Effect()
    {
        intensity = 0.4f;
        vignette.enabled.Override(true);
        vignette.intensity.Override(intensity);

        yield return new WaitForSeconds(0.5f);

        while (intensity > 0)
        {
                intensity -= 0.01f;

                if(intensity < 0) intensity = 0;

                vignette.intensity.Override(intensity);

                yield return new WaitForSeconds(0.1f);   
        }
    }
}
