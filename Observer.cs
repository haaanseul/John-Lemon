using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;
    bool m_IsPlayerInRange;
    

    void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update()
    {
        if(m_IsPlayerInRange) // 플레이어 위치에 따라 가시선 확인
        {
            // 게임 오브젝트에서 JohnLemon까지의 방향(벡터 B-A)
            Vector3 direction = player.position - transform.position + Vector3.up;

            // ray 생성
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit; // 부딪히는 대상에 관한 정보

            if(Physics.Raycast(ray, out raycastHit)) // Raycast는 무언가에 부딪히면 참, 아니면 거짓을 반환
            {
                if(raycastHit.collider.transform == player) // 플레이어랑 부딪혔다면
                {
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }

}
