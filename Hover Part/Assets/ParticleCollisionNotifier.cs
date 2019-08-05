namespace Assets.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Script to put on a ParticleSystem, which allows other scripts to subscribe to be notified when particles collide with stuff.
    /// </summary>
    public class ParticleCollisionNotifier : MonoBehaviour
    {
        /// <summary>
        /// The collision events.
        /// </summary>
        private List<ParticleCollisionEvent> collisionEvents;

        /// <summary>
        /// The particle system.
        /// </summary>
        private ParticleSystem myParticleSystem;

        /// <summary>
        /// Delegate for particle collisions.
        /// </summary>
        /// <param name="collision">The particle collision event.</param>
        public delegate void ParticleCollisionDelegate(ParticleCollisionEvent collision);

        /// <summary>
        /// Gets or sets the particle collision delegate.
        /// </summary>
        /// <value>
        /// The particle collision delegate.
        /// </value>
        public ParticleCollisionDelegate ParticleCollision { get; set;  }

        /// <summary>
        /// Unity Awake method.
        /// </summary>
        private void Awake()
        {
            this.myParticleSystem = this.GetComponent<ParticleSystem>();
            this.collisionEvents = new List<ParticleCollisionEvent>();
        }

        /// <summary>
        /// Called when particle collision.
        /// </summary>
        /// <param name="other">The other.</param>
        private void OnParticleCollision(GameObject other)
        {
            if (this.ParticleCollision == null)
            {
                return;
            }

            int numCollisionEvents = this.myParticleSystem.GetCollisionEvents(other, this.collisionEvents);
            int i = 0;
            while (i < numCollisionEvents)
            {
                // Notify subscribers of all particle collisions.
                this.ParticleCollision.Invoke(this.collisionEvents[i]);
                i++;
            }
        }
    }
}