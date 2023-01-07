using UnityEngine;

public static class Booter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void ExecuteAfter()
    {
        //Object.Instantiate(Resources.Load("AudioMaster"));
        Object.Instantiate(Resources.Load("PersistantCore"));
    }

}
