using UnityEngine;

/// <summary>
/// ゲームアワード用：シンプルなプレイヤー制御
/// ・X,Y移動のみ
/// ・Xboxコントローラー対応
/// ・アニメーション切り替え対応
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerControllerSimple : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3.0f;
    public float dashSpeed = 6.0f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 move;
    private bool isDashing;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); // モデルが子にある想定
    }

    void Update()
    {
        Move();
        UpdateAnimation();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        isDashing = Input.GetButton("Dash");

        float speed = isDashing ? dashSpeed : walkSpeed;

        // X,Y軸のみ移動（Zは固定）
        move = new Vector3(x, y, 0f);
        controller.Move(move * speed * Time.deltaTime);

        // 向き変更（入力があるときのみ）
        if (move.sqrMagnitude > 0.01f)
        {
            Vector3 lookDir = move.normalized;
            transform.forward = lookDir;
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        float speedValue = move.magnitude;

        animator.SetFloat("Speed", speedValue);
        animator.SetBool("IsDash", isDashing && speedValue > 0.1f);
    }
}
