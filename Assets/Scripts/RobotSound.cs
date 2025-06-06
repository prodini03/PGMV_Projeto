using UnityEngine;

public class RobotSound : MonoBehaviour
{
    public AudioClip armMovement;
    public AudioClip hoverLoop;

    private AudioSource armMovementPlayer;
    private AudioSource hoverPlayer;

    void Start()
    {
        armMovementPlayer = gameObject.AddComponent<AudioSource>();
        armMovementPlayer.playOnAwake = false;

        hoverPlayer = gameObject.AddComponent<AudioSource>();
        hoverPlayer.clip = hoverLoop;
        hoverPlayer.loop = true;
        hoverPlayer.playOnAwake = true;
        hoverPlayer.volume = 0.1f; 
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
