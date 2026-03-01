using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class Bullet : MonoBehaviour
{
    //弾の速度（Unity側で調整して
    public float speed = 5f;

    //向き（進行方向）
    public int direction = 1; // 1: right, -1: left

    // 弾を撃ったプレイヤーのID
    public int ownerID; 

    /// <summary>
    /// 毎フレーム
    /// </summary>
    void Update()
    {
        //移動
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);


        //画面外へ行ったら削除
        if (transform.position.x > 10 || transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 何かに当たった
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //ハシゴに当たった
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            //ハシゴにダメージ与える
            other.GetComponent<Ladder>().Damage();

            //弾を削除
            Destroy(gameObject);
        }


        //プレイヤーに当たった
        if (other.CompareTag("Player"))
        {

            //弾を削除
            Destroy(gameObject);


            GameObject.Find("GameDirector").GetComponent<GameDirector>().LadderTransfer(ownerID, other.gameObject);


        }


    }
}