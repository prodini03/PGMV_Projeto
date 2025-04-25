using UnityEngine;

public class RobotSound : MonoBehaviour
{
    public AudioClip armMovement;
    public AudioClip hoverLoop;

    private AudioSource armMovementPlayer;
    private AudioSource hoverPlayer;

    void Start()
    {
        // Arm movement player
        armMovementPlayer = gameObject.AddComponent<AudioSource>();
        armMovementPlayer.playOnAwake = false;

        // Hover sound player
        hoverPlayer = gameObject.AddComponent<AudioSource>();
        hoverPlayer.clip = hoverLoop;
        hoverPlayer.loop = true;
        hoverPlayer.playOnAwake = true;
        hoverPlayer.volume = 0.1f; // Adjust volume if needed
        hoverPlayer.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            armMovementPlayer.clip = armMovement;
            armMovementPlayer.Play();
        }
    }
}
