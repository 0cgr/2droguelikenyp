using UnityEngine;
using UnityEngine.UIElements;

namespace TutorialVersion
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public BoardManager BoardManager;
        public PlayerController PlayerController;
        public UIDocument UIDoc;

        public TurnManager TurnManager { get; private set;}
        
        private int m_FoodAmount = 20;
        private Label m_FoodLabel;
        private int m_CurrentLevel = 1;
        
        private VisualElement m_GameOverPanel;
        private Label m_GameOverMessage;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
      
            Instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Turn sistemi devreye girer ve ui � ona g�re ayarlar
            TurnManager = new TurnManager();
            TurnManager.OnTick += OnTurnHappen;
  
            m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
  
            m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
            m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");

            StartNewGame();
        }

        public void StartNewGame()
        {
            //seviye ve yemek
            m_GameOverPanel.style.visibility = Visibility.Hidden;
  
            m_CurrentLevel = 1;
            m_FoodAmount = 20;
            m_FoodLabel.text = "Food : " + m_FoodAmount;
  
            BoardManager.Clean();
            BoardManager.Init();
  
            PlayerController.Init();
            PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
        }

        
        void OnTurnHappen()
        {
            // Her turda yemek miktar�n� azalt
            ChangeFood(-1);
        }
        
        public void ChangeFood(int amount)
        {
            m_FoodAmount += amount;
            m_FoodLabel.text = "Food : " + m_FoodAmount;

            // E�er yemek miktar� s�f�r veya daha azsa, oyunu bitir
            if (m_FoodAmount <= 0)
            {
                PlayerController.GameOver();
                m_GameOverPanel.style.visibility = Visibility.Visible;
                m_GameOverMessage.text = "Game Over!\n\nSurvived " + m_CurrentLevel + " days";
            }
        }
        // Yeni bir seviyeye ge�i�
        public void NewLevel()
        {
            BoardManager.Clean();
            BoardManager.Init();
            PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

            m_CurrentLevel++;
        }
    }
}