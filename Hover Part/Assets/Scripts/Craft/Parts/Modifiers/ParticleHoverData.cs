namespace Assets.Scripts.Craft.Parts.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Assets.Scripts.Design;
    using ModApi.Craft.Parts;
    using ModApi.Craft.Parts.Attributes;
    using ModApi.Design.PartProperties;
    using UnityEngine;

    /// <summary>
    /// Modifier data for particle hover script.
    /// </summary>
    /// <seealso cref="ModApi.Craft.Parts.PartModifierData{Assets.Scripts.Craft.Parts.Modifiers.ParticleHoverScript}" />
    [Serializable]
    [DesignerPartModifier("ParticleHover")]
    [PartModifierTypeId("HoverPart_ParticleHover")]
    public class ParticleHoverData : PartModifierData<ParticleHoverScript>
    {
        /// <summary>
        /// The emission angle
        /// </summary>
        [SerializeField]
        [DesignerPropertySlider(MinValue = 0, MaxValue = 45, NumberOfSteps = 10)]
        private int emissionAngle = 15;

        /// <summary>
        /// The emission rate
        /// </summary>
        [SerializeField]
        [DesignerPropertySlider(MinValue = 100, MaxValue = 5000, NumberOfSteps = 50, Label = "Emission Rate")]
        private int emissionRate = 1000;

        /// <summary>
        /// The emission velocity
        /// </summary>
        [SerializeField]
        [DesignerPropertySlider(MinValue = 10, MaxValue = 250, NumberOfSteps = 25, Label = "Emission Velocity")]
        private int emissionVelocity = 50;

        /// <summary>
        /// The magic force multiplier
        /// </summary>
        [SerializeField]
        [DesignerPropertySlider(MinValue = 0, MaxValue = 500, NumberOfSteps = 51, Label = "Magic Force")]
        private int magicForceMultiplier = 200;

        /// <summary>
        /// Gets the emission angle.
        /// </summary>
        /// <value>
        /// The emission angle.
        /// </value>
        public int EmissionAngle
        {
            get
            {
                return this.emissionAngle;
            }
        }

        /// <summary>
        /// Gets the emission rate.
        /// </summary>
        /// <value>
        /// The emission rate.
        /// </value>
        public int EmissionRate
        {
            get
            {
                return this.emissionRate;
            }
        }

        /// <summary>
        /// Gets the emission velocity.
        /// </summary>
        /// <value>
        /// The emission velocity.
        /// </value>
        public int EmissionVelocity
        {
            get
            {
                return this.emissionVelocity;
            }
        }

        /// <summary>
        /// Gets the force multiplier.
        /// </summary>
        /// <value>
        /// The force multiplier.
        /// </value>
        public int ForceMultiplier
        {
            get
            {
                return this.magicForceMultiplier;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the properties panel is open for this modifier.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [properties open]; otherwise, <c>false</c>.
        /// </value>
        public bool PropertiesOpen { get; set; } = false;

        /// <summary>
        /// Called when the part modifier data is initialized in the designer scene.
        /// </summary>
        /// <param name="d">The designer part properties.</param>
        protected override void OnDesignerInitialization(IDesignerPartPropertiesModifierInterface d)
        {
            base.OnDesignerInitialization(d);
            d.OnActivated(() => Symmetry.ExecuteOnSymmetricPartModifiers(this, true, x => x.PropertiesOpen = true));
            d.OnDeactivated(() => Symmetry.ExecuteOnSymmetricPartModifiers(this, true, x => x.PropertiesOpen = false));
            d.OnPropertyChanged(() => this.emissionAngle, (x, y) => { Symmetry.SynchronizePartModifiers(this.Part.PartScript); Symmetry.ExecuteOnSymmetricPartModifiers(this, true, (z) => z.Script?.SetEmissionAngle(x)); });
            d.OnPropertyChanged(() => this.emissionRate, (x, y) => { Symmetry.SynchronizePartModifiers(this.Part.PartScript); Symmetry.ExecuteOnSymmetricPartModifiers(this, true, (z) => z.Script?.SetEmissionRate(x)); });
            d.OnPropertyChanged(() => this.emissionVelocity, (x, y) => { Symmetry.SynchronizePartModifiers(this.Part.PartScript); Symmetry.ExecuteOnSymmetricPartModifiers(this, true, (z) => z.Script?.SetEmissionVelocity(x)); });
        }
    }
}