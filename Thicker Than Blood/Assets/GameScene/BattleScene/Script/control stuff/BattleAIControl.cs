﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAIControl : MonoBehaviour {
    public bool enemyPlaced;
    public Troop curControlled;
	// Use this for initialization
	void Start () {
        enemyPlaced = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!enemyPlaced && BattleCentralControl.battleStart)
        {
            placeEnemyOnMap();
            enemyPlaced = true;
        }
        if (!BattleCentralControl.playerTurn)
        {
            aggressive(BattleCentralControl.enemyParty.leader.troop);

            BattleCentralControl.playerTurn = true;
            BattleCentralControl.startTurnPrep();
        }
	}
    void placeEnemyOnMap()
    {
        int randX = (int)Random.Range(0, BattleCentralControl.gridXMax);
        int memberInBattle = 0;
        foreach (Person unit in BattleCentralControl.enemyParty.partyMember)
        {
            if (memberInBattle <= BattleCentralControl.enemyParty.leader.getTroopMaxNum())
            {
                List<Grid> placedGrids = new List<Grid>();
                int posZ, posX;
                int zRange = BattleCentralControl.enemyParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax);
                posZ = BattleCentralControl.gridZMax;
                switch (unit.troopType)
                {
                    case TroopType.crossbowman:
                        posZ = (int)(BattleCentralControl.gridZMax - 1 - zRange / 3);
                        break;
                    case TroopType.musketeer:
                        posZ = (int)(BattleCentralControl.gridZMax - 1 - zRange / 3);
                        break;
                    case TroopType.swordsman:
                        posZ = (int)(BattleCentralControl.gridZMax - 1 - zRange * 2 / 3);
                        break;
                    case TroopType.halberdier:
                        posZ = (int)(BattleCentralControl.gridZMax - 1 - zRange);
                        break;
                    case TroopType.cavalry:
                        posZ = (int)(BattleCentralControl.gridZMax - 1 - zRange);
                        break;
                    case TroopType.recruitType:
                        posZ = (int)(BattleCentralControl.gridZMax - 1 - zRange);
                        break;
                }
                posX = randX;
                while (BattleCentralControl.map[posX, posZ].troop != null)
                {
                    if (posX >= BattleCentralControl.gridXMax - 1)
                    {
                        posX = 0;
                    }
                    posX += 1;
                }

                var pos = new Vector3(posX, 1, posZ);
                var rot = new Quaternion(0, 0, 0, 0);
                GameObject unitToPlace = TroopDataBase.troopDataBase.getTroopObject(unit.faction, unit.troopType, unit.ranking);
                GameObject gridToPlace = BattleCentralControl.gridToObj[BattleCentralControl.map[posX, posZ]];
                unitToPlace = gridToPlace.GetComponent<GridObject>().placeTroopOnGrid(unitToPlace, pos, rot);
                unitToPlace.GetComponent<Troop>().placed(unit, BattleCentralControl.map[posX, posZ]);
                memberInBattle += 1;
            }
        }
    }


    void aggressive(Troop troop)
    {
        troop.controlled = true;
        if (getSeen().Count > 0)
        {
            blindForward(troop);
        } else
        {
            blindForward(troop);
        }
    }
    void blindForward(Troop troop)
    {
        //troop.curGrid.x, troop.curGrid.z - 10
        troop.troopMoveToPlace(BattleCentralControl.map[troop.getCurrentGrid().x, troop.getCurrentGrid().z - 15]);

        //BattleCentralControl.gridToObj[BattleCentralControl.map[1,1]].GetComponent<GridObject>().moveTroopToGrid(troop.gameObject);
    }
    List<Troop> getSeen()
    {
        List<Troop> result = new List<Troop>();
        foreach(Person unit in BattleCentralControl.playerParty.partyMember)
        {
            if (unit.troop != null && unit.troop.seenStatus && !result.Contains(unit.troop))
            {
                result.Add(unit.troop);
            }
        }
        return result;
    }
}
