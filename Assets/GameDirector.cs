using System.Data;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;


/// <summary>
/// ゲーム全体の処理（リザルトシーンに行っても消えない）
/// </summary>
public class GameDirector : MonoBehaviour
{
    //残り時間（秒）
    float timeLeft = 180f;

    //生き残ってるプレイヤーの数
    int plaerCount = 0;

    //残り時間ＵＩ
    public TextMeshProUGUI timeText;

    //勝者ＩＤ
    public int winner = -1;


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
    }

    /// <summary>
    /// 毎フレーム
    /// </summary>
    void Update()
    {
        //残り時間表示
        timeText.text = "" + (int)timeLeft / 60 + ":" + (int)timeLeft % 60;

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
        }
    }


    /// <summary>
    /// 王様ターンへ移行
    /// </summary>
    public void ChangeState()
    {
        //残り時間は120秒
        timeLeft = 120;
        state = GameState.KING;
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
        //残ったプレイヤーのIDを取得
        winner = GameObject.FindWithTag("Player").GetComponent<PlayerController>().ID;

        //リザルトシーンへ
        SceneManager.LoadScene("ResultScene");
    }
}