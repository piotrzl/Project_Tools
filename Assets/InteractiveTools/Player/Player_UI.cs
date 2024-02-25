using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
   // PlayerControler playerControler;
    [SerializeField] TMP_Text interactionMessage;
    [SerializeField] Image interactionDelayImage;

    [SerializeField] TMP_Text PauseText;

    private void Start()
    {
        PlayerControler playerControler = FindObjectOfType<PlayerControler>();

        if (playerControler) 
        {
            playerControler.InteractionMessageEvent += UpdateInteractionMessage;
            playerControler.InteractionChargingEvent += UpdateInteractionDelayImage;
        }

        interactionMessage.gameObject.SetActive(false);
        interactionDelayImage.gameObject.SetActive(false);

        GameMenager.Instance.PauseGameEvent += DisplayPauseText;
    }

    

    void UpdateInteractionMessage(string newMessage) 
    {
        if(newMessage != null) 
        {
            interactionMessage.gameObject.SetActive(true);
            interactionMessage.text = newMessage;
        }
        else 
        {
            interactionMessage.gameObject.SetActive(false);
        }
    }

    void UpdateInteractionDelayImage(float updateFill) 
    {
        if(updateFill < 0) 
        {
            interactionDelayImage.gameObject.SetActive(false);
        }
        else 
        {
            if(!interactionDelayImage.gameObject.activeSelf)
                interactionDelayImage.gameObject.SetActive(true);

            interactionDelayImage.fillAmount = updateFill;


        }

        
    }

    public void DisplayPauseText(bool display) 
    {
        PauseText.gameObject.SetActive(display);
    }




    private void OnDestroy()
    {
        PlayerControler playerControler = FindObjectOfType<PlayerControler>();

        if (playerControler)
        {
            playerControler.InteractionMessageEvent -= UpdateInteractionMessage;
            playerControler.InteractionChargingEvent -= UpdateInteractionDelayImage;
        }
    }
}
