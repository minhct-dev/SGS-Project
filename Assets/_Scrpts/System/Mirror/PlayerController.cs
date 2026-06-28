using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Collections;
using Unity.VisualScripting;
using Mirror.Examples.Basic;
using System.Security.Cryptography;
using Object = UnityEngine.Object;
using UnityEngine.Rendering;
using Mirror.BouncyCastle.Security;
[Serializable]
public class PlayerController : NetworkBehaviour
{
    [SyncVar]
    public bool isRoomMaster = false;
    [Header("Player Info")]
    [SyncVar(hook = nameof(UpdatePlayerName))] public string username;

    // SyncVar hook to call a command whenever a username changes (like when players load in initially).

    //[Header("Portrait")]
    //public Sprite portrait; //For the player's icon at the top left of the screen & in the PartyHUD.(now it for nothing tho)

    [Header("Deck and Hand")]
    public readonly SyncListCardInstance currentHand = new();

    [Header("Stats")]
    [SyncVar] public int maxHP = 4;
    [SyncVar] public int currentHP = 0;
    [SyncVar] public Vector3 playerPosition;
    // Quicker access for UI scripts
    [HideInInspector] public static PlayerController localPlayer;
    [HideInInspector] public bool hasOpponent = false;
    [SerializeField] private PlayerUI playerUI;

    [Header("Answering card state: ")]
    public bool isSelecting = false;
    public bool isPlayedCard = false;
    public List<CardInstanceData> answeredCards = new List<CardInstanceData>();
    public CardInteractData chosenInteractData;

    [Header("UI Lock State")]
    [HideInInspector] public bool isWaitingForServer = false;

    [Header("Status Buffs")]
    [SyncVar] public bool isDrunk = false;
    [SyncVar] public bool HasUseWineCard = false;

    //[HideInInspector] public PlayerInfo opponentInfo; // We can't pass a Player class through the Network, but we can pass structs. 
    // We store all our enemy's info in a PlayerInfo struct so we can pass it through the network when needed.

    // [HideInInspector] public static GameManager gameManager;
    [SyncVar, HideInInspector] public bool firstPlayer = false;
    //overide from networkbehavior
    private void Start()
    {
        //popup for player choose card 

    }

    public void Update()
    {

        // Get EnemyInfo as soon as another player connects. Only start updating once our Player has been loaded in properly (username will be set if loaded in).
        if (!hasOpponent && username != "")
        {
            UpdateEnemyInfo();
        }
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localPlayer = this;
        //Debug.Log($"OnStartLocalPlayer {gameObject.name}");
        // Get and update the player's username and stats
        LocalPlayerUI ui = FindAnyObjectByType<LocalPlayerUI>();
        CmdLoadPlayer(PlayerPrefs.GetString("Name"));
        ui.InitializeUI(this);
        CmdLoadHP(4);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //Debug.Log($"OnStartClient {gameObject.name} isLocalPlayer = {isLocalPlayer}");
        if (!isLocalPlayer)
        {
            int positionIndex = PlayerPortraitCreator.Instance.GetNextAvailableIndex();
            PlayerUI playerportrait = PlayerPortraitCreator.Instance.CreatePlayerPotrait(playerUI, this, positionIndex);
            playerPosition = playerportrait.gameObject.transform.position;
        }
        if (isLocalPlayer)
        {
            currentHand.Callback += OnHandChanged;
        }
    }

    //---------------------
    public void UpdatePlayerName(string oldUser, string newUser)
    {
        // Update username
        username = newUser;
        // Update game object's name in editor (only useful for debugging).
        gameObject.name = username;
    }
    //hook: OnHandChanged triggered when hand add more new card
    private void OnHandChanged(SyncListCardInstance.Operation op, int index, CardInstanceData oldItem, CardInstanceData newItem)
    {
        if (!isLocalPlayer) return;
        switch (op)
        {
            case SyncListCardInstance.Operation.OP_ADD:
                // Bài được thêm vào tay
                // (newItem chứa dữ liệu lá bài mới, oldItem là null/default)
                // Debug.Log("Card added to " + this.name + " hand");
                break;

            case SyncListCardInstance.Operation.OP_REMOVEAT:
                // BÀI BỊ XÓA KHỎI TAY NẰM Ở ĐÂY
                OnHandRemove(index);
                break;

            case SyncListCardInstance.Operation.OP_CLEAR:
                // (Tùy chọn) Phòng trường hợp bạn dùng hàm currentHand.Clear() để lột sạch bài
                Debug.Log("Toàn bộ bài trên tay đã bị xóa sạch!");
                break;
        }
    }
    //Call when one card in player's hand removed
    private void OnHandRemove(int index)
    {
        // TODO: code update UI 
        CardSystem.Instance.RemoveCardVisualAtIndex(index);
    }
    //Command to reduce card in hand when playcard
    [Command]
    public void CmdPlayCard(CardInstanceData cardInstanceData, uint[] listTargetIds)
    {
        if (TurnManagerSystem.Instance.activePlayerNetId != this.netId)
        {
            Debug.Log($"Currently is not player {this.name} turn!!");
            return;
        }
        if (TurnManagerSystem.Instance.currentPhase != TurnPhase.Play)
        {
            Debug.Log("Currently is not PLAY PHASE!!");
            return;
        }
        PlayCardGA playCardGA = new PlayCardGA(this, cardInstanceData, listTargetIds);
        ActionSystem.Instance.Perform(playCardGA);
    }

