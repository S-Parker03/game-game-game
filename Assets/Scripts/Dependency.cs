using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class Dependency : MonoBehaviour
{
    // Total value for dependency
    private float dependencyPercent;

    // visual slider
    public Slider dependSlider;

    // camera component needed for dependency camera effects
    public Camera cam;

    // post processing volume for camera effects
    PostProcessVolume volume;

    //allowing read-only access to dependency
    public float DependencyPercent => dependencyPercent;

    void Start()
    {
        // game starts with dependency at 0 and find the post processing effects volume
        dependencyPercent = 0;
        volume = cam.GetComponent<PostProcessVolume>();
    }

    void Update()
    {
        // manages dependency over time (decreases)
        dependencyPercent -=0.01f;
        dependencyPercent = Mathf.Clamp(dependencyPercent, 0, 100);

        // assign dependecy value to slider and call dependency post processing effects
        dependSlider.value = dependencyPercent;
        DependecyEffects();
    }

    // method to safely change dependency
    public void changeDependency(float value){
        dependencyPercent += value;
        dependencyPercent = Mathf.Clamp(dependencyPercent, 0, 100);
    }


    // Dependency camera effects function
    void DependecyEffects(){
        if (volume.profile.TryGetSettings<ChromaticAberration>(out ChromaticAberration chromaticAberration))
        {
            float chrom_aber_val = dependencyPercent;

            // map the values of the 
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
