using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

// MAGEnabledChanged Event Handler
public delegate void MAGEnabledChangedHandler(Object sender);
// RESEnabledChanged Event Handler
public delegate void RESEnabledChangedHandler(Object sender);
// AdeptTabEnabledChanged Event Handler
public delegate void AdeptTabEnabledChangedHandler(Object sender);
// MagicianTabEnabledChanged Event Handler
public delegate void MagicianTabEnabledChangedHandler(Object sender);
// TechnomancerTabEnabledChanged Event Handler
public delegate void TechnomancerTabEnabledChangedHandler(Object sender);
// InitiationTabEnabledChanged Event Handler
public delegate void InitiationTabEnabledChangedHandler(Object sender);
// CritterTabEnabledChanged Event Handler
public delegate void CritterTabEnabledChangedHandler(Object sender);
// SensitiveSystemChanged Event Handler
public delegate void SensitiveSystemChangedHandler(Object sender);
// UneducatedChanged Event Handler
public delegate void UneducatedChangedHandler(Object sender);
// UncouthChanged Event Handler
public delegate void UncouthChangedHandler(Object sender);
// InformChanged Event Handler
public delegate void InfirmChangedHandler(Object sender);
// CharacterNameChanged Event Handler
public delegate void CharacterNameChangedHandler(Object sender);
// BlackMarketEnabledChanged Event Handler
public delegate void BlackMarketEnabledChangedHandler(Object sender);

namespace Chummer
{
	public enum CharacterBuildMethod
	{
        Priority = 0,
		BP = 1,
		Karma = 2,
	}

	/// <summary>
	/// Class that holds all of the information that makes up a complete Character.
	/// </summary>
	public class Character
	{
		private readonly ImprovementManager _objImprovementManager;
		private readonly CharacterOptions _objOptions = new CharacterOptions();

		private string _strFileName = "";
		private string _strSettingsFileName = "default.xml";
		private bool _blnIgnoreRules = false;
		private int _intKarma = 0;
		private int _intTotalKarma = 0;
		private int _intStreetCred = 0;
		private int _intNotoriety = 0;
		private int _intPublicAwareness = 0;
		private int _intBurntStreetCred = 0;
		private int _intNuyen = 0;
		private int _intMaxAvail = 12;

		// General character info.
		private string _strName = "";
		private string _strMugshot = "";
		private string _strSex = "";
		private string _strAge = "";
		private string _strEyes = "";
		private string _strHeight = "";
		private string _strWeight = "";
		private string _strSkin = "";
		private string _strHair = "";
		private string _strDescription = "";
		private string _strBackground = "";
		private string _strConcept = "";
		private string _strNotes = "";
		private string _strAlias = "";
		private string _strPlayerName = "";
		private string _strGameNotes = "";

        private string _strAppVersionFirst = "";
        private string _strAppVersionCareer = "";

		// If true, the Character creation has been finalized and is maintained through Karma.
		private bool _blnCreated = false;

		// Build Points
        private int _intAttributePoints = 0;
        private int _intSpecialAttributePoints = 0;
		private int _intBuildPoints = 400;
		private int _intKnowledgeSkillPoints = 0;
		private decimal _decNuyenMaximumBP = 50m;
		private decimal _decNuyenBP = 0m;
		private int _intBuildKarma = 0;
		private CharacterBuildMethod _objBuildMethod = CharacterBuildMethod.BP;

		// Metatype Information.
		private string _strMetatype = "";
		private string _strMetavariant = "";
		private string _strMetatypeCategory = "";
        private Guid _guiMetatype = Guid.Empty;
        private int _intWalk = 0;
        private int _intRun = 0;
        private int _intSprint = 0;
		private int _intMetatypeBP = 0;
		private int _intMutantCritterBaseSkills = 0;

		// Special Tab Flags.
		private bool _blnAdeptEnabled = false;
		private bool _blnMagicianEnabled = false;
		private bool _blnTechnomancerEnabled = false;
		private bool _blnInitiationEnabled = false;
		private bool _blnCritterEnabled = false;
        private bool _blnSensitiveSystem = false;
		private bool _blnUneducated = false;
		private bool _blnUncouth = false;
		private bool _blnInfirm = false;
		private bool _blnIsCritter = false;
		private bool _blnPossessed = false;
		private bool _blnBlackMarket = false;
		
		// Attributes.
		private Attribute _attBOD = new Attribute("BOD");
		private Attribute _attAGI = new Attribute("AGI");
		private Attribute _attREA = new Attribute("REA");
		private Attribute _attSTR = new Attribute("STR");
		private Attribute _attCHA = new Attribute("CHA");
		private Attribute _attINT = new Attribute("INT");
		private Attribute _attLOG = new Attribute("LOG");
		private Attribute _attWIL = new Attribute("WIL");
		private Attribute _attINI = new Attribute("INI");
		private Attribute _attEDG = new Attribute("EDG");
		private Attribute _attMAG = new Attribute("MAG");
		private Attribute _attRES = new Attribute("RES");
		private Attribute _attESS = new Attribute("ESS");
		private bool _blnMAGEnabled = false;
		private bool _blnRESEnabled = false;
		private bool _blnGroupMember = false;
		private string _strGroupName = "";
		private string _strGroupNotes = "";
		private int _intInitiateGrade = 0;
		private int _intSubmersionGrade = 0;
		private int _intResponse = 1;
		private int _intSignal = 1;
		private int _intMaxSkillRating = 0;

		// Pseudo-Attributes use for Mystic Adepts.
		private int _intMAGMagician = 0;
		private int _intMAGAdept = 0;

		// Magic Tradition.
        private Guid _guiMagicTradition = new Guid();
		// Technomancer Stream.
        private Guid _guiTechnomancerStream = new Guid();

		// Condition Monitor Progress.
		private int _intPhysicalCMFilled = 0;
		private int _intStunCMFilled = 0;
		
		// Lists.
		private List<Improvement> _lstImprovements = new List<Improvement>();
		private List<Skill> _lstSkills = new List<Skill>();
		private List<SkillGroup> _lstSkillGroups = new List<SkillGroup>();
		private List<Contact> _lstContacts = new List<Contact>();
		private List<Spirit> _lstSpirits = new List<Spirit>();
		private List<Spell> _lstSpells = new List<Spell>();
		private List<Focus> _lstFoci = new List<Focus>();
		private List<StackedFocus> _lstStackedFoci = new List<StackedFocus>();
		private List<Power> _lstPowers = new List<Power>();
		private List<TechProgram> _lstTechPrograms = new List<TechProgram>();
		private List<MartialArt> _lstMartialArts = new List<MartialArt>();
		private List<MartialArtManeuver> _lstMartialArtManeuvers = new List<MartialArtManeuver>();
		private List<Equipment> _lstArmor = new List<Equipment>();
		private List<Equipment> _lstCyberware = new List<Equipment>();
		private List<Equipment> _lstWeapons = new List<Equipment>();
		private List<Quality> _lstQualities = new List<Quality>();
		private List<Lifestyle> _lstLifestyles = new List<Lifestyle>();
		private List<Equipment> _lstGear = new List<Equipment>();
		private List<Equipment> _lstVehicles = new List<Equipment>();
		private List<Metamagic> _lstMetamagics = new List<Metamagic>();
		private List<ExpenseLogEntry> _lstExpenseLog = new List<ExpenseLogEntry>();
		private List<CritterPower> _lstCritterPowers = new List<CritterPower>();
		private List<InitiationGrade> _lstInitiationGrades = new List<InitiationGrade>();
		private List<string> _lstOldQualities = new List<string>();
		private List<string> _lstLocations = new List<string>();
		private List<string> _lstArmorBundles = new List<string>();
		private List<string> _lstWeaponLocations = new List<string>();
		private List<string> _lstImprovementGroups = new List<string>();
		private List<CalendarWeek> _lstCalendar = new List<CalendarWeek>();

		// Events.
		public event MAGEnabledChangedHandler MAGEnabledChanged;
		public event RESEnabledChangedHandler RESEnabledChanged;
		public event AdeptTabEnabledChangedHandler AdeptTabEnabledChanged;
		public event MagicianTabEnabledChangedHandler MagicianTabEnabledChanged;
		public event TechnomancerTabEnabledChangedHandler TechnomancerTabEnabledChanged;
		public event InitiationTabEnabledChangedHandler InitiationTabEnabledChanged;
		public event CritterTabEnabledChangedHandler CritterTabEnabledChanged;
        public event SensitiveSystemChangedHandler SensitiveSystemChanged;
		public event UneducatedChangedHandler UneducatedChanged;
		public event UncouthChangedHandler UncouthChanged;
		public event InfirmChangedHandler InfirmChanged;
		public event CharacterNameChangedHandler CharacterNameChanged;
		public event BlackMarketEnabledChangedHandler BlackMarketEnabledChanged;

		private frmViewer _frmPrintView;
		
		#region Initialization, Save, Load, Print, and Reset Methods
		/// <summary>
		/// Character.
		/// </summary>
		public Character()
		{
			_attBOD._objCharacter = this;
			_attAGI._objCharacter = this;
			_attREA._objCharacter = this;
			_attSTR._objCharacter = this;
			_attCHA._objCharacter = this;
			_attINT._objCharacter = this;
			_attLOG._objCharacter = this;
			_attWIL._objCharacter = this;
			_attINI._objCharacter = this;
			_attEDG._objCharacter = this;
			_attMAG._objCharacter = this;
			_attRES._objCharacter = this;
			_attESS._objCharacter = this;
			_objImprovementManager = new ImprovementManager(this);
		}

		/// <summary>
		/// Save the Character to an XML file.
		/// </summary>
		public void Save()
		{
            // Record application versions so we can determine which version of the app was used when the character was saved for the first time
            // and when they are moved to Career Mode. This may help identify issues with the character if there are known bugs in an older version.
            if (_strAppVersionFirst == string.Empty)
                _strAppVersionFirst = Application.ProductVersion.ToString().Replace("0.0.0.", string.Empty);
            if (_blnCreated && _strAppVersionCareer == string.Empty)
                _strAppVersionCareer = Application.ProductVersion.ToString().Replace("0.0.0.", string.Empty);

			FileStream objStream = new FileStream(_strFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			XmlTextWriter objWriter = new XmlTextWriter(objStream, Encoding.Unicode);
			objWriter.Formatting = Formatting.Indented;
			objWriter.Indentation = 1;
			objWriter.IndentChar = '\t';

			objWriter.WriteStartDocument();

			// <character>
			objWriter.WriteStartElement("character");

            // <appversionfirst />
            objWriter.WriteElementString("appversionfirst", _strAppVersionFirst);
            // <appversioncareer />
            objWriter.WriteElementString("appversioncareer", _strAppVersionCareer);
			// <appversioncurrent />
			objWriter.WriteElementString("appversioncurrent", Application.ProductVersion.ToString().Replace("0.0.0.", string.Empty));
			// <gameedition />
			objWriter.WriteElementString("gameedition", "SR5");

			// <settings />
			objWriter.WriteElementString("settings", _strSettingsFileName);

			// <metatype />
			objWriter.WriteElementString("metatype", _strMetatype);
            // <metatypeid />
            objWriter.WriteElementString("metatypeid", _guiMetatype.ToString());
			// <metatypebp />
			objWriter.WriteElementString("metatypebp", _intMetatypeBP.ToString());
			// <metavariant />
			objWriter.WriteElementString("metavariant", _strMetavariant);
			// <metatypecategory />
			objWriter.WriteElementString("metatypecategory", _strMetatypeCategory);
			// <walk />
			objWriter.WriteElementString("walk", _intWalk.ToString());
            // <run />
            objWriter.WriteElementString("run", _intRun.ToString());
            // <sprint />
            objWriter.WriteElementString("sprint", _intSprint.ToString());
			// <mutantcritterbaseskills />
			objWriter.WriteElementString("mutantcritterbaseskills", _intMutantCritterBaseSkills.ToString());

			// <name />
			objWriter.WriteElementString("name", _strName);
			// <mugshot />
			objWriter.WriteElementString("mugshot", _strMugshot);
			// <sex />
			objWriter.WriteElementString("sex", _strSex);
			// <age />
			objWriter.WriteElementString("age", _strAge);
			// <eyes />
			objWriter.WriteElementString("eyes", _strEyes);
			// <height />
			objWriter.WriteElementString("height", _strHeight);
			// <weight />
			objWriter.WriteElementString("weight", _strWeight);
			// <skin />
			objWriter.WriteElementString("skin", _strSkin);
			// <hair />
			objWriter.WriteElementString("hair", _strHair);
			// <description />
			objWriter.WriteElementString("description", _strDescription);
			// <background />
			objWriter.WriteElementString("background", _strBackground);
			// <concept />
			objWriter.WriteElementString("concept", _strConcept);
			// <notes />
			objWriter.WriteElementString("notes", _strNotes);
			// <alias />
			objWriter.WriteElementString("alias", _strAlias);
			// <playername />
			objWriter.WriteElementString("playername", _strPlayerName);
			// <gamenotes />
			objWriter.WriteElementString("gamenotes", _strGameNotes);

			// <ignorerules />
			if (_blnIgnoreRules)
				objWriter.WriteElementString("ignorerules", _blnIgnoreRules.ToString());
			// <iscritter />
			if (_blnIsCritter)
				objWriter.WriteElementString("iscritter", _blnIsCritter.ToString());
			if (_blnPossessed)
				objWriter.WriteElementString("possessed", _blnPossessed.ToString());

			// <karma />
			objWriter.WriteElementString("karma", _intKarma.ToString());
			// <totalkarma />
			objWriter.WriteElementString("totalkarma", _intTotalKarma.ToString());
			// <streetcred />
			objWriter.WriteElementString("streetcred", _intStreetCred.ToString());
			// <notoriety />
			objWriter.WriteElementString("notoriety", _intNotoriety.ToString());
			// <publicaware />
			objWriter.WriteElementString("publicawareness", _intPublicAwareness.ToString());
			// <burntstreetcred />
			objWriter.WriteElementString("burntstreetcred", _intBurntStreetCred.ToString());
			// <created />
			objWriter.WriteElementString("created", _blnCreated.ToString());
			// <maxavail />
			objWriter.WriteElementString("maxavail", _intMaxAvail.ToString());
			// <nuyen />
			objWriter.WriteElementString("nuyen", _intNuyen.ToString());

			// <buildpoints />
			objWriter.WriteElementString("bp", _intBuildPoints.ToString());
			// <buildkarma />
			objWriter.WriteElementString("buildkarma", _intBuildKarma.ToString());
			// <buildmethod />
			objWriter.WriteElementString("buildmethod", _objBuildMethod.ToString());

			// <knowpts />
			objWriter.WriteElementString("knowpts", _intKnowledgeSkillPoints.ToString());

			// <nuyenbp />
			objWriter.WriteElementString("nuyenbp", _decNuyenBP.ToString());
			// <nuyenmaxbp />
			objWriter.WriteElementString("nuyenmaxbp", _decNuyenMaximumBP.ToString());

			// <adept />
			objWriter.WriteElementString("adept", _blnAdeptEnabled.ToString());
			// <magician />
			objWriter.WriteElementString("magician", _blnMagicianEnabled.ToString());
			// <technomancer />
			objWriter.WriteElementString("technomancer", _blnTechnomancerEnabled.ToString());
			// <initiationoverride />
			objWriter.WriteElementString("initiationoverride", _blnInitiationEnabled.ToString());
			// <critter />
			objWriter.WriteElementString("critter", _blnCritterEnabled.ToString());
            // <sensitivesystem />
            objWriter.WriteElementString("sensitivesystem", _blnSensitiveSystem.ToString());
			// <uneducated />
			objWriter.WriteElementString("uneducated", _blnUneducated.ToString());
			// <uncouth />
			objWriter.WriteElementString("uncouth", _blnUncouth.ToString());
			// <infirm />
			objWriter.WriteElementString("infirm", _blnInfirm.ToString());
			// <blackmarket />
			objWriter.WriteElementString("blackmarket", _blnBlackMarket.ToString());

			// <attributes>
			objWriter.WriteStartElement("attributes");
			_attBOD.Save(objWriter);
			_attAGI.Save(objWriter);
			_attREA.Save(objWriter);
			_attSTR.Save(objWriter);
			_attCHA.Save(objWriter);
			_attINT.Save(objWriter);
			_attLOG.Save(objWriter);
			_attWIL.Save(objWriter);
			_attINI.Save(objWriter);
			_attEDG.Save(objWriter);
			_attMAG.Save(objWriter);
			_attRES.Save(objWriter);
			_attESS.Save(objWriter);
			// Include any special A.I. Attributes if applicable.
			if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
			{
				objWriter.WriteElementString("response", _intResponse.ToString());
				objWriter.WriteElementString("signal", _intSignal.ToString());
			}
			if (_intMaxSkillRating > 0)
				objWriter.WriteElementString("maxskillrating", _intMaxSkillRating.ToString());
			// </attributes>
			objWriter.WriteEndElement();

			// <magenabled />
			objWriter.WriteElementString("magenabled", _blnMAGEnabled.ToString());
			// <initiategrade />
			objWriter.WriteElementString("initiategrade", _intInitiateGrade.ToString());
			// <resenabled />
			objWriter.WriteElementString("resenabled", _blnRESEnabled.ToString());
			// <submersiongrade />
			objWriter.WriteElementString("submersiongrade", _intSubmersionGrade.ToString());
			// <groupmember />
			objWriter.WriteElementString("groupmember", _blnGroupMember.ToString());
			// <groupname />
			objWriter.WriteElementString("groupname", _strGroupName);
			// <groupnotes />
			objWriter.WriteElementString("groupnotes", _strGroupNotes);

			// External reader friendly stuff.
			objWriter.WriteElementString("totaless", Essence.ToString());

			// Write out the Mystic Adept MAG split info.
			if (_blnAdeptEnabled && _blnMagicianEnabled)
			{
				objWriter.WriteElementString("magsplitadept", _intMAGAdept.ToString());
				objWriter.WriteElementString("magsplitmagician", _intMAGMagician.ToString());
			}

			// Write the Magic Tradition.
			objWriter.WriteElementString("tradition", _guiMagicTradition.ToString());
			// Write the Technomancer Stream.
			objWriter.WriteElementString("stream", _guiTechnomancerStream.ToString());

			// Condition Monitor Progress.
			// <physicalcmfilled />
			objWriter.WriteElementString("physicalcmfilled", _intPhysicalCMFilled.ToString());
			// <stuncmfilled />
			objWriter.WriteElementString("stuncmfilled", _intStunCMFilled.ToString());

			// <skillgroups>
			objWriter.WriteStartElement("skillgroups");
			foreach (SkillGroup objSkillGroup in _lstSkillGroups)
			{
				objSkillGroup.Save(objWriter);
			}
			// </skillgroups>
			objWriter.WriteEndElement();

			// <skills>
			objWriter.WriteStartElement("skills");
			foreach (Skill objSkill in _lstSkills)
			{
				objSkill.Save(objWriter);
			}
			// </skills>
			objWriter.WriteEndElement();

			// <contacts>
			objWriter.WriteStartElement("contacts");
			foreach (Contact objContact in _lstContacts)
			{
				objContact.Save(objWriter);
			}
			// </contacts>
			objWriter.WriteEndElement();

			// <spells>
			objWriter.WriteStartElement("spells");
			foreach (Spell objSpell in _lstSpells)
			{
				objSpell.Save(objWriter);
			}
			// </spells>
			objWriter.WriteEndElement();

			// <foci>
			objWriter.WriteStartElement("foci");
			foreach (Focus objFocus in _lstFoci)
			{
				objFocus.Save(objWriter);
			}
			// </foci>
			objWriter.WriteEndElement();

			// <stackedfoci>
			objWriter.WriteStartElement("stackedfoci");
			foreach (StackedFocus objStack in _lstStackedFoci)
			{
				objStack.Save(objWriter);
			}
			// </stackedfoci>
			objWriter.WriteEndElement();

			// <powers>
			objWriter.WriteStartElement("powers");
			foreach (Power objPower in _lstPowers)
			{
				objPower.Save(objWriter);
			}
			// </powers>
			objWriter.WriteEndElement();

			// <spirits>
			objWriter.WriteStartElement("spirits");
			foreach (Spirit objSpirit in _lstSpirits)
			{
				objSpirit.Save(objWriter);
			}
			// </spirits>
			objWriter.WriteEndElement();

			// <techprograms>
			objWriter.WriteStartElement("techprograms");
			foreach (TechProgram objProgram in _lstTechPrograms)
			{
				objProgram.Save(objWriter);
			}
			// </techprograms>
			objWriter.WriteEndElement();

			// <martialarts>
			objWriter.WriteStartElement("martialarts");
			foreach (MartialArt objMartialArt in _lstMartialArts)
			{
				objMartialArt.Save(objWriter);
			}
			// </martialarts>
			objWriter.WriteEndElement();

			// <martialartmaneuvers>
			objWriter.WriteStartElement("martialartmaneuvers");
			foreach (MartialArtManeuver objManeuver in _lstMartialArtManeuvers)
			{
				objManeuver.Save(objWriter);
			}
			// </martialartmaneuvers>
			objWriter.WriteEndElement();

			// <armors>
			objWriter.WriteStartElement("armors");
			foreach (Armor objArmor in _lstArmor)
			{
				objArmor.Save(objWriter);
			}
			// </armors>
			objWriter.WriteEndElement();

			// <weapons>
			objWriter.WriteStartElement("weapons");
			foreach (Weapon objWeapon in _lstWeapons)
			{
				objWeapon.Save(objWriter);
			}
			// </weapons>
			objWriter.WriteEndElement();

			// <cyberwares>
			objWriter.WriteStartElement("cyberwares");
			foreach (Cyberware objCyberware in _lstCyberware)
			{
				objCyberware.Save(objWriter);
			}
			// </cyberwares>
			objWriter.WriteEndElement();

			// <qualities>
			objWriter.WriteStartElement("qualities");
			foreach (Quality objQuality in _lstQualities)
			{
				objQuality.Save(objWriter);
			}
			// </qualities>
			objWriter.WriteEndElement();

			// <lifestyles>
			objWriter.WriteStartElement("lifestyles");
			foreach (Lifestyle objLifestyle in _lstLifestyles)
			{
				objLifestyle.Save(objWriter);
			}
			// </lifestyles>
			objWriter.WriteEndElement();

			// <gears>
			objWriter.WriteStartElement("gears");
			foreach (Gear objGear in _lstGear)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(this);
					objCommlink = (Commlink)objGear;
					objCommlink.Save(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(this);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Save(objWriter);
				}
				else
				{
					objGear.Save(objWriter);
				}
			}
			// </gears>
			objWriter.WriteEndElement();

			// <vehicles>
			objWriter.WriteStartElement("vehicles");
			foreach (Vehicle objVehicle in _lstVehicles)
			{
				objVehicle.Save(objWriter);
			}
			// </vehicles>
			objWriter.WriteEndElement();

			// <metamagics>
			objWriter.WriteStartElement("metamagics");
			foreach (Metamagic objMetamagic in _lstMetamagics)
			{
				objMetamagic.Save(objWriter);
			}
			// </metamagics>
			objWriter.WriteEndElement();

			// <critterpowers>
			objWriter.WriteStartElement("critterpowers");
			foreach (CritterPower objPower in _lstCritterPowers)
			{
				objPower.Save(objWriter);
			}
			// </critterpowers>
			objWriter.WriteEndElement();

			// <initiationgrades>
			objWriter.WriteStartElement("initiationgrades");
			foreach (InitiationGrade objGrade in _lstInitiationGrades)
			{
				objGrade.Save(objWriter);
			}
			// </initiationgrades>
			objWriter.WriteEndElement();

			// <improvements>
			objWriter.WriteStartElement("improvements");
			foreach (Improvement objImprovement in _lstImprovements)
			{
				objImprovement.Save(objWriter);
			}
			// </improvements>
			objWriter.WriteEndElement();

			// <expenses>
			objWriter.WriteStartElement("expenses");
			foreach (ExpenseLogEntry objExpenseLogEntry in _lstExpenseLog)
			{
				objExpenseLogEntry.Save(objWriter);
			}
			// </expenses>
			objWriter.WriteEndElement();

			// <locations>
			objWriter.WriteStartElement("locations");
			foreach (string strLocation in _lstLocations)
			{
				objWriter.WriteElementString("location", strLocation);
			}
			// </locations>
			objWriter.WriteEndElement();

			// <armorbundles>
			objWriter.WriteStartElement("armorbundles");
			foreach (string strBundle in _lstArmorBundles)
			{
				objWriter.WriteElementString("armorbundle", strBundle);
			}
			// </armorbundles>
			objWriter.WriteEndElement();

			// <weaponlocations>
			objWriter.WriteStartElement("weaponlocations");
			foreach (string strLocation in _lstWeaponLocations)
			{
				objWriter.WriteElementString("weaponlocation", strLocation);
			}
			// </weaponlocations>
			objWriter.WriteEndElement();

			// <improvementgroups>
			objWriter.WriteStartElement("improvementgroups");
			foreach (string strGroup in _lstImprovementGroups)
			{
				objWriter.WriteElementString("improvementgroup", strGroup);
			}
			// </improvementgroups>
			objWriter.WriteEndElement();

			// <calendar>
			objWriter.WriteStartElement("calendar");
			foreach (CalendarWeek objWeek in _lstCalendar)
			{
				objWeek.Save(objWriter);
			}
			objWriter.WriteEndElement();
			// </calendar>

			// </character>
			objWriter.WriteEndElement();

			objWriter.WriteEndDocument();
			objWriter.Close();
			objStream.Close();
		}

