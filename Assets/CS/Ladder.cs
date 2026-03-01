using UnityEngine;

/// <summary>
/// 設置されてるハシゴの処理
/// </summary>
public class Ladder : MonoBehaviour
{
    //耐久力
    int life = 2;


    /// <summary>
    /// ダメージ（弾がハシゴに当たると呼ばれる）
    /// </summary>
    public void Damage()
    {
        life--;
        if (life <= 0)
        {
            //繋がってるハシゴは親子関係になってるので、1個消せば、その上のハシゴも消える
            Destroy(gameObject);
        }
    }
}
