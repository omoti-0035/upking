using UnityEngine;


/// <summary>
/// 落下するハシゴアイテムのジェネレーター
/// </summary>
public class LadderGenerator : MonoBehaviour
{
    //アイテムのプレファブ
    public GameObject ladderPrefab;

    //何秒おきに落下させるか（Unity側で調整）
    public float coolTime = 3.0f;

    //経過時間
    float delta = 0.0f;



    // Update is called once per frame
    void Update()
    {
        //経過時間
        delta -= Time.deltaTime;

        //時間になったら
        if(delta <= 0.0f)
        {
            //２～４個アイテムを生成
            int count = Random.Range(2, 5);
            for(int i = 0; i < count; i++)
            {
                Vector3 position = new Vector3(
                    Random.Range(-7.0f, 7.0f),
                    transform.position.y,
                    0.0f
                );
                Instantiate(ladderPrefab, position, Quaternion.identity);
            }

            //クールタイム設定
            delta = coolTime;
        }
    }
}
