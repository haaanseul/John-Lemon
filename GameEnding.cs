using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬을 재시작하기 위한 네임스페이스

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f; // 페이드 아웃 시간
    public float displayImageDuration = 1f; // 이미지 페이드 인 후, 추가 시간
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup; // 게임을 마쳤을 때, Canvas Group
    public CanvasGroup caughtBackgroundImageCanvasGroup; // 잡혔을 때, Canvas Group
    public AudioSource exitAudio; // 탈출 오디오
    public AudioSource caughtAudio; // 잡혔을 때 오디오
    
    bool m_IsPlayerAtExit; // 페이드인,아웃 두 종류의 상태이기 때문에 bool 변수
    bool m_IsPlayerCaught; // 플레이어가 잡혔는지 확인하는 bool 변수
    float m_Timer; // 페이드 끝나기 전 게임이 종료되지 않기 위한 타이머
    bool m_HasAudioPlayed; // 오디오가 한 번만 플레이되도록 설정, 기본적으로 false 값

    void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                m_IsPlayerAtExit = true;
            }
        }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }

        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed) // 오디오가 플레이되지 않았을 경우
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime; // 마지막 프레임 이후 경과 시간

        imageCanvasGroup.alpha = m_Timer / fadeDuration; // alpha가 불투명도, 시간 경과에 따라 불투명해짐
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0); // 씬 재시작, 0은 씬의 인덱스로 몇 번째 씬인지 나타냄
            }
            else
            {
                GameManager.i.GameOver();
                Application.Quit (); // 종료
            }
        }
    }
}
