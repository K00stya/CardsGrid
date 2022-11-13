using System.Runtime.InteropServices;

public static class Yandex
{
    [DllImport("__Internal")]
    public static extern void SetActiveRateButton();
    
    [DllImport("__Internal")]
    public static extern void RateGame();
    
    [DllImport("__Internal")]
    public static extern void SaveOnYandex(string data);
    
    [DllImport("__Internal")]
    public static extern void LoadFromYandex();
    
    [DllImport("__Internal")]
    public static extern void SetToLeaderboard(string name, int value);

    [DllImport("__Internal")]
    public static extern void GetLeaderBoard(string name);
    
    [DllImport("__Internal")]
    public static extern string GetLang();
    
    [DllImport("__Internal")]
    public static extern string ShowFullScreenAd();

    [DllImport("__Internal")]
    public static extern void ShowRewardAd();
}
