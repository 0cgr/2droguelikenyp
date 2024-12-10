using UnityEngine;


namespace TutorialVersion
{
    public class FoodObject : CellObject
    {
        public int AmountGranted = 5;
        
        public override void PlayerEntered()
        {
            Destroy(gameObject);

            
            GameManager.Instance.ChangeFood(AmountGranted);
        }
    }
}