		/// <summary>
		/// Load the Character from an XML file.
		/// </summary>
		public bool Load()
		{

			XmlDocument objXmlDocument = new XmlDocument();
			objXmlDocument.Load(_strFileName);

			XmlNode objXmlCharacter = objXmlDocument.SelectSingleNode("/character");
			XmlNodeList objXmlNodeList;

			try
			{
				_blnIgnoreRules = Convert.ToBoolean(objXmlCharacter["ignorerules"].InnerText);
			}
			catch
			{
				_blnIgnoreRules = false;
			}
			try
			{
				_blnCreated = Convert.ToBoolean(objXmlCharacter["created"].InnerText);
			}
			catch
			{
			}

			ResetCharacter();

			// Get the game edition of the file if possible and make sure it's intended to be used with this version of the application.
			try
			{
				if (objXmlCharacter["gameedition"].InnerText != string.Empty && objXmlCharacter["gameedition"].InnerText != "SR5")
				{
					MessageBox.Show(LanguageManager.Instance.GetString("Message_IncorrectGameVersion_SR4"), LanguageManager.Instance.GetString("MessageTitle_IncorrectGameVersion"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
			catch
			{
			}

			// Get the name of the settings file in use if possible.
			try
			{
				_strSettingsFileName = objXmlCharacter["settings"].InnerText;
			}
			catch
			{
			}

			// Load the character's settings file.
			if (!_objOptions.Load(_strSettingsFileName))
				return false;
			
            // Application version information.
            if (objXmlCharacter["appversionfirst"] != null)
                _strAppVersionFirst = objXmlCharacter["appversionfirst"].InnerText;
            if (objXmlCharacter["appversioncareer"] != null)
                _strAppVersionCareer = objXmlCharacter["appversioncareer"].InnerText;

			// Metatype information.
			_strMetatype = objXmlCharacter["metatype"].InnerText;
            _guiMetatype = Guid.Parse(objXmlCharacter["metatypeid"].InnerText);
			_intWalk = Convert.ToInt32(objXmlCharacter["walk"].InnerText);
            _intRun = Convert.ToInt32(objXmlCharacter["run"].InnerText);
            _intSprint = Convert.ToInt32(objXmlCharacter["sprint"].InnerText);
			_intMetatypeBP = Convert.ToInt32(objXmlCharacter["metatypebp"].InnerText);
			_strMetavariant = objXmlCharacter["metavariant"].InnerText;
			try
			{
				_strMetatypeCategory = objXmlCharacter["metatypecategory"].InnerText;
			}
			catch
			{
			}
			try
			{
				_intMutantCritterBaseSkills = Convert.ToInt32(objXmlCharacter["mutantcritterbaseskills"].InnerText);
			}
			catch
			{
			}
			
			// General character information.
			_strName = objXmlCharacter["name"].InnerText;
			try
			{
				_strMugshot = objXmlCharacter["mugshot"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strSex = objXmlCharacter["sex"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strAge = objXmlCharacter["age"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strEyes = objXmlCharacter["eyes"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strHeight = objXmlCharacter["height"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strWeight = objXmlCharacter["weight"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strSkin = objXmlCharacter["skin"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strHair = objXmlCharacter["hair"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strDescription = objXmlCharacter["description"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strBackground = objXmlCharacter["background"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strConcept = objXmlCharacter["concept"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strNotes = objXmlCharacter["notes"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strAlias = objXmlCharacter["alias"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strPlayerName = objXmlCharacter["playername"].InnerText;
			}
			catch
			{
			}
			try
			{
				_strGameNotes = objXmlCharacter["gamenotes"].InnerText;
			}
			catch
			{
			}

			try
			{
				_blnIsCritter = Convert.ToBoolean(objXmlCharacter["iscritter"].InnerText);
			}
			catch
			{
			}

			try
			{
				_blnPossessed = Convert.ToBoolean(objXmlCharacter["possessed"].InnerText);
			}
			catch
			{
			}
			
			try
			{
				_intKarma = Convert.ToInt32(objXmlCharacter["karma"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intTotalKarma = Convert.ToInt32(objXmlCharacter["totalkarma"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intStreetCred = Convert.ToInt32(objXmlCharacter["streetcred"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intNotoriety = Convert.ToInt32(objXmlCharacter["notoriety"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intPublicAwareness = Convert.ToInt32(objXmlCharacter["publicawareness"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intBurntStreetCred = Convert.ToInt32(objXmlCharacter["burntstreetcred"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intMaxAvail = Convert.ToInt32(objXmlCharacter["maxavail"].InnerText);
			}
			catch
			{
			}
			try
			{
				_intNuyen = Convert.ToInt32(objXmlCharacter["nuyen"].InnerText);
			}
			catch
			{
			}

			// Build Points/Karma.
			_intBuildPoints = Convert.ToInt32(objXmlCharacter["bp"].InnerText);
			try
			{
				_intBuildKarma = Convert.ToInt32(objXmlCharacter["buildkarma"].InnerText);
			}
			catch
			{
			}
			try
			{
				_objBuildMethod = ConvertToCharacterBuildMethod(objXmlCharacter["buildmethod"].InnerText);
			}
			catch
			{
			}
			_intKnowledgeSkillPoints = Convert.ToInt32(objXmlCharacter["knowpts"].InnerText);
			_decNuyenBP = Convert.ToDecimal(objXmlCharacter["nuyenbp"].InnerText, GlobalOptions.Instance.CultureInfo);
			_decNuyenMaximumBP = Convert.ToDecimal(objXmlCharacter["nuyenmaxbp"].InnerText, GlobalOptions.Instance.CultureInfo);
			_blnAdeptEnabled = Convert.ToBoolean(objXmlCharacter["adept"].InnerText);
			_blnMagicianEnabled = Convert.ToBoolean(objXmlCharacter["magician"].InnerText);
			_blnTechnomancerEnabled = Convert.ToBoolean(objXmlCharacter["technomancer"].InnerText);
			try
			{
				_blnInitiationEnabled = Convert.ToBoolean(objXmlCharacter["initiationoverride"].InnerText);
			}
			catch
			{
			}
			try
			{
				_blnCritterEnabled = Convert.ToBoolean(objXmlCharacter["critter"].InnerText);
			}
			catch
			{
			}
            try
            {
                _blnSensitiveSystem = Convert.ToBoolean(objXmlCharacter["sensitivesystem"].InnerText);
            }
            catch
            {
            }
			try
			{
				_blnUneducated = Convert.ToBoolean(objXmlCharacter["uneducated"].InnerText);
			}
			catch
			{
			}
			try
			{
				_blnUncouth = Convert.ToBoolean(objXmlCharacter["uncouth"].InnerText);
			}
			catch
			{
			}
			try
			{
				_blnInfirm = Convert.ToBoolean(objXmlCharacter["infirm"].InnerText);
			}
			catch
			{
			}
			try
			{
				_blnBlackMarket = Convert.ToBoolean(objXmlCharacter["blackmarket"].InnerText);
			}
			catch
			{
			}
			_blnMAGEnabled = Convert.ToBoolean(objXmlCharacter["magenabled"].InnerText);
			try
			{
				_intInitiateGrade = Convert.ToInt32(objXmlCharacter["initiategrade"].InnerText);
			}
			catch
			{
			}
			_blnRESEnabled = Convert.ToBoolean(objXmlCharacter["resenabled"].InnerText);
			try
			{
				_intSubmersionGrade = Convert.ToInt32(objXmlCharacter["submersiongrade"].InnerText);
			}
			catch
			{
			}
			try
			{
				_blnGroupMember = Convert.ToBoolean(objXmlCharacter["groupmember"].InnerText);
			}
			catch
			{
			}
			try
			{
				_strGroupName = objXmlCharacter["groupname"].InnerText;
				_strGroupNotes = objXmlCharacter["groupnotes"].InnerText;
			}
			catch
			{
			}

			// Improvements.
			XmlNodeList objXmlImprovementList = objXmlDocument.SelectNodes("/character/improvements/improvement");
			foreach (XmlNode objXmlImprovement in objXmlImprovementList)
			{
				Improvement objImprovement = new Improvement();
				objImprovement.Load(objXmlImprovement);
				_lstImprovements.Add(objImprovement);
			}

			// Qualities
			objXmlNodeList = objXmlDocument.SelectNodes("/character/qualities/quality");
			foreach (XmlNode objXmlQuality in objXmlNodeList)
			{
				if (objXmlQuality["name"] != null)
				{
					Quality objQuality = new Quality(this);
					objQuality.Load(objXmlQuality);
					_lstQualities.Add(objQuality);
				}
			}

			// Attributes.
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"BOD\"]");
			_attBOD.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"AGI\"]");
			_attAGI.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"REA\"]");
			_attREA.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"STR\"]");
			_attSTR.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"CHA\"]");
			_attCHA.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"INT\"]");
			_attINT.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"LOG\"]");
			_attLOG.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"WIL\"]");
			_attWIL.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"INI\"]");
			_attINI.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"EDG\"]");
			_attEDG.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"MAG\"]");
			_attMAG.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"RES\"]");
			_attRES.Load(objXmlCharacter);
			objXmlCharacter = objXmlDocument.SelectSingleNode("/character/attributes/attribute[name = \"ESS\"]");
			_attESS.Load(objXmlCharacter);

			// A.I. Attributes.
			try
			{
				_intSignal = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/attributes/signal").InnerText);
				_intResponse = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/attributes/response").InnerText);
			}
			catch
			{
			}

			// Force.
			try
			{
				_intMaxSkillRating = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/attributes/maxskillrating").InnerText);
			}
			catch
			{
			}

			// Attempt to load the split MAG Attribute information for Mystic Adepts.
			if (_blnAdeptEnabled && _blnMagicianEnabled)
			{
				try
				{
					_intMAGAdept = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/magsplitadept").InnerText);
					_intMAGMagician = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/magsplitmagician").InnerText);
				}
				catch
				{
				}
			}

			// Attempt to load the Magic Tradition.
			try
			{
				_guiMagicTradition = Guid.Parse(objXmlDocument.SelectSingleNode("/character/tradition").InnerText);
			}
			catch
			{
			}
			// Attempt to load the Technomancer Stream.
			try
			{
				_guiTechnomancerStream = Guid.Parse(objXmlDocument.SelectSingleNode("/character/stream").InnerText);
			}
			catch
			{
			}

			// Attempt to load Condition Monitor Progress.
			try
			{
				_intPhysicalCMFilled = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/physicalcmfilled").InnerText);
				_intStunCMFilled = Convert.ToInt32(objXmlDocument.SelectSingleNode("/character/stuncmfilled").InnerText);
			}
			catch
			{
			}

			// Skills.
			foreach (Skill objSkill in _lstSkills)
			{
				XmlNode objXmlSkill = objXmlDocument.SelectSingleNode("/character/skills/skill[name = \"" + objSkill.Name + "\"]");
				if (objXmlSkill != null)
				{
					objSkill.Load(objXmlSkill);
				}
			}

			// Exotic Skills.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/skills/skill[exotic = \"True\"]");
			foreach (XmlNode objXmlSkill in objXmlNodeList)
			{
				Skill objSkill = new Skill(this);
				objSkill.Load(objXmlSkill);
				_lstSkills.Add(objSkill);
			}

			// SkillGroups.
			foreach (SkillGroup objGroup in _lstSkillGroups)
			{
				XmlNode objXmlSkill = objXmlDocument.SelectSingleNode("/character/skillgroups/skillgroup[name = \"" + objGroup.Name + "\"]");
				if (objXmlSkill != null)
				{
					objGroup.Load(objXmlSkill);
					// If the character is set to ignore rules or is in Career Mode, Skill Groups should have a maximum Rating of 6 unless they have been given a higher maximum Rating already.
					if ((_blnIgnoreRules || _blnCreated) && objGroup.RatingMaximum < 6)
						objGroup.RatingMaximum = 6;
				}
			}

			// Knowledge Skills.
			List<ListItem> lstKnowledgeSkillOrder = new List<ListItem>();
			objXmlNodeList = objXmlDocument.SelectNodes("/character/skills/skill[knowledge = \"True\"]");
			// Sort the Knowledge Skills in alphabetical order.
			foreach (XmlNode objXmlSkill in objXmlNodeList)
			{
				ListItem objGroup = new ListItem();
				objGroup.Value = objXmlSkill["name"].InnerText;
				objGroup.Name = objXmlSkill["name"].InnerText;
				lstKnowledgeSkillOrder.Add(objGroup);
			}
			SortListItem objSort = new SortListItem();
			lstKnowledgeSkillOrder.Sort(objSort.Compare);

			foreach (ListItem objItem in lstKnowledgeSkillOrder)
			{
				Skill objSkill = new Skill(this);
				XmlNode objNode = objXmlDocument.SelectSingleNode("/character/skills/skill[knowledge = \"True\" and name = " + CleanXPath(objItem.Value) + "]");
				objSkill.Load(objNode);
				_lstSkills.Add(objSkill);
			}

			// Contacts.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/contacts/contact");
			foreach (XmlNode objXmlContact in objXmlNodeList)
			{
				Contact objContact = new Contact(this);
				objContact.Load(objXmlContact);
				_lstContacts.Add(objContact);
			}

			// Armor.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/armors/armor");
			foreach (XmlNode objXmlArmor in objXmlNodeList)
			{
				Armor objArmor = new Armor(this);
				objArmor.Load(objXmlArmor);
				_lstArmor.Add(objArmor);
			}

			// Weapons.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/weapons/weapon");
			foreach (XmlNode objXmlWeapon in objXmlNodeList)
			{
				Weapon objWeapon = new Weapon(this);
				objWeapon.Load(objXmlWeapon);
				_lstWeapons.Add(objWeapon);
			}

			// Cyberware/Bioware.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/cyberwares/cyberware");
			foreach (XmlNode objXmlCyberware in objXmlNodeList)
			{
				Cyberware objCyberware = new Cyberware(this);
				objCyberware.Load(objXmlCyberware);
				_lstCyberware.Add(objCyberware);
			}

			// Spells.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/spells/spell");
			foreach (XmlNode objXmlSpell in objXmlNodeList)
			{
				Spell objSpell = new Spell(this);
				objSpell.Load(objXmlSpell);
				_lstSpells.Add(objSpell);
			}

			// Foci.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/foci/focus");
			foreach (XmlNode objXmlFocus in objXmlNodeList)
			{
				Focus objFocus = new Focus();
				objFocus.Load(objXmlFocus);
				_lstFoci.Add(objFocus);
			}

			// Stacked Foci.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/stackedfoci/stackedfocus");
			foreach (XmlNode objXmlStack in objXmlNodeList)
			{
				StackedFocus objStack = new StackedFocus(this);
				objStack.Load(objXmlStack);
				_lstStackedFoci.Add(objStack);
			}

			// Powers.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/powers/power");
			foreach (XmlNode objXmlPower in objXmlNodeList)
			{
				Power objPower = new Power(this);
				objPower.Load(objXmlPower);
				_lstPowers.Add(objPower);
			}

			// Spirits/Sprites.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/spirits/spirit");
			foreach (XmlNode objXmlSpirit in objXmlNodeList)
			{
				Spirit objSpirit = new Spirit(this);
				objSpirit.Load(objXmlSpirit);
				_lstSpirits.Add(objSpirit);
			}

			// Compex Forms/Technomancer Programs.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/techprograms/techprogram");
			foreach (XmlNode objXmlProgram in objXmlNodeList)
			{
				TechProgram objProgram = new TechProgram(this);
				objProgram.Load(objXmlProgram);
				_lstTechPrograms.Add(objProgram);
			}

			// Martial Arts.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/martialarts/martialart");
			foreach (XmlNode objXmlArt in objXmlNodeList)
			{
				MartialArt objMartialArt = new MartialArt(this);
				objMartialArt.Load(objXmlArt);
				_lstMartialArts.Add(objMartialArt);
			}

			// Martial Art Maneuvers.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/martialartmaneuvers/martialartmaneuver");
			foreach (XmlNode objXmlManeuver in objXmlNodeList)
			{
				MartialArtManeuver objManeuver = new MartialArtManeuver(this);
				objManeuver.Load(objXmlManeuver);
				_lstMartialArtManeuvers.Add(objManeuver);
			}

			// Lifestyles.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/lifestyles/lifestyle");
			foreach (XmlNode objXmlLifestyle in objXmlNodeList)
			{
				Lifestyle objLifestyle = new Lifestyle(this);
				objLifestyle.Load(objXmlLifestyle);
				_lstLifestyles.Add(objLifestyle);
			}

			// <gears>
			objXmlNodeList = objXmlDocument.SelectNodes("/character/gears/gear");
			foreach (XmlNode objXmlGear in objXmlNodeList)
			{
				switch (objXmlGear["category"].InnerText)
				{
					case "Commlink":
					case "Commlink Upgrade":
						Commlink objCommlink = new Commlink(this);
						objCommlink.Load(objXmlGear);
						_lstGear.Add(objCommlink);
						break;
					case "Commlink Operating System":
					case "Commlink Operating System Upgrade":
						OperatingSystem objOperatingSystem = new OperatingSystem(this);
						objOperatingSystem.Load(objXmlGear);
						_lstGear.Add(objOperatingSystem);
						break;
					default:
						Gear objGear = new Gear(this);
						objGear.Load(objXmlGear);
						_lstGear.Add(objGear);
						break;
				}
			}

			// Vehicles.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/vehicles/vehicle");
			foreach (XmlNode objXmlVehicle in objXmlNodeList)
			{
				Vehicle objVehicle = new Vehicle(this);
				objVehicle.Load(objXmlVehicle);
				_lstVehicles.Add(objVehicle);
			}

			// Metamagics/Echoes.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/metamagics/metamagic");
			foreach (XmlNode objXmlMetamagic in objXmlNodeList)
			{
				Metamagic objMetamagic = new Metamagic(this);
				objMetamagic.Load(objXmlMetamagic);
				_lstMetamagics.Add(objMetamagic);
			}

			// Critter Powers.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/critterpowers/critterpower");
			foreach (XmlNode objXmlPower in objXmlNodeList)
			{
				CritterPower objPower = new CritterPower(this);
				objPower.Load(objXmlPower);
				_lstCritterPowers.Add(objPower);
			}

			// Initiation Grades.
			objXmlNodeList = objXmlDocument.SelectNodes("/character/initiationgrades/initiationgrade");
			foreach (XmlNode objXmlGrade in objXmlNodeList)
			{
				InitiationGrade objGrade = new InitiationGrade(this);
				objGrade.Load(objXmlGrade);
				_lstInitiationGrades.Add(objGrade);
			}

			// Expense Log Entries.
			XmlNodeList objXmlExpenseList = objXmlDocument.SelectNodes("/character/expenses/expense");
			foreach (XmlNode objXmlExpense in objXmlExpenseList)
			{
				ExpenseLogEntry objExpenseLogEntry = new ExpenseLogEntry();
				objExpenseLogEntry.Load(objXmlExpense);
				_lstExpenseLog.Add(objExpenseLogEntry);
			}

			// Locations.
			XmlNodeList objXmlLocationList = objXmlDocument.SelectNodes("/character/locations/location");
			foreach (XmlNode objXmlLocation in objXmlLocationList)
			{
				_lstLocations.Add(objXmlLocation.InnerText);
			}

			// Armor Bundles.
			XmlNodeList objXmlBundleList = objXmlDocument.SelectNodes("/character/armorbundles/armorbundle");
			foreach (XmlNode objXmlBundle in objXmlBundleList)
			{
				_lstArmorBundles.Add(objXmlBundle.InnerText);
			}

			// Weapon Locations.
			XmlNodeList objXmlWeaponLocationList = objXmlDocument.SelectNodes("/character/weaponlocations/weaponlocation");
			foreach (XmlNode objXmlLocation in objXmlWeaponLocationList)
			{
				_lstWeaponLocations.Add(objXmlLocation.InnerText);
			}

			// Improvement Groups.
			XmlNodeList objXmlGroupList = objXmlDocument.SelectNodes("/character/improvementgroups/improvementgroup");
			foreach (XmlNode objXmlGroup in objXmlGroupList)
			{
				_lstImprovementGroups.Add(objXmlGroup.InnerText);
			}

			// Calendar.
			XmlNodeList objXmlWeekList = objXmlDocument.SelectNodes("/character/calendar/week");
			foreach (XmlNode objXmlWeek in objXmlWeekList)
			{
				CalendarWeek objWeek = new CalendarWeek();
				objWeek.Load(objXmlWeek);
				_lstCalendar.Add(objWeek);
			}

			return true;
		}

		/// <summary>
		/// Print this character information to a MemoryStream. This creates only the character object itself, not any of the opening or closing XmlDocument items.
		/// This can be used to write multiple characters to a single XmlDocument.
		/// </summary>
		/// <param name="objStream">MemoryStream to use.</param>
		/// <param name="objWriter">XmlTextWriter to write to.</param>
		public void PrintToStream(MemoryStream objStream, XmlTextWriter objWriter)
		{
			XmlDocument objXmlDocument = new XmlDocument();

			XmlDocument objMetatypeDoc = new XmlDocument();
			XmlNode objMetatypeNode;
			string strMetatype = "";
			string strMetavariant = "";
			// Get the name of the Metatype and Metavariant.
			objMetatypeDoc = XmlManager.Instance.Load("metatypes.xml");
			{
				objMetatypeNode = objMetatypeDoc.SelectSingleNode("/chummer/metatypes/metatype[name = \"" + _strMetatype + "\"]");
				if (objMetatypeNode == null)
					objMetatypeDoc = XmlManager.Instance.Load("critters.xml");
				objMetatypeNode = objMetatypeDoc.SelectSingleNode("/chummer/metatypes/metatype[name = \"" + _strMetatype + "\"]");

				if (objMetatypeNode["translate"] != null)
					strMetatype = objMetatypeNode["translate"].InnerText;
				else
					strMetatype = _strMetatype;

				if (_strMetavariant != "")
				{
					objMetatypeNode = objMetatypeNode.SelectSingleNode("metavariants/metavariant[name = \"" + _strMetavariant + "\"]");

					if (objMetatypeNode["translate"] != null)
						strMetavariant = objMetatypeNode["translate"].InnerText;
					else
						strMetavariant = _strMetavariant;
				}
			}

			Guid guiImage = new Guid();
			guiImage = Guid.NewGuid();
			// This line left in for debugging. Write the output to a fixed file name.
			//FileStream objStream = new FileStream("D:\\temp\\print.xml", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);//(_strFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

			// <character>
			objWriter.WriteStartElement("character");

			// <metatype />
			objWriter.WriteElementString("metatype", strMetatype);
			// <metatype_english />
			objWriter.WriteElementString("metatype_english", _strMetatype);
			// <metavariant />
			objWriter.WriteElementString("metavariant", strMetavariant);
			// <metavariant_english />
			objWriter.WriteElementString("metavariant_english", _strMetavariant);
			// <walk />
			objWriter.WriteElementString("walk", _intWalk.ToString());
            // <run />
            objWriter.WriteElementString("run", _intRun.ToString());
            // <sprint />
            objWriter.WriteElementString("sprint", _intSprint.ToString());

			// If the character does not have a name, call them Unnamed Character. This prevents a transformed document from having a self-terminated title tag which causes browser to not rendering anything.
			// <name />
			if (_strName != "")
				objWriter.WriteElementString("name", _strName);
			else
				objWriter.WriteElementString("name", LanguageManager.Instance.GetString("String_UnnamedCharacter"));

			// Since IE is retarded and can't handle base64 images before IE9, we need to dump the image to a temporary directory and re-write the information.
			// If you give it an extension of jpg, gif, or png, it expects the file to be in that format and won't render the image unless it was originally that type.
			// But if you give it the extension img, it will render whatever you give it (which doesn't make any damn sense, but that's IE for you).
			string strMugshotPath = "";
			if (_strMugshot != "")
			{
                if (!Directory.Exists(GlobalOptions.ApplicationPath() + Path.DirectorySeparatorChar + "mugshots"))
                    Directory.CreateDirectory(GlobalOptions.ApplicationPath() + Path.DirectorySeparatorChar + "mugshots");
				byte[] bytImage = Convert.FromBase64String(_strMugshot);
				MemoryStream objImageStream = new MemoryStream(bytImage, 0, bytImage.Length);
				objImageStream.Write(bytImage, 0, bytImage.Length);
				Image imgMugshot = Image.FromStream(objImageStream, true);
                imgMugshot.Save(GlobalOptions.ApplicationPath() + Path.DirectorySeparatorChar + "mugshots" + Path.DirectorySeparatorChar + guiImage.ToString() + ".img");
                strMugshotPath = "file://" + (GlobalOptions.ApplicationPath() + Path.DirectorySeparatorChar + "mugshots" + Path.DirectorySeparatorChar + guiImage.ToString() + ".img").Replace(Path.DirectorySeparatorChar, '/');
			}
			// <mugshot />
			objWriter.WriteElementString("mugshot", strMugshotPath);
			// <mugshotbase64 />
			objWriter.WriteElementString("mugshotbase64", _strMugshot);
			// <sex />
			objWriter.WriteElementString("sex", _strSex);
			// <age />
			objWriter.WriteElementString("age", _strAge);
			// <eyes />
			objWriter.WriteElementString("eyes", _strEyes);
			// <height />
			objWriter.WriteElementString("height", _strHeight);
			// <weight />
			objWriter.WriteElementString("weight", _strWeight);
			// <skin />
			objWriter.WriteElementString("skin", _strSkin);
			// <hair />
			objWriter.WriteElementString("hair", _strHair);
			// <description />
			objWriter.WriteElementString("description", _strDescription);
			// <background />
			objWriter.WriteElementString("background", _strBackground);
			// <concept />
			objWriter.WriteElementString("concept", _strConcept);
			// <notes />
			objWriter.WriteElementString("notes", _strNotes);
			// <alias />
			objWriter.WriteElementString("alias", _strAlias);
			// <playername />
			objWriter.WriteElementString("playername", _strPlayerName);
			// <gamenotes />
			objWriter.WriteElementString("gamenotes", _strGameNotes);

			// <karma />
			objWriter.WriteElementString("karma", _intKarma.ToString());
			// <totalkarma />
			objWriter.WriteElementString("totalkarma", String.Format("{0:###,###,##0}", Convert.ToInt32(CareerKarma)));
			// <streetcred />
			objWriter.WriteElementString("streetcred", _intStreetCred.ToString());
			// <calculatedstreetcred />
			objWriter.WriteElementString("calculatedstreetcred", CalculatedStreetCred.ToString());
			// <totalstreetcred />
			objWriter.WriteElementString("totalstreetcred", TotalStreetCred.ToString());
			// <burntstreetcred />
			objWriter.WriteElementString("burntstreetcred", _intBurntStreetCred.ToString());
			// <notoriety />
			objWriter.WriteElementString("notoriety", _intNotoriety.ToString());
			// <calculatednotoriety />
			objWriter.WriteElementString("calculatednotoriety", CalculatedNotoriety.ToString());
			// <totalnotoriety />
			objWriter.WriteElementString("totalnotoriety", TotalNotoriety.ToString());
			// <publicawareness />
			objWriter.WriteElementString("publicawareness", _intPublicAwareness.ToString());
			// <calculatedpublicawareness />
			objWriter.WriteElementString("calculatedpublicawareness", CalculatedPublicAwareness.ToString());
			// <totalpublicawareness />
			objWriter.WriteElementString("totalpublicawareness", TotalPublicAwareness.ToString());
			// <created />
			objWriter.WriteElementString("created", _blnCreated.ToString());
			// <nuyen />
			objWriter.WriteElementString("nuyen", _intNuyen.ToString());

			// <adept />
			objWriter.WriteElementString("adept", _blnAdeptEnabled.ToString());
			// <magician />
			objWriter.WriteElementString("magician", _blnMagicianEnabled.ToString());
			// <technomancer />
			objWriter.WriteElementString("technomancer", _blnTechnomancerEnabled.ToString());
			// <critter />
			objWriter.WriteElementString("critter", _blnCritterEnabled.ToString());

			// <tradition />
			objWriter.WriteElementString("tradition", _guiMagicTradition.ToString());
			// <stream />
			objWriter.WriteElementString("stream", _guiTechnomancerStream.ToString());
			// <drain />
			if (_guiMagicTradition != Guid.Empty)
			{
				string strDrainAtt = "";
				objXmlDocument = new XmlDocument();
				objXmlDocument = XmlManager.Instance.Load("traditions.xml");

				XmlNode objXmlTradition = objXmlDocument.SelectSingleNode("/chummer/traditions/tradition[id = \"" + _guiMagicTradition.ToString() + "\"]");
				strDrainAtt = objXmlTradition["drain"].InnerText;

				XPathNavigator nav = objXmlDocument.CreateNavigator();
				string strDrain = strDrainAtt.Replace("BOD", _attBOD.TotalValue.ToString());
				strDrain = strDrain.Replace("AGI", _attAGI.TotalValue.ToString());
				strDrain = strDrain.Replace("REA", _attREA.TotalValue.ToString());
				strDrain = strDrain.Replace("STR", _attSTR.TotalValue.ToString());
				strDrain = strDrain.Replace("CHA", _attCHA.TotalValue.ToString());
				strDrain = strDrain.Replace("INT", _attINT.TotalValue.ToString());
				strDrain = strDrain.Replace("LOG", _attLOG.TotalValue.ToString());
				strDrain = strDrain.Replace("WIL", _attWIL.TotalValue.ToString());
				strDrain = strDrain.Replace("MAG", _attMAG.TotalValue.ToString());
				XPathExpression xprDrain = nav.Compile(strDrain);

				// Add any Improvements for Drain Resistance.
				int intDrain = Convert.ToInt32(nav.Evaluate(xprDrain)) + _objImprovementManager.ValueOf(Improvement.ImprovementType.DrainResistance);

				objWriter.WriteElementString("drain", strDrainAtt + " (" + intDrain.ToString() + ")");
			}
			if (_guiTechnomancerStream != Guid.Empty)
			{
				string strDrainAtt = "";
				objXmlDocument = new XmlDocument();
				objXmlDocument = XmlManager.Instance.Load("streams.xml");

				XmlNode objXmlTradition = objXmlDocument.SelectSingleNode("/chummer/traditions/tradition[id = \"" + _guiTechnomancerStream.ToString() + "\"]");
				strDrainAtt = objXmlTradition["drain"].InnerText;

				XPathNavigator nav = objXmlDocument.CreateNavigator();
				string strDrain = strDrainAtt.Replace("BOD", _attBOD.TotalValue.ToString());
				strDrain = strDrain.Replace("AGI", _attAGI.TotalValue.ToString());
				strDrain = strDrain.Replace("REA", _attREA.TotalValue.ToString());
				strDrain = strDrain.Replace("STR", _attSTR.TotalValue.ToString());
				strDrain = strDrain.Replace("CHA", _attCHA.TotalValue.ToString());
				strDrain = strDrain.Replace("INT", _attINT.TotalValue.ToString());
				strDrain = strDrain.Replace("LOG", _attLOG.TotalValue.ToString());
				strDrain = strDrain.Replace("WIL", _attWIL.TotalValue.ToString());
				strDrain = strDrain.Replace("RES", _attRES.TotalValue.ToString());
				XPathExpression xprDrain = nav.Compile(strDrain);

				// Add any Improvements for Fading Resistance.
				int intDrain = Convert.ToInt32(nav.Evaluate(xprDrain)) + _objImprovementManager.ValueOf(Improvement.ImprovementType.FadingResistance);

				objWriter.WriteElementString("drain", strDrainAtt + " (" + intDrain.ToString() + ")");
			}

			// <attributes>
			objWriter.WriteStartElement("attributes");
			_attBOD.Print(objWriter);
			_attAGI.Print(objWriter);
			_attREA.Print(objWriter);
			_attSTR.Print(objWriter);
			_attCHA.Print(objWriter);
			_attINT.Print(objWriter);
			_attLOG.Print(objWriter);
			_attWIL.Print(objWriter);
			_attINI.Print(objWriter);
			_attEDG.Print(objWriter);
			_attMAG.Print(objWriter);
			_attRES.Print(objWriter);
			if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
			{
				objWriter.WriteElementString("signal", _intSignal.ToString());
				objWriter.WriteElementString("response", _intResponse.ToString());
				objWriter.WriteElementString("system", System.ToString());
				objWriter.WriteElementString("firewall", Firewall.ToString());
				objWriter.WriteElementString("rating", Rating.ToString());
			}

			objWriter.WriteStartElement("attribute");
			objWriter.WriteElementString("name", "ESS");
			objWriter.WriteElementString("base", Essence.ToString());
			objWriter.WriteEndElement();

			// </attributes>
			objWriter.WriteEndElement();

			// <armorvalue />
			objWriter.WriteElementString("armorvalue", TotalArmorValue.ToString());

			// Condition Monitors.
			// <physicalcm />
			objWriter.WriteElementString("physicalcm", PhysicalCM.ToString());
			// <stuncm />
			objWriter.WriteElementString("stuncm", StunCM.ToString());

			// Condition Monitor Progress.
			// <physicalcmfilled />
			objWriter.WriteElementString("physicalcmfilled", _intPhysicalCMFilled.ToString());
			// <stuncmfilled />
			objWriter.WriteElementString("stuncmfilled", _intStunCMFilled.ToString());

			// <cmthreshold>
			objWriter.WriteElementString("cmthreshold", CMThreshold.ToString());
			// <cmthresholdoffset>
			objWriter.WriteElementString("cmthresholdoffset", CMThresholdOffset.ToString());
			// <cmoverflow>
			objWriter.WriteElementString("cmoverflow", CMOverflow.ToString());

			// Calculate Initiatives.
			// Initiative.
			// Start by adding INT and REA together.
			int intINI = _attINT.TotalValue + _attREA.TotalValue;
			// Add modifiers.
			intINI += _attINI.AttributeModifiers;
			// Add in any Initiative Improvements.
			intINI += _objImprovementManager.ValueOf(Improvement.ImprovementType.Initiative);
			// If INI exceeds the Metatype maximum set it back to the maximum.
			if (intINI > _attINI.MetatypeAugmentedMaximum)
				intINI = _attINI.MetatypeAugmentedMaximum;

			objWriter.WriteStartElement("init");
			objWriter.WriteElementString("base", (_attINT.Value + _attREA.Value).ToString());
			objWriter.WriteElementString("total", intINI.ToString());
			objWriter.WriteEndElement();

			// Initiative Dice.
			int intIP = 1 + Convert.ToInt32(_objImprovementManager.ValueOf(Improvement.ImprovementType.InitiativeDice));
			objWriter.WriteStartElement("ip");
			objWriter.WriteElementString("base", "1");
			objWriter.WriteElementString("total", intIP.ToString());
			objWriter.WriteEndElement();

			// Astral Initiative.
			if (_blnMAGEnabled)
			{
				int intAstralInit = _attINT.TotalValue * 2;

				objWriter.WriteStartElement("astralinit");
				objWriter.WriteElementString("base", intAstralInit.ToString());
				objWriter.WriteEndElement();

				objWriter.WriteStartElement("astralip");
				objWriter.WriteElementString("base", "3");
				objWriter.WriteEndElement();
			}

			// Matrix Initiative.
			objWriter.WriteStartElement("matrixinit");
			objWriter.WriteElementString("base", MatrixInitiative);
			objWriter.WriteEndElement();

			objWriter.WriteStartElement("matrixip");
			objWriter.WriteElementString("base", MatrixInitiativePasses);
			objWriter.WriteEndElement();

			// <magenabled />
			objWriter.WriteElementString("magenabled", _blnMAGEnabled.ToString());
			// <initiategrade />
			objWriter.WriteElementString("initiategrade", _intInitiateGrade.ToString());
			// <resenabled />
			objWriter.WriteElementString("resenabled", _blnRESEnabled.ToString());
			// <submersiongrade />
			objWriter.WriteElementString("submersiongrade", _intSubmersionGrade.ToString());
			// <groupmember />
			objWriter.WriteElementString("groupmember", _blnGroupMember.ToString());
			// <groupname />
			objWriter.WriteElementString("groupname", _strGroupName);
			// <groupnotes />
			objWriter.WriteElementString("groupnotes", _strGroupNotes);

			// <composure />
			objWriter.WriteElementString("composure", Composure.ToString());
			// <judgeintentions />
			objWriter.WriteElementString("judgeintentions", JudgeIntentions.ToString());
			// <liftandcarry />
			objWriter.WriteElementString("liftandcarry", LiftAndCarry.ToString());
			// <memory />
			objWriter.WriteElementString("memory", Memory.ToString());
			// <liftweight />
			objWriter.WriteElementString("liftweight", (_attSTR.TotalValue * 15).ToString());
			// <carryweight />
			objWriter.WriteElementString("carryweight", (_attSTR.TotalValue * 10).ToString());

			// Staple on the alternate Leadership Skills.
			Skill objLeadership = new Skill(this);
			Skill objLeadershipCommand = new Skill(this);
			Skill objLeadershipDirect = new Skill(this);

			if (_objOptions.PrintLeadershipAlternates)
			{
				foreach (Skill objSkill in _lstSkills)
				{
					if (objSkill.Name == "Leadership")
					{
						objLeadership = objSkill;
						break;
					}
				}

				// Leadership, Command.
				objLeadershipCommand.Name = objLeadership.DisplayName + ", " + LanguageManager.Instance.GetString("String_SkillCommand");
				objLeadershipCommand.SkillGroup = objLeadership.SkillGroup;
				objLeadershipCommand.SkillCategory = objLeadership.SkillCategory;
				objLeadershipCommand.IsGrouped = objLeadership.IsGrouped;
				objLeadershipCommand.Default = objLeadership.Default;
				objLeadershipCommand.Rating = objLeadership.Rating;
				objLeadershipCommand.RatingMaximum = objLeadership.RatingMaximum;
				objLeadershipCommand.KnowledgeSkill = objLeadership.KnowledgeSkill;
				objLeadershipCommand.ExoticSkill = objLeadership.ExoticSkill;
				objLeadershipCommand.Specialization = objLeadership.Specialization;
				objLeadershipCommand.Attribute = "LOG";
				objLeadershipCommand.Source = objLeadership.Source;
				objLeadershipCommand.Page = objLeadership.Page;
				_lstSkills.Add(objLeadershipCommand);

				// Leadership, Direct Fire
				objLeadershipDirect.Name = objLeadership.DisplayName + ", " + LanguageManager.Instance.GetString("String_SkillDirectFire");
				objLeadershipDirect.SkillGroup = objLeadership.SkillGroup;
				objLeadershipDirect.SkillCategory = objLeadership.SkillCategory;
				objLeadershipDirect.IsGrouped = objLeadership.IsGrouped;
				objLeadershipDirect.Default = objLeadership.Default;
				objLeadershipDirect.Rating = objLeadership.Rating;
				objLeadershipDirect.RatingMaximum = objLeadership.RatingMaximum;
				objLeadershipDirect.KnowledgeSkill = objLeadership.KnowledgeSkill;
				objLeadershipDirect.ExoticSkill = objLeadership.ExoticSkill;
				objLeadershipDirect.Specialization = objLeadership.Specialization;
				objLeadershipDirect.Attribute = "INT";
				objLeadershipDirect.Source = objLeadership.Source;
				objLeadershipDirect.Page = objLeadership.Page;
				_lstSkills.Add(objLeadershipDirect);
			}

			// Staple on the alternate Arcana Skills.
			Skill objArcana = new Skill(this);
			Skill objArcanaMetamagic = new Skill(this);
			Skill objArcanaArtificing = new Skill(this);

			if (_objOptions.PrintArcanaAlternates)
			{
				foreach (Skill objSkill in _lstSkills)
				{
					if (objSkill.Name == "Arcana")
					{
						objArcana = objSkill;
						break;
					}
				}
				// Arcana, Metamagic.
				objArcanaMetamagic.Name = objArcana.DisplayName + ", " + LanguageManager.Instance.GetString("String_SkillMetamagic");
				objArcanaMetamagic.SkillGroup = objArcana.SkillGroup;
				objArcanaMetamagic.SkillCategory = objArcana.SkillCategory;
				objArcanaMetamagic.IsGrouped = objArcana.IsGrouped;
				objArcanaMetamagic.Default = objArcana.Default;
				objArcanaMetamagic.Rating = objArcana.Rating;
				objArcanaMetamagic.RatingMaximum = objArcana.RatingMaximum;
				objArcanaMetamagic.KnowledgeSkill = objArcana.KnowledgeSkill;
				objArcanaMetamagic.ExoticSkill = objArcana.ExoticSkill;
				objArcanaMetamagic.Specialization = objArcana.Specialization;
				objArcanaMetamagic.Attribute = "INT";
				objArcanaMetamagic.Source = objArcana.Source;
				objArcanaMetamagic.Page = objArcana.Page;
				_lstSkills.Add(objArcanaMetamagic);

				// Arcana, Artificing
				objArcanaArtificing.Name = objArcana.DisplayName + ", " + LanguageManager.Instance.GetString("String_SkillArtificing");
				objArcanaArtificing.SkillGroup = objArcana.SkillGroup;
				objArcanaArtificing.SkillCategory = objArcana.SkillCategory;
				objArcanaArtificing.IsGrouped = objArcana.IsGrouped;
				objArcanaArtificing.Default = objArcana.Default;
				objArcanaArtificing.Rating = objArcana.Rating;
				objArcanaArtificing.RatingMaximum = objArcana.RatingMaximum;
				objArcanaArtificing.KnowledgeSkill = objArcana.KnowledgeSkill;
				objArcanaArtificing.ExoticSkill = objArcana.ExoticSkill;
				objArcanaArtificing.Specialization = objArcana.Specialization;
				objArcanaArtificing.Attribute = "MAG";
				objArcanaArtificing.Source = objArcana.Source;
				objArcanaArtificing.Page = objArcana.Page;
				_lstSkills.Add(objArcanaArtificing);
			}

			// <skills>
			objWriter.WriteStartElement("skills");
			foreach (Skill objSkill in _lstSkills)
			{
				if (_objOptions.PrintSkillsWithZeroRating || (!_objOptions.PrintSkillsWithZeroRating && objSkill.Rating > 0) || objSkill.KnowledgeSkill)
					objSkill.Print(objWriter);
			}
			// </skills>
			objWriter.WriteEndElement();

			// Remove the stapled on Leadership Skills now that we're done with them.
			if (_objOptions.PrintLeadershipAlternates)
			{
				_lstSkills.Remove(objLeadershipCommand);
				_lstSkills.Remove(objLeadershipDirect);
			}

			// Remove the stapled on Arcana Skills now that we're done with them.
			if (_objOptions.PrintArcanaAlternates)
			{
				_lstSkills.Remove(objArcanaMetamagic);
				_lstSkills.Remove(objArcanaArtificing);
			}

			// <contacts>
			objWriter.WriteStartElement("contacts");
			foreach (Contact objContact in _lstContacts)
			{
				objContact.Print(objWriter);
			}
			// </contacts>
			objWriter.WriteEndElement();

			// <spells>
			objWriter.WriteStartElement("spells");
			foreach (Spell objSpell in _lstSpells)
			{
				objSpell.Print(objWriter);
			}
			// </spells>
			objWriter.WriteEndElement();

			// <powers>
			objWriter.WriteStartElement("powers");
			foreach (Power objPower in _lstPowers)
			{
				objPower.Print(objWriter);
			}
			// </powers>
			objWriter.WriteEndElement();

			// <spirits>
			objWriter.WriteStartElement("spirits");
			foreach (Spirit objSpirit in _lstSpirits)
			{
				objSpirit.Print(objWriter);
			}
			// </spirits>
			objWriter.WriteEndElement();

			// <techprograms>
			objWriter.WriteStartElement("techprograms");
			foreach (TechProgram objProgram in _lstTechPrograms)
			{
				objProgram.Print(objWriter);
			}
			// </techprograms>
			objWriter.WriteEndElement();

			// <martialarts>
			objWriter.WriteStartElement("martialarts");
			foreach (MartialArt objMartialArt in _lstMartialArts)
			{
				objMartialArt.Print(objWriter);
			}
			// </martialarts>
			objWriter.WriteEndElement();

			// <martialartmaneuvers>
			objWriter.WriteStartElement("martialartmaneuvers");
			foreach (MartialArtManeuver objManeuver in _lstMartialArtManeuvers)
			{
				objManeuver.Print(objWriter);
			}
			// </martialartmaneuvers>
			objWriter.WriteEndElement();

			// <armors>
			objWriter.WriteStartElement("armors");
			foreach (Armor objArmor in _lstArmor)
			{
				objArmor.Print(objWriter);
			}
			// </armors>
			objWriter.WriteEndElement();

			// <weapons>
			objWriter.WriteStartElement("weapons");
			foreach (Weapon objWeapon in _lstWeapons)
			{
				objWeapon.Print(objWriter);
			}
			// </weapons>
			objWriter.WriteEndElement();

			// <cyberwares>
			objWriter.WriteStartElement("cyberwares");
			foreach (Cyberware objCyberware in _lstCyberware)
			{
				objCyberware.Print(objWriter);
			}
			// </cyberwares>
			objWriter.WriteEndElement();

			// Load the Qualities file so we can figure out whether or not each Quality should be printed.
			objXmlDocument = XmlManager.Instance.Load("qualities.xml");

			// <qualities>
			objWriter.WriteStartElement("qualities");
			foreach (Quality objQuality in _lstQualities)
			{
				objQuality.Print(objWriter);
			}
			// </qualities>
			objWriter.WriteEndElement();

			// <lifestyles>
			objWriter.WriteStartElement("lifestyles");
			foreach (Lifestyle objLifestyle in _lstLifestyles)
			{
				objLifestyle.Print(objWriter);
			}
			// </lifestyles>
			objWriter.WriteEndElement();

			// <gears>
			objWriter.WriteStartElement("gears");
			foreach (Gear objGear in _lstGear)
			{
				// Use the Gear's SubClass if applicable.
				if (objGear.GetType() == typeof(Commlink))
				{
					Commlink objCommlink = new Commlink(this);
					objCommlink = (Commlink)objGear;
					objCommlink.Print(objWriter);
				}
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					OperatingSystem objOperatinSystem = new OperatingSystem(this);
					objOperatinSystem = (OperatingSystem)objGear;
					objOperatinSystem.Print(objWriter);
				}
				else
				{
					objGear.Print(objWriter);
				}
			}
			// If the character is a Technomander, write out the Living Persona "Commlink".
			if (_blnTechnomancerEnabled)
			{
				int intFirewall = _attWIL.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaFirewall);
				int intResponse = _attINT.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaResponse);
				int intSignal = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(_attRES.TotalValue, GlobalOptions.Instance.CultureInfo) / 2))) + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaSignal);
				int intSystem = _attLOG.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaSystem);

				// Make sure none of the Attributes exceed the Technomancer's RES.
				intFirewall = Math.Min(intFirewall, _attRES.TotalValue);
				intResponse = Math.Min(intResponse, _attRES.TotalValue);
				intSignal = Math.Min(intSignal, _attRES.TotalValue);
				intSystem = Math.Min(intSystem, _attRES.TotalValue);

				Commlink objLivingPersona = new Commlink(this);
				objLivingPersona.Name = LanguageManager.Instance.GetString("String_LivingPersona");
				objLivingPersona.Category = LanguageManager.Instance.GetString("String_Commlink");
				objLivingPersona.Response = intResponse;
				objLivingPersona.Signal = intSignal;
				objLivingPersona.Source = _objOptions.LanguageBookShort("SR5");
				objLivingPersona.Page = "239";
				objLivingPersona.IsLivingPersona = true;

				OperatingSystem objLivingPersonaOS = new OperatingSystem(this);
				objLivingPersonaOS.Name = LanguageManager.Instance.GetString("String_LivingPersona");
				objLivingPersonaOS.Category = LanguageManager.Instance.GetString("String_CommlinkOperatingSystem");
				objLivingPersonaOS.Firewall = intFirewall;
				objLivingPersonaOS.System = intSystem;
				objLivingPersonaOS.Source = _objOptions.LanguageBookShort("SR5");
				objLivingPersonaOS.Page = "239";

				Gear objLivingPersonaFilter = new Gear(this);
				objLivingPersonaFilter.Name = LanguageManager.Instance.GetString("String_BiofeedbackFilter");
				objLivingPersonaFilter.Category = LanguageManager.Instance.GetString("String_LivingPersonaGear");
				objLivingPersonaFilter.MaxRating = _attCHA.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaBiofeedback);
				objLivingPersonaFilter.Rating = _attCHA.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaBiofeedback);
				objLivingPersonaFilter.Source = _objOptions.LanguageBookShort("SR5");
				objLivingPersonaFilter.Page = "239";

				objLivingPersona.Gears.Add(objLivingPersonaOS);
				objLivingPersona.Gears.Add(objLivingPersonaFilter);

				objLivingPersona.Print(objWriter);
			}
			// </gears>
			objWriter.WriteEndElement();

			// <vehicles>
			objWriter.WriteStartElement("vehicles");
			foreach (Vehicle objVehicle in _lstVehicles)
			{
				objVehicle.Print(objWriter);
			}
			// </vehicles>
			objWriter.WriteEndElement();

			// <metamagics>
			objWriter.WriteStartElement("metamagics");
			foreach (Metamagic objMetamagic in _lstMetamagics)
			{
				objMetamagic.Print(objWriter);
			}
			// </metamagics>
			objWriter.WriteEndElement();

			// <critterpowers>
			objWriter.WriteStartElement("critterpowers");
			foreach (CritterPower objPower in _lstCritterPowers)
			{
				objPower.Print(objWriter);
			}
			// </critterpowers>
			objWriter.WriteEndElement();

			// Print the Expense Log Entries if the option is enabled.
			if (_objOptions.PrintExpenses)
			{
				// <expenses>
				objWriter.WriteStartElement("expenses");
				_lstExpenseLog.Sort(ExpenseLogEntry.CompareDate);
				foreach (ExpenseLogEntry objExpense in _lstExpenseLog)
					objExpense.Print(objWriter);
				// </expenses>
				objWriter.WriteEndElement();
			}

			// </character>
			objWriter.WriteEndElement();
		}

		/// <summary>
		/// Print this character and open the View Character window.
		/// </summary>
		/// <param name="blnDialog">Whether or not the window should be shown as a dialogue window.</param>
		public void Print(bool blnDialog = true)
		{
			// Write the Character information to a MemoryStream so we don't need to create any files.
			MemoryStream objStream = new MemoryStream();
			XmlTextWriter objWriter = new XmlTextWriter(objStream, Encoding.UTF8);

			// Being the document.
			objWriter.WriteStartDocument();

			// </characters>
			objWriter.WriteStartElement("characters");

			PrintToStream(objStream, objWriter);

			// </characters>
			objWriter.WriteEndElement();

			// Finish the document and flush the Writer and Stream.
			objWriter.WriteEndDocument();
			objWriter.Flush();
			objStream.Flush();

			// Read the stream.
			StreamReader objReader = new StreamReader(objStream);
			objStream.Position = 0;
			XmlDocument objCharacterXML = new XmlDocument();

			// Put the stream into an XmlDocument and send it off to the Viewer.
			string strXML = objReader.ReadToEnd();
			objCharacterXML.LoadXml(strXML);

			objWriter.Close();
			objStream.Close();

			// If a reference to the Viewer window does not yet exist for this character, open a new Viewer window and set the reference to it.
			// If a Viewer window already exists for this character, use it instead.
			if (_frmPrintView == null)
			{
				List<Character> lstCharacters = new List<Character>();
				lstCharacters.Add(this);
				frmViewer frmViewCharacter = new frmViewer();
				frmViewCharacter.Characters = lstCharacters;
				frmViewCharacter.CharacterXML = objCharacterXML;
				_frmPrintView = frmViewCharacter;
				if (blnDialog)
					frmViewCharacter.ShowDialog();
				else
					frmViewCharacter.Show();
			}
			else
			{
				_frmPrintView.Activate();
				_frmPrintView.RefreshView();
			}
		}

		/// <summary>
		/// Reset all of the Character information and start from scratch.
		/// </summary>
		private void ResetCharacter()
		{
			_intBuildPoints = 400;
			_intKnowledgeSkillPoints = 0;
			_decNuyenMaximumBP = 50m;
			_intKarma = 0;

			// Reset Metatype Information.
			_strMetatype = "";
            _guiMetatype = Guid.Empty;
			_strMetavariant = "";
			_strMetatypeCategory = "";
			_intMetatypeBP = 0;
			_intMutantCritterBaseSkills = 0;
            _intWalk = 0;
            _intRun = 0;
            _intSprint = 0;

			// Reset Special Tab Flags.
			_blnAdeptEnabled = false;
			_blnMagicianEnabled = false;
			_blnTechnomancerEnabled = false;
			_blnInitiationEnabled = false;
			_blnCritterEnabled = false;
		
			// Reset Attributes.
			_attBOD = new Attribute("BOD");
			_attBOD._objCharacter = this;
			_attAGI = new Attribute("AGI");
			_attAGI._objCharacter = this;
			_attREA = new Attribute("REA");
			_attREA._objCharacter = this;
			_attSTR = new Attribute("STR");
			_attSTR._objCharacter = this;
			_attCHA = new Attribute("CHA");
			_attCHA._objCharacter = this;
			_attINT = new Attribute("INT");
			_attINT._objCharacter = this;
			_attLOG = new Attribute("LOG");
			_attLOG._objCharacter = this;
			_attWIL = new Attribute("WIL");
			_attWIL._objCharacter = this;
			_attINI = new Attribute("INI");
			_attINI._objCharacter = this;
			_attEDG = new Attribute("EDG");
			_attEDG._objCharacter = this;
			_attMAG = new Attribute("MAG");
			_attMAG._objCharacter = this;
			_attRES = new Attribute("RES");
			_attRES._objCharacter = this;
			_attESS = new Attribute("ESS");
			_attESS._objCharacter = this;
			_blnMAGEnabled = false;
			_blnRESEnabled = false;
			_blnGroupMember = false;
			_strGroupName = "";
			_strGroupNotes = "";
			_intInitiateGrade = 0;
			_intSubmersionGrade = 0;

			_intMAGAdept = 0;
			_intMAGMagician = 0;
			_guiMagicTradition = Guid.Empty;
			_guiTechnomancerStream = Guid.Empty;

			// Reset all of the Lists.
			_lstImprovements = new List<Improvement>();
			_lstSkills = new List<Skill>();
			_lstSkillGroups = new List<SkillGroup>();
			_lstContacts = new List<Contact>();
			_lstSpirits = new List<Spirit>();
			_lstSpells = new List<Spell>();
			_lstFoci = new List<Focus>();
			_lstStackedFoci = new List<StackedFocus>();
			_lstPowers = new List<Power>();
			_lstTechPrograms = new List<TechProgram>();
			_lstMartialArts = new List<MartialArt>();
			_lstMartialArtManeuvers = new List<MartialArtManeuver>();
			_lstArmor = new List<Equipment>();
			_lstCyberware = new List<Equipment>();
			_lstMetamagics = new List<Metamagic>();
			_lstWeapons = new List<Equipment>();
			_lstLifestyles = new List<Lifestyle>();
			_lstGear = new List<Equipment>();
			_lstVehicles = new List<Equipment>();
			_lstExpenseLog = new List<ExpenseLogEntry>();
			_lstCritterPowers = new List<CritterPower>();
			_lstInitiationGrades = new List<InitiationGrade>();
			_lstQualities = new List<Quality>();
			_lstOldQualities = new List<string>();
			_lstCalendar = new List<CalendarWeek>();

			BuildSkillList();
			BuildSkillGroupList();
		}
		#endregion

		#region Helper Methods
		/// <summary>
		/// Build the list of Skill Groups.
		/// </summary>
		private void BuildSkillGroupList()
		{
			XmlDocument objXmlDocument = XmlManager.Instance.Load("skills.xml");

			// Populate the Skill Group list.
			XmlNodeList objXmlGroupList = objXmlDocument.SelectNodes("/chummer/skillgroups/name");

			// First pass, build up a list of all of the Skill Groups so we can sort them in alphabetical order for the current language.
			List<ListItem> lstSkillOrder = new List<ListItem>();
			foreach (XmlNode objXmlGroup in objXmlGroupList)
			{
				ListItem objGroup = new ListItem();
				objGroup.Value = objXmlGroup.InnerText;
				if (objXmlGroup.Attributes["translate"] != null)
					objGroup.Name = objXmlGroup.Attributes["translate"].InnerText;
				else
					objGroup.Name = objXmlGroup.InnerText;
				lstSkillOrder.Add(objGroup);
			}
			SortListItem objSort = new SortListItem();
			lstSkillOrder.Sort(objSort.Compare);

			// Second pass, retrieve the Skill Groups in the order they're presented in the list.
			foreach (ListItem objItem in lstSkillOrder)
			{
				XmlNode objXmlGroup = objXmlDocument.SelectSingleNode("/chummer/skillgroups/name[. = \"" + objItem.Value + "\"]");
				SkillGroup objGroup = new SkillGroup();
				objGroup.Name = objXmlGroup.InnerText;
				// If rules are ignored, then Skill Groups can go up to a maximum Rating of 6.
				if (!_blnIgnoreRules && !_blnCreated)
					objGroup.RatingMaximum = 4;
				else
					objGroup.RatingMaximum = 6;
				_lstSkillGroups.Add(objGroup);
			}
		}

		/// <summary>
		/// Buid the list of Skills.
		/// </summary>
		private void BuildSkillList()
		{
			// Load the Skills information.
			XmlDocument objXmlDocument = XmlManager.Instance.Load("skills.xml");

			// Populate the Skills list.
            XmlNodeList objXmlSkillList = objXmlDocument.SelectNodes("/chummer/skills/skill[not(exotic) and (" + Options.BookXPath() + ")]");

			// First pass, build up a list of all of the Skills so we can sort them in alphabetical order for the current language.
			List<ListItem> lstSkillOrder = new List<ListItem>();
			foreach (XmlNode objXmlSkill in objXmlSkillList)
			{
				ListItem objSkill = new ListItem();
				objSkill.Value = objXmlSkill["name"].InnerText;
				if (objXmlSkill["translate"] != null)
					objSkill.Name = objXmlSkill["translate"].InnerText;
				else
					objSkill.Name = objXmlSkill["name"].InnerText;
				lstSkillOrder.Add(objSkill);
			}
			SortListItem objSort = new SortListItem();
			lstSkillOrder.Sort(objSort.Compare);

			// Second pass, retrieve the Skills in the order they're presented in the list.
			foreach (ListItem objItem in lstSkillOrder)
			{
				XmlNode objXmlSkill = objXmlDocument.SelectSingleNode("/chummer/skills/skill[name = \"" + objItem.Value + "\"]");
				Skill objSkill = new Skill(this);
				objSkill.Name = objXmlSkill["name"].InnerText;
				objSkill.SkillCategory = objXmlSkill["category"].InnerText;
				objSkill.SkillGroup = objXmlSkill["skillgroup"].InnerText;
				objSkill.Attribute = objXmlSkill["attribute"].InnerText;
				if (objXmlSkill["default"].InnerText.ToLower() == "yes")
					objSkill.Default = true;
				else
					objSkill.Default = false;
				if (objXmlSkill["source"] != null)
					objSkill.Source = objXmlSkill["source"].InnerText;
				if (objXmlSkill["page"] != null)
					objSkill.Page = objXmlSkill["page"].InnerText;
				_lstSkills.Add(objSkill);
			}
		}

		/// <summary>
		/// Retrieve the name of the Object that created an Improvement.
		/// </summary>
		/// <param name="objImprovement">Improvement to check.</param>
		public string GetObjectName(Improvement objImprovement)
		{
			string strReturn = "";
			switch (objImprovement.ImproveSource)
			{
				case Improvement.ImprovementSource.Bioware:
				case Improvement.ImprovementSource.Cyberware:
					foreach (Cyberware objCyberware in _lstCyberware)
					{
						if (objCyberware.InternalId == objImprovement.SourceName)
						{
							strReturn = objCyberware.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.Gear:
					foreach (Gear objGear in _lstGear)
					{
						if (objGear.InternalId == objImprovement.SourceName)
						{
							strReturn = objGear.DisplayNameShort;
							break;
						}
						else
						{
							foreach (Gear objChild in objGear.Gears)
							{
								if (objChild.InternalId == objImprovement.SourceName)
								{
									strReturn = objChild.DisplayNameShort;
									break;
								}
								else
								{
									foreach (Gear objSubChild in objChild.Gears)
									{
										if (objSubChild.InternalId == objImprovement.SourceName)
										{
											strReturn = objSubChild.DisplayNameShort;
											break;
										}
									}
								}
							}
						}
					}
					break;
				case Improvement.ImprovementSource.Spell:
					foreach (Spell objSpell in _lstSpells)
					{
						if (objSpell.InternalId == objImprovement.SourceName)
						{
							strReturn = objSpell.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.Power:
					foreach (Power objPower in _lstPowers)
					{
						if (objPower.InternalId == objImprovement.SourceName)
						{
							strReturn = objPower.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.CritterPower:
					foreach (CritterPower objPower in _lstCritterPowers)
					{
						if (objPower.InternalId == objImprovement.SourceName)
						{
							strReturn = objPower.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.Metamagic:
				case Improvement.ImprovementSource.Echo:
					foreach (Metamagic objMetamagic in _lstMetamagics)
					{
						if (objMetamagic.InternalId == objImprovement.SourceName)
						{
							strReturn = objMetamagic.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.Armor:
					foreach (Armor objArmor in _lstArmor)
					{
						if (objArmor.InternalId == objImprovement.SourceName)
						{
							strReturn = objArmor.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.ArmorMod:
					foreach (Armor objArmor in _lstArmor)
					{
						foreach (ArmorMod objMod in objArmor.ArmorMods)
						{
							if (objMod.InternalId == objImprovement.SourceName)
							{
								strReturn = objMod.DisplayNameShort;
								break;
							}
						}
					}
					break;
				case Improvement.ImprovementSource.ComplexForm:
					foreach (TechProgram objProgram in _lstTechPrograms)
					{
						if (objProgram.InternalId == objImprovement.SourceName)
						{
							strReturn = objProgram.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.Quality:
					foreach (Quality objQuality in _lstQualities)
					{
						if (objQuality.InternalId == objImprovement.SourceName)
						{
							strReturn = objQuality.DisplayNameShort;
							break;
						}
					}
					break;
				case Improvement.ImprovementSource.MartialArtAdvantage:
					foreach (MartialArt objMartialArt in _lstMartialArts)
					{
						foreach (MartialArtAdvantage objAdvantage in objMartialArt.Advantages)
						{
							if (objAdvantage.InternalId == objImprovement.SourceName)
							{
								strReturn = objAdvantage.DisplayNameShort;
								break;
							}
						}
					}
					break;
				default:
					if (objImprovement.SourceName == "Armor Encumbrance")
						strReturn = LanguageManager.Instance.GetString("String_ArmorEncumbrance");
					else
					{
						// If this comes from a custom Improvement, use the name the player gave it instead of showing a GUID.
						if (objImprovement.CustomName != string.Empty)
							strReturn = objImprovement.CustomName;
						else
							strReturn = objImprovement.SourceName;
					}
					break;
			}
			return strReturn;
		}

		/// <summary>
		/// Clean an XPath string.
		/// </summary>
		/// <param name="strValue">String to clean.</param>
		private string CleanXPath(string strValue)
		{
			string strReturn = string.Empty;
			string strSearch = strValue;
			char[] chrQuotes = new char[] { '\'', '"' };

			int intQuotePos = strSearch.IndexOfAny(chrQuotes);
			if (intQuotePos == -1)
			{
				strReturn = "'" + strSearch + "'";
			}
			else
			{
				strReturn = "concat(";
				while (intQuotePos != -1)
				{
					string strSubstring = strSearch.Substring(0, intQuotePos);
					strReturn += "'" + strSubstring + "', ";
					if (strSearch.Substring(intQuotePos, 1) == "'")
					{
						strReturn += "\"'\", ";
					}
					else
					{
						//must be a double quote
						strReturn += "'\"', ";
					}
					strSearch = strSearch.Substring(intQuotePos + 1, strSearch.Length - intQuotePos - 1);
					intQuotePos = strSearch.IndexOfAny(chrQuotes);
				}
				strReturn += "'" + strSearch + "')";
			}
			return strReturn;

		}
		#endregion

		#region Basic Properties
		/// <summary>
		/// Character Options object.
		/// </summary>
		public CharacterOptions Options
		{
			get
			{
				return _objOptions;
			}
		}

		/// <summary>
		/// Name of the file the Character is saved to.
		/// </summary>
		public string FileName
		{
			get
			{
				return _strFileName;
			}
			set
			{
				_strFileName = value;
			}
		}

		/// <summary>
		/// Name of the settings file the Character uses. 
		/// </summary>
		public string SettingsFile
		{
			get
			{
				return _strSettingsFileName;
			}
			set
			{
				_strSettingsFileName = value;
				_objOptions.Load(_strSettingsFileName);
			}
		}

		/// <summary>
		/// Whether or not the character has been saved as Created and can no longer be modified using the Build system.
		/// </summary>
		public bool Created
		{
			get
			{
				return _blnCreated;
			}
			set
			{
				_blnCreated = value;
			}
		}

		/// <summary>
		/// Character's name.
		/// </summary>
		public string Name
		{
			get
			{
				return _strName;
			}
			set
			{
				_strName = value;
				try
				{
					CharacterNameChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Character's portrait encoded using Base64.
		/// </summary>
		public string Mugshot
		{
			get
			{
				return _strMugshot;
			}
			set
			{
				_strMugshot = value;
			}
		}

		/// <summary>
		/// Character's sex.
		/// </summary>
		public string Sex
		{
			get
			{
				return _strSex;
			}
			set
			{
				_strSex = value;
			}
		}

		/// <summary>
		/// Character's age.
		/// </summary>
		public string Age
		{
			get
			{
				return _strAge;
			}
			set
			{
				_strAge = value;
			}
		}

		/// <summary>
		/// Character's eyes.
		/// </summary>
		public string Eyes
		{
			get
			{
				return _strEyes;
			}
			set
			{
				_strEyes = value;
			}
		}

		/// <summary>
		/// Character's height.
		/// </summary>
		public string Height
		{
			get
			{
				return _strHeight;
			}
			set
			{
				_strHeight = value;
			}
		}

		/// <summary>
		/// Character's weight.
		/// </summary>
		public string Weight
		{
			get
			{
				return _strWeight;
			}
			set
			{
				_strWeight = value;
			}
		}

		/// <summary>
		/// Character's skin.
		/// </summary>
		public string Skin
		{
			get
			{
				return _strSkin;
			}
			set
			{
				_strSkin = value;
			}
		}

		/// <summary>
		/// Character's hair.
		/// </summary>
		public string Hair
		{
			get
			{
				return _strHair;
			}
			set
			{
				_strHair = value;
			}
		}

		/// <summary>
		/// Character's description.
		/// </summary>
		public string Description
		{
			get
			{
				return _strDescription;
			}
			set
			{
				_strDescription = value;
			}
		}

		/// <summary>
		/// Character's background.
		/// </summary>
		public string Background
		{
			get
			{
				return _strBackground;
			}
			set
			{
				_strBackground = value;
			}
		}

		/// <summary>
		/// Character's concept.
		/// </summary>
		public string Concept
		{
			get
			{
				return _strConcept;
			}
			set
			{
				_strConcept = value;
			}
		}

		/// <summary>
		/// Character notes.
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
		/// General gameplay notes.
		/// </summary>
		public string GameNotes
		{
			get
			{
				return _strGameNotes;
			}
			set
			{
				_strGameNotes = value;
			}
		}

		/// <summary>
		/// Player name.
		/// </summary>
		public string PlayerName
		{
			get
			{
				return _strPlayerName;
			}
			set
			{
				_strPlayerName = value;
			}
		}

		/// <summary>
		/// Character's alias.
		/// </summary>
		public string Alias
		{
			get
			{
				return _strAlias;
			}
			set
			{
				_strAlias = value;
				try
				{
					CharacterNameChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Street Cred.
		/// </summary>
		public int StreetCred
		{
			get
			{
				return _intStreetCred;
			}
			set
			{
				_intStreetCred = value;
			}
		}

		/// <summary>
		/// Burnt Street Cred.
		/// </summary>
		public int BurntStreetCred
		{
			get
			{
				return _intBurntStreetCred;
			}
			set
			{
				_intBurntStreetCred = value;
			}
		}

		/// <summary>
		/// Notoriety.
		/// </summary>
		public int Notoriety
		{
			get
			{
				return _intNotoriety;
			}
			set
			{
				_intNotoriety = value;
			}
		}

		/// <summary>
		/// Public Awareness.
		/// </summary>
		public int PublicAwareness
		{
			get
			{
				return _intPublicAwareness;
			}
			set
			{
				_intPublicAwareness = value;
			}
		}

		/// <summary>
		/// Number of Physical Condition Monitor Boxes that are filled.
		/// </summary>
		public int PhysicalCMFilled
		{
			get
			{
				return _intPhysicalCMFilled;
			}
			set
			{
				_intPhysicalCMFilled = value;
			}
		}

		/// <summary>
		/// Number of Stun Condition Monitor Boxes that are filled.
		/// </summary>
		public int StunCMFilled
		{
			get
			{
				return _intStunCMFilled;
			}
			set
			{
				_intStunCMFilled = value;
			}
		}

		/// <summary>
		/// Whether or not character creation rules should be ignored.
		/// </summary>
		public bool IgnoreRules
		{
			get
			{
				return _blnIgnoreRules;
			}
			set
			{
				_blnIgnoreRules = value;
			}
		}

		/// <summary>
		/// Karma.
		/// </summary>
		public int Karma
		{
			get
			{
				return _intKarma;
			}
			set
			{
				_intKarma = value;
			}
		}

		/// <summary>
		/// Total amount of Karma the character has earned over the career.
		/// </summary>
		public int CareerKarma
		{
			get
			{
				int intKarma = 0;

				foreach (ExpenseLogEntry objEntry in _lstExpenseLog)
				{
					// Since we're only interested in the amount they have earned, only count values that are greater than 0 and are not refunds.
					if (objEntry.Type == ExpenseType.Karma && objEntry.Amount > 0 && objEntry.Refund == false)
						intKarma += objEntry.Amount;
				}

				return intKarma;
			}
		}

		/// <summary>
		/// Total amount of Nuyen the character has earned over the career.
		/// </summary>
		public int CareerNuyen
		{
			get
			{
				int intNuyen = 0;

				foreach (ExpenseLogEntry objEntry in _lstExpenseLog)
				{
					// Since we're only interested in the amount they have earned, only count values that are greater than 0 and are not refunds.
					if (objEntry.Type == ExpenseType.Nuyen && objEntry.Amount > 0 && objEntry.Refund == false)
						intNuyen += objEntry.Amount;
				}

				return intNuyen;
			}
		}

		/// <summary>
		/// Whether or not the character is a Critter.
		/// </summary>
		public bool IsCritter
		{
			get
			{
				return _blnIsCritter;
			}
			set
			{
				_blnIsCritter = value;
			}
		}

		/// <summary>
		/// Whether or not the character is possessed by a Spirit.
		/// </summary>
		public bool Possessed
		{
			get
			{
				return _blnPossessed;
			}
			set
			{
				_blnPossessed = value;
			}
		}

		/// <summary>
		/// Maximum item Availability for new characters.
		/// </summary>
		public int MaximumAvailability
		{
			get
			{
				return _intMaxAvail;
			}
			set
			{
				_intMaxAvail = value;
			}
		}
		#endregion

		#region Attributes
		/// <summary>
		/// Get an Attribute by its name.
		/// </summary>
		/// <param name="strAttribute">Attribute name to retrieve.</param>
		public Attribute GetAttribute(string strAttribute)
		{
			switch (strAttribute)
			{
				case "BOD":
				case "BODBase":
					return _attBOD;
				case "AGI":
				case "AGIBase":
					return _attAGI;
				case "REA":
				case "REABase":
					return _attREA;
				case "STR":
				case "STRBase":
					return _attSTR;
				case "CHA":
				case "CHABase":
					return _attCHA;
				case "INT":
				case "INTBase":
					return _attINT;
				case "LOG":
				case "LOGBase":
					return _attLOG;
				case "WIL":
				case "WILBase":
					return _attWIL;
				case "INI":
					return _attINI;
				case "EDG":
				case "EDGBase":
					return _attEDG;
				case "MAG":
				case "MAGBase":
					return _attMAG;
				case "RES":
				case "RESBase":
					return _attRES;
				case "ESS":
					return _attESS;
				default:
					return _attBOD;
			}
		}

		/// <summary>
		/// Body (BOD) Attribute.
		/// </summary>
		public Attribute BOD
		{
			get
			{
				return _attBOD;
			}
		}

		/// <summary>
		/// Agility (AGI) Attribute.
		/// </summary>
		public Attribute AGI
		{
			get
			{
				return _attAGI;
			}
		}

		/// <summary>
		/// Reaction (REA) Attribute.
		/// </summary>
		public Attribute REA
		{
			get
			{
				return _attREA;
			}
		}

		/// <summary>
		/// Strength (STR) Attribute.
		/// </summary>
		public Attribute STR
		{
			get
			{
				return _attSTR;
			}
		}

		/// <summary>
		/// Charisma (CHA) Attribute.
		/// </summary>
		public Attribute CHA
		{
			get
			{
				return _attCHA;
			}
		}

		/// <summary>
		/// Intuition (INT) Attribute.
		/// </summary>
		public Attribute INT
		{
			get
			{
				return _attINT;
			}
		}

		/// <summary>
		/// Logic (LOG) Attribute.
		/// </summary>
		public Attribute LOG
		{
			get
			{
				return _attLOG;
			}
		}

		/// <summary>
		/// Willpower (WIL) Attribute.
		/// </summary>
		public Attribute WIL
		{
			get
			{
				return _attWIL;
			}
		}

		/// <summary>
		/// Initiative (INI) Attribute.
		/// </summary>
		public Attribute INI
		{
			get
			{
				return _attINI;
			}
		}

		/// <summary>
		/// Edge (EDG) Attribute.
		/// </summary>
		public Attribute EDG
		{
			get
			{
				return _attEDG;
			}
		}

		/// <summary>
		/// Magic (MAG) Attribute.
		/// </summary>
		public Attribute MAG
		{
			get
			{
				return _attMAG;
			}
		}

		/// <summary>
		/// Resonance (RES) Attribute.
		/// </summary>
		public Attribute RES
		{
			get
			{
				return _attRES;
			}
		}

		/// <summary>
		/// Essence (ESS) Attribute.
		/// </summary>
		public Attribute ESS
		{
			get
			{
				return _attESS;
			}
		}

		/// <summary>
		/// Is the MAG Attribute enabled?
		/// </summary>
		public bool MAGEnabled
		{
			get
			{
				return _blnMAGEnabled;
			}
			set
			{
				bool blnOldValue = _blnMAGEnabled;
				_blnMAGEnabled = value;
				try
				{
					if (blnOldValue != value)
						MAGEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Amount of MAG invested in Adept for Mystic Adepts.
		/// </summary>
		public int MAGAdept
		{
			get
			{
				return _intMAGAdept;
			}
			set
			{
				_intMAGAdept = value;
			}
		}

		/// <summary>
		/// Amount of MAG invested in Magician for Mystic Adepts.
		/// </summary>
		public int MAGMagician
		{
			get
			{
				return _intMAGMagician;
			}
			set
			{
				_intMAGMagician = value;
			}
		}

		/// <summary>
		/// Magician's Tradition.
		/// </summary>
		public Guid MagicTradition
		{
			get
			{
				return _guiMagicTradition;
			}
			set
			{
				_guiMagicTradition = value;
			}
		}

		/// <summary>
		/// Technomancer's Stream.
		/// </summary>
		public Guid TechnomancerStream
		{
			get
			{
				return _guiTechnomancerStream;
			}
			set
			{
				_guiTechnomancerStream = value;
			}
		}

		/// <summary>
		/// Initiate Grade.
		/// </summary>
		public int InitiateGrade
		{
			get
			{
				return _intInitiateGrade;
			}
			set
			{
				_intInitiateGrade = value;
			}
		}

		/// <summary>
		/// Is the RES Attribute enabled?
		/// </summary>
		public bool RESEnabled
		{
			get
			{
				return _blnRESEnabled;
			}
			set
			{
				bool blnOldValue = _blnRESEnabled;
				_blnRESEnabled = value;
				try
				{
					if (blnOldValue != value)
						RESEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Submersion Grade.
		/// </summary>
		public int SubmersionGrade
		{
			get
			{
				return _intSubmersionGrade;
			}
			set
			{
				_intSubmersionGrade = value;
			}
		}

		/// <summary>
		/// Whether or not the character is a member of a Group or Network.
		/// </summary>
		public bool GroupMember
		{
			get
			{
				return _blnGroupMember;
			}
			set
			{
				_blnGroupMember = value;
			}
		}

		/// <summary>
		/// The name of the Group the Initiate has joined.
		/// </summary>
		public string GroupName
		{
			get
			{
				return _strGroupName;
			}
			set
			{
				_strGroupName = value;
			}
		}

		/// <summary>
		/// Notes for the Group the Initiate has joined.
		/// </summary>
		public string GroupNotes
		{
			get
			{
				return _strGroupNotes;
			}
			set
			{
				_strGroupNotes = value;
			}
		}

		/// <summary>
		/// Character's Essence.
		/// </summary>
		public decimal Essence
		{
			get
			{
				decimal decESS = Convert.ToDecimal(_attESS.MetatypeMaximum, GlobalOptions.Instance.CultureInfo) + Convert.ToDecimal(_objImprovementManager.ValueOf(Improvement.ImprovementType.Essence), GlobalOptions.Instance.CultureInfo);
				// Run through all of the pieces of Cyberware and include their Essence cost. Cyberware and Bioware costs are calculated separately. The higher value removes its full cost from the
				// character's ESS while the lower removes half of its cost from the character's ESS.
				decimal decCyberware = 0m;
				decimal decBioware = 0m;
				decimal decHole = 0m;
				foreach (Cyberware objCyberware in _lstCyberware)
				{
					if (objCyberware.Name == "Essence Hole")
						decHole += objCyberware.CalculatedESS;
					else
					{
						if (objCyberware.SourceType == Improvement.ImprovementSource.Cyberware)
							decCyberware += objCyberware.CalculatedESS;
						else if (objCyberware.SourceType == Improvement.ImprovementSource.Bioware)
							decBioware += objCyberware.CalculatedESS;
					}
				}
				if (decCyberware > decBioware)
					decESS -= decCyberware + (decBioware / 2);
				else
					decESS -= decBioware + (decCyberware / 2);
				// Deduct the Essence Hole value.
				decESS -= decHole;

				return decESS;
			}
		}

		/// <summary>
		/// Essence consumed by Cyberware.
		/// </summary>
		public decimal CyberwareEssence
		{
			get
			{
				decimal decESS = Convert.ToDecimal(_attESS.MetatypeMaximum, GlobalOptions.Instance.CultureInfo) + Convert.ToDecimal(_objImprovementManager.ValueOf(Improvement.ImprovementType.Essence), GlobalOptions.Instance.CultureInfo);
				// Run through all of the pieces of Cyberware and include their Essence cost. Cyberware and Bioware costs are calculated separately. The higher value removes its full cost from the
				// character's ESS while the lower removes half of its cost from the character's ESS.
				decimal decCyberware = 0m;
				decimal decBioware = 0m;
				decimal decHole = 0m;
				foreach (Cyberware objCyberware in _lstCyberware)
				{
					if (objCyberware.Name == "Essence Hole")
						decHole += objCyberware.CalculatedESS;
					else
					{
						if (objCyberware.SourceType == Improvement.ImprovementSource.Cyberware)
							decCyberware += objCyberware.CalculatedESS;
						else if (objCyberware.SourceType == Improvement.ImprovementSource.Bioware)
							decBioware += objCyberware.CalculatedESS;
					}
				}
				if (decCyberware > decBioware)
					return decCyberware;
				else
					return decCyberware / 2;
			}
		}

		/// <summary>
		/// Essence consumed by Bioware.
		/// </summary>
		public decimal BiowareEssence
		{
			get
			{
				decimal decESS = Convert.ToDecimal(_attESS.MetatypeMaximum, GlobalOptions.Instance.CultureInfo) + Convert.ToDecimal(_objImprovementManager.ValueOf(Improvement.ImprovementType.Essence), GlobalOptions.Instance.CultureInfo);
				// Run through all of the pieces of Cyberware and include their Essence cost. Cyberware and Bioware costs are calculated separately. The higher value removes its full cost from the
				// character's ESS while the lower removes half of its cost from the character's ESS.
				decimal decCyberware = 0m;
				decimal decBioware = 0m;
				decimal decHole = 0m;
				foreach (Cyberware objCyberware in _lstCyberware)
				{
					if (objCyberware.Name == "Essence Hole")
						decHole += objCyberware.CalculatedESS;
					else
					{
						if (objCyberware.SourceType == Improvement.ImprovementSource.Cyberware)
							decCyberware += objCyberware.CalculatedESS;
						else if (objCyberware.SourceType == Improvement.ImprovementSource.Bioware)
							decBioware += objCyberware.CalculatedESS;
					}
				}
				if (decCyberware > decBioware)
					return decBioware / 2;
				else
					return decBioware;
			}
		}

		/// <summary>
		/// Essence consumed by Essence Holes.
		/// </summary>
		public decimal EssenceHole
		{
			get
			{
				decimal decESS = Convert.ToDecimal(_attESS.MetatypeMaximum, GlobalOptions.Instance.CultureInfo) + Convert.ToDecimal(_objImprovementManager.ValueOf(Improvement.ImprovementType.Essence), GlobalOptions.Instance.CultureInfo);
				// Run through all of the pieces of Cyberware and include their Essence cost. Cyberware and Bioware costs are calculated separately. The higher value removes its full cost from the
				// character's ESS while the lower removes half of its cost from the character's ESS.
				decimal decCyberware = 0m;
				decimal decBioware = 0m;
				decimal decHole = 0m;
				foreach (Cyberware objCyberware in _lstCyberware)
				{
					if (objCyberware.Name == "Essence Hole")
						decHole += objCyberware.CalculatedESS;
					else
					{
						if (objCyberware.SourceType == Improvement.ImprovementSource.Cyberware)
							decCyberware += objCyberware.CalculatedESS;
						else if (objCyberware.SourceType == Improvement.ImprovementSource.Bioware)
							decBioware += objCyberware.CalculatedESS;
					}
				}

				return decHole;
			}
		}

		/// <summary>
		/// Character's maximum Essence.
		/// </summary>
		public decimal EssenceMaximum
		{
			get
			{
                decimal decESS = Convert.ToDecimal(_attESS.MetatypeMaximum, GlobalOptions.Instance.CultureInfo) + Convert.ToDecimal(_objImprovementManager.ValueOf(Improvement.ImprovementType.EssenceMax), GlobalOptions.Instance.CultureInfo);
				return decESS;
			}
		}

		/// <summary>
		/// Character's total Essence Loss penalty.
		/// </summary>
		public int EssencePenalty
		{
			get
			{
				int intReturn = 0;
				// Subtract the character's current Essence from its maximum. Round the remaining amount up to get the total penalty to MAG and RES.
				intReturn = Convert.ToInt32(Math.Ceiling(EssenceMaximum - Essence));

				return intReturn;
			}
		}

		/// <summary>
		/// Initiative.
		/// </summary>
		public string Initiative
		{
			get
			{
				string strReturn = "";

				// Start by adding INT and REA together.
				int intINI = _attINT.TotalValue + _attREA.TotalValue;
				// Add modifiers.
				intINI += _attINI.AttributeModifiers;
				// Add in any Initiative Improvements.
				intINI += _objImprovementManager.ValueOf(Improvement.ImprovementType.Initiative) + WoundModifiers;

				// If INI exceeds the Metatype maximum set it back to the maximum.
                //if (intINI > _attINI.MetatypeAugmentedMaximum)
                //    intINI = _attINI.MetatypeAugmentedMaximum;
                //if (intINI < 0)
                //    intINI = 0;
				if (_attINT.Value + _attREA.Value != intINI)
					strReturn = (_attINT.Value + _attREA.Value).ToString() + " (" + intINI.ToString() + ")";
				else
					strReturn = (_attINT.Value + _attREA.Value).ToString();

				return strReturn;
			}
		}

		/// <summary>
		/// Initiative Dice.
		/// </summary>
		public string InitiativeDice
		{
			get
			{
				string strReturn = "";

				const int intIP = 1;
                int intExtraIP = 1 + Convert.ToInt32(_objImprovementManager.ValueOf(Improvement.ImprovementType.InitiativeDice));
                // There is a hard limit of 5D6 for Initiative Tests.
                if (intExtraIP > 5)
                    intExtraIP = 5;
				if (intIP != intExtraIP)
					strReturn = "1 (" + intExtraIP.ToString() + ")";
				else
					strReturn = intIP.ToString();

				return strReturn;
			}
		}

		/// <summary>
		/// Astral Initiative.
		/// </summary>
		public string AstralInitiative
		{
			get
			{
				string strReturn = "";

				int intINI = (_attINT.TotalValue * 2) + WoundModifiers;
				if (intINI < 0)
					intINI = 0;
				strReturn = (_attINT.TotalValue * 2).ToString();
				if (intINI != _attINT.TotalValue * 2)
					strReturn += " (" + intINI.ToString() + ")";

				return strReturn;
			}
		}

		/// <summary>
		/// Astral Initiative Passes.
		/// </summary>
		public string AstralInitiativePasses
		{
			get
			{
				return "3";
			}
		}

		/// <summary>
		/// Matrix Initiative.
		/// </summary>
		public string MatrixInitiative
		{
			get
			{
				string strReturn = "";
				int intMatrixInit = 0;

				// This is always calculated since characters can have a Matrix Initiative without actually being a Technomancer.
				if (!TechnomancerEnabled)
				{
					intMatrixInit = _attINT.TotalValue;
					int intCommlinkResponse = 0;

					// Retrieve the Response for the character's active Commlink.
					foreach (Commlink objCommlink in _lstGear.OfType<Commlink>())
					{
						if (objCommlink.IsActive)
							intCommlinkResponse = objCommlink.TotalResponse;
					}
					intMatrixInit += intCommlinkResponse;

					// Add in any Matrix Initiative Improvements.
					intMatrixInit += _objImprovementManager.ValueOf(Improvement.ImprovementType.MatrixInitiative);
				}
				else
				{
					// Technomancer Matrix Initiative = INT * 2 + 1 + any Living Persona bonuses.
					intMatrixInit = (_attINT.TotalValue * 2) + 1 + _objImprovementManager.ValueOf(Improvement.ImprovementType.LivingPersonaResponse);
				}

				// Sprites have a forced value, so use that instead.
				if (_strMetatype.EndsWith("Sprite"))
					intMatrixInit = _attINI.MetatypeMinimum;
				// A.I.s caculate their totals differently. (INT + Response)
				if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
					intMatrixInit = (_attINT.TotalValue + Response);

				int intINI = intMatrixInit + WoundModifiers;
				if (intINI < 0)
					intINI = 0;

				strReturn = intMatrixInit.ToString();
				if (intINI != intMatrixInit)
					strReturn += " (" + intINI.ToString() + ")";

				return strReturn;
			}
		}

		/// <summary>
		/// Matrix Initiative Passes.
		/// </summary>
		public string MatrixInitiativePasses
		{
			get
			{
				string strReturn = "";
				int intIP = 0;

				if (!TechnomancerEnabled)
				{
					// Standard characters get 1 IP + any Matrix Initiative Pass bonuses.
					intIP = 1 + _objImprovementManager.ValueOf(Improvement.ImprovementType.MatrixInitiativePass);
				}
				else
				{
					// Techomancers get 3 IPs + any Matrix Initiative Pass bonuses.
					intIP = 3 + _objImprovementManager.ValueOf(Improvement.ImprovementType.MatrixInitiativePass);
				}

				// A.I.s always have 3 Matrix Initiative Passes.
				if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
					intIP = 3;

				// Add in any additional Matrix Initiative Pass bonuses.
				intIP += _objImprovementManager.ValueOf(Improvement.ImprovementType.MatrixInitiativePassAdd);

				strReturn = intIP.ToString();

				return strReturn;
			}
		}

		/// <summary>
		/// An A.I.'s Rating.
		/// </summary>
		public int Rating
		{
			get
			{
				double dblAverage = Convert.ToDouble(_attCHA.TotalValue, GlobalOptions.Instance.CultureInfo) + Convert.ToDouble(_attINT.TotalValue, GlobalOptions.Instance.CultureInfo) + Convert.ToDouble(_attLOG.TotalValue, GlobalOptions.Instance.CultureInfo) + Convert.ToDouble(_attWIL.TotalValue, GlobalOptions.Instance.CultureInfo);
				dblAverage = Math.Ceiling(dblAverage / 4);
				return Convert.ToInt32(dblAverage);
			}
		}

		/// <summary>
		/// An A.I.'s System.
		/// </summary>
		public int System
		{
			get
			{
				double dblAverage = Convert.ToDouble(_attINT.TotalValue, GlobalOptions.Instance.CultureInfo) + Convert.ToDouble(_attLOG.TotalValue, GlobalOptions.Instance.CultureInfo);
				dblAverage = Math.Ceiling(dblAverage / 2);
				return Convert.ToInt32(dblAverage);
			}
		}

		/// <summary>
		/// An A.I.'s Firewall.
		/// </summary>
		public int Firewall
		{
			get
			{
				double dblAverage = Convert.ToDouble(_attCHA.TotalValue, GlobalOptions.Instance.CultureInfo) + Convert.ToDouble(_attWIL.TotalValue, GlobalOptions.Instance.CultureInfo);
				dblAverage = Math.Ceiling(dblAverage / 2);
				return Convert.ToInt32(dblAverage);
			}
		}

		/// <summary>
		/// An A.I.'s Signal.
		/// </summary>
		public int Signal
		{
			get
			{
				return _intSignal;
			}
			set
			{
				_intSignal = value;
			}
		}

		/// <summary>
		/// An A.I.'s Response.
		/// </summary>
		public int Response
		{
			get
			{
				return _intResponse;
			}
			set
			{
				_intResponse = value;
			}
		}

		/// <summary>
		/// Maximum Skill Rating.
		/// </summary>
		public int MaxSkillRating
		{
			get
			{
				return _intMaxSkillRating;
			}
			set
			{
				_intMaxSkillRating = value;
			}
		}
		#endregion

		#region Special Attribute Tests
        /// <summary>
        /// Surprise (REA + INT).
        /// </summary>
        public int Surprise
        {
            get
            {
                return _attREA.TotalValue + _attINT.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.Surprise);
            }
        }

		/// <summary>
		/// Composure (WIL + CHA).
		/// </summary>
		public int Composure
		{
			get
			{
				return _attWIL.TotalValue + _attCHA.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.Composure);
			}
		}

		/// <summary>
		/// Judge Intentions (INT + CHA).
		/// </summary>
		public int JudgeIntentions
		{
			get
			{
				return _attINT.TotalValue + _attCHA.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.JudgeIntentions);
			}
		}

		/// <summary>
		/// Lifting and Carrying (STR + BOD).
		/// </summary>
		public int LiftAndCarry
		{
			get
			{
				return _attSTR.TotalValue + _attBOD.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.LiftAndCarry);
			}
		}

		/// <summary>
		/// Memory (LOG + WIL).
		/// </summary>
		public int Memory
		{
			get
			{
				return _attLOG.TotalValue + _attWIL.TotalValue + _objImprovementManager.ValueOf(Improvement.ImprovementType.Memory);
			}
		}
		#endregion

		#region Reputation
		/// <summary>
		/// Amount of Street Cred the character has earned through standard means.
		/// </summary>
		public int CalculatedStreetCred
		{
			get
			{
				// Street Cred = Career Karma / 10, rounded normally (34 = 3 Street Cred, 35 = 4 Street Cred; .5 is rounded up).
				int intRemainder = (int)(Convert.ToDouble(CareerKarma, GlobalOptions.Instance.CultureInfo) % 10.0);
				double dblEarned = 0.0;
				if (intRemainder < 5)
					dblEarned = Math.Floor(Convert.ToDouble(CareerKarma, GlobalOptions.Instance.CultureInfo) / 10.0);
				else
					dblEarned = Math.Ceiling(Convert.ToDouble(CareerKarma, GlobalOptions.Instance.CultureInfo) / 10.0);
				int intReturn = Convert.ToInt32(dblEarned);

				// Deduct burnt Street Cred.
				intReturn -= _intBurntStreetCred;

				return intReturn;
			}
		}

		/// <summary>
		/// Character's total amount of Street Cred (earned + GM awarded).
		/// </summary>
		public int TotalStreetCred
		{
			get
			{
				return Math.Max(CalculatedStreetCred + StreetCred, 0);
			}
		}

		/// <summary>
		/// Street Cred Tooltip.
		/// </summary>
		public string StreetCredTooltip
		{
			get
			{
				string strReturn = "";

				strReturn += "(" + LanguageManager.Instance.GetString("String_CareerKarma") + " (" + CareerKarma.ToString() + ")";
				if (BurntStreetCred != 0)
					strReturn += " - " + LanguageManager.Instance.GetString("String_BurntStreetCred") + " (" + BurntStreetCred.ToString() + ")";
				strReturn += ") ÷ 10";

				return strReturn;
			}
		}

		/// <summary>
		/// Amount of Notoriety the character has earned through standard means.
		/// </summary>
		public int CalculatedNotoriety
		{
			get
			{
				// Notoriety is simply the total value of Notoriety Improvements + the number of Enemies they have.
				int intReturn = _objImprovementManager.ValueOf(Improvement.ImprovementType.Notoriety);

				foreach (Contact objContact in _lstContacts)
				{
					if (objContact.EntityType == ContactType.Enemy)
						intReturn += 1;
				}

				return intReturn;
			}
		}

		/// <summary>
		/// Character's total amount of Notoriety (earned + GM awarded - burnt Street Cred).
		/// </summary>
		public int TotalNotoriety
		{
			get
			{
				return CalculatedNotoriety + Notoriety - (BurntStreetCred / 2);
			}
		}

		/// <summary>
		/// Tooltip to use for Notoriety total.
		/// </summary>
		public string NotorietyTooltip
		{
			get
			{
				string strReturn = "";
				int intEnemies = 0;
				
				foreach (Improvement objImprovement in _lstImprovements)
				{
					if (objImprovement.ImproveType == Improvement.ImprovementType.Notoriety)
						strReturn += " + " + GetObjectName(objImprovement) + " (" + objImprovement.Value.ToString() + ")";
				}
				
				foreach (Contact objContact in _lstContacts)
				{
					if (objContact.EntityType == ContactType.Enemy)
						intEnemies += 1;
				}

				if (intEnemies > 0)
					strReturn += " + " + LanguageManager.Instance.GetString("Label_SummaryEnemies") + " (" + intEnemies.ToString() + ")";

				if (BurntStreetCred > 0)
					strReturn += " - " + LanguageManager.Instance.GetString("String_BurntStreetCred") + " (" + (BurntStreetCred / 2).ToString() + ")";

				strReturn = strReturn.Trim();
				if (strReturn.StartsWith("+") || strReturn.StartsWith("-"))
					strReturn = strReturn.Substring(2, strReturn.Length - 2);

				return strReturn;
			}
		}

		/// <summary>
		/// Amount of Public Awareness the character has earned through standard means.
		/// </summary>
		public int CalculatedPublicAwareness
		{
			get
			{
				// Public Awareness is calculated as (Street Cred + Notoriety) / 3, rounded down.
				double dblAwareness = Convert.ToDouble(TotalStreetCred, GlobalOptions.Instance.CultureInfo) + Convert.ToDouble(TotalNotoriety, GlobalOptions.Instance.CultureInfo);
				dblAwareness = Math.Floor(dblAwareness / 3);

				int intReturn = Convert.ToInt32(dblAwareness);

				if (intReturn < 0)
					intReturn = 0;

				return intReturn;
			}
		}

		/// <summary>
		/// Character's total amount of Public Awareness (earned + GM awarded).
		/// </summary>
		public int TotalPublicAwareness
		{
			get
			{
				return Math.Max(CalculatedPublicAwareness + PublicAwareness, 0);
			}
		}

		/// <summary>
		/// Public Awareness Tooltip.
		/// </summary>
		public string PublicAwarenessTooltip
		{
			get
			{
				string strReturn = "";

				strReturn += "(" + LanguageManager.Instance.GetString("String_StreetCred") + " (" + TotalStreetCred.ToString() + ") + " + LanguageManager.Instance.GetString("String_Notoriety") + " (" + TotalNotoriety.ToString() + ")) ÷ 3";

				return strReturn;
			}
		}
		#endregion

		#region List Properties
		/// <summary>
		/// Improvements.
		/// </summary>
		public List<Improvement> Improvements
		{
			get
			{
				return _lstImprovements;
			}
		}

		/// <summary>
		/// Skills (Active and Knowledge).
		/// </summary>
		public List<Skill> Skills
		{
			get
			{
				// If the List is not yet populated, go populate it.
				if (_lstSkills.Count == 0)
					BuildSkillList();
				return _lstSkills;
			}
		}

		/// <summary>
		/// Skill Groups.
		/// </summary>
		public List<SkillGroup> SkillGroups
		{
			get
			{
				// If the List is not yet populated, go populate it.
				if (_lstSkillGroups.Count == 0)
					BuildSkillGroupList();
				return _lstSkillGroups;
			}
		}

		/// <summary>
		/// Contacts and Enemies.
		/// </summary>
		public List<Contact> Contacts
		{
			get
			{
				return _lstContacts;
			}
		}

		/// <summary>
		/// Spirits and Sprites.
		/// </summary>
		public List<Spirit> Spirits
		{
			get
			{
				return _lstSpirits;
			}
		}

		/// <summary>
		/// Magician Spells.
		/// </summary>
		public List<Spell> Spells
		{
			get
			{
				return _lstSpells;
			}
		}

		/// <summary>
		/// Foci.
		/// </summary>
		public List<Focus> Foci
		{
			get
			{
				return _lstFoci;
			}
		}

		/// <summary>
		/// Stacked Foci.
		/// </summary>
		public List<StackedFocus> StackedFoci
		{
			get
			{
				return _lstStackedFoci;
			}
		}

		/// <summary>
		/// Adept Powers.
		/// </summary>
		public List<Power> Powers
		{
			get
			{
				return _lstPowers;
			}
		}

		/// <summary>
		/// Technomancer Complex Forms.
		/// </summary>
		public List<TechProgram> TechPrograms
		{
			get
			{
				return _lstTechPrograms;
			}
		}

		/// <summary>
		/// Martial Arts.
		/// </summary>
		public List<MartialArt> MartialArts
		{
			get
			{
				return _lstMartialArts;
			}
		}

		/// <summary>
		/// Martial Arts Maneuvers.
		/// </summary>
		public List<MartialArtManeuver> MartialArtManeuvers
		{
			get
			{
				return _lstMartialArtManeuvers;
			}
		}

		/// <summary>
		/// Armor.
		/// </summary>
		public List<Equipment> Armor
		{
			get
			{
				return _lstArmor;
			}
		}

		/// <summary>
		/// Cyberware and Bioware.
		/// </summary>
		public List<Equipment> Cyberware
		{
			get
			{
				return _lstCyberware;
			}
		}

		/// <summary>
		/// Weapons.
		/// </summary>
		public List<Equipment> Weapons
		{
			get
			{
				return _lstWeapons;
			}
		}

		/// <summary>
		/// Lifestyles.
		/// </summary>
		public List<Lifestyle> Lifestyles
		{
			get
			{
				return _lstLifestyles;
			}
		}

		/// <summary>
		/// Gear.
		/// </summary>
		public List<Equipment> Gear
		{
			get
			{
				return _lstGear;
			}
		}

		/// <summary>
		/// Vehicles.
		/// </summary>
		public List<Equipment> Vehicles
		{
			get
			{
				return _lstVehicles;
			}
		}

		/// <summary>
		/// Metamagics and Echoes.
		/// </summary>
		public List<Metamagic> Metamagics
		{
			get
			{
				return _lstMetamagics;
			}
		}

		/// <summary>
		/// Critter Powers.
		/// </summary>
		public List<CritterPower> CritterPowers
		{
			get
			{
				return _lstCritterPowers;
			}
		}

		/// <summary>
		/// Initiation and Submersion Grades.
		/// </summary>
		public List<InitiationGrade> InitiationGrades
		{
			get
			{
				return _lstInitiationGrades;
			}
		}

		/// <summary>
		/// Expenses (Karma and Nuyen).
		/// </summary>
		public List<ExpenseLogEntry> ExpenseEntries
		{
			get
			{
				return _lstExpenseLog;
			}
		}

		/// <summary>
		/// Qualities (Positive and Negative).
		/// </summary>
		public List<Quality> Qualities
		{
			get
			{
				return _lstQualities;
			}
		}

		/// <summary>
		/// Locations.
		/// </summary>
		public List<string> Locations
		{
			get
			{
				return _lstLocations;
			}
		}

		/// <summary>
		/// Armor Bundles.
		/// </summary>
		public List<string> ArmorBundles
		{
			get
			{
				return _lstArmorBundles;
			}
		}

		/// <summary>
		/// Weapon Locations.
		/// </summary>
		public List<string> WeaponLocations
		{
			get
			{
				return _lstWeaponLocations;
			}
		}

		/// <summary>
		/// Improvement Groups.
		/// </summary>
		public List<string> ImprovementGroups
		{
			get
			{
				return _lstImprovementGroups;
			}
		}

		/// <summary>
		/// Calendar.
		/// </summary>
		public List<CalendarWeek> Calendar
		{
			get
			{
				return _lstCalendar;
			}
		}
		#endregion

		#region Armor Properties
		/// <summary>
		/// The Character's highest Armor Rating.
		/// </summary>
		public int ArmorValue
		{
			get
			{
				int intHighest = 0;
				int intArmor = 0;

				// Run through the list of Armor currently worn and retrieve the highest total value.
				foreach (Armor objArmor in _lstArmor)
				{
					// Don't look at items that start with "+" since we'll consider those next.
					if (!objArmor.ArmorValue.StartsWith("+"))
					{
						if (objArmor.TotalArmorValue > intHighest && objArmor.Equipped)
							intHighest = objArmor.TotalArmorValue;
					}
				}

				intArmor = intHighest;

				// Run through the list of Armor currently worn again and look at items that start with "+" since they stack with the highest Armor.
				int intStacking = 0;
				foreach (Armor objArmor in _lstArmor)
				{
					if (objArmor.ArmorValue.StartsWith("+") && objArmor.Equipped)
						intStacking += objArmor.TotalArmorValue;
				}

				return intArmor + intStacking;
			}
		}

		/// <summary>
		/// The Character's total Armor Value.
		/// </summary>
		public int TotalArmorValue
		{
			get
			{
				int intHighest = 0;
				int intArmor = 0;

				// Run through the list of Armor currently worn and retrieve the highest total value.
				foreach (Armor objArmor in _lstArmor)
				{
					// Don't look at items that start with "+" since we'll consider those next.
					if (!objArmor.ArmorValue.StartsWith("+"))
					{
						if (objArmor.TotalArmorValue > intHighest && objArmor.Equipped)
							intHighest = objArmor.TotalArmorValue;
					}
				}

                intArmor = intHighest;

				// Run through the list of Armor currently worn again and look at items that start with "+" since they stack with the highest Armor.
				int intStacking = 0;
				foreach (Armor objArmor in _lstArmor)
				{
					if (objArmor.ArmorValue.StartsWith("+") && objArmor.Equipped)
						intStacking += objArmor.TotalArmorValue;
				}

				// Add any Armor modifiers.
				intArmor += _objImprovementManager.ValueOf(Improvement.ImprovementType.ArmorValue);

				return intArmor + intStacking;
			}
		}

		/// <summary>
		/// Armor Encumbrance modifier.
		/// </summary>
		public int ArmorEncumbrance
		{
			get
			{
		        // To calculate Armor Encumbrance, add up all of the Armor Enhancements that a character has (anything start with +).
                // For every 2 full points this exceeds the character's STR, they suffer a -1 penalty to AGI and REA.
                int intTotal = 0;
                int intPenalty = 0;

                foreach (Armor objArmor in _lstArmor)
                {
                    if (objArmor.ArmorValue.StartsWith("+") && objArmor.Equipped)
                        intTotal += objArmor.TotalArmorValue;
                }

                intPenalty = Convert.ToInt32(Math.Floor(Convert.ToDecimal((intTotal - STR.TotalValue), GlobalOptions.Instance.CultureInfo) / 2));
                if (intPenalty < 0)
                    intPenalty = 0;

                return intPenalty;
			}
		}
		#endregion

		#region Condition Monitors
		/// <summary>
		/// Number of Physical Condition Monitor boxes.
		/// </summary>
		public int PhysicalCM
		{
			get
			{
				double dblBOD = _attBOD.TotalValue;
				int intCMPhysical = (int)Math.Ceiling(dblBOD / 2) + 8;
				// Include Improvements in the Condition Monitor values.
				intCMPhysical += Convert.ToInt32(_objImprovementManager.ValueOf(Improvement.ImprovementType.PhysicalCM));
				if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
				{
					// A.I.s add 1/2 their System to Physical CM since they do not have BOD.
					double dblSystem = System;
					intCMPhysical += (int)Math.Ceiling(dblSystem / 2);
				}
				return intCMPhysical;
			}
		}

		/// <summary>
		/// Number of Stun Condition Monitor boxes.
		/// </summary>
		public int StunCM
		{
			get
			{
				double dblWIL = _attWIL.TotalValue;
				int intCMStun = (int)Math.Ceiling(dblWIL / 2) + 8;
				// Include Improvements in the Condition Monitor values.
				intCMStun += Convert.ToInt32(_objImprovementManager.ValueOf(Improvement.ImprovementType.StunCM));
				if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
				{
					// A.I. do not have a Stun Condition Monitor.
					intCMStun = 0;
				}
				return intCMStun;
			}
		}

		/// <summary>
		/// Number of Condition Monitor boxes are needed to reach a Condition Monitor Threshold.
		/// </summary>
		public int CMThreshold
		{
			get
			{
				int intCMThreshold = 0;
				intCMThreshold = 3 + Convert.ToInt32(_objImprovementManager.ValueOf(Improvement.ImprovementType.CMThreshold));
				return intCMThreshold;
			}
		}

		/// <summary>
		/// Number of additioal boxes appear before the first Condition Monitor penalty.
		/// </summary>
		public int CMThresholdOffset
		{
			get
			{
				int intCMThresholdOffset = _objImprovementManager.ValueOf(Improvement.ImprovementType.CMThresholdOffset);
				return intCMThresholdOffset;
			}
		}

		/// <summary>
		/// Number of Overflow Condition Monitor boxes.
		/// </summary>
		public int CMOverflow
		{
			get
			{
				// Characters get a number of overflow boxes equal to their BOD (plus any Improvements). One more boxes is added to mark the character as dead.
				double dblBOD = _attBOD.TotalValue;
				int intCMOverflow = Convert.ToInt32(dblBOD) + _objImprovementManager.ValueOf(Improvement.ImprovementType.CMOverflow) + 1;
				if (_strMetatype.EndsWith("A.I.") || _strMetatypeCategory == "Technocritters" || _strMetatypeCategory == "Protosapients")
				{
					// A.I. do not have an Overflow Condition Monitor.
					intCMOverflow = 0;
				}
				return intCMOverflow;
			}
		}

		/// <summary>
		/// Total modifiers from Condition Monitor damage.
		/// </summary>
		public int WoundModifiers
		{
			get
			{
				int intModifier = 0;
				foreach (Improvement objImprovement in _lstImprovements)
				{
					if (objImprovement.ImproveSource == Improvement.ImprovementSource.ConditionMonitor && objImprovement.Enabled)
						intModifier += objImprovement.Value;
				}

				return intModifier;
			}
		}
		#endregion

		#region Build Properties
		/// <summary>
		/// Method being used to build the character.
		/// </summary>
		public CharacterBuildMethod BuildMethod
		{
			get
			{
				return _objBuildMethod;
			}
			set
			{
				_objBuildMethod = value;
			}
		}

        /// <summary>
        /// Number of points the character can spend on Physical and Mental Attributes.
        /// </summary>
        public int AttributePoints
        {
            get
            {
                return _intAttributePoints;
            }
            set
            {
                _intAttributePoints = value;
            }
        }

        /// <summary>
        /// Number of points the character can spend on Special Attributes.
        /// </summary>
        public int SpecialAttributePoints
        {
            get
            {
                return _intSpecialAttributePoints;
            }
            set
            {
                _intSpecialAttributePoints = value;
            }
        }

		/// <summary>
		/// Number of Build Points that are used to create the character.
		/// </summary>
		public int BuildPoints
		{
			get
			{
				return _intBuildPoints;
			}
			set
			{
				_intBuildPoints = value;
			}
		}

		/// <summary>
		/// Amount of Karma that is used to create the character.
		/// </summary>
		public int BuildKarma
		{
			get
			{
				return _intBuildKarma;
			}
			set
			{
				_intBuildKarma = value;
			}
		}

		/// <summary>
		/// Amount of Nuyen the character has.
		/// </summary>
		public int Nuyen
		{
			get
			{
				return _intNuyen;
			}
			set
			{
				_intNuyen = value;
			}
		}

		/// <summary>
		/// Number of Build Points put into Nuyen.
		/// </summary>
		public decimal NuyenBP
		{
			get
			{
				return _decNuyenBP;
			}
			set
			{
				_decNuyenBP = value;
			}
		}

		/// <summary>
		/// Maximum number of Build Points that can be spent on Nuyen.
		/// </summary>
		public decimal NuyenMaximumBP
		{
			get
			{
				decimal decImprovement = Convert.ToDecimal(_objImprovementManager.ValueOf(Improvement.ImprovementType.NuyenMaxBP), GlobalOptions.Instance.CultureInfo);
				if (_objBuildMethod == CharacterBuildMethod.Karma)
					decImprovement *= 2.0m;

				return Math.Max(_decNuyenMaximumBP, decImprovement);
			}
			set
			{
				_decNuyenMaximumBP = value;
			}
		}

		/// <summary>
		/// Number of free Knowledge Skill Points the character has.
		/// </summary>
		public int KnowledgeSkillPoints
		{
			get
			{
				return _intKnowledgeSkillPoints;
			}
			set
			{
				_intKnowledgeSkillPoints = value;
			}
		}
		#endregion

        #region Attribute Points
        /// <summary>
        /// Determine the number of points that have been spent on Primary Attributes.
        /// </summary>
        public int AttributePointsSpent()
        {
            // Retrieve the character's Primary Attributes.
            Attribute objBOD = GetAttribute("BOD");
            Attribute objAGI = GetAttribute("AGI");
            Attribute objREA = GetAttribute("REA");
            Attribute objSTR = GetAttribute("STR");
            Attribute objCHA = GetAttribute("CHA");
            Attribute objINT = GetAttribute("INT");
            Attribute objLOG = GetAttribute("LOG");
            Attribute objWIL = GetAttribute("WIL");

            int intReturn = 0;
            // Calculate the number of Attribute points that have been spent by subtracting each Attribute's current value from its minimum value.
            intReturn += (objBOD.Value - objBOD.MetatypeMinimum);
            intReturn += (objAGI.Value - objAGI.MetatypeMinimum);
            intReturn += (objREA.Value - objREA.MetatypeMinimum);
            intReturn += (objSTR.Value - objSTR.MetatypeMinimum);
            intReturn += (objCHA.Value - objCHA.MetatypeMinimum);
            intReturn += (objINT.Value - objINT.MetatypeMinimum);
            intReturn += (objLOG.Value - objLOG.MetatypeMinimum);
            intReturn += (objWIL.Value - objWIL.MetatypeMinimum);

            return intReturn;
        }

        /// <summary>
        /// Determine the number of points that have been spent on Special Attributes.
        /// </summary>
        public int SpecialAttributePointsSpent()
        {
            // Retrieve the character's Primary Attributes.
            Attribute objEDG = GetAttribute("EDG");
            Attribute objMAG = GetAttribute("MAG");
            Attribute objRES = GetAttribute("RES");

            int intReturn = 0;
            // Calculate the number of Attribute points that have been spent by subtracting each Attribute's current value from its minimum value.
            intReturn += (objEDG.Value - objEDG.MetatypeMinimum);
            intReturn += (objMAG.Value - objMAG.MetatypeMinimum);
            intReturn += (objRES.Value - objRES.MetatypeMinimum);

            return intReturn;
        }
        #endregion

        #region Metatype/Metavariant Information
        /// <summary>
		/// Character's Metatype.
		/// </summary>
		public string Metatype
		{
			get
			{
				return _strMetatype;
			}
			set
			{
				_strMetatype = value;
			}
		}

        /// <summary>
        /// External GUID of the character's Metatype.
        /// </summary>
        public string MetatypeExternalId
        {
            get
            {
                return _guiMetatype.ToString();
            }
            set
            {
                _guiMetatype = Guid.Parse(value);
            }
        }

		/// <summary>
		/// Character's Metavariant.
		/// </summary>
		public string Metavariant
		{
			get
			{
				return _strMetavariant;
			}
			set
			{
				_strMetavariant = value;
			}
		}

		/// <summary>
		/// Metatype Category.
		/// </summary>
		public string MetatypeCategory
		{
			get
			{
				return _strMetatypeCategory;
			}
			set
			{
				_strMetatypeCategory = value;
			}
		}

		/// <summary>
		/// The number of Skill points the Critter had before it became a Mutant Critter.
		/// </summary>
		public int MutantCritterBaseSkills
		{
			get
			{
				return _intMutantCritterBaseSkills;
			}
			set
			{
				_intMutantCritterBaseSkills = value;
			}
		}

        /// <summary>
        /// Walk multiplier.
        /// </summary>
        public int WalkRate
        {
            get
            {
                return _intWalk;
            }
            set
            {
                _intWalk = value;
            }
        }

        /// <summary>
        /// Run multiplier.
        /// </summary>
        public int RunRate
        {
            get
            {
                return _intRun;
            }
            set
            {
                _intRun = value;
            }
        }

        /// <summary>
        /// Sprint increase.
        /// </summary>
        public int SprintRate
        {
            get
            {
                return _intSprint;
            }
            set
            {
                _intSprint = value;
            }
        }

		/// <summary>
		/// Character's walking Movement rate.
		/// </summary>
		private int WalkingRate
		{
			get
			{
                return AGI.TotalValue * _intWalk;
			}
		}

		/// <summary>
		/// Character's running Movement rate.
		/// </summary>
		private int RunningRate
		{
			get
			{
                return AGI.TotalValue * _intRun;
			}
		}

		/// <summary>
		/// Full Movement (Movement, Swim, and Fly) for printouts.
		/// </summary>
		public string FullMovement()
		{
			string strReturn = "";

            strReturn += LanguageManager.Instance.GetString("String_Walk") + " " + WalkingRate.ToString() + LanguageManager.Instance.GetString("String_Meter") + ", ";
            strReturn += LanguageManager.Instance.GetString("String_Run") + " " + RunningRate.ToString() + LanguageManager.Instance.GetString("String_Meter") + ", ";
            strReturn += LanguageManager.Instance.GetString("String_Sprint") + " "  + SprintRate.ToString() + LanguageManager.Instance.GetString("String_Meter") + "/" + LanguageManager.Instance.GetString("String_Hit");

			return strReturn;
		}

		/// <summary>
		/// BP cost of character's Metatype.
		/// </summary>
		public int MetatypeBP
		{
			get
			{
				return _intMetatypeBP;
			}
			set
			{
				_intMetatypeBP = value;
			}
		}

		/// <summary>
		/// Whether or not the character is a non-Free Sprite.
		/// </summary>
		public bool IsSprite
		{
			get
			{
				if (_strMetatypeCategory.EndsWith("Sprites") && !_strMetatypeCategory.StartsWith("Free"))
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// Whether or not the character is a Free Sprite.
		/// </summary>
		public bool IsFreeSprite
		{
			get
			{
				if (_strMetatypeCategory == "Free Sprite")
					return true;
				else
					return false;
			}
		}
		#endregion

		#region Special Functions and Enabled Check Properties
		/// <summary>
		/// Whether or not Adept options are enabled.
		/// </summary>
		public bool AdeptEnabled
		{
			get
			{
				return _blnAdeptEnabled;
			}
			set
			{
				bool blnOldValue = _blnAdeptEnabled;
				_blnAdeptEnabled = value;
				try
				{
					if (blnOldValue != value)
						AdeptTabEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not Magician options are enabled.
		/// </summary>
		public bool MagicianEnabled
		{
			get
			{
				return _blnMagicianEnabled;
			}
			set
			{
				bool blnOldValue = _blnMagicianEnabled;
				_blnMagicianEnabled = value;
				try
				{
					if (blnOldValue != value)
						MagicianTabEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not Technomancer options are enabled.
		/// </summary>
		public bool TechnomancerEnabled
		{
			get
			{
				return _blnTechnomancerEnabled;
			}
			set
			{
				bool blnOldValue = _blnTechnomancerEnabled;
				_blnTechnomancerEnabled = value;
				try
				{
					if (blnOldValue != value)
						TechnomancerTabEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not the Initiation tab should be shown (override for BP mode).
		/// </summary>
		public bool InitiationEnabled
		{
			get
			{
				return _blnInitiationEnabled;
			}
			set
			{
				bool blnOldValue = _blnInitiationEnabled;
				_blnInitiationEnabled = value;
				try
				{
					if (blnOldValue != value)
						InitiationTabEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not Critter options are enabled.
		/// </summary>
		public bool CritterEnabled
		{
			get
			{
				return _blnCritterEnabled;
			}
			set
			{
				bool blnOldValue = _blnCritterEnabled;
				_blnCritterEnabled = value;
				try
				{
					if (blnOldValue != value)
						CritterTabEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not Black Market is enabled.
		/// </summary>
		public bool BlackMarket
		{
			get
			{
				return _blnBlackMarket;
			}
			set
			{
				bool blnOldValue = _blnBlackMarket;
				_blnBlackMarket = value;
				try
				{
					if (blnOldValue != value)
						BlackMarketEnabledChanged(this);
				}
				catch
				{
				}
			}
		}

        /// <summary>
        /// Whether or not Sensitive System is enabled.
        /// </summary>
        public bool SensitiveSystem
        {
            get
            {
                return _blnSensitiveSystem;
            }
            set
            {
                bool blnOldValue = _blnSensitiveSystem;
                _blnSensitiveSystem = value;
                try
                {
                    if (blnOldValue != value)
                        SensitiveSystemChanged(this);
                }
                catch
                {
                }
            }
        }

		/// <summary>
		/// Whether or not Uneducated is enabled.
		/// </summary>
		public bool Uneducated
		{
			get
			{
				return _blnUneducated;
			}
			set
			{
				bool blnOldValue = _blnUneducated;
				_blnUneducated = value;
				try
				{
					if (blnOldValue != value)
						UneducatedChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not Uncouth is enabled.
		/// </summary>
		public bool Uncouth
		{
			get
			{
				return _blnUncouth;
			}
			set
			{
				bool blnOldValue = _blnUncouth;
				_blnUncouth = value;
				try
				{
					if (blnOldValue != value)
						UncouthChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Whether or not Infirm is enabled.
		/// </summary>
		public bool Infirm
		{
			get
			{
				return _blnInfirm;
			}
			set
			{
				bool blnOldValue = _blnInfirm;
				_blnInfirm = value;
				try
				{
					if (blnOldValue != value)
						InfirmChanged(this);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Convert a string to a CharacterBuildMethod.
		/// </summary>
		/// <param name="strValue">String value to convert.</param>
		public CharacterBuildMethod ConvertToCharacterBuildMethod(string strValue)
		{
			switch (strValue)
			{
				case "Karma":
					return CharacterBuildMethod.Karma;
				default:
					return CharacterBuildMethod.BP;
			}
		}

		/// <summary>
		/// Extended Availability Test information for an item based on the character's Negotiate Skill.
		/// </summary>
		/// <param name="intCost">Item's cost.</param>
		/// <param name="strAvail">Item's Availability.</param>
		public string AvailTest(int intCost, string strAvail)
		{
			string strReturn = "";
			string strInterval = "";
			int intAvail = 0;
			int intTest = 0;

			try
			{
				intAvail = Convert.ToInt32(strAvail.Replace(LanguageManager.Instance.GetString("String_AvailRestricted"), string.Empty).Replace(LanguageManager.Instance.GetString("String_AvailForbidden"), string.Empty));
			}
			catch
			{
				intAvail = 0;
			}

			bool blnCalculate = true;

			if (intAvail == 0 || (!strAvail.Contains(LanguageManager.Instance.GetString("String_AvailRestricted")) && !strAvail.Contains(LanguageManager.Instance.GetString("String_AvailForbidden"))))
				blnCalculate = false;

			if (blnCalculate)
			{
				// Determine the interval based on the item's price.
				if (intCost <= 100)
					strInterval = "12 " + LanguageManager.Instance.GetString("String_Hours");
				else if (intCost > 100 && intCost <= 1000)
					strInterval = "1 " + LanguageManager.Instance.GetString("String_Day");
				else if (intCost > 1000 && intCost <= 10000)
					strInterval = "2 " + LanguageManager.Instance.GetString("String_Days");
				else
					strInterval = "1 " + LanguageManager.Instance.GetString("String_Week");

				// Find the character's Negotiation total.
				foreach (Skill objSkill in _lstSkills)
				{
					if (objSkill.Name == "Negotiation")
						intTest = objSkill.TotalRating;
				}

				strReturn = intTest.ToString() + " (" + intAvail.ToString() + ", " + strInterval + ")";
			}
			else
				strReturn = LanguageManager.Instance.GetString("String_None");

			return strReturn;
		}

		/// <summary>
		/// Whether or not Adapsin is enabled.
		/// </summary>
		public bool AdapsinEnabled
		{
			get
			{
				bool blnReturn = false;
				foreach (Improvement objImprovement in _lstImprovements)
				{
					if (objImprovement.ImproveType == Improvement.ImprovementType.Adapsin && objImprovement.Enabled)
					{
						blnReturn = true;
						break;
					}
				}

				return blnReturn;
			}
		}

		/// <summary>
		/// Whether or not the character has access to Activesofts, Knowsofts, and Linguasofts.
		/// </summary>
		public bool SkillsoftAccess
		{
			get
			{
				foreach (Improvement objImprovement in _lstImprovements)
				{
					if (objImprovement.ImproveType == Improvement.ImprovementType.SkillsoftAccess && objImprovement.Enabled)
						return true;
				}

				return false;
			}
		}

        /// <summary>
        /// Whether or not the character has access to Knowsofts and Linguasofts.
        /// </summary>
        public bool KnowsoftAccess
        {
            get
            {
                foreach (Improvement objImprovement in _lstImprovements)
                {
                    if (objImprovement.ImproveType == Improvement.ImprovementType.KnowsoftAccess && objImprovement.Enabled)
                        return true;
                }

                return false;
            }
        }

		/// <summary>
		/// Determine whether or not the character has any Improvements of a given ImprovementType.
		/// </summary>
		/// <param name="objImprovementType">ImprovementType to search for.</param>
		public bool HasImprovement(Improvement.ImprovementType objImprovementType, bool blnRequireEnabled = false)
		{
			foreach (Improvement objImprovement in _lstImprovements)
			{
				if (objImprovement.ImproveType == objImprovementType)
				{
					if (!blnRequireEnabled || (blnRequireEnabled && objImprovement.Enabled))
						return true;
				}
			}

			return false;
		}
		#endregion

		#region Application Properties
		/// <summary>
		/// The frmViewer window being used by the character.
		/// </summary>
		public frmViewer PrintWindow
		{
			get
			{
				return _frmPrintView;
			}
			set
			{
				_frmPrintView = value;
			}
		}
		#endregion

        #region Methods to load the Metatype
        /// <summary>
        /// Load the character's Metatype information from the XML file.
        /// </summary>
        /// <param name="guiMetatype">GUID of the Metatype to load.</param>
        /// <param name="strXmlFile">Name of the XML file to load from (default: metatypes.xml).</param>
        /// <param name="intForce">Force that the Spirit was summoned with (default: 0).</param>
        /// <param name="blnBloodSpirit">Whether or not this character is a Blood Spirit (default: false).</param>
        /// <param name="blnPossessionBased">Whether or not this Spirit uses a Possession-based tradition (default: false).</param>
        /// <param name="strPossessionMethod">Possession method the Spirit uses (default: empty).</param>
        public void LoadMetatype(Guid guiMetatype, string strXmlFile = "metatypes.xml", int intForce = 0, bool blnBloodSpirit = false, bool blnPossessionBased = false, string strPossessionMethod = "")
        {
            ImprovementManager objImprovementManager = new ImprovementManager(this);
            XmlDocument objXmlDocument = XmlManager.Instance.Load(strXmlFile);

            XmlNode objXmlMetatype = objXmlDocument.SelectSingleNode("/chummer/metatypes/metatype[id = \"" + guiMetatype.ToString() + "\"]");

            _guiMetatype = guiMetatype;

            // Set Metatype information.
            if (strXmlFile != "critters.xml")
            {
                BOD.AssignLimits(ExpressionToString(objXmlMetatype["bodmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["bodmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["bodaug"].InnerText, intForce, 0));
                AGI.AssignLimits(ExpressionToString(objXmlMetatype["agimin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["agimax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["agiaug"].InnerText, intForce, 0));
                REA.AssignLimits(ExpressionToString(objXmlMetatype["reamin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["reamax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["reaaug"].InnerText, intForce, 0));
                STR.AssignLimits(ExpressionToString(objXmlMetatype["strmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["strmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["straug"].InnerText, intForce, 0));
                CHA.AssignLimits(ExpressionToString(objXmlMetatype["chamin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["chamax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["chaaug"].InnerText, intForce, 0));
                INT.AssignLimits(ExpressionToString(objXmlMetatype["intmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["intmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["intaug"].InnerText, intForce, 0));
                LOG.AssignLimits(ExpressionToString(objXmlMetatype["logmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["logmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["logaug"].InnerText, intForce, 0));
                WIL.AssignLimits(ExpressionToString(objXmlMetatype["wilmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["wilmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["wilaug"].InnerText, intForce, 0));
                MAG.AssignLimits(ExpressionToString(objXmlMetatype["magmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["magmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["magaug"].InnerText, intForce, 0));
                RES.AssignLimits(ExpressionToString(objXmlMetatype["resmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["resmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["resaug"].InnerText, intForce, 0));
                EDG.AssignLimits(ExpressionToString(objXmlMetatype["edgmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["edgmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["edgaug"].InnerText, intForce, 0));
                ESS.AssignLimits(ExpressionToString(objXmlMetatype["essmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["essmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["essaug"].InnerText, intForce, 0));
            }
            else
            {
                int intMinModifier = -3;
                if (objXmlMetatype["category"].InnerText == "Mutant Critters")
                    intMinModifier = 0;
                BOD.AssignLimits(ExpressionToString(objXmlMetatype["bodmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["bodmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["bodmin"].InnerText, intForce, 3));
                AGI.AssignLimits(ExpressionToString(objXmlMetatype["agimin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["agimin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["agimin"].InnerText, intForce, 3));
                REA.AssignLimits(ExpressionToString(objXmlMetatype["reamin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["reamin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["reamin"].InnerText, intForce, 3));
                STR.AssignLimits(ExpressionToString(objXmlMetatype["strmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["strmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["strmin"].InnerText, intForce, 3));
                CHA.AssignLimits(ExpressionToString(objXmlMetatype["chamin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["chamin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["chamin"].InnerText, intForce, 3));
                INT.AssignLimits(ExpressionToString(objXmlMetatype["intmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["intmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["intmin"].InnerText, intForce, 3));
                LOG.AssignLimits(ExpressionToString(objXmlMetatype["logmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["logmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["logmin"].InnerText, intForce, 3));
                WIL.AssignLimits(ExpressionToString(objXmlMetatype["wilmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["wilmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["wilmin"].InnerText, intForce, 3));
                MAG.AssignLimits(ExpressionToString(objXmlMetatype["magmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["magmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["magmin"].InnerText, intForce, 3));
                RES.AssignLimits(ExpressionToString(objXmlMetatype["resmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["resmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["resmin"].InnerText, intForce, 3));
                EDG.AssignLimits(ExpressionToString(objXmlMetatype["edgmin"].InnerText, intForce, intMinModifier), ExpressionToString(objXmlMetatype["edgmin"].InnerText, intForce, 3), ExpressionToString(objXmlMetatype["edgmin"].InnerText, intForce, 3));
                ESS.AssignLimits(ExpressionToString(objXmlMetatype["essmin"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["essmax"].InnerText, intForce, 0), ExpressionToString(objXmlMetatype["essaug"].InnerText, intForce, 0));
            }

            // If we're working with a Critter, set the Attributes to their default values.
            if (strXmlFile == "critters.xml")
            {
                BOD.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["bodmin"].InnerText, intForce, 0));
                AGI.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["agimin"].InnerText, intForce, 0));
                REA.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["reamin"].InnerText, intForce, 0));
                STR.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["strmin"].InnerText, intForce, 0));
                CHA.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["chamin"].InnerText, intForce, 0));
                INT.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["intmin"].InnerText, intForce, 0));
                LOG.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["logmin"].InnerText, intForce, 0));
                WIL.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["wilmin"].InnerText, intForce, 0));
                MAG.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["magmin"].InnerText, intForce, 0));
                RES.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["resmin"].InnerText, intForce, 0));
                EDG.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["edgmin"].InnerText, intForce, 0));
                ESS.Value = Convert.ToInt32(ExpressionToString(objXmlMetatype["essmax"].InnerText, intForce, 0));
            }

            // Sprites can never have Physical Attributes or WIL.
            if (objXmlMetatype["category"].InnerText.EndsWith("Sprite"))
            {
                BOD.AssignLimits("0", "0", "0");
                AGI.AssignLimits("0", "0", "0");
                REA.AssignLimits("0", "0", "0");
                STR.AssignLimits("0", "0", "0");
                WIL.AssignLimits("0", "0", "0");
            }

            Metatype = objXmlMetatype["name"].InnerText;
            MetatypeCategory = objXmlMetatype["category"].InnerText;
            Metavariant = "";
            MetatypeBP = 0;

            _intWalk = Convert.ToInt32(objXmlMetatype["walk"].InnerText);
            _intRun = Convert.ToInt32(objXmlMetatype["run"].InnerText);
            _intSprint = Convert.ToInt32(objXmlMetatype["sprint"].InnerText);

            // Load the Qualities file.
            XmlDocument objXmlQualityDocument = XmlManager.Instance.Load("qualities.xml");

            // Determine if the Metatype has any bonuses.
            if (objXmlMetatype["bonus"] != null)
                objImprovementManager.CreateImprovements(Improvement.ImprovementSource.Metatype, Metatype, objXmlMetatype.SelectSingleNode("bonus"), false, 1, Metatype);

            // Create the Qualities that come with the Metatype.
            foreach (XmlNode objXmlQualityItem in objXmlMetatype.SelectNodes("qualities/positive/quality"))
            {
                XmlNode objXmlQuality = objXmlQualityDocument.SelectSingleNode("/chummer/qualities/quality[id = \"" + objXmlQualityItem.InnerText + "\"]");
                TreeNode objNode = new TreeNode();
                List<Weapon> objWeapons = new List<Weapon>();
                List<TreeNode> objWeaponNodes = new List<TreeNode>();
                Quality objQuality = new Quality(this);
                string strForceValue = "";
                if (objXmlQualityItem.Attributes["select"] != null)
                    strForceValue = objXmlQualityItem.Attributes["select"].InnerText;
                QualitySource objSource = new QualitySource();
                objSource = QualitySource.Metatype;
                if (objXmlQualityItem.Attributes["removable"] != null)
                    objSource = QualitySource.MetatypeRemovable;
                objQuality.Create(objXmlQuality, this, objSource, objNode, objWeapons, objWeaponNodes, strForceValue);
                Qualities.Add(objQuality);

                // Add any created Weapons to the character.
                foreach (Weapon objWeapon in objWeapons)
                    Weapons.Add(objWeapon);
            }
            foreach (XmlNode objXmlQualityItem in objXmlMetatype.SelectNodes("qualities/negative/quality"))
            {
                XmlNode objXmlQuality = objXmlQualityDocument.SelectSingleNode("/chummer/qualities/quality[id = \"" + objXmlQualityItem.InnerText + "\"]");
                TreeNode objNode = new TreeNode();
                List<Weapon> objWeapons = new List<Weapon>();
                List<TreeNode> objWeaponNodes = new List<TreeNode>();
                Quality objQuality = new Quality(this);
                string strForceValue = "";
                if (objXmlQualityItem.Attributes["select"] != null)
                    strForceValue = objXmlQualityItem.Attributes["select"].InnerText;
                QualitySource objSource = new QualitySource();
                objSource = QualitySource.Metatype;
                if (objXmlQualityItem.Attributes["removable"] != null)
                    objSource = QualitySource.MetatypeRemovable;
                objQuality.Create(objXmlQuality, this, objSource, objNode, objWeapons, objWeaponNodes, strForceValue);
                Qualities.Add(objQuality);

                // Add any created Weapons to the character.
                foreach (Weapon objWeapon in objWeapons)
                    Weapons.Add(objWeapon);
            }

            // Run through the character's Attributes one more time and make sure their value matches their minimum value.
            if (strXmlFile == "metatypes.xml")
            {
                BOD.Value = BOD.TotalMinimum;
                AGI.Value = AGI.TotalMinimum;
                REA.Value = REA.TotalMinimum;
                STR.Value = STR.TotalMinimum;
                CHA.Value = CHA.TotalMinimum;
                INT.Value = INT.TotalMinimum;
                LOG.Value = LOG.TotalMinimum;
                WIL.Value = WIL.TotalMinimum;
            }

            // Add any Critter Powers the Metatype/Critter should have.
            XmlNode objXmlCritter = objXmlDocument.SelectSingleNode("/chummer/metatypes/metatype[id = \"" + guiMetatype.ToString() + "\"]");

            objXmlDocument = XmlManager.Instance.Load("critterpowers.xml");
            foreach (XmlNode objXmlPower in objXmlCritter.SelectNodes("powers/power"))
            {
                XmlNode objXmlCritterPower = objXmlDocument.SelectSingleNode("/chummer/powers/power[id = \"" + objXmlPower.InnerText + "\"]");
                TreeNode objNode = new TreeNode();
                CritterPower objPower = new CritterPower(this);
                string strForcedValue = "";
                int intRating = 0;

                if (objXmlPower.Attributes["rating"] != null)
                    intRating = Convert.ToInt32(objXmlPower.Attributes["rating"].InnerText);
                if (objXmlPower.Attributes["select"] != null)
                    strForcedValue = objXmlPower.Attributes["select"].InnerText;

                objPower.Create(objXmlCritterPower, this, objNode, intRating, strForcedValue);
                objPower.CountTowardsLimit = false;
                CritterPowers.Add(objPower);
            }

            // If this is a Blood Spirit, add their free Critter Powers.
            if (blnBloodSpirit)
            {
                XmlNode objXmlCritterPower;
                TreeNode objNode;
                CritterPower objPower;
                bool blnAddPower = true;

                // Energy Drain.
                foreach (CritterPower objFindPower in CritterPowers)
                {
                    if (objFindPower.Name == "Energy Drain")
                    {
                        blnAddPower = false;
                        break;
                    }
                }
                if (blnAddPower)
                {
                    objXmlCritterPower = objXmlDocument.SelectSingleNode("/chummer/powers/power[name = \"Energy Drain\"]");
                    objNode = new TreeNode();
                    objPower = new CritterPower(this);
                    objPower.Create(objXmlCritterPower, this, objNode, 0, "");
                    objPower.CountTowardsLimit = false;
                    CritterPowers.Add(objPower);
                }

                // Fear.
                blnAddPower = true;
                foreach (CritterPower objFindPower in CritterPowers)
                {
                    if (objFindPower.Name == "Fear")
                    {
                        blnAddPower = false;
                        break;
                    }
                }
                if (blnAddPower)
                {
                    objXmlCritterPower = objXmlDocument.SelectSingleNode("/chummer/powers/power[name = \"Fear\"]");
                    objNode = new TreeNode();
                    objPower = new CritterPower(this);
                    objPower.Create(objXmlCritterPower, this, objNode, 0, "");
                    objPower.CountTowardsLimit = false;
                    CritterPowers.Add(objPower);
                }

                // Natural Weapon.
                objXmlCritterPower = objXmlDocument.SelectSingleNode("/chummer/powers/power[name = \"Natural Weapon\"]");
                objNode = new TreeNode();
                objPower = new CritterPower(this);
                objPower.Create(objXmlCritterPower, this, objNode, 0, "DV " + intForce.ToString() + "P, AP 0");
                objPower.CountTowardsLimit = false;
                CritterPowers.Add(objPower);

                // Evanescence.
                blnAddPower = true;
                foreach (CritterPower objFindPower in CritterPowers)
                {
                    if (objFindPower.Name == "Evanescence")
                    {
                        blnAddPower = false;
                        break;
                    }
                }
                if (blnAddPower)
                {
                    objXmlCritterPower = objXmlDocument.SelectSingleNode("/chummer/powers/power[name = \"Evanescence\"]");
                    objNode = new TreeNode();
                    objPower = new CritterPower(this);
                    objPower.Create(objXmlCritterPower, this, objNode, 0, "");
                    objPower.CountTowardsLimit = false;
                    CritterPowers.Add(objPower);
                }
            }

            // Remove the Critter's Materialization Power if they have it. Add the Possession or Inhabitation Power if the Possession-based Tradition checkbox is checked.
            if (blnPossessionBased)
            {
                foreach (CritterPower objCritterPower in CritterPowers)
                {
                    if (objCritterPower.Name == "Materialization")
                    {
                        CritterPowers.Remove(objCritterPower);
                        break;
                    }
                }

                // Add the selected Power.
                XmlNode objXmlCritterPower = objXmlDocument.SelectSingleNode("/chummer/powers/power[name = \"" + strPossessionMethod + "\"]");
                TreeNode objNode = new TreeNode();
                CritterPower objPower = new CritterPower(this);
                objPower.Create(objXmlCritterPower, this, objNode, 0, "");
                objPower.CountTowardsLimit = false;
                CritterPowers.Add(objPower);
            }

            // Set the Skill Ratings for the Critter.
            foreach (XmlNode objXmlSkill in objXmlCritter.SelectNodes("skills/skill"))
            {
                if (objXmlSkill.InnerText.Contains("Exotic"))
                {
                    Skill objExotic = new Skill(this);
                    objExotic.ExoticSkill = true;
                    objExotic.Attribute = "AGI";
                    if (objXmlSkill.Attributes["spec"] != null)
                        objExotic.Specialization = objXmlSkill.Attributes["spec"].InnerText;
                    if (Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0)) > 6)
                        objExotic.RatingMaximum = Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0));
                    objExotic.Rating = Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0));
                    objExotic.Name = objXmlSkill.InnerText;
                    Skills.Add(objExotic);
                }
                else
                {
                    foreach (Skill objSkill in Skills)
                    {
                        if (objSkill.Name == objXmlSkill.InnerText)
                        {
                            if (objXmlSkill.Attributes["spec"] != null)
                                objSkill.Specialization = objXmlSkill.Attributes["spec"].InnerText;
                            if (Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0)) > 6)
                                objSkill.RatingMaximum = Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0));
                            objSkill.Rating = Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0));
                            break;
                        }
                    }
                }
            }

            // Set the Skill Group Ratings for the Critter.
            foreach (XmlNode objXmlSkill in objXmlCritter.SelectNodes("skills/group"))
            {
                foreach (SkillGroup objSkill in SkillGroups)
                {
                    if (objSkill.Name == objXmlSkill.InnerText)
                    {
                        objSkill.RatingMaximum = Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0));
                        objSkill.Rating = Convert.ToInt32(ExpressionToString(objXmlSkill.Attributes["rating"].InnerText, intForce, 0));
                        break;
                    }
                }
            }

            // Set the Knowledge Skill Ratings for the Critter.
            foreach (XmlNode objXmlSkill in objXmlCritter.SelectNodes("skills/knowledge"))
            {
                Skill objKnowledge = new Skill(this);
                objKnowledge.Name = objXmlSkill.InnerText;
                objKnowledge.KnowledgeSkill = true;
                if (objXmlSkill.Attributes["spec"] != null)
                    objKnowledge.Specialization = objXmlSkill.Attributes["spec"].InnerText;
                objKnowledge.SkillCategory = objXmlSkill.Attributes["category"].InnerText;
                if (Convert.ToInt32(objXmlSkill.Attributes["rating"].InnerText) > 6)
                    objKnowledge.RatingMaximum = Convert.ToInt32(objXmlSkill.Attributes["rating"].InnerText);
                objKnowledge.Rating = Convert.ToInt32(objXmlSkill.Attributes["rating"].InnerText);
                Skills.Add(objKnowledge);
            }

            // If this is a Critter with a Force (which dictates their Skill Rating/Maximum Skill Rating), set their Skill Rating Maximums.
            if (intForce > 0)
            {
                int intMaxRating = intForce;
                // Determine the highest Skill Rating the Critter has.
                foreach (Skill objSkill in Skills)
                {
                    if (objSkill.RatingMaximum > intMaxRating)
                        intMaxRating = objSkill.RatingMaximum;
                }

                // Now that we know the upper limit, set all of the Skill Rating Maximums to match.
                foreach (Skill objSkill in Skills)
                    objSkill.RatingMaximum = intMaxRating;
                foreach (SkillGroup objGroup in SkillGroups)
                    objGroup.RatingMaximum = intMaxRating;

                // Set the MaxSkillRating for the character so it can be used later when they add new Knowledge Skills or Exotic Skills.
                MaxSkillRating = intMaxRating;
            }

            // Add any Complex Forms the Critter comes with (typically Sprites)
            XmlDocument objXmlProgramDocument = XmlManager.Instance.Load("programs.xml");
            foreach (XmlNode objXmlComplexForm in objXmlCritter.SelectNodes("complexforms/complexform"))
            {
                int intRating = 0;
                if (objXmlComplexForm.Attributes["rating"] != null)
                    intRating = Convert.ToInt32(ExpressionToString(objXmlComplexForm.Attributes["rating"].InnerText, intForce, 0));
                string strForceValue = "";
                if (objXmlComplexForm.Attributes["select"] != null)
                    strForceValue = objXmlComplexForm.Attributes["select"].InnerText;
                XmlNode objXmlProgram = objXmlProgramDocument.SelectSingleNode("/chummer/programs/program[name = \"" + objXmlComplexForm.InnerText + "\"]");
                TreeNode objNode = new TreeNode();
                TechProgram objProgram = new TechProgram(this);
                objProgram.Create(objXmlProgram, this, objNode, strForceValue);
                objProgram.Rating = intRating;
                TechPrograms.Add(objProgram);

                // Add the Program Option if applicable.
                if (objXmlComplexForm.Attributes["option"] != null)
                {
                    int intOptRating = 0;
                    if (objXmlComplexForm.Attributes["optionrating"] != null)
                        intOptRating = Convert.ToInt32(ExpressionToString(objXmlComplexForm.Attributes["optionrating"].InnerText, intForce, 0));
                    string strOptForceValue = "";
                    if (objXmlComplexForm.Attributes["optionselect"] != null)
                        strOptForceValue = objXmlComplexForm.Attributes["optionselect"].InnerText;
                    XmlNode objXmlOption = objXmlProgramDocument.SelectSingleNode("/chummer/options/option[name = \"" + objXmlComplexForm.Attributes["option"].InnerText + "\"]");
                    TreeNode objNodeOpt = new TreeNode();
                    TechProgramOption objOption = new TechProgramOption(this);
                    objOption.Create(objXmlOption, this, objNodeOpt, strOptForceValue);
                    objOption.Rating = intOptRating;
                    objProgram.Options.Add(objOption);
                }
            }

            // Add any Gear the Critter comes with (typically Programs for A.I.s)
            XmlDocument objXmlGearDocument = XmlManager.Instance.Load("gear.xml");
            foreach (XmlNode objXmlGear in objXmlCritter.SelectNodes("gears/gear"))
            {
                int intRating = 0;
                if (objXmlGear.Attributes["rating"] != null)
                    intRating = Convert.ToInt32(ExpressionToString(objXmlGear.Attributes["rating"].InnerText, intForce, 0));
                string strForceValue = "";
                if (objXmlGear.Attributes["select"] != null)
                    strForceValue = objXmlGear.Attributes["select"].InnerText;
                XmlNode objXmlGearItem = objXmlGearDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + objXmlGear.InnerText + "\"]");
                TreeNode objNode = new TreeNode();
                Gear objGear = new Gear(this);
                List<Weapon> lstWeapons = new List<Weapon>();
                List<TreeNode> lstWeaponNodes = new List<TreeNode>();
                objGear.Create(objXmlGearItem, this, objNode, intRating, lstWeapons, lstWeaponNodes, strForceValue);
                objGear.Cost = "0";
                objGear.Cost3 = "0";
                objGear.Cost6 = "0";
                objGear.Cost10 = "0";
                Gear.Add(objGear);
            }

            // If this is a Mutant Critter, count up the number of Skill points they start with.
            if (MetatypeCategory == "Mutant Critters")
            {
                foreach (Skill objSkill in Skills)
                    MutantCritterBaseSkills += objSkill.Rating;
            }
        }

        /// <summary>
        /// Convert Force, 1D6, or 2D6 into a usable value.
        /// </summary>
        /// <param name="strIn">Expression to convert.</param>
        /// <param name="intForce">Force value to use.</param>
        /// <param name="intOffset">Dice offset.</param>
        private string ExpressionToString(string strIn, int intForce, int intOffset)
        {
            int intValue = 0;
            XmlDocument objXmlDocument = new XmlDocument();
            XPathNavigator nav = objXmlDocument.CreateNavigator();
            XPathExpression xprAttribute = nav.Compile(strIn.Replace("/", " div ").Replace("F", intForce.ToString()).Replace("1D6", intForce.ToString()).Replace("2D6", intForce.ToString()));
            // This statement is wrapped in a try/catch since trying 1 div 2 results in an error with XSLT.
            try
            {
                intValue = Convert.ToInt32(nav.Evaluate(xprAttribute).ToString());
            }
            catch
            {
                intValue = 1;
            }
            intValue += intOffset;
            if (intForce > 0)
            {
                if (intValue < 1)
                    intValue = 1;
            }
            else
            {
                if (intValue < 0)
                    intValue = 0;
            }
            return intValue.ToString();
        }
        #endregion

        #region Limits
        /// <summary>
        /// Physical Limit.
        /// </summary>
        public int PhysicalLimit()
        {
            // Physical Limit is calculated as [(STR x 2) + BOD + REA] / 3 (round up).
            int intReturn = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(((_attSTR.TotalValue * 2) + _attBOD.TotalValue + _attREA.TotalValue) / 3)));
            intReturn += _objImprovementManager.ValueOf(Improvement.ImprovementType.PhysicalLimit);
            return intReturn;
        }

        /// <summary>
        /// Mental Limit.
        /// </summary>
        public int MentalLimit()
        {
            // Mental Limit is calculated as [(LOG x 2) + INT + WIL] / 3 (round up).
            int intReturn = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(((_attLOG.TotalValue * 2) + _attINT.TotalValue + _attWIL.TotalValue) / 3)));
            intReturn += _objImprovementManager.ValueOf(Improvement.ImprovementType.MentalLimit);
            return intReturn;
        }

        /// <summary>
        /// Social Limit.
        /// </summary>
        public int SocialLimit()
        {
            // Social Limit is calculated as [(CHA x 2) + WIL + ESS] / 3 (round up).
            int intReturn = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(((_attCHA.TotalValue * 2) + _attWIL.TotalValue + Convert.ToInt32(Essence)) / 3)));
            intReturn += _objImprovementManager.ValueOf(Improvement.ImprovementType.SocialLimit);
            return intReturn;
        }

        /// <summary>
        /// Astral Limit is calculated as the greater of Mental Limit and Social Limit.
        /// </summary>
        public int AstralLimit()
        {
            int intReturn = Math.Max(MentalLimit(), SocialLimit());
            return intReturn;
        }
        #endregion
    }
}