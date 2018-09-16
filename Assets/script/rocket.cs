using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour {

    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;

    Rigidbody rigidbody;

    AudioSource audioSource;
    [SerializeField] AudioClip main_engine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem main_engineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;

    enum State { Alive, Dead, Transending}
    State state = State.Alive;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }
    void Update ()
    {
        if (!(state == State.Dead))
        {
            Thrust();
            RocketRotate();
        }
    }

    private void RocketRotate()
    {
        rigidbody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame); ;
        }
        rigidbody.freezeRotation = false;

    }

    private void Thrust()
    {
       
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up*mainThrust);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(main_engine);
            main_engineParticle.Play();
        }
        else
        {
            audioSource.Stop();
            main_engineParticle.Stop();
        }
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "friendly")
        {
            print("ok");
        }
        if(collision.gameObject.tag == "Enemy")
        {
            state = State.Dead;
            audioSource.Stop();
            main_engineParticle.Stop();
            deathParticle.Play();
            audioSource.PlayOneShot(death);
            Destroy(gameObject,1f);
            Invoke("LoadFirstLevel", 2f);
        }
        if(collision.gameObject.tag == "Finish")
        {
            audioSource.Stop();
            state = State.Transending;
            successParticle.Play();
            audioSource.PlayOneShot(success);
            Invoke("LoadNextLevel",1f);
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        
        SceneManager.LoadScene(0);
    }
}
