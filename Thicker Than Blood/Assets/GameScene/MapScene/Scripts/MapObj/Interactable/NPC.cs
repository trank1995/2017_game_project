﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPC : Interactable {
    public NavMeshAgent npcAgent;
    public Party npcParty;
    public override void Start()
    {
        base.Start();
        npcAgent = transform.GetComponent<NavMeshAgent>();
        
    }
    public override void Update()
    {
        base.Update();
        npcAgent.speed = npcParty.getTravelSpeed();
    }
    public override void interact()
    {
        base.interact();
        DialogueSystem.Instance.addNewDialogue(npcParty, dialogue, PanelType.NPC);
        DialogueSystem.Instance.createDialogue(PanelType.NPC, npcParty);
    }
    public override void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            //start dialogue
            DialogueSystem.Instance.addNewDialogue(npcParty, dialogue, PanelType.NPC);
            DialogueSystem.Instance.createDialogue(PanelType.NPC, npcParty);

        }
    }

    public override void inspect(bool inspecting)
    {
        if (inspecting)
        {
            inspectPanel.SetActive(true);

        }
        else
        {
            inspectPanel.SetActive(false);
        }
    }

    public virtual void roam()
    {

    }
    
    public virtual void grow()
    {

    }
    
}
