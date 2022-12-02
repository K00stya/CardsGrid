using System.Runtime.InteropServices;

public static class Yandex
{
    [DllImport("__Internal")]
    public static extern void SetActiveRateButton();
}
