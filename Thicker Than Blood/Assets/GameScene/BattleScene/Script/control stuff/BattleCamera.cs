﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour {
    public static bool startBattle = false;
    public static GameObject target, freeMoveTarget;
    public static CameraMode cameraMode = CameraMode.toggleToCenter;
    public static GameObject mapCenter = null;
    public bool orbity = false;

    private bool lockCamera = false;
    private Vector3 positionOffset = Vector3.zero;
    private Vector3 freeMoveOffset = Vector3.zero;
    private FreeMoveMode freeMoveMode = FreeMoveMode.behind;
    private Vector3 behindPositionOffsetBase = new Vector3(0, 1, -2).normalized;
    private Vector3 rightPositionOffsetBase = new Vector3(2, 1, 0).normalized;
    private Vector3 leftPositionOffsetBase = new Vector3(-2, 1, 0).normalized;
    private Vector3 frontPositionOffsetBase = new Vector3(0, 1, 2).normalized;
    private Vector3 curFreeMovePositionOffsetBase, forward, right, left, backward;
    private Vector3 positionOffsetBase = Vector3.zero;
    private int multiplier, freeMoveMultiplier;
    private int counter;
    private bool switchedMode = false;
    private bool freeMoveOnDesignedAngle = true;
    private void Start()
    {
        multiplier = 10;
        freeMoveMultiplier = 8;
        positionOffsetBase = (new Vector3(0, 1, 2)).normalized;
        positionOffset = multiplier * positionOffsetBase;
        //freeMoveTarget = mapCenter;
        //freeMoveTarget.transform.position = mapCenter.transform.position;
        target = mapCenter;
        curFreeMovePositionOffsetBase = behindPositionOffsetBase;
        freeMoveOffset = curFreeMovePositionOffsetBase * freeMoveMultiplier;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (startBattle)
        {
            if (target != null)
            {
                switch (cameraMode)
                {
                    case CameraMode.toggleToCenter:
                        freeMoveTarget = mapCenter;
                        transform.LookAt(mapCenter.transform);
                        if (switchedMode)
                        {
                            transform.position = Vector3.Slerp(transform.position, mapCenter.transform.position + positionOffset, Time.deltaTime * 13f);
                        } else
                        {
                            transform.position = Vector3.Slerp(transform.position, mapCenter.transform.position + positionOffset, Time.deltaTime * 5f);
                            if (Vector3.Distance(transform.position, mapCenter.transform.position + positionOffset) < .1f)
                            {
                                switchedMode = true;
                            }

                        }
                        if (!TroopPlacing.pointerInPlacingPanel)
                        {
                            zoom();
                        }
                        rotate();
                        break;
                    case CameraMode.freeMove:
                        //var rotationFreeMove = Quaternion.LookRotation(freeMoveTarget.transform.position - transform.position);
                        //freeMoveOffset = freeMoveMultiplier * curFreeMovePositionOffsetBase;
                        //if (switchedMode)
                        //{
                        //    transform.rotation = Quaternion.Slerp(transform.rotation, rotationFreeMove, Time.deltaTime * 10f);
                        //    transform.position = Vector3.Slerp(transform.position, freeMoveTarget.transform.position + freeMoveOffset, Time.deltaTime * 5f);
                        //    if (Vector3.Distance(transform.position, freeMoveTarget.transform.position + freeMoveOffset) < 1f)
                        //    {
                        //        freeMoveOnDesignedAngle = true;
                        //    } else
                        //    {
                        //        freeMoveOnDesignedAngle = false;
                        //    }
                        //} else
                        //{
                        //    transform.rotation = Quaternion.Slerp(transform.rotation, rotationFreeMove, Time.deltaTime * 7f);
                        //    transform.position = Vector3.Slerp(transform.position, freeMoveTarget.transform.position + freeMoveOffset, Time.deltaTime * 3f);
                        //    if (Vector3.Distance(transform.position, freeMoveTarget.transform.position + freeMoveOffset) < .1f)
                        //    {
                        //        switchedMode = true;
                        //    }
                        //}
                        //
                        //freeMove();
                        if (Camera.current != null)
                        {
                            Vector3 dir = transform.forward;
                            if (Input.GetKey(KeyCode.W)) //forward
                            {
                                transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(dir.x, 0, dir.z), Time.deltaTime * 10f);
                            }
                            if (Input.GetKey(KeyCode.S)) //backward
                            {
                                transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(-dir.x, 0, -dir.z), Time.deltaTime * 10f);
                            }
                            if (Input.GetKey(KeyCode.A)) //left
                            {
                                transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(-dir.z, 0, dir.x), Time.deltaTime * 10f);
                            }
                            if (Input.GetKey(KeyCode.D)) //right
                            {
                                transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(dir.z, 0, -dir.x), Time.deltaTime * 10f);
                            }
                            
                        }
                           
                        if (!TroopPlacing.pointerInPlacingPanel)
                        {
                            freeMoveZoom();
                        }
                        break;
                    case CameraMode.mapObject:
                        var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                        
                        if (switchedMode)
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 13f);
                            transform.position = Vector3.Slerp(transform.position, target.transform.position + positionOffset, Time.deltaTime * 3f);
                        } else
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
                            transform.position = Vector3.Slerp(transform.position, target.transform.position + positionOffset, Time.deltaTime * 3f);
                            if (Vector3.Distance(transform.position, target.transform.position + positionOffset) < .01f)
                            {
                                switchedMode = true;
                            }
                        }
                        
                        freeMoveTarget = getGrid(target);
                        rotate();
                        if (!TroopPlacing.pointerInPlacingPanel)
                        {
                            zoom();
                        }
                        break;

                }
                if (Input.GetKey(KeyCode.Space) && cameraMode != CameraMode.toggleToCenter)
                {
                    if (BattleInteraction.curControlled == null)
                    {
                        switchedMode = false;
                        cameraMode = CameraMode.toggleToCenter;
                        positionOffsetBase = (new Vector3(0, 1, 2)).normalized;
                        target = mapCenter;
                        positionOffset = 4 * positionOffsetBase;
                    } else
                    {
                        switchedMode = false;
                        if (cameraMode == CameraMode.freeMove)
                        {
                            positionOffsetBase = curFreeMovePositionOffsetBase.normalized;
                        }

                        cameraMode = CameraMode.mapObject;
                    }
                    
                }
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && cameraMode != CameraMode.freeMove)
                {
                    switchedMode = false;
                    //getToNearestAngle(positionOffset);
                    cameraMode = CameraMode.freeMove;
                }
                if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && cameraMode != CameraMode.mapObject)
                {
                    switchedMode = false;
                    if (cameraMode == CameraMode.freeMove)
                    {
                        positionOffsetBase = curFreeMovePositionOffsetBase.normalized;
                    }
                    
                    cameraMode = CameraMode.mapObject;
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    lockCamera = !lockCamera;
                }
            }

        }
    }

    private void zoom()
    {
        if (!lockCamera)
        {
            if (Input.mousePosition.y > Screen.height - 2) //up, camera zoom in
            {
                counter++;
                if (counter == 3)
                {
                    multiplier--;
                    counter = 0;
                }
            }
            if (Input.mousePosition.y < 2) //down, camera zoom out
            {
                counter++;
                if (counter == 3)
                {
                    multiplier++;
                    counter = 0;
                }
            }
            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f) //scroll up
            {
                multiplier -= 3;
            }
            if (d < 0f)
            {
                multiplier += 3;
            }
            multiplier = Mathf.Clamp(multiplier, 5, 20);
            positionOffset = multiplier * positionOffsetBase;
        }
        
    }
    private void freeMoveZoom()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f) //scroll up
        {
            freeMoveMultiplier -= 3;
        }
        if (d < 0f)
        {
            freeMoveMultiplier += 3;
        }
        freeMoveMultiplier = Mathf.Clamp(freeMoveMultiplier, 5, 35);
        freeMoveOffset = freeMoveMultiplier * curFreeMovePositionOffsetBase;
    }
    private void rotate()
    {
        if (!lockCamera)
        {
            
            if ((Input.mousePosition.x > Screen.width - 2 ) || (Input.GetKey(KeyCode.E))) //right
            {
                positionOffsetBase = Quaternion.Euler(0, -1, 0) * positionOffsetBase;
            }
            if (Input.mousePosition.x < 2 || (Input.GetKey(KeyCode.Q))) //left
            {
                positionOffsetBase = Quaternion.Euler(0, 1, 0) * positionOffsetBase;
            }

            positionOffset = multiplier * positionOffsetBase;
        }
        
    }
    
    private void freeMove()
    {
        switchAngle();
        //Debug.Log(freeMoveMode);
        //WASD
        if (Input.GetKey(KeyCode.W))
        {
            if (Camera.current != null)
            {
                freeMoveTarget = getClosestGridObj(freeMoveTarget.transform.position + forward);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Camera.current != null)
            {
                freeMoveTarget = getClosestGridObj(freeMoveTarget.transform.position + left);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Camera.current != null)
            {
                freeMoveTarget = getClosestGridObj(freeMoveTarget.transform.position + backward);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Camera.current != null)
            {
                freeMoveTarget = getClosestGridObj(freeMoveTarget.transform.position + right);
            }
        }
    }
    private void getToNearestAngle(Vector3 offset)
    {
        float tangent = offset.z / offset.x;
        if (tangent >= 1 || tangent <= -1)
        {
            if (offset.z > 0.0f)
            {
                freeMoveMode = FreeMoveMode.front;
            } else
            {
                freeMoveMode = FreeMoveMode.behind;
            }
        } else
        {
            if (offset.x > 0.0f)
            {
                freeMoveMode = FreeMoveMode.right;
            }
            else
            {
                freeMoveMode = FreeMoveMode.left;
            }
        }
    }
    private void switchAngle()
    {
        if (Input.GetKeyUp(KeyCode.Q) && freeMoveOnDesignedAngle)
        {
            switchedMode = false;
            Debug.Log(freeMoveMode);
            switch (freeMoveMode)
            {
                case FreeMoveMode.behind:
                    freeMoveMode = FreeMoveMode.left;
                    break;
                case FreeMoveMode.left:
                    freeMoveMode = FreeMoveMode.front;
                    break;
                case FreeMoveMode.front:
                    freeMoveMode = FreeMoveMode.right;
                    break;
                case FreeMoveMode.right:
                    freeMoveMode = FreeMoveMode.behind;
                    break;
            }
            Debug.Log("after: " + freeMoveMode);
        }
        if (Input.GetKeyUp(KeyCode.E) && freeMoveOnDesignedAngle)
        {
            switchedMode = false;
            switch (freeMoveMode)
            {
                case FreeMoveMode.behind:
                    freeMoveMode = FreeMoveMode.right;
                    break;
                case FreeMoveMode.left:
                    freeMoveMode = FreeMoveMode.behind;
                    break;
                case FreeMoveMode.front:
                    freeMoveMode = FreeMoveMode.left;
                    break;
                case FreeMoveMode.right:
                    freeMoveMode = FreeMoveMode.front;
                    break;
            }
        }
        switch(freeMoveMode)
        {
            case FreeMoveMode.behind:
                curFreeMovePositionOffsetBase = behindPositionOffsetBase;
                forward = new Vector3(0, 0.0f, 1);
                right = new Vector3(1, 0.0f, 0);
                left = new Vector3(-1, 0.0f, 0);
                backward = new Vector3(0, 0.0f, -1);
                break;
            case FreeMoveMode.left:
                curFreeMovePositionOffsetBase = leftPositionOffsetBase;
                forward = new Vector3(1, 0.0f, 0);
                right = new Vector3(0, 0.0f, -1);
                left = new Vector3(0, 0.0f, 1);
                backward = new Vector3(-1, 0.0f, 0);
                break;
            case FreeMoveMode.front:
                curFreeMovePositionOffsetBase = frontPositionOffsetBase;
                forward = new Vector3(0, 0.0f, -1);
                right = new Vector3(-1, 0.0f, 0);
                left = new Vector3(1, 0.0f, 0);
                backward = new Vector3(0, 0.0f, 1);
                break;
            case FreeMoveMode.right:
                curFreeMovePositionOffsetBase = rightPositionOffsetBase;
                forward = new Vector3(-1, 0.0f, 0);
                right = new Vector3(0, 0.0f, 1);
                left = new Vector3(0, 0.0f, -1);
                backward = new Vector3(1, 0.0f, 0);
                break;
        }
    }
    private GameObject getClosestGridObj(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 0, BattleCentralControl.gridXMax - 1);
        pos.z = Mathf.Clamp(pos.z, 0, BattleCentralControl.gridZMax - 1);
        return BattleCentralControl.map[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)].gridObject;
    }
    private GameObject getGrid(GameObject obj)
    {
        if (obj.tag == "Grid")
        {
            return obj;
        } else if (obj.tag == "Troop")
        {
            return obj.GetComponent<Troop>().curGrid.gridObject;
        } else
        {
            return null;
        }
    }
}

public enum CameraMode
{
    toggleToCenter,
    freeMove,
    mapObject
};
public enum FreeMoveMode
{
    behind,
    right,
    left,
    front
}