using TMPro;
using UnityEngine;




/// <summary>
/// リザルトシーンの処理（Canvasにアタッチしてある）
/// </summary>
public class ResultScene : MonoBehaviour
{
    //テキストＵＩ
    public TextMeshProUGUI winnerText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameDirector gd = GameObject.Find("GameDirector")
                                   .GetComponent<GameDirector>();


        if (gd.isDraw)
        {
            // ★ドロー表示
            winnerText.text = "DRAW";
        }
        else
        {
            // ★勝者表示
            winnerText.text = gd.winner + " WIN!";
        }
    }




}



