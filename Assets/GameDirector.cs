using System.Data;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;

/// <summary>
/// ゲーム全体の処理（リザルトシーンに行っても消えない）
/// </summary>
public class GameDirector : MonoBehaviour
{
    //残り時間（秒）
    float timeLeft = 90f;

    //生き残ってるプレイヤーの数
    int plaerCount = 0;

    //残り時間ＵＩ
    public TextMeshProUGUI timeText;

    //勝者ＩＤ
    public string winner = "";

    public GameObject king;

    public GameObject[] players; // シーンにいるプレイヤー（4人分）
    public Image Phaikei;
    public Image PGauge;


    /// <summary>
    /// ゲームの状態
    /// </summary>
    public enum GameState
    {
        TAKE,   //王冠獲得ターン
        KING    //王様ターン
    }
    GameState state = GameState.TAKE;   //最初は王冠獲得ターン


    /// <summary>
    /// 最初
    /// </summary>
    void Start()
    {
        //シーンが切り替わっても消えないように
        DontDestroyOnLoad(gameObject);

        //プレイヤーの数をカウント
        plaerCount = GameObject.FindGameObjectsWithTag("Player").Length;

        int count = GameManager.Instance.playerCount;

        for (int i = count; i < players.Length; i++)
        {
            Destroy(players[i]);
            Phaikei.gameObject.SetActive(false);
            PGauge.gameObject.SetActive(false);
        }
        // ★ Destroy後に正しい人数を入れる
        plaerCount = count;
    }

    /// <summary>
    /// 毎フレーム
    /// </summary>
    void Update()
    {
        if (timeText != null)
        {
            //残り時間表示
            timeText.text = "" + (int)timeLeft / 60 + ":" + (int)timeLeft % 60;
        }
        //残り時間を減らす
        timeLeft -= Time.deltaTime;

        //残り時間が0
        if(timeLeft <= 0)
        {
            //王冠獲得シーンだったら
            if (state == GameState.TAKE)
            {
                //王冠を消す
                Destroy(GameObject.FindWithTag("Crown"));
            }

            //王様ターン
            else
            {
                //王様を消す
                //Destroy(king);
                
                //プレイヤーを消す
                DestroyPlayers();

                //ゲーム終了へ
                Invoke("ToResult", 0.5f);
            }
        }
    }


    /// <summary>
    /// 王様ターンへ移行
    /// </summary>
    public void ChangeState()
    {
        //残り時間は30秒
        timeLeft = 30;
        state = GameState.KING;
    }


    /// <summary>
    /// ハシゴの受け渡し
    /// </summary>
    /// <param name="attacker">攻撃した側のID</param>
    /// <param name="receiver">食らった側のID</param>
    public void LadderTransfer(int attacker, GameObject receiver)
    {
        //攻撃されたキャラからハシゴを減らす
        bool haveLadder = receiver.GetComponent<PlayerController>().LoseLadder();

        //攻撃されたキャラがハシゴを持っていたら
        if(haveLadder)
        {
            //タグが "Player" のオブジェクトをすべて取得
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            //取得したリストをループで回してIDをチェック
            foreach (GameObject playerObj in players)
            {
                PlayerController pc = playerObj.GetComponent<PlayerController>();
                if (pc != null && pc.ID == attacker)
                {
                    // 条件に一致するオブジェクトを見つけた！
                    pc.GetComponent<PlayerController>().ObtainingLadder();

                    break; // 見つかったのでループを抜ける
                }
            }
        }
    }


    /// <summary>
    /// 誰か死んだ（ビームに当たったプレイヤーから呼ばれる）
    /// </summary>
    public void Death()
    {
        //人数減らす
        plaerCount--;

        //残り1人
        if(plaerCount <= 1)
        {
            Invoke("ToResult", 0.5f);
        }
    }

    void ToResult()
    {
        winner = "";

        // "Player"タグがついた全オブジェクトを配列として取得
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // foreach文で一つずつ取り出して処理を行う
        foreach (GameObject player in players)
        {
            winner += "P" + player.GetComponent<PlayerController>().ID + " ";
        }


        //リザルトシーンへ
        SceneManager.LoadScene("ResultScene");
    }
void DestroyPlayers()
{
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    foreach (GameObject p in players)
    {
        PlayerController pc = p.GetComponent<PlayerController>();

        if (pc != null && pc.isKing == false)
        {
            Destroy(p);
        }
    }
}
}

