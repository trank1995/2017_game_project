﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {
    public static DialogueSystem Instance { get; set; }
    string npcName, snpcName, townName, cityName;
    public GameObject NPCInteractionPanel, SNPCInteractionPanel, townInteractionPanel,
        cityInteractionPanel, makeSurePanel, statusPanel;
    public GameObject cityFirstLayerPanel1, cityFirstLayerPanel2, townFirstLayerPanel1,
        townFirstLayerPanel2;
    public GameObject townRestockPanel, cityRestockPanel, townRestockBuy, townRestockCancel,
        townRestockSlideBar, cityRestockBuy, cityRestockCancel, cityRestockSlideBar;
    public GameObject cityBackground, townBackground, npcBckground, snpcBackground;
    public GameObject npcTalk, npcAttack, npcAmbush, npcRetreat, npcBribe, npcLeave;
    public GameObject snpcTalk, snpcOptOne, snpcOptTwo, snpcLeave;
    public GameObject cityTalk, cityThreaten, cityMarket, cityHall, cityArmory, cityTavern,
        cityBrothel, cityChurch, cityPillage, cityRansom, cityRetreat, cityRestock,
        cityTrade, cityBillboard, cityTroop, cityChar, cityRest, cityBartender,
        cityGossip, cityBrothelRest, cityOrgy, cityIndulgence, cityLeave, cityReturn;
    public GameObject townTalk, townRestock, townThreaten, townRecruit, townInvest,
        townLeave, townPillage, townRansom, townRetreat;
    public GameObject yesButton, noButton;
    public Text makeSureMsg, townRestockAmount, cityRestockAmount;
    public Texture2D cityTalkImg, cityThreatenImg, cityMarketImg, cityHallImg, cityArmoryImg,
        cityTavernImg, cityBrothelImg, cityChurchImg;

    public List<string> npcDialogueLines = new List<string>();
    public List<string> snpcDialogueLines = new List<string>();
    public List<string> cityDialogueLines = new List<string>();
    public List<string> townDialogueLines = new List<string>();
    public List<string> dialogueLines = new List<string>();

    public Text npcDialogueText, snpcDialogueText, cityDialogueText, townDialogueText;
    public Text npcNameText, snpcNameText, cityNameText, townNameText;

    int npcDialogueIndex, snpcDialogueIndex, cityDialogueIndex, townDialogueIndex;
    

    void Awake () {
        NPCPanelInitialization();
        SNPCPanelInitialization();
        cityPanelInitialization();
        townPanelInitialization();
        
        //inactivate all panels
        NPCInteractionPanel.SetActive(false);
        SNPCInteractionPanel.SetActive(false);
        cityInteractionPanel.SetActive(false);
        townInteractionPanel.SetActive(false);
        makeSurePanel.SetActive(false);
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
	}

    public void NPCPanelInitialization()
    {
        cityBackground.GetComponent<RawImage>().texture = cityTalkImg;
        //npcTalk, npcAttack, npcAmbush, npcRetreat, npcBribe, npcLeave;
        npcTalk.GetComponent<Button>().onClick.AddListener(delegate { continueDialogue("NPC"); });
        npcAttack.GetComponent<Button>().onClick.AddListener(delegate () { npcConfirm("attackNPC"); });
        npcAmbush.GetComponent<Button>().onClick.AddListener(delegate () { npcConfirm("ambushNPC"); });
        npcRetreat.GetComponent<Button>().onClick.AddListener(delegate () { npcConfirm("retreatNPC"); });
        npcBribe.GetComponent<Button>().onClick.AddListener(delegate () { npcConfirm("bribeNPC"); ; });
        npcLeave.GetComponent<Button>().onClick.AddListener(delegate () { npcConfirm("leaveNPC"); });
    }
    public void SNPCPanelInitialization()
    {
        //snpcTalk, snpcOptOne, snpcOptTwo, snpcLeave;
    }
    public void cityPanelInitialization()
    {
        cityTalk.GetComponent<Button>().onClick.AddListener(delegate { continueDialogue("city"); });
        cityThreaten.GetComponent<Button>().onClick.AddListener(delegate { cityConfirm("threatenCity"); });
        cityMarket.GetComponent<Button>().onClick.AddListener(delegate { marketCity(); });
        cityHall.GetComponent<Button>().onClick.AddListener(delegate { hallCity(); });
        cityArmory.GetComponent<Button>().onClick.AddListener(delegate { armoryCity(); });
        cityTavern.GetComponent<Button>().onClick.AddListener(delegate { tavernCity(); });
        cityBrothel.GetComponent<Button>().onClick.AddListener(delegate { brothelCity(); });
        cityChurch.GetComponent<Button>().onClick.AddListener(delegate { churchCity(); });
        cityPillage.GetComponent<Button>().onClick.AddListener(delegate { pillageCity(); });
        cityRansom.GetComponent<Button>().onClick.AddListener(delegate { cityConfirm("ransomCity"); });
        cityRetreat.GetComponent<Button>().onClick.AddListener(delegate { cityConfirm("retreatCity"); });
        cityRestock.GetComponent<Button>().onClick.AddListener(delegate { restockCity(); });
        cityTrade.GetComponent<Button>().onClick.AddListener(delegate { tradeCity(); });
        cityBillboard.GetComponent<Button>().onClick.AddListener(delegate { billboardCity(); });
        cityTroop.GetComponent<Button>().onClick.AddListener(delegate { troopCity(); });
        cityChar.GetComponent<Button>().onClick.AddListener(delegate { charCity(); });
        cityRest.GetComponent<Button>().onClick.AddListener(delegate { restCity(); });
        cityBartender.GetComponent<Button>().onClick.AddListener(delegate { bartenderCity(); });
        cityGossip.GetComponent<Button>().onClick.AddListener(delegate { gossipCity(); });
        cityBrothelRest.GetComponent<Button>().onClick.AddListener(delegate { brothelRestCity(); });
        cityOrgy.GetComponent<Button>().onClick.AddListener(delegate { orgyCity(); });
        cityChurch.GetComponent<Button>().onClick.AddListener(delegate { churchCity(); });
        cityLeave.GetComponent<Button>().onClick.AddListener(delegate { leaveCity(); });
        cityReturn.GetComponent<Button>().onClick.AddListener(delegate { returnCity(); });
        cityRestockCancel.GetComponent<Button>().onClick.AddListener(delegate { restockCancelCity(); });
        cityRestockBuy.GetComponent<Button>().onClick.AddListener(delegate { restockBuyCity(); });
        cityRestockAmount.text = ((int)cityRestockSlideBar.GetComponent<Slider>().value).ToString();
        cityRestockSlideBar.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            cityRestockAmount.text = ((int)cityRestockSlideBar.GetComponent<Slider>().value).ToString(); });
        returnCity();
    }
    public void townPanelInitialization()
    {
        townTalk.GetComponent<Button>().onClick.AddListener(delegate { continueDialogue("town"); });
        townThreaten.GetComponent<Button>().onClick.AddListener(delegate () { townConfirm("threatenTown"); });
        townRestock.GetComponent<Button>().onClick.AddListener(delegate () { restockTown(); });
        townRecruit.GetComponent<Button>().onClick.AddListener(delegate () { recruitTown(); });
        townInvest.GetComponent<Button>().onClick.AddListener(delegate () { investTown(); });
        townLeave.GetComponent<Button>().onClick.AddListener(delegate () { closeDialogue("town"); });
        townPillage.GetComponent<Button>().onClick.AddListener(delegate () { pillageTown(); });
        townPillage.SetActive(false);
        townRansom.GetComponent<Button>().onClick.AddListener(delegate () { ransomTown(); });
        townRansom.SetActive(false);
        townRetreat.GetComponent<Button>().onClick.AddListener(delegate () { retreatTown(); });
        townRetreat.SetActive(false);
        townRestockPanel.SetActive(false);
        townRestockAmount.text = ((int)townRestockSlideBar.GetComponent<Slider>().value).ToString();
        townRestockSlideBar.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            townRestockAmount.text = ((int)townRestockSlideBar.GetComponent<Slider>().value).ToString();
        });

    }
    
    //NPC
    public void npcConfirm(string funcName)
    {
        string msg = "Are you sure?";
        makeSurePanel.SetActive(true);
        switch (funcName)
        {
            case "attackNPC":
                msg = "Violence is not the answer.";
                makeSureMsg.text = msg;
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { attackNPC(); });
                break;
            case "ambushNPC":
                msg = "Sneaky bastard.";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { ambushNPC(); });
                break;
            case "retreatNPC":
                msg = "Coward!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { retreatNPC(); });
                break;
            case "bribeNPC":
                msg = "Rich!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { bribeNPC(); });
                break;
            case "leaveNPC":
                msg = "Bye?";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { leaveNPC(); });
                break;
        }
        noButton.GetComponent<Button>().onClick.AddListener(delegate () { makeSurePanel.SetActive(false); });


    }
    public void attackNPC()
    {
        makeSurePanel.SetActive(false);
        closeDialogue("NPC");
        createBattleScene();
    }
    public void ambushNPC()
    {
        makeSurePanel.SetActive(false);
        closeDialogue("NPC");
        createBattleScene();
    } //TODO: based on INT
    public void retreatNPC() //TODO: based on INT
    {
        makeSurePanel.SetActive(false);
        closeDialogue("NPC");
    }
    public void bribeNPC()
    {
        makeSurePanel.SetActive(false);
        closeDialogue("NPC"); //TODO: AI will leave
    } //TODO: demand gold based on CHR
    public void leaveNPC()
    {
        makeSurePanel.SetActive(false);
        closeDialogue("NPC");
    } //TODO:based on CHR if hostility > 0;
    //SNPC
    public void talkSNPC()
    {

    }
    public void optOneSNPC()
    {

    }
    public void optTwoSNPC()
    {

    }
    //City
    public void cityConfirm(string funcName)
    {
        string msg = "Are you sure?";
        makeSurePanel.SetActive(true);
        switch (funcName)
        {
            case "threatenCity":
                msg = "There is no going back.";
                makeSureMsg.text = msg;
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { threatenCity(); });
                break;
            case "retreatCity":
                msg = "Coward!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { retreatCity(); });
                break;
            case "ransomCity":
                msg = "Rich!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { ransomCity(); });
                break;
            case "leaveCity":
                msg = "Bye?";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { leaveCity(); });
                break;
        }
        yesButton.GetComponent<Button>().onClick.AddListener(delegate () { makeSurePanel.SetActive(false); });
        noButton.GetComponent<Button>().onClick.AddListener(delegate () { makeSurePanel.SetActive(false); });


    }
    public void threatenCity()
    {
        swapCityBackground(cityThreatenImg);
        cityPillage.SetActive(true);
        cityRansom.SetActive(true);
        cityRetreat.SetActive(true);
        cityMenuButtons(false);
    }
    public void marketCity()
    {
        swapCityBackground(cityMarketImg);
        cityTrade.SetActive(true);
        cityRestock.SetActive(true);
        cityReturn.SetActive(true);
        cityMenuButtons(false);
    }
    public void hallCity()
    {
        swapCityBackground(cityHallImg);
        cityBillboard.SetActive(true);
        cityReturn.SetActive(true);
        cityMenuButtons(false);
    }
    public void armoryCity()
    {
        swapCityBackground(cityArmoryImg);
        cityTroop.SetActive(true);
        cityChar.SetActive(true);
        cityReturn.SetActive(true);
        cityMenuButtons(false);
    }
    public void tavernCity()
    {
        swapCityBackground(cityTavernImg);
        cityRest.SetActive(true);
        cityBartender.SetActive(true);
        cityGossip.SetActive(true);
        cityReturn.SetActive(true);
        cityMenuButtons(false);
    }
    public void brothelCity()
    {
        swapCityBackground(cityBrothelImg);
        cityBrothelRest.SetActive(true);
        cityOrgy.SetActive(true);
        cityReturn.SetActive(true);
        cityMenuButtons(false);
    }
    public void churchCity()
    {
        swapCityBackground(cityChurchImg);
        cityIndulgence.SetActive(true);
        cityReturn.SetActive(true);
        cityMenuButtons(false);
    }
    public void pillageCity()
    {
        cityPillage.SetActive(false);
        cityRansom.SetActive(false);
        cityRetreat.SetActive(false);
        cityMenuButtons(true);
        closeDialogue("city");
        createBattleScene();
        
    }
    public void ransomCity()
    {
        cityPillage.SetActive(false);
        cityRansom.SetActive(false);
        cityRetreat.SetActive(false);
        cityMenuButtons(true);
        closeDialogue("city");
    }
    public void retreatCity()
    {
        cityPillage.SetActive(false);
        cityRansom.SetActive(false);
        cityRetreat.SetActive(false);
        cityMenuButtons(true);
        closeDialogue("city");
    }
    public void restockCity()
    {
        cityRestockPanel.SetActive(true);
        cityRestockSlideBar.GetComponent<Slider>().maxValue = Player.mainParty.cash;
        cityRestockSlideBar.GetComponent<Slider>().minValue = 0;
    }
    public void restockCancelCity()
    {
        townRestockSlideBar.GetComponent<Slider>().value = 0;
        cityRestockPanel.SetActive(false);
    }
    public void restockBuyCity()
    {

        townRestockSlideBar.GetComponent<Slider>().value = 0;
    }
    public void tradeCity()
    {

    }
    public void billboardCity()
    {

    }
    public void troopCity()
    {

    }
    public void charCity()
    {

    }
    public void restCity()
    {

    }
    public void bartenderCity()
    {

    }
    public void gossipCity()
    {

    }
    public void brothelRestCity()
    {

    }
    public void orgyCity()
    {

    }
    public void indulgenceCity()
    {

    }
    public void returnCity()
    {
        cityRestockPanel.SetActive(false);
        cityPillage.SetActive(false);
        cityRansom.SetActive(false);
        cityRetreat.SetActive(false);
        cityRestock.SetActive(false);
        cityTrade.SetActive(false);
        cityBillboard.SetActive(false);
        cityTroop.SetActive(false);
        cityChar.SetActive(false);
        cityRest.SetActive(false);
        cityBartender.SetActive(false);
        cityGossip.SetActive(false);
        cityBrothelRest.SetActive(false);
        cityOrgy.SetActive(false);
        cityIndulgence.SetActive(false);
        cityReturn.SetActive(false);
        cityMenuButtons(true);
    }
    public void leaveCity()
    {
        closeDialogue("city");
    }
    public void swapCityBackground(Texture2D img)
    {
        cityBackground.GetComponent<RawImage>().texture = img;
    }
    public void cityMenuButtons(bool hide)
    {
        cityTalk.SetActive(hide);
        cityThreaten.SetActive(hide);
        cityMarket.SetActive(hide);
        cityHall.SetActive(hide);
        cityArmory.SetActive(hide);
        cityTavern.SetActive(hide);
        cityBrothel.SetActive(hide);
        cityChurch.SetActive(hide);
        cityLeave.SetActive(hide);
        cityFirstLayerPanel1.SetActive(hide);
        cityFirstLayerPanel2.SetActive(hide);
        if (hide)
        {
            swapCityBackground(cityTalkImg);
        }
    }

    //Town
    public void townConfirm(string funcName)
    {
        string msg = "Are you sure?";
        makeSurePanel.SetActive(true);
        switch (funcName)
        {
            case "threatenTown":
                msg = "There is no going back.";
                //clearDialogue
                //addNewDialogue("town", )
                //createDialogue()
                makeSureMsg.text = msg;
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { threatenTown(); });
                break;
            case "recruitTown":
                msg = "Hire!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { recruitTown(); });
                break;
            case "investTown":
                msg = "Rich!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { investTown(); });
                break;
            case "retreatTown":
                msg = "Coward!";
                yesButton.GetComponent<Button>().onClick.AddListener(delegate () { retreatTown(); });
                break;
        }
        yesButton.GetComponent<Button>().onClick.AddListener(delegate () { makeSurePanel.SetActive(false); });
        noButton.GetComponent<Button>().onClick.AddListener(delegate () { makeSurePanel.SetActive(false); });


    }
    public void threatenTown()
    {
        
        townMenuButton(false);
        townPillage.SetActive(true);
        townRansom.SetActive(true);
        townRetreat.SetActive(true);
    }
    public void restockTown()
    {
        townRestock.SetActive(true);
        cityRestockSlideBar.GetComponent<Slider>().maxValue = Player.mainParty.cash;
        cityRestockSlideBar.GetComponent<Slider>().minValue = 0;
    }
    public void restockBuyTown()
    {

        townRestockSlideBar.GetComponent<Slider>().value = 0;
    }
    public void restockCancelTown()
    {
        townRestockSlideBar.GetComponent<Slider>().value = 0;
        townRestock.SetActive(false);
    }
    public void restockTownCancel()
    {
        townRestock.SetActive(false);
    }
    public void recruitTown()
    {

    }
    public void investTown()
    {

    }
    public void pillageTown()
    {
        townMenuButton(true);
        townPillage.SetActive(false);
        townRansom.SetActive(false);
        townRetreat.SetActive(false);
        createBattleScene();
        closeDialogue("town");
    }
    public void ransomTown()
    {
        townMenuButton(true);
        townPillage.SetActive(false);
        townRansom.SetActive(false);
        townRetreat.SetActive(false);
        closeDialogue("town");
    }
    public void retreatTown()
    {
        townMenuButton(true);
        townPillage.SetActive(false);
        townRansom.SetActive(false);
        townRetreat.SetActive(false);
        closeDialogue("town");
    }
    public void townMenuButton(bool hide)
    {
        townTalk.SetActive(hide);
        townRestock.SetActive(hide);
        townThreaten.SetActive(hide);
        townRecruit.SetActive(hide);
        townInvest.SetActive(hide);
        townLeave.SetActive(hide);
    }
    public void swapTownBackground(Texture2D img)
    {
        townBackground.GetComponent<RawImage>().texture = img;
    }


    //Common
    public void addNewDialogue(string name, string[] lines, string panelType) //panelType = "town", "city", "SNPC" or "NPC"
    {
        if (panelType == "NPC")
        {
            npcDialogueIndex = 0;
            npcDialogueLines = new List<string>(lines.Length);
            npcDialogueLines.AddRange(lines);
            npcName = name;
            createDialogue("NPC");
        }
        else if (panelType == "SNPC")
        {
            snpcDialogueIndex = 0;
            snpcDialogueLines = new List<string>(lines.Length);
            snpcDialogueLines.AddRange(lines);
            snpcName = name;
            createDialogue("SNPC");
        }
        else if (panelType == "city")
        {
            cityDialogueIndex = 0;
            cityDialogueLines = new List<string>(lines.Length);
            cityDialogueLines.AddRange(lines);
            cityName = name;
            createDialogue("city");
        }
        else if (panelType == "town")
        {
            townDialogueIndex = 0;
            townDialogueLines = new List<string>(lines.Length);
            townDialogueLines.AddRange(lines);
            townName = name;
            createDialogue("town");
        }
        
        
    }
    public void createDialogue(string panelType)
    {
        if (panelType == "NPC")
        {
            npcDialogueText.text = npcDialogueLines[npcDialogueIndex];
            npcNameText.text = npcName;
            NPCInteractionPanel.SetActive(true);
        }
        else if (panelType == "SNPC")
        {
            snpcDialogueText.text = snpcDialogueLines[snpcDialogueIndex];
            snpcNameText.text = npcName;
            SNPCInteractionPanel.SetActive(true);
        }
        else if (panelType == "city")
        {
            swapCityBackground(cityTalkImg);
            cityDialogueText.text = cityDialogueLines[cityDialogueIndex];
            cityNameText.text = cityName;
            cityInteractionPanel.SetActive(true);
        }
        else if (panelType == "town")
        {
            townDialogueText.text = townDialogueLines[townDialogueIndex];
            townNameText.text = townName;
            townInteractionPanel.SetActive(true);

        }
        statusPanel.SetActive(false);
        Time.timeScale = 0.0f;
    }
    public void continueDialogue(string panelType)
    {
        if (panelType == "NPC")
        {
            if (npcDialogueIndex < npcDialogueLines.Count - 1)
            {
                npcDialogueIndex++;
                npcDialogueText.text = npcDialogueLines[npcDialogueIndex];
            }
        }
        if (panelType == "SNPC")
        {
            if (snpcDialogueIndex < snpcDialogueLines.Count - 1)
            {
                snpcDialogueIndex++;
                snpcDialogueText.text = snpcDialogueLines[snpcDialogueIndex];
            }
        }
        else if (panelType == "city")
        {
            if (cityDialogueIndex < cityDialogueLines.Count - 1)
            {
                cityDialogueIndex++;
                cityDialogueText.text = cityDialogueLines[cityDialogueIndex];
            }
        }
        if (panelType == "town")
        {
            if (townDialogueIndex < townDialogueLines.Count - 1)
            {
                townDialogueIndex++;
                townDialogueText.text = townDialogueLines[townDialogueIndex];
            }
        }
        
        
    }
    public void closeDialogue(string panelType)
    {
        if (panelType == "NPC")
        {
            NPCInteractionPanel.SetActive(false);
        }
        else if (panelType == "SNPC")
        {
            SNPCInteractionPanel.SetActive(false);
        }
        else if (panelType == "city")
        {
            cityInteractionPanel.SetActive(false);
        }
        else if (panelType == "town")
        {
            townInteractionPanel.SetActive(false);
            
        }
        WorldInteraction.chasing = false;
        statusPanel.SetActive(true);
        Time.timeScale = 1.0f;
    }
    public void createBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
    
}