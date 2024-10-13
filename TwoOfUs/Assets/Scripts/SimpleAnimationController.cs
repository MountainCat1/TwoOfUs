using UnityEngine;

public class SimpleAnimationController : MonoBehaviour
{
    public AnimationClip walkAnimation;
    public AnimationClip idleAnimation;
    public AnimationClip useAnimation;

    private Animator animator;
    private AnimatorOverrideController overrideController;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Ensure an Animator component is attached
        if (animator == null)
        {
            Debug.LogError("Animator component missing from GameObject.");
            return;
        }

        // Check if the animation clips are assigned
        if (walkAnimation == null || idleAnimation == null || useAnimation == null)
        {
            Debug.LogError("Please assign all animation clips in the Inspector.");
            return;
        }

        // Set the layer weight in case it's 0
        animator.SetLayerWeight(0, 1);

        // Create an AnimatorOverrideController from the existing RuntimeAnimatorController
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController.name = "Custom OverrideController";

        overrideController["Idle"] = idleAnimation;
        overrideController["Walk"] = walkAnimation;
        overrideController["Use"] = useAnimation;
        
        // Assign the AnimatorOverrideController to the Animator
        animator.runtimeAnimatorController = overrideController;

        // Override the default animations with the ones provided in the Inspector
        


        // Log success
        Debug.Log("Animations have been successfully overridden.");

        // Play the Idle animation to start
        animator.Play("Idle", 0, 0f);
        
        foreach (var clipPair in overrideController.animationClips)
        {
            Debug.Log("Overriding: " + clipPair.name);
        }

    }


    public void PlayWalk()
    {
        Debug.Log("Playing Walk animation.");
        animator.Play("Walk");
    }

    public void PlayIdle()
    {
        Debug.Log("Playing Idle animation.");
        animator.Play("Idle");
    }

    public void PlayUse()
    {
        Debug.Log("Playing Use animation.");
        animator.Play("Use");
    }
}