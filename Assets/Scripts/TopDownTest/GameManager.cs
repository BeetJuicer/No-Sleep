using UnityEngine;

namespace TopDown
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState
        {
            Gameplay,
            Sequence
        }

        public static GameManager Instance { get; private set; }
        public GameState CurrentGameState { get; private set; } = GameState.Gameplay;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void StartSequence()
        {
            CurrentGameState = GameState.Sequence; 
        }

        public void StopSequence()
        {
            CurrentGameState = GameState.Gameplay;
        }
    }
}
