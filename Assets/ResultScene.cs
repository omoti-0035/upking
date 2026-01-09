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
        //「○Ｐ　ＷＩＮ！！」
        winnerText.text = "" + GameObject.Find("GameDirector").GetComponent<GameDirector>().winner + "WIN!";
    }


}
