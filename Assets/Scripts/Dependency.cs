using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class Dependency : MonoBehaviour
{

    private float dependencyPercent;

    // visual slider
    public Slider dependSlider;
    public Camera cam;
    PostProcessVolume volume;

    //allowing read-only access to dependency
    public float DependencyPercent => dependencyPercent;
    // Start is called before the first frame update
    void Start()
    {
        dependencyPercent = 0;
        volume = cam.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        //manages dependency over time (decreases)
        dependencyPercent -=0.01f;
        dependencyPercent = Mathf.Clamp(dependencyPercent, 0, 100);

        dependSlider.value = dependencyPercent;
        DependecyEffects();
    }

    //method to safely change dependency
    public void changeDependency(float value){
        dependencyPercent += value;
        dependencyPercent = Mathf.Clamp(dependencyPercent, 0, 100);
    }

    // Dependency camera effects
    void DependecyEffects(){
        if (volume.profile.TryGetSettings<ChromaticAberration>(out ChromaticAberration chromaticAberration))
        {
            float chrom_aber_val = dependencyPercent;

            // (hue - min1)/(max1 - min1)
            chrom_aber_val = (chrom_aber_val - 0) / (50 - 0);
            // hue * (max2 - min 2) + min2
            chrom_aber_val = chrom_aber_val * (1 - 0) + 0;
            chromaticAberration.intensity.value = chrom_aber_val;

        } else if (volume.profile.TryGetSettings<LensDistortion>(out LensDistortion lensDistortion)){
            lensDistortion.intensity.value = - dependencyPercent;
        }
    }
}
