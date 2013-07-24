using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Chummer
{
    public class Improvement
    {
        public enum ImprovementType
        {
            Skill = 0,
            Attribute = 1,
            Text = 2,
			ArmorValue = 3,
			Reach = 5,
			Nuyen = 6,
			Essence = 7,
			Reaction = 8,
			PhysicalCM = 9,
			StunCM = 10,
			UnarmedDV = 11,
			SkillGroup = 12,
			SkillCategory = 13,
			SkillAttribute = 14,
			InitiativeDice = 15,
			MatrixInitiative = 16,
			MatrixInitiativePass = 17,
			LifestyleCost = 18,
			CMThreshold = 19,
			EnhancedArticulation = 20,
			WeaponCategoryDV = 21,
			CyberwareEssCost = 22,
			SpecialTab = 23,
			Initiative = 24,
			Uneducated = 25,
			LivingPersonaResponse = 26,
			LivingPersonaSignal = 27,
			LivingPersonaFirewall = 28,
			LivingPersonaSystem = 29,
			LivingPersonaBiofeedback = 30,
			Smartlink = 31,
			BiowareEssCost = 32,
			GenetechCostMultiplier = 33,
			BasicBiowareEssCost = 34,
			TransgenicsBiowareCost = 35,
			SoftWeave = 36,
			SensitiveSystem = 37,
			ConditionMonitor = 38,
			UnarmedDVPhysical = 39,
			MovementPercent = 40,
			Adapsin = 41,
			FreePositiveQualities = 42,
			FreeNegativeQualities = 43,
			NuyenMaxBP = 44,
			CMOverflow = 45,
			FreeSpiritPowerPoints = 46,
			AdeptPowerPoints = 47,
			ArmorEncumbrancePenalty = 48,
			Uncouth = 49,
			Initiation = 50,
			Submersion = 51,
			Infirm = 52,
			Skillwire = 53,
			DamageResistance = 54,
			RestrictedItemCount = 55,
			AdeptLinguistics = 56,
			SwimPercent = 57,
			FlyPercent = 58,
			FlySpeed = 59,
			JudgeIntentions = 60,
			LiftAndCarry = 61,
			Memory = 62,
			Concealability = 63,
			SwapSkillAttribute = 64,
			DrainResistance = 65,
			FadingResistance = 66,
			MatrixInitiativePassAdd = 67,
			Composure = 69,
			UnarmedAP = 70,
			CMThresholdOffset = 71,
			Restricted = 72,
			Notoriety = 73,
			SpellCategory = 74,
			ThrowRange = 75,
			SkillsoftAccess = 76,
			AddSprite = 77,
			BlackMarketDiscount = 78,
			SelectWeapon = 79,
			ComplexFormLimit = 80,
			SpellLimit = 81,
			QuickeningMetamagic = 82,
			BasicLifestyleCost = 83,
			ThrowSTR = 84,
			IgnoreCMPenaltyStun = 85,
			IgnoreCMPenaltyPhysical = 86,
            EssenceMax = 87,
            Accuracy = 88,
            Surprise = 89,
            MentalLimit = 90,
            PhysicalLimit = 91,
            SocialLimit = 92,
            WeaponSkillDV = 93,
            WeaponSkillAccuracy = 94,
            KnowsoftAccess = 95,
        }

        public enum ImprovementSource
        {
            Quality = 0,
            Power = 1,
			Metatype = 2,
			Cyberware = 3,
			Metavariant = 4,
			Bioware = 5,
			Nanotech = 6,
			Genetech = 7,
			ArmorEncumbrance = 8,
			Gear = 9,
			Spell = 10,
			MartialArtAdvantage = 11,
			Initiation = 12,
			Submersion = 13,
			Metamagic = 14,
			Echo = 15,
			Armor = 16, 
			ArmorMod = 17,
			EssenceLoss = 18,
			ConditionMonitor = 19,
			CritterPower = 20,
			ComplexForm = 21,
			EdgeUse = 22,
			MutantCritter = 23,
			Cyberzombie = 24,
			StackedFocus = 25,
			AttributeLoss = 26,
            Priority = 27,
			Custom = 999,
        }

		private string _strImprovedName = "";
        private string _strSourceName = "";
		private int _intMin = 0;
		private int _intMax = 0;
        private int _intAug = 0;
        private int _intAugMax = 0;
        private int _intVal = 0;
        private int _intRating = 1;
		private string _strExclude = "";
		private string _strUniqueName = "";
        private ImprovementType _objImprovementType;
        private ImprovementSource _objImprovementSource;
		private bool _blnCustom = false;
		private string _strCustomName = "";
		private string _strCustomId = "";
		private string _strCustomGroup = "";
		private string _strNotes = "";
		private bool _blnAddToRating = false;
		private bool _blnEnabled = true;
		private int _intOrder = 0;

		#region Helper Methods
		/// <summary>
		/// Convert a string to an ImprovementType.
		/// </summary>
		/// <param name="strValue">String value to convert.</param>
		private ImprovementType ConvertToImprovementType(string strValue)
		{
			switch (strValue)
			{
				case "Attribute":
					return ImprovementType.Attribute;
				case "Text":
					return ImprovementType.Text;
				case "ArmorValue":
					return ImprovementType.ArmorValue;
				case "Reach":
					return ImprovementType.Reach;
				case "Nuyen":
					return ImprovementType.Nuyen;
				case "Essence":
					return ImprovementType.Essence;
				case "Reaction":
					return ImprovementType.Reaction;
				case "PhysicalCM":
					return ImprovementType.PhysicalCM;
				case "StunCM":
					return ImprovementType.StunCM;
				case "UnarmedDV":
					return ImprovementType.UnarmedDV;
				case "SkillGroup":
					return ImprovementType.SkillGroup;
				case "SkillCategory":
					return ImprovementType.SkillCategory;
				case "SkillAttribute":
					return ImprovementType.SkillAttribute;
				case "InitiativeDice":
					return ImprovementType.InitiativeDice;
				case "MatrixInitiative":
					return ImprovementType.MatrixInitiative;
				case "MatrixInitiativePass":
					return ImprovementType.MatrixInitiativePass;
				case "LifestyleCost":
					return ImprovementType.LifestyleCost;
				case "CMThreshold":
					return ImprovementType.CMThreshold;
				case "EnhancedArticulation":
					return ImprovementType.EnhancedArticulation;
				case "WeaponCategoryDV":
					return ImprovementType.WeaponCategoryDV;
				case "CyberwareEssCost":
					return ImprovementType.CyberwareEssCost;
				case "SpecialTab":
					return ImprovementType.SpecialTab;
				case "Initiative":
					return ImprovementType.Initiative;
				case "Uneducated":
					return ImprovementType.Uneducated;
				case "LivingPersonaResponse":
					return ImprovementType.LivingPersonaResponse;
				case "LivingPersonaSignal":
					return ImprovementType.LivingPersonaSignal;
				case "LivingPersonaFirewall":
					return ImprovementType.LivingPersonaFirewall;
				case "LivingPersonaSystem":
					return ImprovementType.LivingPersonaSystem;
				case "LivingPersonaBiofeedback":
					return ImprovementType.LivingPersonaBiofeedback;
				case "Smartlink":
					return ImprovementType.Smartlink;
				case "BiowareEssCost":
					return ImprovementType.BiowareEssCost;
				case "GenetechCostMultiplier":
					return ImprovementType.GenetechCostMultiplier;
				case "BasicBiowareEssCost":
					return ImprovementType.BasicBiowareEssCost;
				case "TransgenicsBiowareCost":
					return ImprovementType.TransgenicsBiowareCost;
				case "SoftWeave":
					return ImprovementType.SoftWeave;
				case "SensitiveSystem":
					return ImprovementType.SensitiveSystem;
				case "ConditionMonitor":
					return ImprovementType.ConditionMonitor;
				case "UnarmedDVPhysical":
					return ImprovementType.UnarmedDVPhysical;
				case "MovementPercent":
					return ImprovementType.MovementPercent;
				case "Adapsin":
					return ImprovementType.Adapsin;
				case "FreePositiveQualities":
					return ImprovementType.FreePositiveQualities;
				case "FreeNegativeQualities":
					return ImprovementType.FreeNegativeQualities;
				case "NuyenMaxBP":
					return ImprovementType.NuyenMaxBP;
				case "CMOverflow":
					return ImprovementType.CMOverflow;
				case "FreeSpiritPowerPoints":
					return ImprovementType.FreeSpiritPowerPoints;
				case "AdeptPowerPoints":
					return ImprovementType.AdeptPowerPoints;
				case "ArmorEncumbrancePenalty":
					return ImprovementType.ArmorEncumbrancePenalty;
				case "Uncouth":
					return ImprovementType.Uncouth;
				case "Initiation":
					return ImprovementType.Initiation;
				case "Submersion":
					return ImprovementType.Submersion;
				case "Infirm":
					return ImprovementType.Infirm;
				case "Skillwire":
					return ImprovementType.Skillwire;
				case "DamageResistance":
					return ImprovementType.DamageResistance;
				case "RestrictedItemCount":
					return ImprovementType.RestrictedItemCount;
				case "AdeptLinguistics":
					return ImprovementType.AdeptLinguistics;
				case "SwimPercent":
					return ImprovementType.SwimPercent;
				case "FlyPercent":
					return ImprovementType.FlyPercent;
				case "FlySpeed":
					return ImprovementType.FlySpeed;
				case "JudgeIntentions":
					return ImprovementType.JudgeIntentions;
				case "LiftAndCarry":
					return ImprovementType.LiftAndCarry;
				case "Memory":
					return ImprovementType.Memory;
				case "SwapSkillAttribute":
					return ImprovementType.SwapSkillAttribute;
				case "DrainResistance":
					return ImprovementType.DrainResistance;
				case "FadingResistance":
					return ImprovementType.FadingResistance;
				case "MatrixInitiativePassAdd":
					return ImprovementType.MatrixInitiativePassAdd;
				case "Composure":
					return ImprovementType.Composure;
				case "UnarmedAP":
					return ImprovementType.UnarmedAP;
				case "CMThresholdOffset":
					return ImprovementType.CMThresholdOffset;
				case "Restricted":
					return ImprovementType.Restricted;
				case "Notoriety":
					return ImprovementType.Notoriety;
				case "SpellCategory":
					return ImprovementType.SpellCategory;
				case "ThrowRange":
					return ImprovementType.ThrowRange;
				case "SkillsoftAccess":
					return ImprovementType.SkillsoftAccess;
				case "AddSprite":
					return ImprovementType.AddSprite;
				case "BlackMarketDiscount":
					return ImprovementType.BlackMarketDiscount;
				case "SelectWeapon":
					return ImprovementType.SelectWeapon;
				case "ComplexFormLimit":
					return ImprovementType.ComplexFormLimit;
				case "SpellLimit":
					return ImprovementType.SpellLimit;
				case "QuickeningMetamagic":
					return ImprovementType.QuickeningMetamagic;
				case "BasicLifestyleCost":
					return ImprovementType.BasicLifestyleCost;
				case "ThrowSTR":
					return ImprovementType.ThrowSTR;
				case "IgnoreCMPenaltyStun":
					return ImprovementType.IgnoreCMPenaltyStun;
				case "IgnoreCMPenaltyPhysical":
					return ImprovementType.IgnoreCMPenaltyPhysical;
                case "EssenceMax":
                    return ImprovementType.EssenceMax;
                case "Accuracy":
                    return ImprovementType.Accuracy;
                case "Surprise":
                    return ImprovementType.Surprise;
                case "MentalLimit":
                    return ImprovementType.MentalLimit;
                case "PhysicalLimit":
                    return ImprovementType.PhysicalLimit;
                case "SocialLimit":
                    return ImprovementType.SocialLimit;
                case "WeaponSkillDV":
                    return ImprovementType.WeaponSkillDV;
                case "WeaponSkillAccuracy":
                    return ImprovementType.WeaponSkillAccuracy;
                case "KnowsoftAccess":
                    return ImprovementType.KnowsoftAccess;
				default:
					return ImprovementType.Skill;
			}
		}

		/// <summary>
		/// Convert a string to an ImprovementSource.
		/// </summary>
		/// <param name="strValue">String value to convert.</param>
		public ImprovementSource ConvertToImprovementSource(string strValue)
		{
			switch (strValue)
			{
				case "Power":
					return ImprovementSource.Power;
				case "Metatype":
					return ImprovementSource.Metatype;
				case "Cyberware":
					return ImprovementSource.Cyberware;
				case "Metavariant":
					return ImprovementSource.Metavariant;
				case "Bioware":
					return ImprovementSource.Bioware;
				case "Nanotech":
					return ImprovementSource.Nanotech;
				case "Genetech":
					return ImprovementSource.Genetech;
				case "ArmorEncumbrance":
					return ImprovementSource.ArmorEncumbrance;
				case "Gear":
					return ImprovementSource.Gear;
				case "Spell":
					return ImprovementSource.Spell;
				case "MartialArtAdvantage":
					return ImprovementSource.MartialArtAdvantage;
				case "Initiation":
					return ImprovementSource.Initiation;
				case "Submersion":
					return ImprovementSource.Submersion;
				case "Metamagic":
					return ImprovementSource.Metamagic;
				case "Echo":
					return ImprovementSource.Echo;
				case "Armor":
					return ImprovementSource.Armor;
				case "ArmorMod":
					return ImprovementSource.ArmorMod;
				case "EssenceLoss":
					return ImprovementSource.EssenceLoss;
				case "ConditionMonitor":
					return ImprovementSource.ConditionMonitor;
				case "CritterPower":
					return ImprovementSource.CritterPower;
				case "ComplexForm":
					return ImprovementSource.ComplexForm;
				case "EdgeUse":
					return ImprovementSource.EdgeUse;
				case "MutantCritter":
					return ImprovementSource.MutantCritter;
				case "Cyberzombie":
					return ImprovementSource.Cyberzombie;
				case "StackedFocus":
					return ImprovementSource.StackedFocus;
				case "AttributeLoss":
					return ImprovementSource.AttributeLoss;
                case "Priority":
                    return ImprovementSource.Priority;
				case "Custom":
					return ImprovementSource.Custom;
				default:
			return ImprovementSource.Quality;
			}
		}
		#endregion

		#region Save and Load Methods
		/// <summary>
		/// Save the object's XML to the XmlWriter.
		/// </summary>
		/// <param name="objWriter">XmlTextWriter to write with.</param>
		public void Save(XmlTextWriter objWriter)
		{
			objWriter.WriteStartElement("improvement");
			if (_strUniqueName != "")
				objWriter.WriteElementString("unique", _strUniqueName);
			objWriter.WriteElementString("improvedname", _strImprovedName);
			objWriter.WriteElementString("sourcename", _strSourceName);
			objWriter.WriteElementString("min", _intMin.ToString());
			objWriter.WriteElementString("max", _intMax.ToString());
			objWriter.WriteElementString("aug", _intAug.ToString());
			objWriter.WriteElementString("augmax", _intAugMax.ToString());
			objWriter.WriteElementString("val", _intVal.ToString());
			objWriter.WriteElementString("rating", _intRating.ToString());
			objWriter.WriteElementString("exclude", _strExclude);
			objWriter.WriteElementString("improvementttype", _objImprovementType.ToString());
			objWriter.WriteElementString("improvementsource", _objImprovementSource.ToString());
			objWriter.WriteElementString("custom", _blnCustom.ToString());
			objWriter.WriteElementString("customname", _strCustomName);
			objWriter.WriteElementString("customid", _strCustomId);
			objWriter.WriteElementString("customgroup", _strCustomGroup);
			objWriter.WriteElementString("addtorating", _blnAddToRating.ToString());
			objWriter.WriteElementString("enabled", _blnEnabled.ToString());
			objWriter.WriteElementString("order", _intOrder.ToString());
			objWriter.WriteElementString("notes", _strNotes);
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Load the Attribute from the XmlNode.
		/// </summary>
		/// <param name="objNode">XmlNode to load.</param>
		public void Load(XmlNode objNode)
		{
			try
			{
				_strUniqueName = objNode["unique"].InnerText;
			}
			catch
			{
			}
			_strImprovedName = objNode["improvedname"].InnerText;
			_strSourceName = objNode["sourcename"].InnerText;
			try
			{
				_intMin = Convert.ToInt32(objNode["min"].InnerText);
			}
			catch
			{
			}
			_intMax = Convert.ToInt32(objNode["max"].InnerText);
			_intAug = Convert.ToInt32(objNode["aug"].InnerText);
			_intAugMax = Convert.ToInt32(objNode["augmax"].InnerText);
			_intVal = Convert.ToInt32(objNode["val"].InnerText);
			_intRating = Convert.ToInt32(objNode["rating"].InnerText);
			_strExclude = objNode["exclude"].InnerText;
			_objImprovementType = ConvertToImprovementType(objNode["improvementttype"].InnerText);
			_objImprovementSource = ConvertToImprovementSource(objNode["improvementsource"].InnerText);
			_blnCustom = Convert.ToBoolean(objNode["custom"].InnerText);
			_strCustomName = objNode["customname"].InnerText;
			try
			{
				_strCustomId = objNode["customid"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strCustomGroup = objNode["customgroup"].InnerText;
			}
			catch
			{
			}
			try
			{
				_blnAddToRating = Convert.ToBoolean(objNode["addtorating"].InnerText);
			}
			catch
			{
			}
			try
			{
				_blnEnabled = Convert.ToBoolean(objNode["enabled"].InnerText);
			}
			catch
			{
			}
			try
			{
				_strNotes = objNode["notes"].InnerText;
			}
			catch
			{
			}
			try
			{
				_intOrder = Convert.ToInt32(objNode["order"].InnerText);
			}
			catch
			{
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Whether or not this is a custom-made (manually created) Improvement.
		/// </summary>
		public bool Custom
		{
			get
			{
				return _blnCustom;
			}
			set
			{
				_blnCustom = value;
			}
		}

		/// <summary>
		/// User-entered name for the custom Improvement.
		/// </summary>
		public string CustomName
		{
			get
			{
				return _strCustomName;
			}
			set
			{
				_strCustomName = value;
			}
		}

		/// <summary>
		/// ID from the Improvements file. Only used for custom-made (manually created) Improvements.
		/// </summary>
		public string CustomId
		{
			get
			{
				return _strCustomId;
			}
			set
			{
				_strCustomId = value;
			}
		}

		/// <summary>
		/// Group name for the Custom Improvement.
		/// </summary>
		public string CustomGroup
		{
			get
			{
				return _strCustomGroup;
			}
			set
			{
				_strCustomGroup = value;
			}
		}

		/// <summary>
		/// User-entered notes for the custom Improvement.
		/// </summary>
		public string Notes
		{
			get
			{
				return _strNotes;
			}
			set
			{
				_strNotes = value;
			}
		}

        /// <summary>
        /// Name of the Skill or Attribute that the Improvement is improving.
        /// </summary>
        public string ImprovedName
        {
            get
            {
                return _strImprovedName;
            }
            set
            {
                _strImprovedName = value;
            }
        }

        /// <summary>
        /// Name of the source that granted this Improvement.
        /// </summary>
        public string SourceName
        {
            get
            {
                return _strSourceName;
            }
            set
            {
                _strSourceName = value;
            }
        }

        /// <summary>
        /// The type of Object that the Improvement is improving.
        /// </summary>
        public ImprovementType ImproveType
        {
            get
            {
                return _objImprovementType;
            }
            set
            {
                _objImprovementType = value;
            }
        }

        /// <summary>
        /// The type of Object that granted this Improvement.
        /// </summary>
        public ImprovementSource ImproveSource
        {
            get
            {
                return _objImprovementSource;
            }
            set
            {
                _objImprovementSource = value;
            }
        }

		/// <summary>
		/// Minimum value modifier.
		/// </summary>
		public int Minimum
		{
			get
			{
				return _intMin;
			}
			set
			{
				_intMin = value;
			}
		}

        /// <summary>
        /// Maximum value modifier.
        /// </summary>
        public int Maximum
        {
            get
            {
                return _intMax;
            }
            set
            {
                _intMax = value;
            }
        }

        /// <summary>
        /// Augmented Maximum value modifier.
        /// </summary>
        public int AugmentedMaximum
        {
            get
            {
                return _intAugMax;
            }
            set
            {
                _intAugMax = value;
            }
        }

        /// <summary>
        /// Augmented score modifier.
        /// </summary>
        public int Augmented
        {
            get
            {
                return _intAug;
            }
            set
            {
                _intAug = value;
            }
        }

        /// <summary>
        /// Value modifier.
        /// </summary>
        public int Value
        {
            get
            {
                return _intVal;
            }
            set
            {
                _intVal = value;
            }
        }

        /// <summary>
        /// The Rating value for the Improvement. This is 1 by default.
        /// </summary>
        public int Rating
        {
            get
            {
                return _intRating;
            }
            set
            {
                _intRating = value;
            }
        }

		/// <summary>
		/// A list of child items that should not receive the Improvement's benefit (typically for excluding a Skill from a Skill Group bonus).
		/// </summary>
		public string Exclude
		{
			get
			{
				return _strExclude;
			}
			set
			{
				_strExclude = value;
			}
		}

		/// <summary>
		/// A Unique name for the Improvement. Only the highest value of any one Improvement that is part of this Unique Name group will be applied.
		/// </summary>
		public string UniqueName
		{
			get
			{
				return _strUniqueName;
			}
			set
			{
				_strUniqueName = value;
			}
		}

		/// <summary>
		/// Whether or not the bonus applies directly to a Skill's Rating
		/// </summary>
		public bool AddToRating
		{
			get
			{
				return _blnAddToRating;
			}
			set
			{
				_blnAddToRating = value;
			}
		}

		/// <summary>
		/// Whether or not the Improvement is enabled and provided its bonus.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return _blnEnabled;
			}
			set
			{
				_blnEnabled = value;
			}
		}

		/// <summary>
		/// Sort order for Custom Improvements.
		/// </summary>
		public int SortOrder
		{
			get
			{
				return _intOrder;
			}
			set
			{
				_intOrder = value;
			}
		}
		#endregion
	}

	public class ImprovementManager
	{
		private readonly Character _objCharacter;
		
		// String that will be used to limit the selection in Pick forms.
		private string _strLimitSelection = "";

		private string _strSelectedValue = "";
		private string _strForcedValue = "";
		private readonly List<Improvement> _lstTransaction = new List<Improvement>();

		public ImprovementManager(Character objCharacter)
		{
			LanguageManager.Instance.Load(null);
			_objCharacter = objCharacter;
		}

		#region Properties
		/// <summary>
		/// Limit what can be selected in Pick forms to a single value. This is typically used when selecting the Qualities for a Metavariant that has a specifiec
		/// Attribute selection for Qualities like Metagenetic Improvement.
		/// </summary>
		public string LimitSelection
		{
			get
			{
				return _strLimitSelection;
			}
			set
			{
				_strLimitSelection = value;
			}
		}

		/// <summary>
		/// The string that was entered or selected from any of the dialogue windows that were presented because of this Improvement.
		/// </summary>
		public string SelectedValue
		{
			get
			{
				return _strSelectedValue;
			}
		}

		/// <summary>
		/// Force any dialogue windows that open to use this string as their selected value.
		/// </summary>
		public string ForcedValue
		{
			set
			{
				_strForcedValue = value;
			}
		}
		#endregion

		#region Helper Methods

		/// <summary>
		/// Retrieve the total Improvement value for the specified ImprovementType.
		/// </summary>
		/// <param name="objImprovementType">ImprovementType to retrieve the value of.</param>
		/// <param name="blnAddToRating">Whether or not we should only retrieve values that have AddToRating enabled.</param>
		/// <param name="strImprovedName">Name to assign to the Improvement.</param>
		public int ValueOf(Improvement.ImprovementType objImprovementType, bool blnAddToRating = false, string strImprovedName = null)
		{
			if (_objCharacter == null)
				return 0;

			List<string> lstUniqueName = new List<string>();
			List<string[,]> lstUniquePair = new List<string[,]>();
			int intValue = 0;
			foreach (Improvement objImprovement in _objCharacter.Improvements)
			{
				if (objImprovement.Enabled && !objImprovement.Custom)
				{
					bool blnAllowed = true;
					// Technomancers cannot benefit from Gear-based Matrix Initiative Pass modifiers (Gear - Sim Modules).
					if (_objCharacter.RESEnabled && objImprovement.ImproveSource == Improvement.ImprovementSource.Gear && objImprovementType == Improvement.ImprovementType.MatrixInitiativePass)
						blnAllowed = false;
					// Ignore items that apply to a Skill's Rating.
					if (objImprovement.AddToRating != blnAddToRating)
						blnAllowed = false;
					// If an Improved Name has been passed, only retrieve values that have this Improved Name.
					if (strImprovedName != null)
					{
						if (strImprovedName != objImprovement.ImprovedName)
							blnAllowed = false;
					}

					if (blnAllowed)
					{
						if (objImprovement.UniqueName != "" && objImprovement.ImproveType == objImprovementType)
						{
							// If this has a UniqueName, run through the current list of UniqueNames seen. If it is not already in the list, add it.
							bool blnFound = false;
							foreach (string strName in lstUniqueName)
							{
								if (strName == objImprovement.UniqueName)
									blnFound = true;
								break;
							}
							if (!blnFound)
								lstUniqueName.Add(objImprovement.UniqueName);

							// Add the values to the UniquePair List so we can check them later.
							string[,] strValues = new string[,] { { objImprovement.UniqueName, objImprovement.Value.ToString() } };
							lstUniquePair.Add(strValues);
						}
						else
						{
							if (objImprovement.ImproveType == objImprovementType)
								intValue += objImprovement.Value;
						}
					}
				}
			}

			// Run through the list of UniqueNames and pick out the highest value for each one.
			foreach (string strName in lstUniqueName)
			{
				int intHighest = -999;
				foreach (string[,] strValues in lstUniquePair)
				{
					if (strValues[0, 0] == strName)
					{
						if (Convert.ToInt32(strValues[0, 1]) > intHighest)
							intHighest = Convert.ToInt32(strValues[0, 1]);
					}
				}
				intValue += intHighest;
			}

			if (lstUniqueName.Contains("precedence1"))
			{
				intValue = 0;
				// Retrieve all of the items that are precedence1 and nothing else.
				foreach (string[,] strValues in lstUniquePair)
				{
					if (strValues[0, 0] == "precedence1")
						intValue += Convert.ToInt32(strValues[0, 1]);
				}
			}

			if (lstUniqueName.Contains("precedence0"))
			{
				// Retrieve only the highest precedence0 value.
				// Run through the list of UniqueNames and pick out the highest value for each one.
				int intHighest = -999;
				foreach (string[,] strValues in lstUniquePair)
				{
					if (strValues[0, 0] == "precedence0")
					{
						if (Convert.ToInt32(strValues[0, 1]) > intHighest)
							intHighest = Convert.ToInt32(strValues[0, 1]);
					}
				}
				intValue = intHighest;
			}

			// Factor in Custom Improvements.
			lstUniqueName = new List<string>();
			lstUniquePair = new List<string[,]>();
			int intCustomValue = 0;
			foreach (Improvement objImprovement in _objCharacter.Improvements)
			{
				if (objImprovement.Enabled && objImprovement.Custom)
				{
					bool blnAllowed = true;
					// Technomancers cannot benefit from Gear-based Matrix Initiative Pass modifiers (Gear - Sim Modules).
					if (_objCharacter.RESEnabled && objImprovement.ImproveSource == Improvement.ImprovementSource.Gear && objImprovementType == Improvement.ImprovementType.MatrixInitiativePass)
						blnAllowed = false;
					// Ignore items that apply to a Skill's Rating.
					if (objImprovement.AddToRating != blnAddToRating)
						blnAllowed = false;
					// If an Improved Name has been passed, only retrieve values that have this Improved Name.
					if (strImprovedName != null)
					{
						if (strImprovedName != objImprovement.ImprovedName)
							blnAllowed = false;
					}

					if (blnAllowed)
					{
						if (objImprovement.UniqueName != "" && objImprovement.ImproveType == objImprovementType)
						{
							// If this has a UniqueName, run through the current list of UniqueNames seen. If it is not already in the list, add it.
							bool blnFound = false;
							foreach (string strName in lstUniqueName)
							{
								if (strName == objImprovement.UniqueName)
									blnFound = true;
								break;
							}
							if (!blnFound)
								lstUniqueName.Add(objImprovement.UniqueName);

							// Add the values to the UniquePair List so we can check them later.
							string[,] strValues = new string[,] { { objImprovement.UniqueName, objImprovement.Value.ToString() } };
							lstUniquePair.Add(strValues);
						}
						else
						{
							if (objImprovement.ImproveType == objImprovementType)
								intCustomValue += objImprovement.Value;
						}
					}
				}
			}

			// Run through the list of UniqueNames and pick out the highest value for each one.
			foreach (string strName in lstUniqueName)
			{
				int intHighest = -999;
				foreach (string[,] strValues in lstUniquePair)
				{
					if (strValues[0, 0] == strName)
					{
						if (Convert.ToInt32(strValues[0, 1]) > intHighest)
							intHighest = Convert.ToInt32(strValues[0, 1]);
					}
				}
				intCustomValue += intHighest;
			}

			return intValue + intCustomValue;
		}

		/// <summary>
		/// Convert a string to an integer, converting "Rating" to a number where appropriate.
		/// </summary>
		/// <param name="strValue">String value to parse.</param>
		/// <param name="intRating">Integer value to replace "Rating" with.</param>
		private int ValueToInt(string strValue, int intRating = 1)
		{
			if (strValue.Contains("Rating") || strValue.Contains("BOD") || strValue.Contains("AGI") || strValue.Contains("REA") || strValue.Contains("STR") || strValue.Contains("CHA") || strValue.Contains("INT") || strValue.Contains("LOG") || strValue.Contains("WIL") || strValue.Contains("EDG") || strValue.Contains("MAG") || strValue.Contains("RES"))
			{
				// If the value contain an Attribute name, replace it with the character's Attribute.
				strValue = strValue.Replace("BOD", _objCharacter.BOD.TotalValue.ToString());
				strValue = strValue.Replace("AGI", _objCharacter.AGI.TotalValue.ToString());
				strValue = strValue.Replace("REA", _objCharacter.REA.TotalValue.ToString());
				strValue = strValue.Replace("STR", _objCharacter.STR.TotalValue.ToString());
				strValue = strValue.Replace("CHA", _objCharacter.CHA.TotalValue.ToString());
				strValue = strValue.Replace("INT", _objCharacter.INT.TotalValue.ToString());
				strValue = strValue.Replace("LOG", _objCharacter.LOG.TotalValue.ToString());
				strValue = strValue.Replace("WIL", _objCharacter.WIL.TotalValue.ToString());
				strValue = strValue.Replace("EDG", _objCharacter.EDG.TotalValue.ToString());
				strValue = strValue.Replace("MAG", _objCharacter.MAG.TotalValue.ToString());
				strValue = strValue.Replace("RES", _objCharacter.RES.TotalValue.ToString());

				XmlDocument objXmlDocument = new XmlDocument();
				XPathNavigator nav = objXmlDocument.CreateNavigator();
				string strReturn = strValue.Replace("Rating", intRating.ToString());
				XPathExpression xprValue = nav.Compile(strReturn);

				// Treat this as a decimal value so any fractions can be rounded down. This is currently only used by the Boosted Reflexes Cyberware from SR2050.
				decimal decValue = Convert.ToDecimal(nav.Evaluate(xprValue).ToString(), GlobalOptions.Instance.CultureInfo);
				decValue = Math.Floor(decValue);
				int intValue = Convert.ToInt32(decValue);

				return Convert.ToInt32(intValue);
			}
			else
			{
				if (strValue.Contains("FixedValues"))
				{
					string[] strValues = strValue.Replace("FixedValues(", string.Empty).Replace(")", string.Empty).Split(',');
					return Convert.ToInt32(strValues[intRating - 1]);
				}
				else
					return Convert.ToInt32(strValue);
			}
		}

		/// <summary>
		/// Determine whether or not an XmlNode with the specified name exists within an XmlNode.
		/// </summary>
		/// <param name="objXmlNode">XmlNode to examine.</param>
		/// <param name="strName">Name of the XmlNode to look for.</param>
		private bool NodeExists(XmlNode objXmlNode, string strName)
		{
			bool blnReturn = false;
			try
			{
				XmlNode objXmlTest = objXmlNode.SelectSingleNode(strName);
				if (objXmlTest != null)
					blnReturn = true;
			}
			catch
			{
			}

			return blnReturn;
		}
		#endregion

		#region Improvement System
		/// <summary>
		/// Create all of the Improvements for an XML Node.
		/// </summary>
		/// <param name="objImprovementSource">Type of object that grants these Improvements.</param>
		/// <param name="strSourceName">Name of the item that grants these Improvements.</param>
		/// <param name="nodBonus">bonus XMLXode from the source data file.</param>
		/// <param name="blnConcatSelectedValue">Whether or not any selected values should be concatinated with the SourceName string when storing.</param>
		/// <param name="intRating">Selected Rating value that is used to replace the Rating string in an Improvement.</param>
		/// <param name="strFriendlyName">Friendly name to show in any dialogue windows that ask for a value.</param>
		public bool CreateImprovements(Improvement.ImprovementSource objImprovementSource, string strSourceName, XmlNode nodBonus, bool blnConcatSelectedValue = false, int intRating = 1, string strFriendlyName = "", bool blnConditional = false)
		{
			if (nodBonus == null)
			{
				_strForcedValue = "";
				_strLimitSelection = "";
				return true;
			}

            if (blnConditional)
            {
                _objCharacter.ImprovementGroups.Add(strSourceName);
            }

			string strUnique = "";
			if (nodBonus.Attributes["unique"] != null)
				strUnique = nodBonus.Attributes["unique"].InnerText;

			bool blnSuccess = true;
			_strSelectedValue = "";

			// If no friendly name was provided, use the one from SourceName.
			if (strFriendlyName == "")
				strFriendlyName = strSourceName;

			if (nodBonus.HasChildNodes)
			{
				// Select Text (custom entry for things like Allergy).
				if (NodeExists(nodBonus, "selecttext"))
				{
					if (_strForcedValue != "")
						_strLimitSelection = _strForcedValue;

					// Display the Select Text window and record the value that was entered.
					frmSelectText frmPickText = new frmSelectText();
					frmPickText.Description = LanguageManager.Instance.GetString("String_Improvement_SelectText").Replace("{0}", strFriendlyName);

					if (_strLimitSelection != "")
					{
						frmPickText.SelectedValue = _strLimitSelection;
						frmPickText.Opacity = 0;
					}

					frmPickText.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickText.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					_strSelectedValue = frmPickText.SelectedValue;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

					// Create the Improvement.
					CreateImprovement(frmPickText.SelectedValue, objImprovementSource, strSourceName, Improvement.ImprovementType.Text, strUnique, blnConditional);
				}
			}

			// If there is no character object, don't attempt to add any Improvements.
			if (_objCharacter == null)
				return true;

			// Check to see what bonuses the node grants.
			if (nodBonus.HasChildNodes)
			{
				// Add an Attribute.
				if (NodeExists(nodBonus, "addattribute"))
				{
					if (nodBonus["addattribute"]["name"].InnerText == "MAG")
					{
						_objCharacter.MAGEnabled = true;
						CreateImprovement("MAG", objImprovementSource, strSourceName, Improvement.ImprovementType.Attribute, "enableattribute", blnConditional, 0, 0);
                        if (nodBonus["addattribute"]["min"] != null)
                            _objCharacter.MAG.MetatypeMinimum = Convert.ToInt32(nodBonus["addattribute"]["min"].InnerText);
                        if (nodBonus["addattribute"]["max"] != null)
                            _objCharacter.MAG.MetatypeMaximum = Convert.ToInt32(nodBonus["addattribute"]["max"].InnerText);
                        if (nodBonus["addattribute"]["aug"] != null)
                            _objCharacter.MAG.MetatypeAugmentedMaximum = Convert.ToInt32(nodBonus["addattribute"]["aug"].InnerText);
                        if (nodBonus["addattribute"]["val"] != null)
                            _objCharacter.MAG.Value = Convert.ToInt32(nodBonus["addattribute"]["val"].InnerText);
					}
					else if (nodBonus["addattribute"]["name"].InnerText == "RES")
					{
						_objCharacter.RESEnabled = true;
						CreateImprovement("RES", objImprovementSource, strSourceName, Improvement.ImprovementType.Attribute, "enableattribute", blnConditional, 0, 0);
                        if (nodBonus["addattribute"]["min"] != null)
                            _objCharacter.RES.MetatypeMinimum = Convert.ToInt32(nodBonus["addattribute"]["min"].InnerText);
                        if (nodBonus["addattribute"]["max"] != null)
                            _objCharacter.RES.MetatypeMaximum = Convert.ToInt32(nodBonus["addattribute"]["max"].InnerText);
                        if (nodBonus["addattribute"]["aug"] != null)
                            _objCharacter.RES.MetatypeAugmentedMaximum = Convert.ToInt32(nodBonus["addattribute"]["aug"].InnerText);
                        if (nodBonus["addattribute"]["val"] != null)
                            _objCharacter.RES.Value = Convert.ToInt32(nodBonus["addattribute"]["val"].InnerText);
					}
				}

				// Enable a special tab.
				if (NodeExists(nodBonus, "enabletab"))
				{
					foreach (XmlNode objXmlEnable in nodBonus["enabletab"].ChildNodes)
					{
						switch (objXmlEnable.InnerText)
						{
							case "magician":
								_objCharacter.MagicianEnabled = true;
                                CreateImprovement("Magician", objImprovementSource, strSourceName, Improvement.ImprovementType.SpecialTab, "enabletab", blnConditional, 0, 0);
								break;
							case "adept":
								_objCharacter.AdeptEnabled = true;
								CreateImprovement("Adept", objImprovementSource, strSourceName, Improvement.ImprovementType.SpecialTab, "enabletab", blnConditional, 0, 0);
								break;
							case "technomancer":
								_objCharacter.TechnomancerEnabled = true;
                                CreateImprovement("Technomancer", objImprovementSource, strSourceName, Improvement.ImprovementType.SpecialTab, "enabletab", blnConditional, 0, 0);
								break;
							case "critter":
								_objCharacter.CritterEnabled = true;
                                CreateImprovement("Critter", objImprovementSource, strSourceName, Improvement.ImprovementType.SpecialTab, "enabletab", blnConditional, 0, 0);
								break;
							case "initiation":
								_objCharacter.InitiationEnabled = true;
                                CreateImprovement("Initiation", objImprovementSource, strSourceName, Improvement.ImprovementType.SpecialTab, "enabletab", blnConditional, 0, 0);
								break;
						}
					}
				}

				// Select Restricted (select Restricted items for Fake Licenses).
				if (NodeExists(nodBonus, "selectrestricted"))
				{
					frmSelectItem frmPickItem = new frmSelectItem();
					frmPickItem.Character = _objCharacter;
					if (_strForcedValue != string.Empty)
						frmPickItem.ForceItem = _strForcedValue;
					frmPickItem.AllowAutoSelect = false;
					frmPickItem.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickItem.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					_strSelectedValue = frmPickItem.SelectedItem;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

					// Create the Improvement.
                    CreateImprovement(frmPickItem.SelectedItem, objImprovementSource, strSourceName, Improvement.ImprovementType.Restricted, strUnique, blnConditional);
				}

				// Select a Skill.
				if (NodeExists(nodBonus, "selectskill"))
				{
					// Display the Select Skill window and record which Skill was selected.
					frmSelectSkill frmPickSkill = new frmSelectSkill(_objCharacter);
					if (strFriendlyName != "")
						frmPickSkill.Description = LanguageManager.Instance.GetString("String_Improvement_SelectSkillNamed").Replace("{0}", strFriendlyName);
					else
						frmPickSkill.Description = LanguageManager.Instance.GetString("String_Improvement_SelectSkill");

					if (nodBonus.SelectSingleNode("selectskill").OuterXml.Contains("skillgroup"))
						frmPickSkill.OnlySkillGroup = nodBonus.SelectSingleNode("selectskill").Attributes["skillgroup"].InnerText;
					else if (nodBonus.SelectSingleNode("selectskill").OuterXml.Contains("skillcategory"))
						frmPickSkill.OnlyCategory = nodBonus.SelectSingleNode("selectskill").Attributes["skillcategory"].InnerText;
					else if (nodBonus.SelectSingleNode("selectskill").OuterXml.Contains("excludecategory"))
						frmPickSkill.ExcludeCategory = nodBonus.SelectSingleNode("selectskill").Attributes["excludecategory"].InnerText;
					else if (nodBonus.SelectSingleNode("selectskill").OuterXml.Contains("limittoskill"))
						frmPickSkill.LimitToSkill = nodBonus.SelectSingleNode("selectskill").Attributes["limittoskill"].InnerText;
					if (_strForcedValue != "")
					{
						frmPickSkill.OnlySkill = _strForcedValue;
						frmPickSkill.Opacity = 0;
					}

					frmPickSkill.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickSkill.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					bool blnAddToRating = false;
					if (nodBonus["selectskill"]["applytorating"] != null)
					{
						if (nodBonus["selectskill"]["applytorating"].InnerText == "yes")
							blnAddToRating = true;
					}

					_strSelectedValue = frmPickSkill.SelectedSkill;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

					// Find the selected Skill.
					foreach (Skill objSkill in _objCharacter.Skills)
					{
						if (frmPickSkill.SelectedSkill.Contains("Exotic Melee Weapon") || frmPickSkill.SelectedSkill.Contains("Exotic Ranged Weapon") || frmPickSkill.SelectedSkill.Contains("Pilot Exotic Vehicle"))
						{
							if (objSkill.Name + " (" + objSkill.Specialization + ")" == frmPickSkill.SelectedSkill)
							{
								// We've found the selected Skill.
								if (nodBonus["selectskill"]["val"] != null)
                                    CreateImprovement(objSkill.Name + " (" + objSkill.Specialization + ")", objImprovementSource, strSourceName, Improvement.ImprovementType.Skill, strUnique, blnConditional, ValueToInt(nodBonus["selectskill"]["val"].InnerText, intRating), 1, 0, 0, 0, 0, "", blnAddToRating);

								if (nodBonus["selectskill"]["max"] != null)
                                    CreateImprovement(objSkill.Name + " (" + objSkill.Specialization + ")", objImprovementSource, strSourceName, Improvement.ImprovementType.Skill, strUnique, blnConditional, 0, 1, 0, ValueToInt(nodBonus["selectskill"]["max"].InnerText, intRating), 0, 0, "", blnAddToRating);

                                if (nodBonus["selectskill"]["weapondv"] != null)
                                    CreateImprovement(objSkill.Name + " (" + objSkill.Specialization + ")", objImprovementSource, strSourceName, Improvement.ImprovementType.WeaponSkillDV, strUnique, blnConditional, ValueToInt(nodBonus["selectskill"]["weapondv"].InnerText, intRating));

                                if (nodBonus["selectskill"]["accuracy"] != null)
                                    CreateImprovement(objSkill.Name + " (" + objSkill.Specialization + ")", objImprovementSource, strSourceName, Improvement.ImprovementType.WeaponSkillAccuracy, strUnique, blnConditional, ValueToInt(nodBonus["selectskill"]["accuracy"].InnerText, intRating));
							}
						}
						else
						{
							if (objSkill.Name == frmPickSkill.SelectedSkill)
							{
								// We've found the selected Skill.
								if (nodBonus["selectskill"]["val"] != null)
                                    CreateImprovement(objSkill.Name, objImprovementSource, strSourceName, Improvement.ImprovementType.Skill, strUnique, blnConditional, ValueToInt(nodBonus["selectskill"]["val"].InnerText, intRating), 1, 0, 0, 0, 0, "", blnAddToRating);

								if (nodBonus["selectskill"]["max"] != null)
                                    CreateImprovement(objSkill.Name, objImprovementSource, strSourceName, Improvement.ImprovementType.Skill, strUnique, blnConditional, 0, 1, 0, ValueToInt(nodBonus["selectskill"]["max"].InnerText, intRating), 0, 0, "", blnAddToRating);

                                if (nodBonus["selectskill"]["weapondv"] != null)
                                    CreateImprovement(objSkill.Name, objImprovementSource, strSourceName, Improvement.ImprovementType.WeaponSkillDV, strUnique, blnConditional, ValueToInt(nodBonus["selectskill"]["weapondv"].InnerText, intRating));

                                if (nodBonus["selectskill"]["accuracy"] != null)
                                    CreateImprovement(objSkill.Name, objImprovementSource, strSourceName, Improvement.ImprovementType.WeaponSkillAccuracy, strUnique, blnConditional, ValueToInt(nodBonus["selectskill"]["accuracy"].InnerText, intRating));
							}
						}
					}
				}

				// Select a Skill Group.
				if (NodeExists(nodBonus, "selectskillgroup"))
				{
					string strExclude = "";
					if (nodBonus["selectskillgroup"].Attributes["excludecategory"] != null)
						strExclude = nodBonus["selectskillgroup"].Attributes["excludecategory"].InnerText;

					frmSelectSkillGroup frmPickSkillGroup = new frmSelectSkillGroup();
					if (strFriendlyName != "")
						frmPickSkillGroup.Description = LanguageManager.Instance.GetString("String_Improvement_SelectSkillGroupName").Replace("{0}", strFriendlyName);
					else
						frmPickSkillGroup.Description = LanguageManager.Instance.GetString("String_Improvement_SelectSkillGroup");

					if (_strForcedValue != "")
					{
						frmPickSkillGroup.OnlyGroup = _strForcedValue;
						frmPickSkillGroup.Opacity = 0;
					}

					if (strExclude != string.Empty)
						frmPickSkillGroup.ExcludeCategory = strExclude;

					frmPickSkillGroup.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickSkillGroup.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					bool blnAddToRating = false;
					if (nodBonus["selectskillgroup"]["applytorating"] != null)
					{
						if (nodBonus["selectskillgroup"]["applytorating"].InnerText == "yes")
							blnAddToRating = true;
					}

					_strSelectedValue = frmPickSkillGroup.SelectedSkillGroup;

                    CreateImprovement(_strSelectedValue, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillGroup, strUnique, blnConditional, ValueToInt(nodBonus["selectskillgroup"]["bonus"].InnerText, intRating), 1, 0, 0, 0, 0, strExclude, blnAddToRating);
				}

				// Select an Attribute.
				if (NodeExists(nodBonus, "selectattribute"))
				{
					// Display the Select Attribute window and record which Skill was selected.
					frmSelectAttribute frmPickAttribute = new frmSelectAttribute();
					if (strFriendlyName != "")
						frmPickAttribute.Description = LanguageManager.Instance.GetString("String_Improvement_SelectAttributeNamed").Replace("{0}", strFriendlyName);
					else
						frmPickAttribute.Description = LanguageManager.Instance.GetString("String_Improvement_SelectAttribute");

					// Add MAG and/or RES to the list of Attributes if they are enabled on the form.
					if (_objCharacter.MAGEnabled)
						frmPickAttribute.AddMAG();
					if (_objCharacter.RESEnabled)
						frmPickAttribute.AddRES();

					if (nodBonus["selectattribute"]["attribute"] != null)
					{
						List<string> strValue = new List<string>();
						foreach (XmlNode objXmlAttribute in nodBonus["selectattribute"].SelectNodes("attribute"))
							strValue.Add(objXmlAttribute.InnerText);
						frmPickAttribute.LimitToList(strValue);
					}

					if (nodBonus["selectattribute"]["excludeattribute"] != null)
					{
						List<string> strValue = new List<string>();
						foreach (XmlNode objXmlAttribute in nodBonus["selectattribute"].SelectNodes("excludeattribute"))
							strValue.Add(objXmlAttribute.InnerText);
						frmPickAttribute.RemoveFromList(strValue);
					}

					// Check to see if there is only one possible selection because of _strLimitSelection.
					if (_strForcedValue != "")
						_strLimitSelection = _strForcedValue;

					if (_strLimitSelection != "")
					{
						frmPickAttribute.SingleAttribute(_strLimitSelection);
						frmPickAttribute.Opacity = 0;
					}

					frmPickAttribute.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickAttribute.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					_strSelectedValue = frmPickAttribute.SelectedAttribute;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

					// Record the improvement.
					int intMin = 0;
					int intAug = 0;
					int intMax = 0;
					int intAugMax = 0;

					// Extract the modifiers.
					if (nodBonus["selectattribute"]["min"] != null)
						intMin = ValueToInt(nodBonus["selectattribute"]["min"].InnerXml, intRating);
					if (nodBonus["selectattribute"]["val"] != null)
						intAug = ValueToInt(nodBonus["selectattribute"]["val"].InnerXml, intRating);
					if (nodBonus["selectattribute"]["max"] != null)
						intMax = ValueToInt(nodBonus["selectattribute"]["max"].InnerXml, intRating);
					if (nodBonus["selectattribute"]["aug"] != null)
						intAugMax = ValueToInt(nodBonus["selectattribute"]["aug"].InnerXml, intRating);

					string strAttribute = frmPickAttribute.SelectedAttribute;

					if (nodBonus["selectattribute"]["affectbase"] != null)
						strAttribute += "Base";

                    CreateImprovement(strAttribute, objImprovementSource, strSourceName, Improvement.ImprovementType.Attribute, strUnique, blnConditional, 0, 1, intMin, intMax, intAug, intAugMax);
				}

				// Select an Attribute.
				if (NodeExists(nodBonus, "swapskillattribute"))
				{
					// Display the Select Attribute window and record which Skill was selected.
					frmSelectAttribute frmPickAttribute = new frmSelectAttribute();
					if (strFriendlyName != "")
						frmPickAttribute.Description = LanguageManager.Instance.GetString("String_Improvement_SelectAttributeNamed").Replace("{0}", strFriendlyName);
					else
						frmPickAttribute.Description = LanguageManager.Instance.GetString("String_Improvement_SelectAttribute");

					List<string> strValue = new List<string>();
					strValue.Add("LOG");
					strValue.Add("WIL");
					strValue.Add("INT");
					strValue.Add("CHA");
					strValue.Add("EDG");
					strValue.Add("MAG");
					strValue.Add("RES");
					frmPickAttribute.RemoveFromList(strValue);

					if (nodBonus["swapskillattribute"]["attribute"] != null)
					{
						List<string> strLimitValue = new List<string>();
						foreach (XmlNode objXmlAttribute in nodBonus["swapskillattribute"].SelectNodes("attribute"))
							strLimitValue.Add(objXmlAttribute.InnerText);
						frmPickAttribute.LimitToList(strLimitValue);
					}

					// Check to see if there is only one possible selection because of _strLimitSelection.
					if (_strForcedValue != "")
						_strLimitSelection = _strForcedValue;

					if (_strLimitSelection != "")
					{
						frmPickAttribute.SingleAttribute(_strLimitSelection);
						frmPickAttribute.Opacity = 0;
					}

					frmPickAttribute.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickAttribute.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					_strSelectedValue = frmPickAttribute.SelectedAttribute;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

                    CreateImprovement(frmPickAttribute.SelectedAttribute, objImprovementSource, strSourceName, Improvement.ImprovementType.SwapSkillAttribute, strUnique, blnConditional);
				}

				// Select a Spell.
				if (NodeExists(nodBonus, "selectspell"))
				{
					// Display the Select Spell window.
					frmSelectSpell frmPickSpell = new frmSelectSpell(_objCharacter);

					if (nodBonus["selectspell"].Attributes["category"] != null)
						frmPickSpell.LimitCategory = nodBonus["selectspell"].Attributes["category"].InnerText;
					if (_strForcedValue != "")
					{
						frmPickSpell.ForceSpellName = _strForcedValue;
						frmPickSpell.Opacity = 0;
					}

					frmPickSpell.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickSpell.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return false;
					}

					_strSelectedValue = frmPickSpell.SelectedSpell;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

                    CreateImprovement(frmPickSpell.SelectedSpell, objImprovementSource, strSourceName, Improvement.ImprovementType.Text, strUnique, blnConditional);
				}

				// Affect a Specific Attribute.
				if (NodeExists(nodBonus, "specificattribute"))
				{
					XmlNodeList objXmlAttributeList = nodBonus.SelectNodes("specificattribute");
					foreach (XmlNode objXmlAttribute in objXmlAttributeList)
					{
						if (objXmlAttribute["name"].InnerText != "ESS")
						{
							// Display the Select Attribute window and record which Attribute was selected.
							// Record the improvement.
							int intMin = 0;
							int intAug = 0;
							int intMax = 0;
							int intAugMax = 0;

							// Extract the modifiers.
							if (objXmlAttribute["min"] != null)
								intMin = ValueToInt(objXmlAttribute["min"].InnerXml, intRating);
							if (objXmlAttribute["val"] != null)
								intAug = ValueToInt(objXmlAttribute["val"].InnerXml, intRating);
							if (objXmlAttribute["max"] != null)
							{
								if (objXmlAttribute["max"].InnerText.Contains("-natural"))
								{
									intMax = Convert.ToInt32(objXmlAttribute["max"].InnerText.Replace("-natural", string.Empty)) - _objCharacter.GetAttribute(objXmlAttribute["name"].InnerText).MetatypeMaximum;
								}
								else
									intMax = ValueToInt(objXmlAttribute["max"].InnerXml, intRating);
							}
							if (objXmlAttribute["aug"] != null)
								intAugMax = ValueToInt(objXmlAttribute["aug"].InnerXml, intRating);

							string strUseUnique = strUnique;
							if (objXmlAttribute["name"].Attributes["precedence"] != null)
								strUseUnique = "precedence" + objXmlAttribute["name"].Attributes["precedence"].InnerText;

							string strAttribute = objXmlAttribute["name"].InnerText;

							if (objXmlAttribute["affectbase"] != null)
								strAttribute += "Base";

                            CreateImprovement(strAttribute, objImprovementSource, strSourceName, Improvement.ImprovementType.Attribute, strUseUnique, blnConditional, 0, 1, intMin, intMax, intAug, intAugMax);
						}
						else
                            CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Essence, "", blnConditional, Convert.ToInt32(objXmlAttribute["val"].InnerText));
					}
				}

				// Change the maximum number of BP that can be spent on Nuyen.
				if (NodeExists(nodBonus, "nuyenmaxbp"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.NuyenMaxBP, "", blnConditional, ValueToInt(nodBonus["nuyenmaxbp"].InnerText, intRating));
				}

				// Change the amount of Nuyen the character has at creation time (this can put the character over the amount they're normally allowed).
				if (NodeExists(nodBonus, "nuyenamt"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Nuyen, strUnique, blnConditional, ValueToInt(nodBonus["nuyenamt"].InnerText, intRating));
				}

				// Improve Condition Monitors.
				if (NodeExists(nodBonus, "conditionmonitor"))
				{
					// Physical Condition.
					if (nodBonus["conditionmonitor"]["physical"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.PhysicalCM, strUnique, blnConditional, ValueToInt(nodBonus["conditionmonitor"]["physical"].InnerText, intRating));

					// Stun Condition.
					if (nodBonus["conditionmonitor"]["stun"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.StunCM, strUnique, blnConditional, ValueToInt(nodBonus["conditionmonitor"]["stun"].InnerText, intRating));

					// Condition Monitor Threshold.
					if (NodeExists(nodBonus.SelectSingleNode("conditionmonitor"), "threshold"))
					{
						string strUseUnique = strUnique;
						if (nodBonus["conditionmonitor"]["threshold"].Attributes["precedence"] != null)
							strUseUnique = "precedence" + nodBonus["conditionmonitor"]["threshold"].Attributes["precedence"].InnerText;

                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.CMThreshold, strUseUnique, blnConditional, ValueToInt(nodBonus.SelectSingleNode("conditionmonitor")["threshold"].InnerText, intRating));
					}

					// Condition Monitor Threshold Offset. (Additioal boxes appear before the FIRST Condition Monitor penalty)
					if (NodeExists(nodBonus.SelectSingleNode("conditionmonitor"), "thresholdoffset"))
					{
						string strUseUnique = strUnique;
						if (nodBonus["conditionmonitor"]["thresholdoffset"].Attributes["precedence"] != null)
							strUseUnique = "precedence" + nodBonus["conditionmonitor"]["thresholdoffset"].Attributes["precedence"].InnerText;

                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.CMThresholdOffset, strUseUnique, blnConditional, ValueToInt(nodBonus.SelectSingleNode("conditionmonitor")["thresholdoffset"].InnerText, intRating));
					}

					// Condition Monitor Overflow.
					if (nodBonus["conditionmonitor"]["overflow"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.CMOverflow, strUnique, blnConditional, ValueToInt(nodBonus["conditionmonitor"]["overflow"].InnerText, intRating));
				}

				// Improve Living Personal Attributes.
				if (NodeExists(nodBonus, "livingpersona"))
				{
					// Response.
					if (nodBonus["livingpersona"]["response"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LivingPersonaResponse, strUnique, blnConditional, ValueToInt(nodBonus["livingpersona"]["response"].InnerText, intRating));

					// Signal.
					if (nodBonus["livingpersona"]["signal"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LivingPersonaSignal, strUnique, blnConditional, ValueToInt(nodBonus["livingpersona"]["signal"].InnerText, intRating));

					// Firewall.
					if (nodBonus["livingpersona"]["firewall"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LivingPersonaFirewall, strUnique, blnConditional, ValueToInt(nodBonus["livingpersona"]["firewall"].InnerText, intRating));

					// System.
					if (nodBonus["livingpersona"]["system"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LivingPersonaSystem, strUnique, blnConditional, ValueToInt(nodBonus["livingpersona"]["system"].InnerText, intRating));

					// Biofeedback Filter.
					if (nodBonus["livingpersona"]["biofeedback"] != null)
                        CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LivingPersonaBiofeedback, strUnique, blnConditional, ValueToInt(nodBonus["livingpersona"]["biofeedback"].InnerText, intRating));
				}

				// The Improvement adjusts a specific Skill.
				if (NodeExists(nodBonus, "specificskill"))
				{
					XmlNodeList objXmlSkillList = nodBonus.SelectNodes("specificskill");
					// Run through each specific Skill since there may be more than one affected.
					foreach (XmlNode objXmlSkill in objXmlSkillList)
					{
						bool blnAddToRating = false;
						if (objXmlSkill["applytorating"] != null)
						{
							if (objXmlSkill["applytorating"].InnerText == "yes")
								blnAddToRating = true;
						}

						string strUseUnique = strUnique;
						if (nodBonus["specificskill"].Attributes["precedence"] != null)
							strUseUnique = "precedence" + nodBonus["specificskill"].Attributes["precedence"].InnerText;

						// Record the improvement.
						if (objXmlSkill["bonus"] != null)
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.Skill, strUseUnique, blnConditional, ValueToInt(objXmlSkill["bonus"].InnerXml, intRating), 1, 0, 0, 0, 0, "", blnAddToRating);
						if (objXmlSkill["max"] != null)
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.Skill, strUseUnique, blnConditional, 0, 1, 0, ValueToInt(objXmlSkill["max"].InnerText, intRating), 0, 0, "", blnAddToRating);
					}
				}

				// The Improvement adjusts a Skill Category.
				if (NodeExists(nodBonus, "skillcategory"))
				{
					XmlNodeList objXmlSkillList = nodBonus.SelectNodes("skillcategory");
					// Run through each of the Skill Categories since there may be more than one affected.
					foreach (XmlNode objXmlSkill in objXmlSkillList)
					{
						bool blnAddToRating = false;
						if (objXmlSkill["applytorating"] != null)
						{
							if (objXmlSkill["applytorating"].InnerText == "yes")
								blnAddToRating = true;
						}
						if (objXmlSkill["exclude"] != null)
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillCategory, strUnique, blnConditional, ValueToInt(objXmlSkill["bonus"].InnerXml, intRating), 1, 0, 0, 0, 0, objXmlSkill["exclude"].InnerText, blnAddToRating);
						else
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillCategory, strUnique, blnConditional, ValueToInt(objXmlSkill["bonus"].InnerXml, intRating), 1, 0, 0, 0, 0, "", blnAddToRating);
					}
				}

				// The Improvement adjusts a Skill Group.
				if (NodeExists(nodBonus, "skillgroup"))
				{
					XmlNodeList objXmlSkillList = nodBonus.SelectNodes("skillgroup");
					// Run through each of the Skill Groups since there may be more than one affected.
					foreach (XmlNode objXmlSkill in objXmlSkillList)
					{
						bool blnAddToRating = false;
						if (objXmlSkill["applytorating"] != null)
						{
							if (objXmlSkill["applytorating"].InnerText == "yes")
								blnAddToRating = true;
						}
						if (objXmlSkill["exclude"] != null)
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillGroup, strUnique, blnConditional, ValueToInt(objXmlSkill["bonus"].InnerXml, intRating), 1, 0, 0, 0, 0, objXmlSkill["exclude"].InnerText, blnAddToRating);
						else
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillGroup, strUnique, blnConditional, ValueToInt(objXmlSkill["bonus"].InnerXml, intRating), 1, 0, 0, 0, 0, "", blnAddToRating);
					}
				}

				// The Improvement adjust Skills with the given Attribute.
				if (NodeExists(nodBonus, "skillattribute"))
				{
					XmlNodeList objXmlSkillList = nodBonus.SelectNodes("skillattribute");
					// Run through each of the Skill Attributes since there may be more than one affected.
					foreach (XmlNode objXmlSkill in objXmlSkillList)
					{
						string strUseUnique = strUnique;
						if (objXmlSkill["name"].Attributes["precedence"] != null)
							strUseUnique = "precedence" + objXmlSkill["name"].Attributes["precedence"].InnerText;

						bool blnAddToRating = false;
						if (objXmlSkill["applytorating"] != null)
						{
							if (objXmlSkill["applytorating"].InnerText == "yes")
								blnAddToRating = true;
						}
						if (objXmlSkill["exclude"] != null)
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillAttribute, strUseUnique, blnConditional, ValueToInt(objXmlSkill["value"].InnerXml, intRating), 1, 0, 0, 0, 0, objXmlSkill["exclude"].InnerText, blnAddToRating);
						else
                            CreateImprovement(objXmlSkill["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SkillAttribute, strUseUnique, blnConditional, ValueToInt(objXmlSkill["value"].InnerXml, intRating), 1, 0, 0, 0, 0, "", blnAddToRating);
					}
				}

				// The Improvement comes from Enhanced Articulation (improves Physical Active Skills linked to a Physical Attribute).
				if (NodeExists(nodBonus, "skillarticulation"))
				{
					XmlNode objXmlSkill = nodBonus.SelectSingleNode("skillarticulation");
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.EnhancedArticulation, strUnique, blnConditional, ValueToInt(objXmlSkill["bonus"].InnerText, intRating));
				}

				// Check for Armor modifiers.
				if (NodeExists(nodBonus, "armorvalue"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.ArmorValue, strUnique, blnConditional, ValueToInt(nodBonus["armorvalue"].InnerText, intRating));
				}

				// Check for Reach modifiers.
				if (NodeExists(nodBonus, "reach"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Reach, strUnique, blnConditional, ValueToInt(nodBonus["reach"].InnerText, intRating));
				}

				// Check for Unarmed Damage Value modifiers.
				if (NodeExists(nodBonus, "unarmeddv"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.UnarmedDV, strUnique, blnConditional, ValueToInt(nodBonus["unarmeddv"].InnerText, intRating));
				}

				// Check for Unarmed Damage Value Physical.
				if (NodeExists(nodBonus, "unarmeddvphysical"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.UnarmedDVPhysical, "", blnConditional);
				}

				// Check for Unarmed Armor Penetration.
				if (NodeExists(nodBonus, "unarmedap"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.UnarmedAP, strUnique, blnConditional, ValueToInt(nodBonus["unarmedap"].InnerText, intRating));
				}

				// Check for Initiative modifiers.
				if (NodeExists(nodBonus, "initiative"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Initiative, strUnique, blnConditional, ValueToInt(nodBonus["initiative"].InnerText, intRating));
				}

				// Check for Initiative Dice modifiers.
				if (NodeExists(nodBonus, "initiativedice"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.InitiativeDice, strUnique, blnConditional, ValueToInt(nodBonus["initiativedice"].InnerText, intRating));
				}

				// Check for Matrix Initiative modifiers.
				if (NodeExists(nodBonus, "matrixinitiative"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.MatrixInitiative, strUnique, blnConditional, ValueToInt(nodBonus["matrixinitiative"].InnerText, intRating));
				}

				// Check for Matrix Initiative Pass modifiers.
				if (NodeExists(nodBonus, "matrixinitiativepass"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.MatrixInitiativePass, "matrixinitiativepass", blnConditional, ValueToInt(nodBonus["matrixinitiativepass"].InnerText, intRating));
				}

				// Check for Matrix Initiative Pass modifiers.
				if (NodeExists(nodBonus, "matrixinitiativepassadd"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.MatrixInitiativePass, strUnique, blnConditional, ValueToInt(nodBonus["matrixinitiativepassadd"].InnerText, intRating));
				}

				// Check for Lifestyle cost modifiers.
				if (NodeExists(nodBonus, "lifestylecost"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LifestyleCost, strUnique, blnConditional, ValueToInt(nodBonus["lifestylecost"].InnerText, intRating));
				}

				// Check for basic Lifestyle cost modifiers.
				if (NodeExists(nodBonus, "basiclifestylecost"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.BasicLifestyleCost, strUnique, blnConditional, ValueToInt(nodBonus["basiclifestylecost"].InnerText, intRating));
				}

				// Check for Genetech Cost modifiers.
				if (NodeExists(nodBonus, "genetechcostmultiplier"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.GenetechCostMultiplier, strUnique, blnConditional, ValueToInt(nodBonus["genetechcostmultiplier"].InnerText, intRating));
				}

				// Check for Genetech: Transgenics Cost modifiers.
				if (NodeExists(nodBonus, "transgenicsgenetechcost"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.TransgenicsBiowareCost, strUnique, blnConditional, ValueToInt(nodBonus["transgenicsgenetechcost"].InnerText, intRating));
				}

				// Check for Basic Bioware Essence Cost modifiers.
				if (NodeExists(nodBonus, "basicbiowareessmultiplier"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.BasicBiowareEssCost, strUnique, blnConditional, ValueToInt(nodBonus["basicbiowareessmultiplier"].InnerText, intRating));
				}

				// Check for Bioware Essence Cost modifiers.
				if (NodeExists(nodBonus, "biowareessmultiplier"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.BiowareEssCost, strUnique, blnConditional, ValueToInt(nodBonus["biowareessmultiplier"].InnerText, intRating));
				}

				// Check for Cybeware Essence Cost modifiers.
				if (NodeExists(nodBonus, "cyberwareessmultiplier"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.CyberwareEssCost, strUnique, blnConditional, ValueToInt(nodBonus["cyberwareessmultiplier"].InnerText, intRating));
				}

				// Check for Uneducated modifiers.
				if (NodeExists(nodBonus, "uneducated"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Uneducated, strUnique, blnConditional);
					_objCharacter.Uneducated = true;
				}

				// Check for Uncouth modifiers.
				if (NodeExists(nodBonus, "uncouth"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Uncouth, strUnique, blnConditional);
					_objCharacter.Uncouth = true;
				}

				// Check for Infirm modifiers.
				if (NodeExists(nodBonus, "infirm"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Infirm, strUnique, blnConditional);
					_objCharacter.Infirm = true;
				}

				// Check for Adept Linguistics.
				if (NodeExists(nodBonus, "adeptlinguistics"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.AdeptLinguistics, strUnique, blnConditional, 1);
				}

				// Check for Weapon Category DV modifiers.
				if (NodeExists(nodBonus, "weaponcategorydv"))
				{
					XmlNodeList objXmlCategoryList = nodBonus.SelectNodes("weaponcategorydv");
					// Run through each of the Skill Groups since there may be more than one affected.
					foreach (XmlNode objXmlCategory in objXmlCategoryList)
                        CreateImprovement(objXmlCategory["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.WeaponCategoryDV, strUnique, blnConditional, ValueToInt(objXmlCategory["bonus"].InnerXml, intRating));
				}

				// Check for Mentor Spirit bonuses.
				if (NodeExists(nodBonus, "selectmentorspirit"))
				{
					frmSelectMentorSpirit frmPickMentorSpirit = new frmSelectMentorSpirit(_objCharacter);
					frmPickMentorSpirit.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickMentorSpirit.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return blnSuccess;
					}

					_strSelectedValue = frmPickMentorSpirit.SelectedMentor;
					string strHoldValue = _strSelectedValue;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

					if (frmPickMentorSpirit.BonusNode != null)
					{
						blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.BonusNode, blnConcatSelectedValue, intRating, strFriendlyName);
						if (!blnSuccess)
						{
							Rollback();
							_strForcedValue = "";
							_strLimitSelection = "";
							return blnSuccess;
						}
					}

                    if (frmPickMentorSpirit.BonusNode2 != null)
                    {
                        blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.BonusNode2, blnConcatSelectedValue, intRating, strFriendlyName);
                        if (!blnSuccess)
                        {
                            Rollback();
                            _strForcedValue = "";
                            _strLimitSelection = "";
                            return blnSuccess;
                        }
                    }

					if (frmPickMentorSpirit.Choice1BonusNode != null)
					{
						string strForce = _strForcedValue;
						_strForcedValue = frmPickMentorSpirit.Choice1;
						blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.Choice1BonusNode, blnConcatSelectedValue, intRating, strFriendlyName);
						if (!blnSuccess)
						{
							Rollback();
							_strForcedValue = "";
							_strLimitSelection = "";
							return blnSuccess;
						}
						_strForcedValue = strForce;
						_objCharacter.Improvements.Last().Notes = frmPickMentorSpirit.Choice1;
					}

					if (frmPickMentorSpirit.Choice2BonusNode != null)
					{
						string strForce = _strForcedValue;
						_strForcedValue = frmPickMentorSpirit.Choice2;
						blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.Choice2BonusNode, blnConcatSelectedValue, intRating, strFriendlyName);
						if (!blnSuccess)
						{
							Rollback();
							_strForcedValue = "";
							_strLimitSelection = "";
							return blnSuccess;
						}
						_strForcedValue = strForce;
						_objCharacter.Improvements.Last().Notes = frmPickMentorSpirit.Choice2;
					}

					_strSelectedValue = strHoldValue;
				}

				// Check for Paragon bonuses.
				if (NodeExists(nodBonus, "selectparagon"))
				{
					frmSelectMentorSpirit frmPickMentorSpirit = new frmSelectMentorSpirit(_objCharacter);
					frmPickMentorSpirit.XmlFile = "paragons.xml";
					frmPickMentorSpirit.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickMentorSpirit.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return blnSuccess;
					}

					_strSelectedValue = frmPickMentorSpirit.SelectedMentor;
					string strHoldValue = _strSelectedValue;
					if (blnConcatSelectedValue)
						strSourceName += " (" + _strSelectedValue + ")";

					if (frmPickMentorSpirit.BonusNode != null)
					{
						blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.BonusNode, blnConcatSelectedValue, intRating, strFriendlyName);
						if (!blnSuccess)
						{
							Rollback();
							_strForcedValue = "";
							_strLimitSelection = "";
							return blnSuccess;
						}
					}

					if (frmPickMentorSpirit.Choice1BonusNode != null)
					{
						string strForce = _strForcedValue;
						_strForcedValue = frmPickMentorSpirit.Choice1;
						blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.Choice1BonusNode, blnConcatSelectedValue, intRating, strFriendlyName);
						if (!blnSuccess)
						{
							Rollback();
							_strForcedValue = "";
							_strLimitSelection = "";
							return blnSuccess;
						}
						_strForcedValue = strForce;
						_objCharacter.Improvements.Last().Notes = frmPickMentorSpirit.Choice1;
					}

					if (frmPickMentorSpirit.Choice2BonusNode != null)
					{
						string strForce = _strForcedValue;
						_strForcedValue = frmPickMentorSpirit.Choice2;
						blnSuccess = CreateImprovements(objImprovementSource, strSourceName, frmPickMentorSpirit.Choice2BonusNode, blnConcatSelectedValue, intRating, strFriendlyName);
						if (!blnSuccess)
						{
							Rollback();
							_strForcedValue = "";
							_strLimitSelection = "";
							return blnSuccess;
						}
						_strForcedValue = strForce;
						_objCharacter.Improvements.Last().Notes = frmPickMentorSpirit.Choice2;
					}

					_strSelectedValue = strHoldValue;
				}

				// Check for Smartlink bonus.
				if (NodeExists(nodBonus, "smartlink"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Smartlink, "smartlink", blnConditional);
				}

				// Check for Adapsin bonus.
				if (NodeExists(nodBonus, "adapsin"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Adapsin, "adapsin", blnConditional);
				}

				// Check for SoftWeave bonus.
				if (NodeExists(nodBonus, "softweave"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.SoftWeave, "softweave", blnConditional);
				}

				// Check for Sensitive System.
				if (NodeExists(nodBonus, "sensitivesystem"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.SensitiveSystem, "sensitivesystem", blnConditional);
                    _objCharacter.SensitiveSystem = true;
				}

				// Check for Movement Percent.
				if (NodeExists(nodBonus, "movementpercent"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.MovementPercent, "", blnConditional, ValueToInt(nodBonus["movementpercent"].InnerText, intRating));
				}

				// Check for Swim Percent.
				if (NodeExists(nodBonus, "swimpercent"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.SwimPercent, "", blnConditional, ValueToInt(nodBonus["swimpercent"].InnerText, intRating));
				}

				// Check for Fly Percent.
				if (NodeExists(nodBonus, "flypercent"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.FlyPercent, "", blnConditional, ValueToInt(nodBonus["flypercent"].InnerText, intRating));
				}

				// Check for Fly Speed.
				if (NodeExists(nodBonus, "flyspeed"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.FlySpeed, "", blnConditional, ValueToInt(nodBonus["flyspeed"].InnerText, intRating));
				}

				// Check for free Positive Qualities.
				if (NodeExists(nodBonus, "freepositivequalities"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.FreePositiveQualities, "", blnConditional, ValueToInt(nodBonus["freepositivequalities"].InnerText, intRating));
				}

				// Check for free Negative Qualities.
				if (NodeExists(nodBonus, "freenegativequalities"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.FreeNegativeQualities, "", blnConditional, ValueToInt(nodBonus["freenegativequalities"].InnerText, intRating));
				}

				// Check for Select Side.
				if (NodeExists(nodBonus, "selectside"))
				{
					frmSelectSide frmPickSide = new frmSelectSide();
					frmPickSide.Description = LanguageManager.Instance.GetString("Label_SelectSide").Replace("{0}", strFriendlyName);
					if (_strForcedValue != "")
						frmPickSide.ForceValue(_strForcedValue);
					else
						frmPickSide.ShowDialog();

					// Make sure the dialogue window was not canceled.
					if (frmPickSide.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return blnSuccess;
					}

					_strSelectedValue = frmPickSide.SelectedSide;
				}

				// Check for Free Spirit Power Points.
				if (NodeExists(nodBonus, "freespiritpowerpoints"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.FreeSpiritPowerPoints, "", blnConditional, ValueToInt(nodBonus["freespiritpowerpoints"].InnerText, intRating));
				}

				// Check for Adept Power Points.
				if (NodeExists(nodBonus, "adeptpowerpoints"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.AdeptPowerPoints, "", blnConditional, ValueToInt(nodBonus["adeptpowerpoints"].InnerText, intRating));
				}

				// Check for Armor Encumbrance Penalty.
				if (NodeExists(nodBonus, "armorencumbrancepenalty"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.ArmorEncumbrancePenalty, "", blnConditional, ValueToInt(nodBonus["armorencumbrancepenalty"].InnerText, intRating));
				}

				// Check for Initiation.
				if (NodeExists(nodBonus, "initiation"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Initiation, "", blnConditional, ValueToInt(nodBonus["initiation"].InnerText, intRating));
					_objCharacter.InitiateGrade += ValueToInt(nodBonus["initiation"].InnerText, intRating);
				}

				// Check for Submersion.
				if (NodeExists(nodBonus, "submersion"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Submersion, "", blnConditional, ValueToInt(nodBonus["submersion"].InnerText, intRating));
					_objCharacter.SubmersionGrade += ValueToInt(nodBonus["submersion"].InnerText, intRating);
				}

				// Check for Skillwires.
				if (NodeExists(nodBonus, "skillwire"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Skillwire, "", blnConditional, ValueToInt(nodBonus["skillwire"].InnerText, intRating));
				}

				// Check for Damage Resistance.
				if (NodeExists(nodBonus, "damageresistance"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.DamageResistance, "damageresistance", blnConditional, ValueToInt(nodBonus["damageresistance"].InnerText, intRating));
				}

				// Check for Restricted Item Count.
				if (NodeExists(nodBonus, "restricteditemcount"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.RestrictedItemCount, "", blnConditional, ValueToInt(nodBonus["restricteditemcount"].InnerText, intRating));
				}

				// Check for Judge Intentions.
				if (NodeExists(nodBonus, "judgeintentions"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.JudgeIntentions, "", blnConditional, ValueToInt(nodBonus["judgeintentions"].InnerText, intRating));
				}

                // Check for Select Inherent Limit.
                if (NodeExists(nodBonus, "selectinherentlimit"))
                {
                    frmSelectItem frmPickItem = new frmSelectItem();
                    frmPickItem.ManualMode = "InherentLimits";
                    frmPickItem.Description = LanguageManager.Instance.GetString("String_SelectInherentLimit");
                    frmPickItem.ShowDialog();

                    if (frmPickItem.DialogResult == DialogResult.Cancel)
                    {
                        Rollback();
                        blnSuccess = false;
                        _strForcedValue = "";
                        _strLimitSelection = "";
                        return blnSuccess;
                    }

                    _strSelectedValue = frmPickItem.SelectedItem;

                    Improvement.ImprovementType objImprovementType = new Improvement.ImprovementType();
                    switch (frmPickItem.SelectedItem)
                    {
                        case "mentallimit":
                            objImprovementType = Improvement.ImprovementType.MentalLimit;
                            break;
                        case "physicallimit":
                            objImprovementType = Improvement.ImprovementType.PhysicalLimit;
                            break;
                        case "sociallimit":
                            objImprovementType = Improvement.ImprovementType.SocialLimit;
                            break;
                    }
                    CreateImprovement(frmPickItem.SelectedItem, objImprovementSource, strSourceName, Improvement.ImprovementType.AddSprite, "", blnConditional);
                    CreateImprovement("", objImprovementSource, strSourceName, objImprovementType, "", blnConditional, ValueToInt(nodBonus["selectinherentlimit"]["value"].InnerText, intRating));
                }
                
                // Check for Mental Limit.
                if (NodeExists(nodBonus, "mentallimit"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.MentalLimit, "", blnConditional, ValueToInt(nodBonus["mentallimit"].InnerText, intRating));
                }

                // Check for Physical Limit.
                if (NodeExists(nodBonus, "physicallimit"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.PhysicalLimit, "", blnConditional, ValueToInt(nodBonus["physicallimit"].InnerText, intRating));
                }

                // Check for Social Limit.
                if (NodeExists(nodBonus, "sociallimit"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.SocialLimit, "", blnConditional, ValueToInt(nodBonus["sociallimit"].InnerText, intRating));
                }

                // Check for Surprise.
                if (NodeExists(nodBonus, "surprise"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Surprise, "", blnConditional, ValueToInt(nodBonus["surprise"].InnerText, intRating));
                }

				// Check for Composure.
				if (NodeExists(nodBonus, "composure"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Composure, "", blnConditional, ValueToInt(nodBonus["composure"].InnerText, intRating));
				}

				// Check for Lift and Carry.
				if (NodeExists(nodBonus, "liftandcarry"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.LiftAndCarry, "", blnConditional, ValueToInt(nodBonus["liftandcarry"].InnerText, intRating));
				}

				// Check for Memory.
				if (NodeExists(nodBonus, "memory"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Memory, "", blnConditional, ValueToInt(nodBonus["memory"].InnerText, intRating));
				}

				// Check for Concealability.
				if (NodeExists(nodBonus, "concealability"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Concealability, "", blnConditional, ValueToInt(nodBonus["concealability"].InnerText, intRating));
				}

				// Check for Drain Resistance.
				if (NodeExists(nodBonus, "drainresist"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.DrainResistance, "", blnConditional, ValueToInt(nodBonus["drainresist"].InnerText, intRating));
				}

				// Check for Fading Resistance.
				if (NodeExists(nodBonus, "fadingresist"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.FadingResistance, "", blnConditional, ValueToInt(nodBonus["fadingresist"].InnerText, intRating));
				}

				// Check for Notoriety.
				if (NodeExists(nodBonus, "notoriety"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Notoriety, "", blnConditional, ValueToInt(nodBonus["notoriety"].InnerText, intRating));
				}

				// Check for Complex Form Limit.
				if (NodeExists(nodBonus, "complexformlimit"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.ComplexFormLimit, "", blnConditional, ValueToInt(nodBonus["complexformlimit"].InnerText, intRating));
				}

				// Check for Spell Limit.
				if (NodeExists(nodBonus, "spelllimit"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.SpellLimit, "", blnConditional, ValueToInt(nodBonus["spelllimit"].InnerText, intRating));
				}

				// Check for Spell Category bonuses.
				if (NodeExists(nodBonus, "spellcategory"))
				{
					XmlNodeList objXmlCategoryList = nodBonus.SelectNodes("spellcategory");

					// Run through each of the Spell Categories since there may be more than one affected.
					foreach (XmlNode nodSpellCategory in objXmlCategoryList)
					{
						string strUseUnique = strUnique;
						if (nodSpellCategory["name"].Attributes["precedence"] != null)
							strUseUnique = "precedence" + nodSpellCategory["name"].Attributes["precedence"].InnerText;

                        CreateImprovement(nodSpellCategory["name"].InnerText, objImprovementSource, strSourceName, Improvement.ImprovementType.SpellCategory, strUseUnique, blnConditional, ValueToInt(nodSpellCategory["val"].InnerText, intRating));
					}
				}

				// Check for Throwing Range bonuses.
				if (NodeExists(nodBonus, "throwrange"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.ThrowRange, strUnique, blnConditional, ValueToInt(nodBonus["throwrange"].InnerText, intRating));
				}

				// Check for Throwing STR bonuses.
				if (NodeExists(nodBonus, "throwstr"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.ThrowSTR, strUnique, blnConditional, ValueToInt(nodBonus["throwstr"].InnerText, intRating));
				}

                // Check for Accuracy bonuses.
                if (NodeExists(nodBonus, "accuracy"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.Accuracy, strUnique, blnConditional, ValueToInt(nodBonus["accuracy"].InnerText, intRating));
                }

				// Check for Skillsoft access.
				if (NodeExists(nodBonus, "skillsoftaccess"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.SkillsoftAccess, "", blnConditional);
				}

                if (NodeExists(nodBonus, "knowsoftaccess"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.KnowsoftAccess, "", blnConditional);
                }

				// Check for Quickening Metamagic.
				if (NodeExists(nodBonus, "quickeningmetamagic"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.QuickeningMetamagic, "", blnConditional);
				}

				// Check for ignore Stun CM Penalty.
				if (NodeExists(nodBonus, "ignorecmpenaltystun"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.IgnoreCMPenaltyStun, "", blnConditional);
				}

				// Check for ignore Physical CM Penalty.
				if (NodeExists(nodBonus, "ignorecmpenaltyphysical"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.IgnoreCMPenaltyPhysical, "", blnConditional);
				}

                // Check for Maximum Essence which will permanently modify the character's Maximum Essence value.
                if (NodeExists(nodBonus, "essencemax"))
                {
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.EssenceMax, "", blnConditional, ValueToInt(nodBonus["essencemax"].InnerText, intRating));
                }

				// Check for Select Sprite.
				if (NodeExists(nodBonus, "selectsprite"))
				{
					XmlDocument objXmlDocument = XmlManager.Instance.Load("critters.xml");
					XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/chummer/metatypes/metatype[contains(category, \"Sprites\")]");
					List<ListItem> lstCritters = new List<ListItem>();
					foreach (XmlNode objXmlNode in objXmlNodeList)
					{
						ListItem objItem = new ListItem();
						if (objXmlNode["translate"] != null)
							objItem.Name = objXmlNode["translate"].InnerText;
						else
							objItem.Name = objXmlNode["name"].InnerText;
						objItem.Value = objItem.Name;
						lstCritters.Add(objItem);
					}

					frmSelectItem frmPickItem = new frmSelectItem();
					frmPickItem.GeneralItems = lstCritters;
					frmPickItem.ShowDialog();

					if (frmPickItem.DialogResult == DialogResult.Cancel)
					{
						Rollback();
						blnSuccess = false;
						_strForcedValue = "";
						_strLimitSelection = "";
						return blnSuccess;
					}

					_strSelectedValue = frmPickItem.SelectedItem;

                    CreateImprovement(frmPickItem.SelectedItem, objImprovementSource, strSourceName, Improvement.ImprovementType.AddSprite, "", blnConditional);
				}

				// Check for Black Market Discount.
				if (NodeExists(nodBonus, "blackmarketdiscount"))
				{
                    CreateImprovement("", objImprovementSource, strSourceName, Improvement.ImprovementType.BlackMarketDiscount, strUnique, blnConditional);
					_objCharacter.BlackMarket = true;
				}

				// Select Weapon (custom entry for things like Spare Clip).
				if (NodeExists(nodBonus, "selectweapon"))
				{
					string strSelectedValue = "";
					if (_strForcedValue != "")
						_strLimitSelection = _strForcedValue;

					if (_objCharacter == null)
					{
						// If the character is null (this is a Vehicle), the user must enter their own string.
						// Display the Select Item window and record the value that was entered.
						frmSelectText frmPickText = new frmSelectText();
						frmPickText.Description = LanguageManager.Instance.GetString("String_Improvement_SelectText").Replace("{0}", strFriendlyName);

						if (_strLimitSelection != "")
						{
							frmPickText.SelectedValue = _strLimitSelection;
							frmPickText.Opacity = 0;
						}

						frmPickText.ShowDialog();

						// Make sure the dialogue window was not canceled.
						if (frmPickText.DialogResult == DialogResult.Cancel)
						{
							Rollback();
							blnSuccess = false;
							_strForcedValue = "";
							_strLimitSelection = "";
							return false;
						}

						_strSelectedValue = frmPickText.SelectedValue;
						if (blnConcatSelectedValue)
							strSourceName += " (" + _strSelectedValue + ")";

						strSelectedValue = frmPickText.SelectedValue;
					}
					else
					{
						List<ListItem> lstWeapons = new List<ListItem>();
						foreach (Weapon objWeapon in _objCharacter.Weapons)
						{
							ListItem objItem = new ListItem();
							objItem.Value = objWeapon.Name;
							objItem.Name = objWeapon.DisplayName;
							lstWeapons.Add(objItem);
						}

						frmSelectItem frmPickItem = new frmSelectItem();
						frmPickItem.Description = LanguageManager.Instance.GetString("String_Improvement_SelectText").Replace("{0}", strFriendlyName);
						frmPickItem.GeneralItems = lstWeapons;

						if (_strLimitSelection != "")
						{
							frmPickItem.ForceItem = _strLimitSelection;
							frmPickItem.Opacity = 0;
						}

						frmPickItem.ShowDialog();

						// Make sure the dialogue window was not canceled.
						if (frmPickItem.DialogResult == DialogResult.Cancel)
						{
							Rollback();
							blnSuccess = false;
							_strForcedValue = "";
							_strLimitSelection = "";
							return false;
						}

						_strSelectedValue = frmPickItem.SelectedItem;
						if (blnConcatSelectedValue)
							strSourceName += " (" + _strSelectedValue + ")";

						strSelectedValue = frmPickItem.SelectedItem;
					}

					// Create the Improvement.
                    CreateImprovement(strSelectedValue, objImprovementSource, strSourceName, Improvement.ImprovementType.Text, strUnique, blnConditional);
				}
			}

			// If we've made it this far, everything went OK, so commit the Improvements.
			Commit();
			// Clear the Forced Value and Limit Selection strings once we're done to prevent these from forcing their values on other Improvements.
			_strForcedValue = "";
			_strLimitSelection = "";
			return blnSuccess;
		}

		/// <summary>
		/// Remove all of the Improvements for an XML Node.
		/// </summary>
		/// <param name="objImprovementSource">Type of object that granted these Improvements.</param>
		/// <param name="strSourceName">Name of the item that granted these Improvements.</param>
		public void RemoveImprovements(Improvement.ImprovementSource objImprovementSource, string strSourceName)
		{
			// If there is no character object, don't try to remove any Improvements.
			if (_objCharacter == null)
				return;

			// A List of Improvements to hold all of the items that will eventually be deleted.
			List<Improvement> objImprovementList = new List<Improvement>();
			foreach (Improvement objImprovement in _objCharacter.Improvements)
			{
				if (objImprovement.ImproveSource == objImprovementSource && objImprovement.SourceName == strSourceName)
					objImprovementList.Add(objImprovement);
			}

			// Now that we have all of the applicable Improvements, remove them from the character.
			foreach (Improvement objImprovement in objImprovementList)
			{
				// Remove the Improvement.
				_objCharacter.Improvements.Remove(objImprovement);

				// Determine if access to any Special Attributes have been lost.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Attribute && objImprovement.UniqueName == "enableattribute")
				{
					if (objImprovement.ImprovedName == "MAG")
					{
						// See if the character has anything else that is granting them access to MAG.
						bool blnFound = false;
						foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
						{
							// Skip items from the current Improvement source.
							if (objCharacterImprovement.SourceName != objImprovement.SourceName)
							{
								if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.Attribute && objCharacterImprovement.UniqueName == "enableattribute" && objCharacterImprovement.ImprovedName == "MAG")
								{
									blnFound = true;
									break;
								}
							}
						}

						if (!blnFound)
							_objCharacter.MAGEnabled = false;
					}
					else if (objImprovement.ImprovedName == "RES")
					{
						// See if the character has anything else that is granting them access to RES.
						bool blnFound = false;
						foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
						{
							// Skip items from the current Improvement source.
							if (objCharacterImprovement.SourceName != objImprovement.SourceName)
							{
								if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.Attribute && objCharacterImprovement.UniqueName == "enableattribute" && objCharacterImprovement.ImprovedName == "RES")
								{
									blnFound = true;
									break;
								}
							}
						}

						if (!blnFound)
							_objCharacter.RESEnabled = false;
					}
				}

				// Determine if access to any special tabs have been lost.
				if (objImprovement.ImproveType == Improvement.ImprovementType.SpecialTab && objImprovement.UniqueName == "enabletab")
				{
					bool blnFound = false;
					switch (objImprovement.ImprovedName)
					{
						case "Magician":
							// See if the character has anything else that is granting them access to the Magician tab.
							foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
							{
								// Skip items from the current Improvement source.
								if (objCharacterImprovement.SourceName != objImprovement.SourceName)
								{
									if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.SpecialTab && objCharacterImprovement.UniqueName == "enabletab" && objCharacterImprovement.ImprovedName == "Magician")
									{
										blnFound = true;
										break;
									}
								}
							}
							
							if (!blnFound)
								_objCharacter.MagicianEnabled = false;
							break;
						case "Adept":
							// See if the character has anything else that is granting them access to the Adept tab.
							foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
							{
								// Skip items from the current Improvement source.
								if (objCharacterImprovement.SourceName != objImprovement.SourceName)
								{
									if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.SpecialTab && objCharacterImprovement.UniqueName == "enabletab" && objCharacterImprovement.ImprovedName == "Adept")
									{
										blnFound = true;
										break;
									}
								}
							}

							if (!blnFound)
								_objCharacter.AdeptEnabled = false;
							break;
						case "Technomancer":
							// See if the character has anything else that is granting them access to the Technomancer tab.
							foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
							{
								// Skip items from the current Improvement source.
								if (objCharacterImprovement.SourceName != objImprovement.SourceName)
								{
									if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.SpecialTab && objCharacterImprovement.UniqueName == "enabletab" && objCharacterImprovement.ImprovedName == "Technomancer")
									{
										blnFound = true;
										break;
									}
								}
							}

							if (!blnFound)
								_objCharacter.TechnomancerEnabled = false;
							break;
						case "Critter":
							// See if the character has anything else that is granting them access to the Critter tab.
							foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
							{
								// Skip items from the current Improvement source.
								if (objCharacterImprovement.SourceName != objImprovement.SourceName)
								{
									if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.SpecialTab && objCharacterImprovement.UniqueName == "enabletab" && objCharacterImprovement.ImprovedName == "Critter")
									{
										blnFound = true;
										break;
									}
								}
							}

							if (!blnFound)
								_objCharacter.CritterEnabled = false;
							break;
						case "Initiation":
							// See if the character has anything else that is granting them access to the Initiation tab.
							foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
							{
								// Skip items from the current Improvement source.
								if (objCharacterImprovement.SourceName != objImprovement.SourceName)
								{
									if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.SpecialTab && objCharacterImprovement.UniqueName == "enabletab" && objCharacterImprovement.ImprovedName == "Initiation")
									{
										blnFound = true;
										break;
									}
								}
							}

							if (!blnFound)
								_objCharacter.InitiationEnabled = false;
							break;
					}
				}

				// Turn of the Black Market flag if it is being removed.
				if (objImprovement.ImproveType == Improvement.ImprovementType.BlackMarketDiscount)
				{
					bool blnFound = false;
					// See if the character has anything else that is granting them access to Black Market.
					foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
					{
						// Skip items from the current Improvement source.
						if (objCharacterImprovement.SourceName != objImprovement.SourceName)
						{
							if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.BlackMarketDiscount)
							{
								blnFound = true;
								break;
							}
						}
					}

					if (!blnFound)
						_objCharacter.BlackMarket = false;
				}

                // Turn of the Sensitive System flag if it is being removed.
                if (objImprovement.ImproveType == Improvement.ImprovementType.SensitiveSystem)
                {
                    bool blnFound = false;
                    // See if the character has anything else that is granting them access to Sensitive System.
                    foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
                    {
                        // Skip items from the current Improvement source.
                        if (objCharacterImprovement.SourceName != objImprovement.SourceName)
                        {
                            if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.SensitiveSystem)
                            {
                                blnFound = true;
                                break;
                            }
                        }
                    }

                    if (!blnFound)
                        _objCharacter.SensitiveSystem = false;
                }

				// Turn of the Uneducated flag if it is being removed.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Uneducated)
				{
					bool blnFound = false;
					// See if the character has anything else that is granting them access to Uneducated.
					foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
					{
						// Skip items from the current Improvement source.
						if (objCharacterImprovement.SourceName != objImprovement.SourceName)
						{
							if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.Uneducated)
							{
								blnFound = true;
								break;
							}
						}
					}

					if (!blnFound)
						_objCharacter.Uneducated = false;
				}

				// Turn off the Uncouth flag if it is being removed.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Uncouth)
				{
					bool blnFound = false;
					// See if the character has anything else that is granting them access to Uncouth.
					foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
					{
						// Skip items from the current Improvement source.
						if (objCharacterImprovement.SourceName != objImprovement.SourceName)
						{
							if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.Uncouth)
							{
								blnFound = true;
								break;
							}
						}
					}

					if (!blnFound)
						_objCharacter.Uncouth = false;
				}

				// Turn off the Infirm flag if it is being removed.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Infirm)
				{
					bool blnFound = false;
					// See if the character has anything else that is granting them access to Infirm.
					foreach (Improvement objCharacterImprovement in _objCharacter.Improvements)
					{
						// Skip items from the current Improvement source.
						if (objCharacterImprovement.SourceName != objImprovement.SourceName)
						{
							if (objCharacterImprovement.ImproveType == Improvement.ImprovementType.Infirm)
							{
								blnFound = true;
								break;
							}
						}
					}

					if (!blnFound)
						_objCharacter.Infirm = false;
				}

				// If the last instance of Adapsin is being removed, convert all Adapsin Cyberware Grades to their non-Adapsin version.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Adapsin)
				{
					if (!_objCharacter.AdapsinEnabled)
					{
						foreach (Cyberware objCyberware in _objCharacter.Cyberware)
						{
							if (objCyberware.Grade.Adapsin)
							{
								// Determine which GradeList to use for the Cyberware.
								GradeList objGradeList;
								if (objCyberware.SourceType == Improvement.ImprovementSource.Bioware)
									objGradeList = GlobalOptions.BiowareGrades;
								else
									objGradeList = GlobalOptions.CyberwareGrades;

								objCyberware.Grade = objGradeList.GetGrade(objCyberware.Grade.Name.Replace("(Adapsin)", string.Empty).Trim());
							}
						}
					}
				}

				// Decrease the character's Initiation Grade.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Initiation)
					_objCharacter.InitiateGrade -= objImprovement.Value;

				// Decrease the character's Submersion Grade.
				if (objImprovement.ImproveType == Improvement.ImprovementType.Submersion)
					_objCharacter.SubmersionGrade -= objImprovement.Value;
			}
		}

		/// <summary>
		/// Create a new Improvement and add it to the Character.
		/// </summary>
		/// <param name="strImprovedName">Speicific name of the Improved object - typically the name of an Attribute being improved.</param>
		/// <param name="objImprovementSource">Type of object that grants this Improvement.</param>
		/// <param name="strSourceName">Name of the item that grants this Improvement.</param>
		/// <param name="objImprovementType">Type of object the Improvement applies to.</param>
		/// <param name="strUnique">Name of the pool this Improvement should be added to - only the single higest value in the pool will be applied to the character.</param>
		/// <param name="intValue">Set a Value for the Improvement.</param>
		/// <param name="intRating">Set a Rating for the Improvement - typically used for Adept Powers.</param>
		/// <param name="intMinimum">Improve the Minimum for an Attribute by the given amount.</param>
		/// <param name="intMaximum">Improve the Maximum for an Attribute by the given amount.</param>
		/// <param name="intAugmented">Improve the Augmented value for an Attribute by the given amount.</param>
		/// <param name="intAugmentedMaximum">Improve the Augmented Maximum value for an Attribute by the given amount.</param>
		/// <param name="strExclude">A list of child items that should not receive the Improvement's benefit (typically for Skill Groups).</param>
		/// <param name="blnAddToRating">Whether or not the bonus applies to a Skill's Rating instead of the dice pool in general.</param>
		public void CreateImprovement(string strImprovedName, Improvement.ImprovementSource objImprovementSource, string strSourceName, Improvement.ImprovementType objImprovementType, string strUnique,
			bool blnConditional, int intValue = 0, int intRating = 1, int intMinimum = 0, int intMaximum = 0, int intAugmented = 0, int intAugmentedMaximum = 0, string strExclude = "", bool blnAddToRating = false)
		{
			// Record the improvement.
			Improvement objImprovement = new Improvement();
			objImprovement.ImprovedName = strImprovedName;
			objImprovement.ImproveSource = objImprovementSource;
			objImprovement.SourceName = strSourceName;
			objImprovement.ImproveType = objImprovementType;
			objImprovement.UniqueName = strUnique;
			objImprovement.Value = intValue;
			objImprovement.Rating = intRating;
			objImprovement.Minimum = intMinimum;
			objImprovement.Maximum = intMaximum;
			objImprovement.Augmented = intAugmented;
			objImprovement.AugmentedMaximum = intAugmentedMaximum;
			objImprovement.Exclude = strExclude;
			objImprovement.AddToRating = blnAddToRating;
            if (blnConditional)
            {
                // Attach the conditional Improvement to a Custom Group and mark it as being disabled by default.
                string strCustomName = strImprovedName;
                if (intValue != 0)
                    strCustomName += " " + LanguageManager.Instance.GetString("Label_CreateImprovementValue") + " " + intValue.ToString();
                if (intMinimum != 0)
                    strCustomName += " " + LanguageManager.Instance.GetString("Label_CreateImprovementMinimum") + " " + intMinimum.ToString();
                if (intMaximum != 0)
                    strCustomName += " " + LanguageManager.Instance.GetString("Label_CreateImprovementMaximum") + " " + intMaximum.ToString();
                if (intAugmented != 0)
                    strCustomName += " " + LanguageManager.Instance.GetString("Label_CreateImprovementAugmented") + " " + intAugmented.ToString();
                
                objImprovement.CustomName = strCustomName;
                objImprovement.CustomGroup = strSourceName;
                objImprovement.Enabled = false;
            }

			// Do not attempt to add the Improvements if the Character is null (as a result of Cyberware being added to a VehicleMod).
			if (_objCharacter != null)
			{
				// Add the Improvement to the list.
				_objCharacter.Improvements.Add(objImprovement);

				// Add the Improvement to the Transaction List.
				_lstTransaction.Add(objImprovement);
			}
		}

		/// <summary>
		/// Clear all of the Improvements from the Transaction List.
		/// </summary>
		public void Commit()
		{
			// Clear all of the Improvements from the Transaction List.
			_lstTransaction.Clear();
		}

		/// <summary>
		/// Rollback all of the Improvements from the Transaction List.
		/// </summary>
		private void Rollback()
		{
			// Remove all of the Improvements that were added.
			foreach (Improvement objImprovement in _lstTransaction)
				RemoveImprovements(objImprovement.ImproveSource, objImprovement.SourceName);

			_lstTransaction.Clear();
		}
		#endregion
	}
}