    //Command request to end turn 
    //Load UI -----------------------------------------
    [Command]
    public void CmdLoadPlayer(string user)
    {
        // Update the player's username, which calls a SyncVar hook.
        // Learn more here : https://mirror-networking.com/docs/Guides/Sync/SyncVarHook.html
        username = user;
    }

    [Command]
    public void CmdLoadHP(int hp)
    {
        maxHP = hp;
        currentHP = hp;
    }

    public void UpdateEnemyInfo()
    {
        // Find all Players and add them to the list.
        PlayerController[] onlinePlayers = Object.FindObjectsByType<PlayerController>();

        // Loop through all online Players (should just be one other Player)
        foreach (PlayerController players in onlinePlayers)
        {
            // Make sure the players are loaded properly (we load the usernames first)
            if (players.username != "")
            {
                // There should only be one other Player online, so if it's not us then it's the enemy.
                if (players != this)
                {
                    //Get & Set PlayerInfo from our Enemy's gameObject
                    //PlayerInfo currentPlayer = new PlayerInfo(players.gameObject);
                    //enemyInfo = currentPlayer;
                    hasOpponent = true;
                    //enemyInfo.data.casterType = Target.OPPONENT;
                    //Debug.LogError("Player " + username + " Enemy " + enemy.username + " / " + enemyInfo.username); // Used for Debugging
                }
            }
        }
    }
    //Command send to sever to request endturn and switch turn to other player
    [Command]
    public void CmdEndTurn()
    {
        TurnManagerSystem.Instance.RequestEndTurn(this);
    }

    [Command]
    public void CmdSubmitRequestedCard(CardInstanceData cardData)
    {
        if (!isSelecting) return;
        currentHand.Remove(cardData);
        CardSystem.Instance.RpcPlayCardVisual(this, cardData);
        answeredCards.Add(cardData);
        isSelecting = false; // Phá vỡ vòng lặp while trên Server ngay lập tức!
        isPlayedCard = true;
        Debug.Log($"[Server] {gameObject.name} đã ném ra lá {cardData.cardId}");
    }
    [Command]
    public void CmdCancelSubmitCard()
    {
        if (!isSelecting) return;
        answeredCards.Clear();
        isSelecting = false; // Phá vỡ vòng lặp while trên Server
        isPlayedCard = false;
        CardView.ForceUnselectAll();
        Debug.Log($"[Server] {gameObject.name} chọn Bỏ qua/Không có bài.");
    }

    /// <summary>
    /// Hàm này được gọi từ TargetInventoryUIManager ở Client để gửi kết quả lên Server
    /// </summary>
    [Command]
    public void CmdInteractWithTargetCard(CardArea area, int handIndex, string cardId)
    {
        // Rào chắn bảo mật: Nếu Server không yêu cầu chọn bài mà Client cố tình gửi lệnh lên thì chặn lại (Chống Hack)
        if (!isSelecting)
        {
            Debug.LogWarning($"[Security] {gameObject.name} cố tình gửi lệnh chọn bài trái phép!");
            return;
        }

        // Lưu lại dữ liệu người chơi vừa chọn
        chosenInteractData = new CardInteractData
        {
            Area = area,
            HandIndex = handIndex,
            CardId = cardId
        };

        // Quan trọng nhất: Tắt cờ trạng thái để Vòng lặp while() trong BreakPerformer được mở khóa và chạy tiếp!
        isSelecting = false;
    }
    public bool IsDead() => currentHP <= 0;
    public bool isMyTurn() => TurnManagerSystem.Instance.activePlayerNetId == this.netId;
    //public bool CanAttack() => Player.gameManager.isOurTurn && waitTurn == 0 && casterType == Target.FRIENDLIES; (extension for future)
    //public bool CantAttack() => Player.gameManager.isOurTurn && waitTurn > 0 && casterType == Target.FRIENDLIES; (extension for future)
}
