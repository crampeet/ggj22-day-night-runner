using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player performs a Slide.
    /// </summary>
    /// <typeparam name="PlayerSlid"></typeparam>
    public class PlayerSlid : Simulation.Event<PlayerSlid>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.audioSource && player.slideAudio)
                player.audioSource.PlayOneShot(player.slideAudio);
        }
    }
}