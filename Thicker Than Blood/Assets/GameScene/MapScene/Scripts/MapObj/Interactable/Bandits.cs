﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bandits : NPC {
    public GameObject spawnPoint;
    const int roamRange = 60;
    // Use this for initialization
    public override void Start () {
        base.Start();
        dialogue = new string[] { "hello", "i will kill u" };
        name = "CoolBandits";
        npcParty = new Party(name, Faction.bandits, 300);
        npcParty.prestige = 0;
        npcParty.notoriety = 80;
        makeParty();
        roam();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
		if (!TimeSystem.pause)
        {
            npcAgent = transform.GetComponent<NavMeshAgent>();
            npcAgent.isStopped = false;
            if (Vector3.Distance(npcAgent.destination,transform.position) <= 10)
            {
                roam();
            }
        } else
        {
            npcAgent.isStopped = true;
        }
        inspectPanel.GetComponent<InspectPanel>().updateTexts(npcParty);
	}
    
    public override void roam()
    {
        if (spawnPoint != null)
        {
            if (Vector3.Distance(transform.position, spawnPoint.transform.position) >= roamRange)
            {
                npcAgent.destination = spawnPoint.transform.position;
            }
            else
            {
                Vector3 randomV = new Vector3(Random.Range(-roamRange, roamRange), 0, Random.Range(-roamRange, roamRange));
                npcAgent.destination = spawnPoint.transform.position + randomV;
            }

        }
        else
        {
            Vector3 randomV = new Vector3(Random.Range(-roamRange, roamRange), 0, Random.Range(-roamRange, roamRange));
            npcAgent.destination = transform.position + randomV;
        }
        
    }
    public void setSpawnPoint(GameObject sp)
    {
        spawnPoint = sp;
    }
    public override void grow()
    {

    }
    public void makeParty()
    {
        for (int i = 0; i < npcParty.getPartySizeLimit(); i ++)
        {
            TroopType tt = npcParty.randomTroopType(20, 20, 10, 30, 10, 10);
            Ranking rk = npcParty.randomRanking(0, 10, 10, 10);
            if (tt == TroopType.recruitType)
            {
                rk = Ranking.recruit;
            }
            Person p = npcParty.makeGenericPerson(tt, rk);
            if (npcParty.battleValue >= p.battleValue)
            {
                if (npcParty.addToParty(npcParty.makeGenericPerson(tt, rk)))
                {
                    npcParty.battleValue -= p.battleValue;
                }
            }
        }
        if (npcParty.battleValue > 0)
        {
            foreach(Person unit in npcParty.partyMember)
            {
                TroopType tt = unit.troopType;
                Ranking rk = unit.ranking;
                if (unit.ranking == Ranking.recruit)
                {
                    tt = npcParty.randomTroopType(0, 20, 10, 30, 10, 10);
                    rk = npcParty.randomRanking(0, 10, 10, 10);
                } else if (unit.ranking == Ranking.militia)
                {
                    rk = npcParty.randomRanking(0, 0, 10, 10);
                } else if (unit.ranking == Ranking.veteran)
                {
                    rk = Ranking.elite;
                }
                npcParty.battleValue = unit.changeRankingTroopType(rk, tt, npcParty.battleValue, true);
            }
        }
    }
}
