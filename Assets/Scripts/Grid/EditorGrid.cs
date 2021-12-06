using System;
using UnityEngine;

// Singleton
public class EditorGrid : MonoBehaviour
{
    [NonSerialized] public static EditorGrid Instance;
    [SerializeField] public Vector2Int Size;
    [NonSerialized] public float Offset = 1f;

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
    }
}
