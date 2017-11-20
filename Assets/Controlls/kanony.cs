


using UnityEngine;

// ... CODE ... //

namespace Assets.Gamelogic.Pirates.Cannons
{
    public class kanony : MonoBehaviour
    {
     
       

        

        private void OnEnable()
        {
            ShipControlsReader.FireLeftTriggered.Add(OnFireLeft);
            ShipControlsReader.FireRightTriggered.Add(OnFireRight);
        }

        private void OnDisable()
        {
            ShipControlsReader.FireLeftTriggered.Remove(OnFireLeft);
            ShipControlsReader.FireRightTriggered.Remove(OnFireRight);
        }

        private void OnFireLeft(FireLeft fireLeft)
        {
            // Process FireLeft event
            AttemptToFireCannons(-transform.right);
        }

        private void OnFireRight(FireRight fireRight)
        {
            // Process FireRight event
            AttemptToFireCannons(transform.right);
        }

        private void AttemptToFireCannons(Vector3 direction)
        {
            // Prevent firing for ships which are dead
            if (HealthReader.Data.currentHealth <= 0)
            {
                return;
            }

            if (cannon != null)
            {
                cannon.Fire(direction);
            }
        }
    }
}
