using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Behaviour : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    [SerializeField] private LayerMask npcLayer;
    public GameObject playerObject;
    private bool _isTalking;
    private bool lockOnce;
    private bool nextLine;

    public Canvas dialogPanel;
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
        if (Physics2D.OverlapBox(playerObject.transform.position, playerObject.transform.localScale, 0, npcLayer))
        {
            _isTalking = true;
        }
        else
        {
            _isTalking = false;
            lockOnce = false;
            ZeroText();
        }

        if (dialogText.text == dialog[index])
        {
            Debug.Log("Once talk");
            NextLine();
        }
    }

    private void FixedUpdate()
    {
        //First time talking lock movement 
        if (_isTalking && !lockOnce)
        {
            lockOnce = true;
            playerObject.GetComponent<Player_Behaviour>().lockMovement();
            //Debug.Log("Lock");
        }
        //Talking
        else if (_isTalking && lockOnce)
        {
            if (!dialogPanel.enabled)
            {
                //Debug.Log("enabled");
                dialogPanel.enabled = true;
                StartCoroutine(Typing());
            }
            Debug.Log("Start Typing");
            //StartCoroutine(Typing());
        }

        if (dialogText.text == dialog[index])
        {           
            Debug.Log("finish line");
            nextLine = true;
        }
    }
    public void NextLine()
    {
        Debug.Log("next line");

        nextLine = false;
        if (index < dialog.Length - 1)
        {
            Debug.Log("next index");
            index++;
            dialogText.text = "";
            //StartCoroutine(Typing());
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
        dialogPanel.enabled = false;
    }
    IEnumerator Typing()
    {
        foreach (char letter in dialog[index].ToCharArray())
        {
            dialogText.text += letter;
            Debug.Log("talk");
            yield return new WaitForSeconds(wordSpeed);
        }
    }
}
