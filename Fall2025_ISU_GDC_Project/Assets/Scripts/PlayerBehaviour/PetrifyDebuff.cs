using UnityEngine;

public class PetrifyDebuff : MonoBehaviour
{
    //private Animator playerAnimator;
    private bool petrified = false;
    public static float petrifyTime = 5.0f;
    private float petrifyTimer = petrifyTime;

    public Rigidbody2D rb;
    //private string ladyJusticeSpecial = "Lady Justice Special";
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.L))
        //     OnLadyJusticeSpecial();

        if (petrified)
        {
            petrifyTimer -= Time.deltaTime;
            //Debug.Log(petrifyTimer);
            if (petrifyTimer >= 0.0f)
                petrify();
            else
            {
                unpetrify();
                petrifyTimer = petrifyTime;
                petrified = false;
            }

        }
    }
    //     public void OnLadyJusticeSpecial()
    // {
    //     if(!selfMovement.petrified&&!hitboxRef.GetCurrentlyAttacking())
    //     {
    //         playerAnimator.Play(ladyJusticeSpecial, 0, 0.0f);
    //     }
    // }
    private void petrify()
    {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
    private void unpetrify()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
