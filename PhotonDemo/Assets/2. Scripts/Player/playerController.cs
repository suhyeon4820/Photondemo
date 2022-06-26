using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class playerController : MonoBehaviour
{
    // For Player Variables

    public float currentMoveSpeed;
    public float moveSpeed;
    public Vector3 velocity;
    public float currentWalkAnimationSpeed;
    public float walkAnimationSpeed;
    public float rotationSpeed;
    public float jumpForce;
    public float runSpeed;
    public float runAnimationSpeed;

    // For Player Component

    private Rigidbody rb_Player;
    public Animator an_Player;
    private PhotonView pView;

    // For Other GameObjects

    private GameObject cameraObj;
    public GameObject playerMesh;


    // Awake is called before the first frame update
    void Awake()
    {

        rb_Player = GetComponent<Rigidbody>();
        an_Player = GetComponent<Animator>();
        pView = GetComponent<PhotonView>();
        cameraObj = Camera.main.gameObject;

    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {



    }


    // Move Function For Move Player

    public void MovePlayer(float forward, float right)
    {
       
        if (pView.IsMine)
        {

            // forward parameter mean virtical axis from keyboard , right parameter mean horizontal axis from keyboard
            // Get Vertical axis and horizontal axis and multiple to camera vertical axis and horizontal axis

            Vector3 translation;

            translation = forward * cameraObj.transform.forward;
            translation += right * cameraObj.transform.right;
            translation.y = 0;


            // check if vertical and horizontal pressed

            if (translation.magnitude > 0.2f)
            {
                // set velocity to equal to translation
                velocity = translation;
            }
            else
            {
                // set velocity to zero
                velocity = Vector3.zero;
            }


            // Move Player By Rigidbody Velocity

            rb_Player.velocity = new Vector3(velocity.normalized.x * moveSpeed, rb_Player.velocity.y, velocity.normalized.z * moveSpeed);

            // Rotate Player

            if (velocity.magnitude > 0.2f)
            {
                transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * rotationSpeed);
            }

            // Move Animation

            an_Player.SetFloat("Velocity", velocity.magnitude * walkAnimationSpeed);

        }

    }


    // Start Jump Function

    public void Jump()
    {

        if (pView.IsMine)
        {

            // jump by rigidbody addforce
            rb_Player.AddForce(Vector3.up * jumpForce);

            // Play Animation
            an_Player.SetTrigger("Jump");

        }

    }

    // End Jump Funcion

    // Start Attack Function

    public void Attack()
    {

        if (pView.IsMine)
        {

            float randomNumber = Random.Range(0, 15);

            if (randomNumber >= 0 && randomNumber <= 5)
            {

                an_Player.SetTrigger("Attack1");

            }
            else if (randomNumber > 5 && randomNumber <= 10)
            {

                an_Player.SetTrigger("Attack2");

            }
            else if (randomNumber > 10 && randomNumber <= 15)
            {

                an_Player.SetTrigger("Attack3");

            }

        }


    }

    // End Attack Function

    // Start Run Function

    public void RunDown()
    {

        if (pView.IsMine)
        {
            moveSpeed = runSpeed;
            walkAnimationSpeed = runAnimationSpeed;
        }

    }

    public void RunUp()
    {

        if (pView.IsMine)
        {
            moveSpeed = currentMoveSpeed;
            walkAnimationSpeed = currentWalkAnimationSpeed;
        }

    }

    // End Run Function


    // Start Death Function

    public void Death()
    {

        if (pView.IsMine)
        {
            an_Player.SetTrigger("Death");
        }

    }

}
