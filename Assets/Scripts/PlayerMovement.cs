using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject arrowSprite;
    [SerializeField] private GameObject targetGO;

    [SerializeField] private float hitPower;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] KeyCode forward, left, right, back;

    [SerializeField] private AudioClip[] hitSound;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        targetGO.transform.position = transform.position;

        if (Input.GetKey(left))
        {
            transform.Rotate(0, -rotationSpeed, 0);
            targetGO.transform.Rotate(0, -rotationSpeed, 0);
        }

        if (Input.GetKey(right))
        {
            transform.Rotate(0, rotationSpeed, 0);
            targetGO.transform.Rotate(0, rotationSpeed, 0);
        }

        if (Input.GetKey(forward))
        {
            GetComponent<Rigidbody>().AddForce((arrowSprite.transform.position - transform.position).normalized * speed, ForceMode.Force);
            animator.SetBool("isRun", true);
        }

        else animator.SetBool("isRun", false);

        if (Input.GetKey(back))
        {
            GetComponent<Rigidbody>().AddForce((arrowSprite.transform.position - transform.position) * -speed/2, ForceMode.Force);
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("PlayerRed"))
        {
            Vector3 hitDirection = transform.position - collision.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(hitDirection * hitPower, ForceMode.Impulse);
            audioSource.PlayOneShot(hitSound[Random.Range(0, hitSound.Length)]);
        }
    }
}