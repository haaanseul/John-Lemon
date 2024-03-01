using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //NavMeshAgent 클래스에 액세스

public class WaypointPatrol : MonoBehaviour
{
    public Transform target; // 캐릭터의 위치를 가리키는 변수
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints; // 순회 웨이포인트 표시하는 오브젝트 배열
    public float sightRange = 1f; // 몬스터의 시야 범위

    int m_CurrentWaypointIndex; // 웨이 포인트 배열의 현재 인덱스

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트를 가져옴

        navMeshAgent.SetDestination(waypoints[0].position); // 최초 목적지 설정
    }

    void Update()
    {
        Vector3 toTarget = target.position - transform.position; // 몬스터에서 캐릭터로 향하는 벡터 계산
        float distance = toTarget.magnitude; // 몬스터와 캐릭터 사이의 거리 계산
        toTarget.Normalize(); // 정규화하여 방향 벡터로 변환

        // 몬스터의 시야 범위 내에 있고, 캐릭터가 몬스터의 정면에 있는 경우
        if (distance <= sightRange && Vector3.Dot(transform.forward, toTarget) > 0.7f)
        {
            navMeshAgent.SetDestination(target.position); // 캐릭터를 향해 이동
        }
        else{
            WalkAround();
        }
    }

    void WalkAround()
    {
        // 목적지에 도착했는지 확인 : 목적지까지 남은 거리 < 인스펙터 창에서 설정한 정지 거리
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            // 목적지 개수 다 채우면 0으로 설정
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            
            // 현재 도달한 웨이포인트를 인덱스로 사용
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }

}