using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    [SerializeField] private GameObject targetGO;
    [SerializeField] private GameObject arrowSprite;
    [SerializeField] private GameObject sideTargets;
    [SerializeField] private GameObject ball;
    [SerializeField] private AudioClip[] hitSound;
    
    [SerializeField] private float hitPower;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Animator animator;

    private static bool isLeft, isRight;
    public bool IsLeft { set { isLeft = value; } }
    public bool IsRight { set { isRight = value; } }

    private void Start()
    {
        isLeft = false;
        isRight = false;
        animator = GetComponent<Animator>();
        arrowSprite.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        SmartAI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Vector3 hitDirection = transform.position - collision.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(hitDirection * hitPower, ForceMode.Impulse);
    }
        }

    private void SmartAI()
    {
        targetGO.transform.position = transform.localPosition;
        sideTargets.transform.position = transform.localPosition;

        if (isLeft)
        {
            gameObject.transform.Rotate(0, -rotationSpeed, 0);
            targetGO.transform.Rotate(0, -rotationSpeed, 0);
            sideTargets.transform.Rotate(0, -rotationSpeed, 0);
        }

        if (isRight)
        {
            gameObject.transform.Rotate(0, rotationSpeed, 0);
            targetGO.transform.Rotate(0, rotationSpeed, 0);
            sideTargets.transform.Rotate(0, rotationSpeed, 0);
        }

        if (!isLeft && !isRight)
        {
            GetComponent<Rigidbody>().AddForce((arrowSprite.transform.position - transform.position).normalized * speed, ForceMode.Force);
            animator.SetBool("isRun", true);
        }
        else animator.SetBool("isRun", false);
    }
}