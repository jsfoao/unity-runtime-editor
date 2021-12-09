using UnityEngine;

public class InputEntity : MonoBehaviour
{
    public static InputEntity Instance;
    public CommandHandler CommandHandler;

    private void Update()
    {
    }

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
        
        CommandHandler = new CommandHandler();
    }
}
