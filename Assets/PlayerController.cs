using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System;


/// <summary>
/// プレイヤーの処理
/// </summary>
public class PlayerController : MonoBehaviour
{
    //Unity側でPlayerのプレハブを選んで調整して
    [Header("Movement")]
    public float walkSpeed = 6.0f;      //歩く速度
    public float dashSpeed = 6.0f;      //ダッシュ時の速度
    public float ladderSpeed = 3.0f;    //ハシゴを上り下りする時の速度
    public float jumpHeight = 1.0f;     //ジャンプ力
    public float gravity = -9.81f;      //重力
    public float maxcooltime = 1.5f;    //プレイヤーの弾クールタイム
    float currentCooldown = 0f;         //クールタイム
    bool canFire = true;
    public int maxladder = 3;

    //各プレファブ
    public GameObject ladderPrefab; //ハシゴ
    public GameObject bulletPrefab; //通常弾
    public GameObject beamPrefab;   //ビーム

    //プレイヤーID（Unity側で番号指定）
    public int ID = 1;

    //プレイヤーの梯子の数
    public TextMeshProUGUI PladdersText;

    //プレイヤーの弾クールタイム
    public Image cooldownGauge;


    //以下は触らない
    private CharacterController controller;
    private Vector3 velocity;
    private Transform modelTransform;
    private Animator animator;　// アニメーション
    private bool isGrounded;    //地面に振れてるかフラグ
    bool isClimbing = false;    //ハシゴ上り下り中フラグ
    int ladderCount = 0;        //持ってるハシゴの数
    int direction = 1;          //向き（右：+1／左：-1）

    bool isLadderRoot = false;  //ハシゴの根元に触れてるかフラグ
    GameObject ladderRootObject = null; //今触れてるハシゴの根元


    public bool isKing = false;    //王様になったかフラグ

    float isMove = 0;       //移動できるまでの残り時間

    //攻撃を受けてスタンしてる時間
    float stanTime = 0;



    //触れてるハシゴの一覧
    private List<Collider> activeLadders = new List<Collider>();

    //ハシゴに触れてるか
    public bool isOnLadder => activeLadders.Count > 0;



    /// <summary>
    /// 最初
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();   //Animator取得
        if (cooldownGauge != null)
            cooldownGauge.fillAmount = 1f;
    }



    /// <summary>
    /// 常に
    /// </summary>
    void Update()
    {
        UpdateCooldownGauge();
        //動けるようになるまでの時間を減らす
        isMove -= Time.deltaTime;
        if (isMove > 0) //まだ残ってるなら操作不能
        {
            return;
        }


        stanTime -= Time.deltaTime;
        if (stanTime > 0) //まだスタン中なら操作不能
        {
            return;
        }


        //触れてるハシゴの管理
        if (activeLadders.Count > 0)
        {
            activeLadders.RemoveAll(l => l == null || !l.enabled || !l.gameObject.activeInHierarchy);
        }
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;


        //移動用ボタン確認
        float moveX = Input.GetAxisRaw("Horizontal" + ID);
        float moveY = Input.GetAxisRaw("Vertical" + ID);
        bool isDashing = Input.GetButton("Dash" + ID) || Input.GetKey(KeyCode.LeftShift);


        //ハシゴを上り下り
        if (isClimbing)
        {
            LadderMove(moveY);
        }

        //地面の上を移動中
        else
        {
            GroundMove(moveX, moveY, isDashing);
        }

        //ハシゴ設置
        SetupLadder();

        //攻撃
        Fire();

        //アニメーション更新
<<<<<<< HEAD
        //UpdateAnimation();
    }

    //private void UpdateAnimation()
    //{
       // animator.Play("Idle");
    //}
=======
      //  UpdateAnimation();
    }

    /*private void UpdateAnimation()
    {
        animator.Play("Idle");
    }*/
