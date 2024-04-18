using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Behaviour : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    [SerializeField] private LayerMask playerLayer;
    public GameObject playerObject;
    private bool _isTalking;
    private bool lockOnce;
    private bool nextLine;

    public GameObject dialogPanel;
    public Text dialogText;
    public string[] dialog;
    private int index;
    [SerializeField] private float wordSpeed;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        lockOnce = false;
        nextLine = false;
    }
    private void Update()
    {
        if (Physics2D.OverlapBox(RB.position, RB.transform.localScale, 0, playerLayer))
        {
            _isTalking = true;
        }
        else
        {
            _isTalking = false;
            lockOnce = false;
            ZeroText();
        }
    }

    private void FixedUpdate()
    {
        //First time talking lock movement 
        if (_isTalking && !lockOnce)
        {
            lockOnce = true;
            playerObject.GetComponent<Player_Behaviour>().lockMovement();
        }
        //Talking
        else if (_isTalking && lockOnce)
        {
            if (dialogPanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                dialogPanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if (dialogText.text == dialog[index])
        {
            nextLine = true;
        }
    }
    public void NextLine()
    {
        nextLine = false;
        if (index < dialog.Length - 1)
        {
            index++;
            dialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }
    public void ZeroText()
    {
        dialogText.text = "";
        index = 0;
        dialogPanel.SetActive(false);
    }
    IEnumerator Typing()
    {
        foreach (char letter in dialog[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }
}
