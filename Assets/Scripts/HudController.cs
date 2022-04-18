//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;
using TMPro;

// this class manages messages sent from various objects sends the appropriate response to the display.
//      including score and time updates, and warnings for things like,
//      trying to pickup an item when your hands are full
public class HudController : MonoBehaviour
{
    // ---data members---
    [SerializeField] private GameObject[] objectsToHideIn1PlayerGame;
    [SerializeField] private TMP_Text player1Name;
    [SerializeField] private TMP_Text player2Name;
    [SerializeField] private TMP_Text player1Timer;
    [SerializeField] private TMP_Text player2Timer;
    [SerializeField] private TMP_Text player1Score;
    [SerializeField] private TMP_Text player2Score;
    [SerializeField] private Transform player1LeftItemSlot;
    [SerializeField] private Transform player1RightItemSlot;
    [SerializeField] private Transform player2LeftItemSlot;
    [SerializeField] private Transform player2RightItemSlot;

    private StageController stageController;

    // ---getters---
    private StageController GetStageController() { return stageController; }
    private Transform GetPlayer1LeftItemSlot() { return player1LeftItemSlot; }
    private Transform GetPlayer1RightItemSlot() { return player1RightItemSlot; }
    private Transform GetPlayer2LeftItemSlot() { return player2LeftItemSlot; }
    private Transform GetPlayer2RightItemSlot() { return player2RightItemSlot; }

    // ---setters---
    private void SetStageController(StageController stageCont) { stageController = stageCont; }

    // ---unity methods---
    private void Update()
    {
        // update player time display
        player1Timer.SetText("" + (int)stageController.GetPlayer1TimeLeft()); // change these to toString methods
        player2Timer.SetText("" + (int)stageController.GetPlayer2TimeLeft());
        player1Score.SetText("" + stageController.GetPlayer1Score());
        player2Score.SetText("" + stageController.GetPlayer2Score());
    }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(StageController stageCont, GameController gameCont)
    {
        SetStageController(GameObject.FindGameObjectWithTag("Stage Controller").GetComponent<StageController>());
        player1Name.SetText(gameCont.GetGameData().GetPlayer1Name());
        player2Name.SetText(gameCont.GetGameData().GetPlayer2Name());
        player1Score.SetText(stageCont.GetPlayer1Score().ToString());
        player2Score.SetText(stageCont.GetPlayer2Score().ToString());
        UpdateInventoryHud(1, null, null);
        UpdateInventoryHud(2, null, null);
    }

    // hides player 2 info panels
    public void HidePlayer2Hud()
    {
        foreach (GameObject g in objectsToHideIn1PlayerGame)
        {
            g.SetActive(false);
        }
    }

    // update the item inventory of the given player
    public void UpdateInventoryHud(int playerNumber, Item leftHandItem, Item rightHandItem)
    {
        if (playerNumber == 1)
        {
            foreach (Transform t in GetPlayer1LeftItemSlot())
            {
                Destroy(t.gameObject);
            }
            foreach (Transform t in GetPlayer1RightItemSlot())
            {
                Destroy(t.gameObject);
            }
            if (leftHandItem != null)
            {
                GameObject newImage = Instantiate(leftHandItem.GetItemImage(), GetPlayer1LeftItemSlot());
                newImage.transform.localPosition = Vector3.zero;
                newImage.SetActive(true);
            }
            if (rightHandItem != null)
            {
                GameObject newImage = Instantiate(rightHandItem.GetItemImage(), GetPlayer1RightItemSlot());
                newImage.transform.localPosition = Vector3.zero;
                newImage.SetActive(true);
            }
        }
        else
        {
            foreach (Transform t in GetPlayer2LeftItemSlot())
            {
                Destroy(t.gameObject);
            }
            foreach (Transform t in GetPlayer2RightItemSlot())
            {
                Destroy(t.gameObject);
            }
            if (leftHandItem != null)
            {
                GameObject newImage = Instantiate(leftHandItem.GetItemImage(), GetPlayer2LeftItemSlot());
                newImage.transform.localPosition = Vector3.zero;
                newImage.SetActive(true);
            }
            if (rightHandItem != null)
            {
                GameObject newImage = Instantiate(rightHandItem.GetItemImage(), GetPlayer2RightItemSlot());
                newImage.transform.localPosition = Vector3.zero;
                newImage.SetActive(true);
            }
        }
    }

    // Flashes a full hands warning on the hud
    public void NoticeFullHands(Player player)
    {
        // Future Feature
        // will flash a symbol over the player
    }

    // Flashes an unchoppable item warning on the hud
    public void NoticeUnchopableItem(Player player)
    {
        // Future Feature
        // will flash a symbol over the player
    }
    
    public void NoticeAddPlayerScore(int player, int score)
    {
        // future feature
        // will flash a floating number over the player
    }

    public void NoticeSubtractPlayerScore(int player, int score)
    {
        // future feature
        // will flash a floating number over the player
    }
}
