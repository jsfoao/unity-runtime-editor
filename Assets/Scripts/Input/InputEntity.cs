using UnityEngine;

public class InputEntity : MonoBehaviour
{
    public static InputEntity Instance;
    public CommandHandler CommandHandler;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CommandHandler.Undo();
        }
        // Debug.Log(CommandHandler.CommandStack.Count);
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
