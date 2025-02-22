using System;

[Serializable]
public class TutorialData
{
    public bool TutorialFinished;

    public void SaveTutorialData()
    {
        SaveManager.SaveTutorialStatus(TutorialFinished);
    }

    public void LoadTutorialData()
    {
        TutorialData loadData = SaveManager.LoadTutorialStatus();

        TutorialFinished = loadData.TutorialFinished;
    }
}
