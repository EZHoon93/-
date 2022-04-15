using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonManger<GameManager>
{
    //public Dictionary

    public UI_Popup_Quiz_Base currentQuiz;  //현재풀고있는 퀴즈

    public Dictionary<Enum, UI_Stage_Base[]> stageListDic = new Dictionary<Enum, UI_Stage_Base[]>();

    public int useHintCount;    //사용한 힌트 개수

    public void RegisterQuizArray(Enum @enum, UI_Stage_Base[] uiStageBaseArray)
    {
        if (stageListDic.ContainsKey(@enum)) return;
        stageListDic.Add(@enum, uiStageBaseArray);
    }

    public void CheckAnswer(string userInput, string answer, Enum @enum)
    {
        var popUPResult = UIManager.Instance.ShowPopupUI(Define.PopUp.ResultAnswer) as UI_PopUp_ResultAnswer;
        bool isSucess = false;
        int newScore = 0;
        if (string.Compare(userInput, answer) == 0)
        {
            isSucess = true;
            var nextQuizObjectList = stageListDic[@enum];
            var nextLevel = currentQuiz.currentQustionLevel + 1;
            
            //다음 스테이지가 없다면
            if (nextLevel >= nextQuizObjectList.Length)
            {
                popUPResult.nextEvent += () =>
                {
                    UIManager.Instance.ClosePopupUI();
                };
            }
            //다음 스테이지 존재한다면
            else
            {
                var nextQuizObject = nextQuizObjectList[nextLevel];
                nextQuizObject.SetActive(true);
                //다음 스테이지 클릭시, 다음 레벨 갖고옴
                popUPResult.nextEvent += () =>
                {
                    currentQuiz.SetupQuizData(nextQuizObject.data , nextLevel, @enum);
                };
            }

            //정답 맞출시 현재 레벨 데이터 저장
            SaveScore(@enum, nextLevel, newScore);
        }
        else
        {
            isSucess = false;

        }

        useHintCount = 0;
        popUPResult.SetupResult(isSucess,newScore);
    }


    protected void SaveScore(Enum @enum, int nextLevel, int newSocre)
    {
        PlayerInfo.SaveDataByEnum(@enum, nextLevel, newSocre);
    }

    public void GetScore()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerInfo.userData.hintKey += 10;
            PlayerInfo.Save();
        }
    }
}