>>>>>>> 40c746916ce905547f307b8577c7d905c6fe1cc6

    //テキスト更新
    void UpdateText()
    {
      PladdersText.text = ladderCount.ToString();  
    }

    void UpdateAnimation(float moveX)
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsClimbing", isClimbing);

        if (moveX != 0)
        {
            direction = moveX > 0 ? 1 : -1;

            Vector3 scale = modelTransform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            modelTransform.localScale = scale;
        }

    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void Fire()
    {
        //攻撃ボタン押した
        if (Input.GetButtonDown("Fire" + ID) && canFire)
        {

            canFire = false;
            StartCoroutine(FireCooldown());
            currentCooldown = 0f;

            //if (animator != null)
                //animator.SetTrigger("Fire");   //攻撃アニメ

            //王様の時
            if (isKing)
            {
                //1秒後にビーム発射メソッドを呼ぶ(0.5秒に変更)
                Invoke("Beam", 0.5f);
                //ビームを撃ったら0.5秒間は行動不能
                isMove = 0.5f;
            }

            //ふつうのプレイヤー
            else
            {
                //通常弾生成
                GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.right * direction * 0.8f, Quaternion.identity);
                if (cooldownGauge != null)
                cooldownGauge.fillAmount = 0f;

                //弾の向きをプレイヤーに合わせる
                bullet.GetComponent<Bullet>().direction = direction;

                //弾を撃ったプレイヤーのIDを設定
                bullet.GetComponent<Bullet>().ownerID = ID;
            }
        }
    }

        void UpdateCooldownGauge()
    {
        if (canFire) return;

        currentCooldown += Time.deltaTime;

        if (cooldownGauge != null)
            cooldownGauge.fillAmount = currentCooldown / maxcooltime;
    }

        //クールタイム処理
    IEnumerator FireCooldown()
{
    yield return new WaitForSeconds(maxcooltime);
    canFire = true;
}

    /// <summary>
    /// ハシゴ設置
    /// </summary>
    private void SetupLadder()
    {
        //設置ボタン押した＆＆地面を歩いてる
        if (Input.GetButtonDown("Setup" + ID) && ladderCount > 0 && isGrounded)
        {

            //ハシゴを生成
            GameObject ladder = Instantiate(ladderPrefab);

            //既存のハシゴの根元に触れてる
            if (isLadderRoot)
            {

                //今作ったハシゴの親を、既存のハシゴにする
                GameObject parent = ladderRootObject;

                int nest = 0;   

                //すでに既存のハシゴに子がいたら、孫にする（繰り返す）
                while (parent.transform.childCount > 1)
                {
                    parent = parent.transform.GetChild(1).gameObject;
                    nest++;
                }

                //もう梯子3段以上
                /*if (nest > 1)
                {
                    Destroy(ladder);
                    return;
                }*/

                //手持ちハシゴ減らす
                ladderCount--;
                UpdateText();
                Debug.Log(ID + "梯子の数:" + ladderCount);




                ladder.transform.parent = parent.transform;
                ladder.transform.localPosition = new Vector3(0, 1, 0);
            }

            //新規
            else
            {
                //高さを調整（地面にピッタリ合わせる)
                float y = (int)(transform.position.y - 0.3f) + 0.5f;
                ladder.transform.position = new Vector3(transform.position.x, y, 0);

                //そのハシゴを（根本）にする
                ladder.tag = "LadderRoot";

                ladderCount--;
                UpdateText();
                Debug.Log(ID + "梯子の数:" + ladderCount);
            }
        }
    }


    /// <summary>
    /// 地面の上を左右に歩く処理
    /// </summary>
    /// <param name="moveX">スティックの左右傾き</param>
    /// <param name="moveY">スティックの上下傾き</param>
    /// <param name="isDashing">ダッシュボタン押してるか</param>
    private void GroundMove(float moveX, float moveY, bool isDashing)
    {
        //ハシゴに触れてる
        if (isOnLadder)
        {
            //ハシゴの上り下り開始
            if ((isLadderRoot == true && moveY > 0.2) || (isLadderRoot == false && moveY < -0.2))
            {
                //位置を調整
                Vector3 pos = transform.position;
                pos.z = -1;
                controller.enabled = false;
                transform.position = pos;
                controller.enabled = true;
                isClimbing = true;

                Debug.Log("Start Climbing");
            }
        }


        //移動計算
        float currentSpeed = isDashing ? dashSpeed : walkSpeed;
        Vector3 horizontalMove = new Vector3(moveX, 0, 0);

        //ジャンプ
        if (Input.GetButtonDown("Jump" + ID) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

            //if (animator != null)
                //animator.SetTrigger("Jump");   //ジャンプアニメ
        }


        //左右移動
        controller.Move(horizontalMove.normalized * currentSpeed * Time.deltaTime);

        //重力落下
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //向きの変更
        if (moveX != 0)
        {
            direction = moveX > 0 ? 1 : -1;
        }
    }



    /// <summary>
    /// ハシゴを上り下り
    /// </summary>
    /// <param name="moveY">スティックの上下傾き</param>
    private void LadderMove(float moveY)
    {
        //ハシゴから離れた
        if (isOnLadder == false)
        {
            isClimbing = false;

            //位置を調整
            Vector3 pos = transform.position;
            pos.z = 0;
            controller.enabled = false;
            transform.position = pos;
            controller.enabled = true;


            Debug.Log("Stop Climbing");
        }

        // 垂直移動
        velocity.y = moveY * ladderSpeed;
        controller.Move(velocity * Time.deltaTime);


        //ハシゴの一番下まで降りきったとき
        if (moveY < 0 && isLadderRoot && transform.position.y <= ladderRootObject.transform.position.y)
        {
            isClimbing = false;

            //位置を調整
            Vector3 pos = transform.position;
            pos.z = 0;
            pos.y = ladderRootObject.transform.position.y;
            controller.enabled = false;
            transform.position = pos;
            controller.enabled = true;


            Debug.Log("Stop Climbing at Ladder Root");
        }
    }



    /// <summary>
    /// ビーム発射
    /// </summary>
    void Beam()
    {
        //ビーム発射
        GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);

        //3秒後に消す
        Destroy(beam, 3.0f);
    }



    /// <summary>
    /// 何かに触れた
    /// </summary>
    /// <param name="other">触れた相手</param>

    private void OnTriggerEnter(Collider other)
    {
        //ハシゴ or ハシゴの根元
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            //触れてるハシゴ一覧に追加
            if (!activeLadders.Contains(other))
            {
                activeLadders.Add(other);
            }

            //触れたのが根本だった
            if (other.CompareTag("LadderRoot"))
            {
                //根元に触れてるフラグを立てる
                isLadderRoot = true;

                //そいつを覚えておく
                ladderRootObject = other.gameObject;
            }
        }


        //降ってきたハシゴアイテム
        else if (other.CompareTag("LadderItem"))
        {
            //プレイヤーの梯子の数が3以下なら回収
            if (ladderCount < maxladder)
            {
            //手持ちをカウントアップ
            ladderCount++;
            UpdateText();

                Debug.Log(ID + "梯子の数:" + ladderCount);

                //アイテムを消す
                Destroy(other.gameObject);
            }
        }


        //王冠
        else if (other.gameObject.name=="Crown")
        {
            //王冠をプレイヤーの子にする
            other.transform.parent = transform;
            other.transform.localPosition = new Vector3(0, 1.0f, 0);
            isKing = true;
            walkSpeed = 10f;

            //王様ターンへ移行
            GameObject.Find("GameDirector").GetComponent<GameDirector>().ChangeState();
            GameObject.Find("GameDirector").GetComponent<GameDirector>().king = this.gameObject;
        }


        //ビーム
        else if (other.CompareTag("Beam"))
        {
            //削除
            Destroy(gameObject);

            //ゲームディレクターに死んだこと伝える（残り人数を調べるため）
            GameObject.Find("GameDirector").GetComponent<GameDirector>().Death();

        }
    }






    /// <summary>
    /// ハシゴから離れた
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            activeLadders.Remove(other);

            if (other.CompareTag("LadderRoot"))
            {
                isLadderRoot = false;
            }
        }
    }

    /// <summary>
    /// ハシゴを失う
    /// </summary>
    /// <returns>梯子を持っていたか</returns>
    public bool LoseLadder()
    {
        stanTime = 1.0f; //1.5秒スタン


        //持ってるハシゴがある
        if (ladderCount > 0)
        {
            ladderCount--;
            UpdateText();
            return true;
        }

        //持ってるハシゴがない
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 攻撃が当たってハシゴを得る
    /// </summary>
    public void ObtainingLadder()
    {
        ladderCount++;

        if(ladderCount > maxladder)
        {
            ladderCount = (int)maxladder;

        }
        UpdateText();
    }
}