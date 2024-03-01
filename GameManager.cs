using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager i; // 전역 변수
    Text timeText;
    Text bestText;

    float t = 0;

    bool isGameOver = false; // 게임이 끝났는지 판단할 변수

    List<float> timeList = new List<float>(); // 각 시도의 시간을 저장할 리스트

    string filePath = "Records.txt"; // 데이터를 저장할 파일 경로

    void Awake()
    {
        i = this; // 전역 변수에 해당 컴포넌트 참조
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        bestText = GameObject.Find("BestText").GetComponent<Text>();
    }

    void Start()
    {
        LoadTimeList(); // 이전에 저장된 시도 시간을 불러옴

        if (PlayerPrefs.HasKey("Best")) // Best 라는 키가 저장되어 있다면
        {
            int b = PlayerPrefs.GetInt("Best"); // b에 저장된 값 저장

            bestText.text = "Best\n" + SetTime(b); // 텍스트 표현
        }
    }

    void Update()
    {
        if (isGameOver) return;

        t += Time.deltaTime;
        timeText.text = "Time\n" + SetTime((int)t);
    }

    string SetTime(int t)
    {
        string min = (t / 60).ToString();

        if (int.Parse(min) < 10) min = "0" + min;

        string sec = (t % 60).ToString();

        if (int.Parse(sec) < 10) sec = "0" + sec;

        return min + ":" + sec;
    }

    public void GameOver()
    {
        if(isGameOver) return;
        
        isGameOver = true;
        timeList.Add(t); // 현재 시도의 시간을 리스트에 추가
        SaveTimeList(); // 시도 시간 리스트를 저장
        SetBestTime();
    }

    void SetBestTime()
    {
        Debug.Log("SetBest 함수에 진입");
        if (PlayerPrefs.HasKey("Best")) // Best 라는 키가 저장되어 있다면
        {
            int b = PlayerPrefs.GetInt("Best"); // b에 저장된 값 저장

            if ((int)t < b) // 저장된 시간보다 현재 플레이 시간이 더 낮다면
            {
                PlayerPrefs.SetInt("Best", (int)t);
                b = (int)t;
            }

            bestText.text = "Best\n" + SetTime(b); // 텍스트 표현
        }
        else
        {
            PlayerPrefs.SetInt("Best", (int)t); // Best라는 키로 현재 플레이 시간 저장
            bestText.text = "Best\n" + SetTime((int)t); // 텍스트 표현
        }
    }

    void SaveTimeList()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (float time in timeList)
            {
                writer.WriteLine(time.ToString());
            }
        }
    }

    void LoadTimeList()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    float time = float.Parse(line);
                    timeList.Add(time);
                }
            }
        }
    }
}
