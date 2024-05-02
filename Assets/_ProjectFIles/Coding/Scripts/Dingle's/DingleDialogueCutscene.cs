using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneDialogue: MonoBehaviour
{
    [SerializeField] private PlayableDirector fullTrans;
    [SerializeField] private PlayableDirector transition;
    [SerializeField] private PlayableDirector chuteTimeline;
    private double transitionDuration;
    private double chuteDuration;
    private double fullTransDuration;
    private bool isTransition;

    public GameObject dialoguePanel;
    
    public Text dialogueText;
    public string[] dialogue;
    private int index;

    [SerializeField] private Canvas TutorialImage;
    private bool isTitle;
    private bool isCutscene;

    
    public float wordSpeed;
    public bool playerIsClose;
    public bool enterConvo;

    private bool typing;


    

    private void Awake()
    {
        
        enterConvo = true;
        playerIsClose = true;
        isTitle = true;
        transition = transition.GetComponent<PlayableDirector>();
        chuteTimeline = chuteTimeline.GetComponent<PlayableDirector>();
        transitionDuration = transition.GetComponent<PlayableDirector>().duration;
        chuteDuration = chuteTimeline.GetComponent<PlayableDirector>().duration;
        fullTrans = fullTrans.GetComponent<PlayableDirector>();
        fullTransDuration = fullTrans.GetComponent<PlayableDirector>().duration;
        
    }
    private void Update()
    {
        
        if (chuteTimeline.time == chuteDuration)
        {
            Debug.Log("Talking");
            isTitle = false;
            dialoguePanel.SetActive(true);




            StartCoroutine(Typing());

            
        }
        if (dialogueText.text == dialogue[index] && !isTitle)
        {
            //contButton.SetActive(true);
            typing = false;
            if (Input.anyKeyDown)
            {
                if (index < dialogue.Length - 1)
                {
                    NextLine();
                    
                }
                if (index >= dialogue.Length - 1 && !typing && !isCutscene)
                {
                    dialoguePanel.SetActive (false);
                    transition.Play();

                }
                

            }
        }
        
        
        
        if (transition.time == transitionDuration)
        {
            Debug.Log("Loading");
            isCutscene = true;
            

        }
        if (Input.anyKeyDown && isCutscene)
        {
            Debug.Log("AAAAAA");
            fullTrans.Play();
        }
        if (fullTrans.time == fullTransDuration)
        {
            Debug.Log("LoadingGame!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //LoadGameHere
        }

    }
    public void NextLine()
    {
        //contButton.SetActive(false);
        if (index < dialogue.Length - 1)
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
        typing = true;
        foreach (char letter in dialogue[index].ToCharArray())
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
        
        

    }


    
    
}