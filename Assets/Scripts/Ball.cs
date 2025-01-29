 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private AudioSource audioSource;
    private bool firstHit;
    private float cuurentVelocity;
    private Rigidbody rb;

    [SerializeField] float hitPower;
    [SerializeField] AudioClip[] hitSound;
    [SerializeField, Range(0,10)] float maxVelocity;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag.Contains("Player"))
        {
            Vector3 hitDirection = transform.position - collision.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(hitDirection * hitPower, ForceMode.Impulse);
        }

        if (firstHit) audioSource.PlayOneShot(hitSound[Random.Range(0, hitSound.Length)]);        
        firstHit = true;
    }

    private void FixedUpdate()
    {
        //����������� ������������ ��������, ����� �������� ����� ���� ������ ����������
        //��� �� �������� �������� Project Settings -> Physics -> Default Max Depenetration Velocity � 10 �� 20
        //��� �� �������� �������� � ���������� ball -> Rigidbody -> Collision Detection � Discrete �� Continious
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}