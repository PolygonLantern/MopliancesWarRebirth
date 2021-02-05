using TMPro;
using UnityEngine;

/// <summary>
/// This script is to hold a reference to the UI elements so other classes can reach and update the values of those UI elements
/// </summary>
public class UIManager : MonoBehaviour
{
   public static UIManager uiManager;
   
   public RectTransform mainPanel;
   public RectTransform playersPanel;
   
   public TMP_Text levelText;

   /// <summary>
   /// Singleton check if a gameObject with this script already exists.
   /// </summary>
   private void Awake()
   {
      if (uiManager == null)
      {
         uiManager = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }


   
}
