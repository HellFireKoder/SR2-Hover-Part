namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi;
    using ModApi.Common;
    using ModApi.Craft.Parts;
    using ModApi.Craft.Parts.Input;
    using ModApi.GameLoop;
    using ModApi.GameLoop.Interfaces;
    using UnityEngine;
    using static UnityEngine.ParticleSystem;

    /// <summary>
    /// Modifier script for particle hover.
    /// </summary>
    /// <seealso cref="ModApi.Craft.Parts.PartModifierScript{Assets.Scripts.Craft.Parts.Modifiers.ParticleHoverData}" />
    /// <seealso cref="ModApi.GameLoop.Interfaces.IFlightUpdate" />
    /// <seealso cref="ModApi.GameLoop.Interfaces.IFlightStart" />
    public class ParticleHoverScript : PartModifierScript<ParticleHoverData>, IDesignerUpdate, IFlightUpdate, IFlightStart
    {
        /// <summary>
        /// The emission module
        /// </summary>
        private EmissionModule _emissionModule;

        /// <summary>
        /// The input
        /// </summary>
        private IInputController _input;

        /// <summary>
        /// The main module
        /// </summary>
        private MainModule _mainModule;

        /// <summary>
        /// The particle system
        /// </summary>
        private ParticleSystem _particleSystem;

        /// <summary>
        /// The shape module
        /// </summary>
        private ShapeModule _shapeModule;

        /// <summary>
        /// Update method that is only called in the designer scene.
        /// </summary>
        /// <param name="frame">The designer frame data.</param>
        void IDesignerUpdate.DesignerUpdate(in DesignerFrameData frame)
        {
            if (this.Data.PropertiesOpen)
            {
                this._emissionModule.rateOverTime = this.Data.EmissionRate;
                this._mainModule.startSpeed = this.Data.EmissionVelocity;
                if (!this._particleSystem.isPlaying)
                {
                    this._particleSystem.Play();
                }
            }
            else if (this._particleSystem.isPlaying)
            {
                this._particleSystem.Stop();
            }
        }

        /// <summary>
        /// Called once in the flight scene before running any update methods on this object.
        /// </summary>
        /// <param name="frame">The flight frame data.</param>
        void IFlightStart.FlightStart(in FlightFrameData frame)
        {
            this._particleSystem.GetComponent<ParticleCollisionNotifier>().ParticleCollision += this.ParticleCollision;
            this._input = this.GetInputController();
        }

        /// <summary>
        /// Update method that is only called in the flight scene when the game is not paused.
        /// </summary>
        /// <param name="frame">The flight frame data.</param>
        void IFlightUpdate.FlightUpdate(in FlightFrameData frame)
        {
            if (!Game.Instance.FlightScene.TimeManager.CurrentMode.WarpMode)
            {
                this._emissionModule.rateOverTime = this.Data.EmissionRate * this._input.Value;
                this._mainModule.startSpeed = this.Data.EmissionVelocity * this._input.Value;
                if (!this._particleSystem.isPlaying)
                {
                    this._particleSystem.Play();
                }
            }
            else
            {
                this._emissionModule.rateOverTime = 0;
                this._mainModule.startSpeed = 0;
            }
        }

        /// <summary>
        /// Called when the part is about to have its picture taken for the part icons. Try and look your best and say cheese!
        /// </summary>
        public override void PrepareForPartIcon()
        {
            base.PrepareForPartIcon();
            this.OnInitialized();
            this._particleSystem.Simulate(1);
        }

        /// <summary>
        /// Sets the emission angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        public void SetEmissionAngle(int angle)
        {
            this._shapeModule.angle = angle;
        }

        /// <summary>
        /// Sets the emission rate.
        /// </summary>
        /// <param name="rate">The rate.</param>
        public void SetEmissionRate(int rate)
        {
            this._emissionModule.rateOverTime = rate;
        }

        public void SetEmissionVelocity(int velocity)
        {
            this._mainModule.startSpeed = velocity;
        }

        /// <summary>
        /// Called when the part modifier script is initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            this._particleSystem = this.GetComponentInChildren<ParticleSystem>();
            this._mainModule = this._particleSystem.main;
            this._emissionModule = this._particleSystem.emission;
            this._shapeModule = this._particleSystem.shape;

            this._mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

            this.SetEmissionAngle(this.Data.EmissionAngle);
        }

        /// <summary>
        /// Called when a particle collides with something.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void ParticleCollision(ParticleCollisionEvent collision)
        {
            float dot = Vector3.Dot(collision.normal, collision.velocity.normalized);
            var force = collision.velocity * (dot * 0.25f + (0.75f * Mathf.Sign(dot))) * Constants.MassScale * this.Data.ForceMultiplier;
            this.PartScript.BodyScript?.RigidBody?.AddForceAtPosition(force / Mathf.Min(1, Time.timeScale), this.transform.position);
        }
    }
}