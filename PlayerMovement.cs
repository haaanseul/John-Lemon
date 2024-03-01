using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f; // 회전 속도
    public GameObject PlayerName; // 플레이어 이름
    public float PlayerNameOffset; // 플레이어 이름 오프셋

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement; // 3차원 공간 벡터 : 게임 오브젝트
    Quaternion m_Rotation = Quaternion.identity; // 회전 저장
   

   
    void Start()
    {
        m_Animator = GetComponent<Animator> (); // Animator 유형의 컴포넌트에 대한 레퍼런스를 구하여 m_Animator라는 변수에 할당
        m_Rigidbody = GetComponent<Rigidbody> ();

        m_AudioSource = GetComponent<AudioSource>();
    }

    // 물리에 맞추어 적시에 호출되는 메소드 함수    <충돌이나 상호 작용>
    void FixedUpdate()
    {
        m_Rigidbody.velocity =Vector3.zero; // 밀림 현상 방지

        // 오브젝트 이동
        float horizontal = Input.GetAxis ("Horizontal"); // A, D 키
        float vertical = Input.GetAxis ("Vertical"); // W, S 키

        m_Movement.Set (horizontal, 0f, vertical); // 오브젝트 위치 변경
        m_Movement.Normalize (); // 정규화

        // 플레이어 이름 위치 = 캐릭터 위치 + 캐릭터 이름 위치를 2D로 변환한 카메라 위치
        PlayerName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, PlayerNameOffset, 0));

        // 애니메이션을 위한 입력 감지
        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f); // 수평 입력 여부 파악
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);     // 수직 입력 여부 파악

        bool isWalking = hasHorizontalInput || hasVerticalInput;         // 입력 여부
        m_Animator.SetBool ("IsWalking", isWalking);                     // isWalking 상태를 입력이 있다면 전환
        
        if(isWalking) // is Walking 상태면 오디오 출력
        {
            if(!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play ();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        /* 이동 방향 (현재 회전 값, 목표 회전 값,
        각도의 변화(단위 라디안, Time.deltaTime은 프레임 간 시간인데 프레임은 변할 수 있기 때문에
        turnSpeed를 곱하여 초당 시간 변화로 만듦), 크기의 변화)
        */
        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward); // 해당 파라미터 방향을 바라보는 회전 생성
    
    }

    void OnAnimatorMove() {
        // 캐릭터의 새 위치 : Rigidbody 현재 위치 + 이동 벡터 * deltaPositon(루트 모션으로 인한 프레임당 위치 이동량)
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        // 캐릭터의 새 회전
        m_Rigidbody.MoveRotation (m_Rotation);
    }

}
