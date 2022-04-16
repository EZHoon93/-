
using UnityEngine;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
public static class GPGSManager 
{

    public static void Login()
    {
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                LoadCloud();
            }
            else
            {
                
            }
        });

    }

    #region 클라우드 저장

    /// <summary>
    /// 긴
    /// </summary>
    /// <returns></returns>

    public static void LoadCloud()
    {
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution("mysave", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
    }

    static void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, LoadData);

        }
    }

    static void LoadData(SavedGameRequestStatus status, byte[] LoadedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string data = System.Text.Encoding.UTF8.GetString(LoadedData);
            var userData = JsonUtility.FromJson<UserData>(data);

            if (userData == null)
            {
                //loginManager.SetActiveCreateNickName(true);
            }
            else
            {
                //loginManager.SetActiveCreateNickName(false);
            }

        }
        else
        {


        }

    }



    public static void SaveCloud()
    {
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, SaveGame);
    }

    static void SaveGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var update = new SavedGameMetadataUpdate.Builder().Build();
            var userData = PlayerInfo.userData;
            string jsonData = JsonUtility.ToJson(userData, true);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);


            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, bytes, SaveData);
        }
    }

    static void SaveData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            
        }
        else
        {

        }
    }



    public static void DeleteCloud()
    {
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, DeleteGame);
    }

    static void DeleteGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform.Instance.SavedGame.Delete(game);
        }
        else
        {

        }
    }

    #endregion
}
