using System.Collections.Generic;
using UnityEngine;

public class QuestParser : MonoBehaviour
{
    public static QuestParser Instance;
    public Dictionary<int, List<Quest>> Quests;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        ReadQuest();

#if UNITY_EDITOR
        Debug.Log("퀘스트들을 성공적으로 불러왔습니다.");
#endif
    }

    private void ReadQuest()
    {
        ITableReader<Dictionary<int, List<Quest>>> reader = new QuestExcelDataReader();
        Quests = reader.ReadTable(QuestExcelDataReader.JsonPath);
    }
}
