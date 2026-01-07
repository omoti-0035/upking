using UnityEngine;


/// <summary>
/// 落ちてくるハシゴアイテムの処理
/// </summary>
public class LadderItem : MonoBehaviour
{
    //落下速度（Unity側で調整）
    public float speed = 3.0f;

    //毎フレーム
    void Update()
    {
        //移動
        this.transform.Translate(Vector3.down * 3.0f * Time.deltaTime);

        //画面外へ行ったら削除
        if(this.transform.position.y < -3)
        {
            Destroy(this.gameObject);
        }
    }
}
