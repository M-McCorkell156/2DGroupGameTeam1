using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class New_Dialog_Behaviour : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;
    public bool enterConvo;

    public LayerMask playerLayer;
    public GameObject playerObject;

    private void Awake()
    {
        enterConvo = true;
    }
    private void Update()
    {
        if (enterConvo && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                //Remove Panel
                zeroText(); 
            }
            else
            {
                enterConvo = false;
                //Actiavte Panel
                dialoguePanel.SetActive(true);
                playerObject.GetComponent<Player_Behaviour>().lockMovement();
                StartCoroutine(Typing());
            }
        }
        if(dialogueText.text == dialogue[index])
        {
            //contButton.SetActive(true);
            if (Input.anyKeyDown)
            {
                NextLine();
            }
        }
    }
    public void NextLine()
    {
        //contButton.SetActive(false);
        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        playerObject.GetComponent<Player_Behaviour>().unlockMovement();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
            //enterConvo = true;
        }
    }
}
