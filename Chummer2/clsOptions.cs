using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;

// MRUChanged Event Handler.
public delegate void MRUChangedHandler();

namespace Chummer
{
	public enum ClipboardContentType
	{
		None = 0,
		Gear = 1,
		Commlink = 2,
		OperatingSystem = 3,
		Cyberware = 4,
		Bioware = 5,
		Armor = 6,
		Weapon = 7,
		Vehicle = 8,
		Lifestyle = 9,
	}

	public class SourcebookInfo
	{
		string _strCode = "";
		string _strPath = "";
		int _intOffset = 0;

		#region Properties
		public string Code
		{
			get
			{
				return _strCode;
			}
			set
			{
				_strCode = value;
			}
		}

		public string Path
		{
			get
			{
				return _strPath;
			}
			set
			{
				_strPath = value;
			}
		}

		public int Offset
		{
			get
			{
				return _intOffset;
			}
			set
			{
				_intOffset = value;
			}
		}
		#endregion
	}

    /// <summary>
    /// Sealed class to hold universal constants such as fixed Weapon IDs for Unarmed Attack.
    /// </summary>
    public sealed class Constants
    {
        public static string UnarmedAttack = "5c737420-5d43-4ba7-9a31-d19af265d8e5";
        public static string ManeuverAutosoft = "a09a458a-51ce-4181-82da-4b015b234e7f";
    }

	/// <summary>
	/// Global Options. A single instance class since Options are common for all characters, reduces execution time and memory usage.
	/// </summary>
	public sealed class GlobalOptions
	{
		static readonly GlobalOptions _objInstance = new GlobalOptions();
		static readonly CultureInfo _objCultureInfo = new CultureInfo("en-US");

		public event MRUChangedHandler MRUChanged;

		private frmMain _frmMainForm;

		private static bool _blnAutomaticUpdate = false;
		private static bool _blnLocalisedUpdatesOnly = false;
		private static bool _blnStartupFullscreen = false;
		private static bool _blnSingleDiceRoller = true;
		private static string _strLanguage = "en-us";
		private static string _strDefaultCharacterSheet = "Shadowrun 5";
		private static bool _blnDatesIncludeTime = true;
		private static bool _blnPrintToFileFirst = false;

		// Omae Information.
		private static string _strOmaeUserName = "";
		private static string _strOmaePassword = "";
		private static bool _blnOmaeAutoLogin = false;

		private XmlDocument _objXmlClipboard = new XmlDocument();
		private ClipboardContentType _objClipboardContentType = new ClipboardContentType();

		public static GradeList CyberwareGrades = new GradeList();
		public static GradeList BiowareGrades = new GradeList();

		// PDF information.
		public static string _strPDFAppPath = "";
		public static List<SourcebookInfo> _lstSourcebookInfo = new List<SourcebookInfo>();

		#region Constructor and Instance
		static GlobalOptions()
		{
			if (!Directory.Exists(Path.Combine(ApplicationPath(), "settings")))
				Directory.CreateDirectory(Path.Combine(ApplicationPath(), "settings"));

			// Automatic Update.
			try
			{
				_blnAutomaticUpdate = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("autoupdate").ToString());
			}
			catch
			{
			}

			// Whether or not the app should only download localised files in the user's selected language.
			try
			{
                _blnLocalisedUpdatesOnly = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("localisedupdatesonly").ToString());
			}
			catch
			{
			}
			
			// Whether or not dates should include the time.
			try
			{
                _blnDatesIncludeTime = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("datesincludetime").ToString());
			}
			catch
			{
			}

			// Whether or not printouts should be sent to a file before loading them in the browser. This is a fix for getting printing to work properly on Linux using Wine.
			try
			{
                _blnPrintToFileFirst = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("printtofilefirst").ToString());
			}
			catch
			{
			}

			// Default character sheet.
			try
			{
                _strDefaultCharacterSheet = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("defaultsheet").ToString();
			}
			catch
			{
			}

			// Omae Settings.
			// Username.
			try
			{
                _strOmaeUserName = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("omaeusername").ToString();
			}
			catch
			{
                try
                {
                    // Try to copy the values from the SR4 version of Chummer.
                    _strOmaeUserName = Registry.CurrentUser.OpenSubKey("Software\\Chummer").GetValue("omaeusername").ToString();
                    RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5");
                    objRegistry.SetValue("omaeusername", _strOmaeUserName);
                }
                catch
                {
                }
			}
			// Password.
			try
			{
                _strOmaePassword = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("omaepassword").ToString();
			}
			catch
			{
                try
                {
                    // Try to copy the values from the SR4 version of Chummer.
                    _strOmaePassword = Registry.CurrentUser.OpenSubKey("Software\\Chummer").GetValue("omaepassword").ToString();
                    RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5");
                    objRegistry.SetValue("omaepassword", _strOmaePassword);
                }
                catch
                {
                }
			}
			// AutoLogin.
			try
			{
                _blnOmaeAutoLogin = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("omaeautologin").ToString());
			}
			catch
			{
                try
                {
                    // Try to copy the values from the SR4 version of Chummer.
                    _blnOmaeAutoLogin = Convert.ToBoolean(Registry.CurrentUser.OpenSubKey("Software\\Chummer").GetValue("omaeautologic").ToString());
                    RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5");
                    objRegistry.SetValue("omaeautologic", _blnOmaeAutoLogin.ToString());
                }
                catch
                {
                }
			}
			// Language.
			try
			{
                _strLanguage = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("language").ToString();
			}
			catch
			{
                try
                {
                    // Try to copy the values from the SR4 version of Chummer.
                    _strLanguage = Registry.CurrentUser.OpenSubKey("Software\\Chummer").GetValue("language").ToString();
                    RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5");
                    objRegistry.SetValue("language", _strLanguage);
                }
                catch
                {
                }
			}
			// Startup in Fullscreen mode.
			try
			{
                _blnStartupFullscreen = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("startupfullscreen").ToString());
			}
			catch
			{
			}
			// Single instace of the Dice Roller window.
			try
			{
                _blnSingleDiceRoller = Convert.ToBoolean(Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("singlediceroller").ToString());
			}
			catch
			{
			}

			// PDF application path.
			try
			{
                _strPDFAppPath = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5").GetValue("pdfapppath").ToString();
			}
			catch
			{
                try
                {
                    // Try to copy the values from the SR4 version of Chummer.
                    _strPDFAppPath = Registry.CurrentUser.OpenSubKey("Software\\Chummer").GetValue("pdfapppath").ToString();
                    RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5");
                    objRegistry.SetValue("pdfapppath", _strPDFAppPath);
                }
                catch
                {
                }
			}

			// Retrieve the SourcebookInfo objects.
			XmlDocument objXmlDocument = XmlManager.Instance.Load("books.xml");
			XmlNodeList objXmlBookList = objXmlDocument.SelectNodes("/chummer/books/book");
			foreach (XmlNode objXmlBook in objXmlBookList)
			{
				try
				{
					SourcebookInfo objSource = new SourcebookInfo();
                    string strTemp = Registry.CurrentUser.CreateSubKey("Software\\ChummerSR5\\Sourcebook").GetValue(objXmlBook["code"].InnerText).ToString();
					string[] strParts = strTemp.Split('|');
					objSource.Code = objXmlBook["code"].InnerText;
					objSource.Path = strParts[0];
					objSource.Offset = Convert.ToInt32(strParts[1]);

					_lstSourcebookInfo.Add(objSource);
				}
				catch
				{
				}
			}

			CyberwareGrades.LoadList(Improvement.ImprovementSource.Cyberware);
			BiowareGrades.LoadList(Improvement.ImprovementSource.Bioware);
		}

		GlobalOptions()
		{
		}

		/// <summary>
		/// Global instance of the GlobalOptions.
		/// </summary>
		public static GlobalOptions Instance
		{
			get
			{
				return _objInstance;
			}
		}
		#endregion

		#region Properties
        /// <summary>
        /// Application's path. This has been implemented so that the application accesses the correct path while unit tests are running.
        /// </summary>
        public static string ApplicationPath()
        {
            string[] strArgs = Environment.GetCommandLineArgs();
            if (strArgs.GetUpperBound(0) > 0)
            {
                if (strArgs[0].Contains("vstest"))
                    return "D:\\source\\Chummer2\\Chummer2\\bin\\Debug";
                else
                    return Application.StartupPath;
            }
            else
                return Application.StartupPath;
        }

		/// <summary>
		/// Whether or not Automatic Updates are enabled.
		/// </summary>
		public bool AutomaticUpdate
		{
			get
			{
				return _blnAutomaticUpdate;
			}
			set
			{
				_blnAutomaticUpdate = value;
			}
		}

		/// <summary>
		/// Whether or not the app should only download localised files in the user's selected language.
		/// </summary>
		public bool LocalisedUpdatesOnly
		{
			get
			{
				return _blnLocalisedUpdatesOnly;
			}
			set
			{
				_blnLocalisedUpdatesOnly = value;
			}
		}

		/// <summary>
		/// Whether or not dates should include the time.
		/// </summary>
		public bool DatesIncludeTime
		{
			get
			{
				return _blnDatesIncludeTime;
			}
			set
			{
				_blnDatesIncludeTime = value;
			}
		}

		/// <summary>
		/// Whether or not printouts should be sent to a file before loading them in the browser. This is a fix for getting printing to work properly on Linux using Wine.
		/// </summary>
		public bool PrintToFileFirst
		{
			get
			{
				return _blnPrintToFileFirst;
			}
			set
			{
				_blnPrintToFileFirst = value;
			}
		}

		/// <summary>
		/// Omae user name.
		/// </summary>
		public string OmaeUserName
		{
			get
			{
				return _strOmaeUserName;
			}
			set
			{
				_strOmaeUserName = value;
			}
		}

		/// <summary>
		/// Omae password (Base64 encoded).
		/// </summary>
		public string OmaePassword
		{
			get
			{
				return _strOmaePassword;
			}
			set
			{
				_strOmaePassword = value;
			}
		}

		/// <summary>
		/// Omae AutoLogin.
		/// </summary>
		public bool OmaeAutoLogin
		{
			get
			{
				return _blnOmaeAutoLogin;
			}
			set
			{
				_blnOmaeAutoLogin = value;
			}
		}

		/// <summary>
		/// Main application form.
		/// </summary>
		public frmMain MainForm
		{
			get
			{
				return _frmMainForm;
			}
			set
			{
				_frmMainForm = value;
			}
		}

		/// <summary>
		/// Language.
		/// </summary>
		public string Language
		{
			get
			{
				return _strLanguage;
			}
			set
			{
				_strLanguage = value;
			}
		}

		/// <summary>
		/// Whether or not the application should start in fullscreen mode.
		/// </summary>
		public bool StartupFullscreen
		{
			get
			{
				return _blnStartupFullscreen;
			}
			set
			{
				_blnStartupFullscreen = value;
			}
		}

		/// <summary>
		/// Whether or not only a single instance of the Dice Roller should be allowed.
		/// </summary>
		public bool SingleDiceRoller
		{
			get
			{
				return _blnSingleDiceRoller;
			}
			set
			{
				_blnSingleDiceRoller = value;
			}
		}

		/// <summary>
		/// CultureInfor for number localization.
		/// </summary>
		public CultureInfo CultureInfo
		{
			get
			{
				return _objCultureInfo;
			}
		}

		/// <summary>
		/// Clipboard.
		/// </summary>
		public XmlDocument Clipboard
		{
			get
			{
				return _objXmlClipboard;
			}
			set
			{
				_objXmlClipboard = value;
			}
		}

		/// <summary>
		/// Type of data that is currently stored in the clipboard.
		/// </summary>
		public ClipboardContentType ClipboardContentType
		{
			get
			{
				return _objClipboardContentType;
			}
			set
			{
				_objClipboardContentType = value;
			}
		}

		/// <summary>
		/// Default character sheet to use when printing.
		/// </summary>
		public string DefaultCharacterSheet
		{
			get
			{
				return _strDefaultCharacterSheet;
			}
			set
			{
				_strDefaultCharacterSheet = value;
			}
		}

		/// <summary>
		/// Path to the user's PDF application.
		/// </summary>
		public string PDFAppPath
		{
			get
			{
				return _strPDFAppPath;
			}
			set
			{
				_strPDFAppPath = value;
			}
		}

		/// <summary>
		/// List of SourcebookInfo.
		/// </summary>
		public List<SourcebookInfo> SourcebookInfo
		{
			get
			{
				return _lstSourcebookInfo;
			}
			set
			{
				_lstSourcebookInfo = value;
			}
		}
		#endregion

		#region MRU Methods
		/// <summary>
		/// Add a file to the most recently used characters.
		/// </summary>
		/// <param name="strFile">Name of the file to add.</param>
		public void AddToMRUList(string strFile)
		{
			List<string> strFiles = ReadMRUList();

			// Make sure the file does not already exist in the MRU list.
			if (strFiles.Contains(strFile))
				strFiles.Remove(strFile);

			// Make sure the file doesn't exist in the sticky MRU list.
			List<string> strStickyFiles = ReadStickyMRUList();
			if (strStickyFiles.Contains(strFile))
				return;

			strFiles.Insert(0, strFile);

			if (strFiles.Count > 10)
				strFiles.RemoveRange(10, strFiles.Count - 10);

			RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\Chummer");
			int i = 0;
			foreach (string strItem in strFiles)
			{
				i++;
				objRegistry.SetValue("mru" + i.ToString(), strItem);
			}
			MRUChanged();
		}

		/// <summary>
		/// Remove a file from the most recently used characters.
		/// </summary>
		/// <param name="strFile">Name of the file to remove.</param>
		public void RemoveFromMRUList(string strFile)
		{
			List<string> strFiles = ReadMRUList();

			foreach (string strItem in strFiles)
			{
				if (strItem == strFile)
				{
					strFiles.Remove(strItem);
					break;
				}
			}

			RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\Chummer");
			int i = 0;
			foreach (string strItem in strFiles)
			{
				i++;
				objRegistry.SetValue("mru" + i.ToString(), strItem);
			}
			if (strFiles.Count < 10)
			{
				for (i = strFiles.Count + 1; i <= 10; i++)
				{
					try
					{
						objRegistry.DeleteValue("mru" + i.ToString());
					}
					catch
					{
					}
				}
			}
			MRUChanged();
		}

		/// <summary>
		/// Retrieve the list of most recently used characters.
		/// </summary>
		public List<string> ReadMRUList()
		{
			RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\Chummer");
			List<string> lstFiles = new List<string>();

			for (int i = 1; i <= 10; i++)
			{
				try
				{
					lstFiles.Add(objRegistry.GetValue("mru" + i.ToString()).ToString());
				}
				catch
				{
				}
			}

			return lstFiles;
		}

		/// <summary>
		/// Add a file to the sticky most recently used characters.
		/// </summary>
		/// <param name="strFile">Name of the file to add.</param>
		public void AddToStickyMRUList(string strFile)
		{
			List<string> strFiles = ReadStickyMRUList();

			// Make sure the file does not already exist in the MRU list.
			if (strFiles.Contains(strFile))
				strFiles.Remove(strFile);

			strFiles.Insert(0, strFile);

			if (strFiles.Count > 10)
				strFiles.RemoveRange(10, strFiles.Count - 10);

			RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\Chummer");
			int i = 0;
			foreach (string strItem in strFiles)
			{
				i++;
				objRegistry.SetValue("stickymru" + i.ToString(), strItem);
			}
			MRUChanged();
		}

		/// <summary>
		/// Remove a file from the sticky most recently used characters.
		/// </summary>
		/// <param name="strFile">Name of the file to remove.</param>
		public void RemoveFromStickyMRUList(string strFile)
		{
			List<string> strFiles = ReadStickyMRUList();

			foreach (string strItem in strFiles)
			{
				if (strItem == strFile)
				{
					strFiles.Remove(strItem);
					break;
				}
			}

			RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\Chummer");
			int i = 0;
			foreach (string strItem in strFiles)
			{
				i++;
				objRegistry.SetValue("stickymru" + i.ToString(), strItem);
			}
			if (strFiles.Count < 10)
			{
				for (i = strFiles.Count + 1; i <= 10; i++)
				{
					try
					{
						objRegistry.DeleteValue("stickymru" + i.ToString());
					}
					catch
					{
					}
				}
			}
			MRUChanged();
		}

		/// <summary>
		/// Retrieve the list of sticky most recently used characters.
		/// </summary>
		public List<string> ReadStickyMRUList()
		{
			RegistryKey objRegistry = Registry.CurrentUser.CreateSubKey("Software\\Chummer");
			List<string> lstFiles = new List<string>();

			for (int i = 1; i <= 10; i++)
			{
				try
				{
					lstFiles.Add(objRegistry.GetValue("stickymru" + i.ToString()).ToString());
				}
				catch
				{
				}
			}

			return lstFiles;
		}
		#endregion
	}

	public class CharacterOptions
	{
		private string _strFileName = "default.xml";
		private string _strName = "Default Settings";

		// Settings.
		private bool _blnConfirmDelete = true;
		private bool _blnConfirmKarmaExpense = true;
		private bool _blnPrintSkillsWithZeroRating = true;
		private bool _blnMoreLethalGameplay = false;
		private bool _blnEnforceSkillMaximumModifiedRating = true;
		private bool _blnCapSkillRating = false;
		private bool _blnPrintExpenses = false;
		private bool _blnAllowSkillRegrouping = false;
		private bool _blnMetatypeCostsKarma = false;
		private bool _blnStrengthAffectsRecoil = false;
		private bool _blnMaximumArmorModifications = false;
		private bool _blnArmorSuitCapacity = false;
		private bool _blnArmorDegradation = false;
		private bool _blnAutomaticCopyProtection = true;
		private bool _blnAutomaticRegistration = true;
		private bool _blnErgonomicProgramsLimit = true;
		private bool _blnUseCalculatedVehicleSensorRatings = false;
		private bool _blnEnforceCapacity = true;
		private bool _blnRestrictRecoil = true;
		private bool _blnCalculateCommlinkResponse = true;
		private bool _blnAlternateComplexFormCost = false;
		private bool _blnAlternateMatrixAttribute = false;
		private bool _blnAllowCustomTransgenics = false;
		private bool _blnBreakSkillGroupsInCreateMode = false;
		private bool _blnExtendAnyDetectionSpell = false;
		private bool _blnAllowSkillDiceRolling = false;
		private bool _blnCreateBackupOnCareer = false;
		private bool _blnPrintLeadershipAlternates = false;
		private bool _blnPrintArcanaAlternates = false;
		private bool _blnPrintNotes = false;
		private string _strBookXPath = "";
		private int _intNuyenPerBP = 5000;
		private int _intMetatypeCostMultiplier = 1;
		private int _intLimbCount = 6;
		private int _intEssenceDecimals = 2;
		private string _strExcludeLimbSlot = "";
		private readonly XmlDocument _objBookDoc = new XmlDocument();

		// BP variables.
		private int _intBPAttribute = 10;
		private int _intBPAttributeMax = 15;
		private int _intBPContact = 1;
		private int _intBPMartialArt = 5;
		private int _intBPMartialArtManeuver = 2;
		private int _intBPSkillGroup = 10;
		private int _intBPActiveSkill = 4;
		private int _intBPActiveSkillSpecialization = 2;
		private int _intBPKnowledgeSkill = 2;
		private int _intBPSpell = 3;
		private int _intBPFocus = 1;
		private int _intBPSpirit = 1;
		private int _intBPComplexForm = 1;
		private int _intBPComplexFormOption = 1;

		// Karma variables.
		private int _intKarmaAttribute = 5;
		private int _intKarmaQuality = 2;
		private int _intKarmaSpecialization = 2;
		private int _intKarmaNewKnowledgeSkill = 2;
		private int _intKarmaNewActiveSkill = 4;
		private int _intKarmaNewSkillGroup = 10;
		private int _intKarmaImproveKnowledgeSkill = 1;
		private int _intKarmaImproveActiveSkill = 2;
		private int _intKarmaImproveSkillGroup = 5;
		private int _intKarmaSpell = 5;
		private int _intKarmaNewComplexForm = 2;
		private int _intKarmaImproveComplexForm = 1;
		private int _intKarmaComplexFormOption = 2;
		private int _intKarmaComplexFormSkillfot = 1;
		private int _intKarmaNuyenPer = 2500;
		private int _intKarmaContact = 2;
		private int _intKarmaCarryover = 5;
		private int _intKarmaSpirit = 2;
		private int _intKarmaManeuver = 4;
		private int _intKarmaInitiation = 3;
		private int _intKarmaMetamagic = 15;
		private int _intKarmaJoinGroup = 5;
		private int _intKarmaLeaveGroup = 1;

		// Karma Foci variables.
		private int _intKarmaAnchoringFocus = 6;
		private int _intKarmaBanishingFocus = 3;
		private int _intKarmaBindingFocus = 3;
		private int _intKarmaCenteringFocus = 6;
		private int _intKarmaCounterspellingFocus = 3;
		private int _intKarmaDiviningFocus = 6;
		private int _intKarmaDowsingFocus = 6;
		private int _intKarmaInfusionFocus = 3;
		private int _intKarmaMaskingFocus = 6;
		private int _intKarmaPowerFocus = 8;
		private int _intKarmaShieldingFocus = 6;
		private int _intKarmaSpellcastingFocus = 4;
		private int _intKarmaSummoningFocus = 4;
		private int _intKarmaSustainingFocus = 2;
		private int _intKarmaSymbolicLinkFocus = 1;
		private int _intKarmaWeaponFocus = 3;

		// Default build settings.
		private string _strBuildMethod = "Priority";
		private int _intBuildPoints = 15;
		private int _intAvailability = 12;

		// Sourcebook list.
		private readonly List<string> _lstBooks = new List<string>();

		#region Initialization, Save, and Load Methods
		public CharacterOptions()
		{
			// Create the settings directory if it does not exist.
			if (!Directory.Exists(Path.Combine(GlobalOptions.ApplicationPath(), "settings")))
				Directory.CreateDirectory(Path.Combine(GlobalOptions.ApplicationPath(), "settings"));

			// If the default.xml settings file does not exist, attempt to read the settings from the Registry (old storage format), then save them to the default.xml file.
			string strFilePath = Path.Combine(GlobalOptions.ApplicationPath(), "settings");
			strFilePath = Path.Combine(strFilePath, "default.xml");
			if (!File.Exists(strFilePath))
			{
				_strFileName = "default.xml";
				Save();
			}
			else
				Load("default.xml");

			// Load the language file.
			LanguageManager.Instance.Load(this);

			// Load the book information.
			_objBookDoc = XmlManager.Instance.Load("books.xml");
		}

		/// <summary>
		/// Save the current settings to the settings file.
		/// </summary>
		public void Save()
		{
			string strFilePath = Path.Combine(GlobalOptions.ApplicationPath(), "settings");
			strFilePath = Path.Combine(strFilePath, _strFileName);
			FileStream objStream = new FileStream(strFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			XmlTextWriter objWriter = new XmlTextWriter(objStream, Encoding.Unicode);
			objWriter.Formatting = Formatting.Indented;
			objWriter.Indentation = 1;
			objWriter.IndentChar = '\t';
			objWriter.WriteStartDocument();

			// <settings>
			objWriter.WriteStartElement("settings");

			// <name />
			objWriter.WriteElementString("name", _strName);
			// <confirmdelete />
			objWriter.WriteElementString("confirmdelete", _blnConfirmDelete.ToString());
			// <confirmkarmaexpense />
			objWriter.WriteElementString("confirmkarmaexpense", _blnConfirmKarmaExpense.ToString());
			// <printzeroratingskills />
			objWriter.WriteElementString("printzeroratingskills", _blnPrintSkillsWithZeroRating.ToString());
			// <morelethalgameplay />
			objWriter.WriteElementString("morelethalgameplay", _blnMoreLethalGameplay.ToString());
			// <enforceskillmaximummodifiedrating />
			objWriter.WriteElementString("enforceskillmaximummodifiedrating", _blnEnforceSkillMaximumModifiedRating.ToString());
			// <capskillrating />
			objWriter.WriteElementString("capskillrating", _blnCapSkillRating.ToString());
			// <printexpenses />
			objWriter.WriteElementString("printexpenses", _blnPrintExpenses.ToString());
			// <nuyenperbp />
			objWriter.WriteElementString("nuyenperbp", _intNuyenPerBP.ToString());
			// <allowskillregrouping />
			objWriter.WriteElementString("allowskillregrouping", _blnAllowSkillRegrouping.ToString());
			// <metatypecostskarma />
			objWriter.WriteElementString("metatypecostskarma", _blnMetatypeCostsKarma.ToString());
			// <metatypecostskarmamultiplier />
			objWriter.WriteElementString("metatypecostskarmamultiplier", _intMetatypeCostMultiplier.ToString());
			// <limbcount />
			objWriter.WriteElementString("limbcount", _intLimbCount.ToString());
			// <excludelimbslot />
			objWriter.WriteElementString("excludelimbslot", _strExcludeLimbSlot);
			// <strengthaffectsrecoil />
			objWriter.WriteElementString("strengthaffectsrecoil", _blnStrengthAffectsRecoil.ToString());
			// <maximumarmormodifications />
			objWriter.WriteElementString("maximumarmormodifications", _blnMaximumArmorModifications.ToString());
			// <armorsuitcapacity />
			objWriter.WriteElementString("armorsuitcapacity", _blnArmorSuitCapacity.ToString());
			// <armordegredation />
			objWriter.WriteElementString("armordegredation", _blnArmorDegradation.ToString());
			// <automaticcopyprotection />
			objWriter.WriteElementString("automaticcopyprotection", _blnAutomaticCopyProtection.ToString());
			// <automaticregistration />
			objWriter.WriteElementString("automaticregistration", _blnAutomaticRegistration.ToString());
			// <ergonomicprogramlimit />
			objWriter.WriteElementString("ergonomicprogramlimit", _blnErgonomicProgramsLimit.ToString());
			// <usecalculatedvehiclesensorratings />
			objWriter.WriteElementString("usecalculatedvehiclesensorratings", _blnUseCalculatedVehicleSensorRatings.ToString());
			// <essencedecimals />
			objWriter.WriteElementString("essencedecimals", _intEssenceDecimals.ToString());
			// <enforcecapacity />
			objWriter.WriteElementString("enforcecapacity", _blnEnforceCapacity.ToString());
			// <restrictrecoil />
			objWriter.WriteElementString("restrictrecoil", _blnRestrictRecoil.ToString());
			// <calculatecommlinkresponse />
			objWriter.WriteElementString("calculatecommlinkresponse", _blnCalculateCommlinkResponse.ToString());
			// <alternatecomplexformcost />
			objWriter.WriteElementString("alternatecomplexformcost", _blnAlternateComplexFormCost.ToString());
			// <alternatematrixattribute />
			objWriter.WriteElementString("alternatematrixattribute", _blnAlternateMatrixAttribute.ToString());
			// <allowcustomtransgenics />
			objWriter.WriteElementString("allowcustomtransgenics", _blnAllowCustomTransgenics.ToString());
			// <breakskillgroupsincreatemode />
			objWriter.WriteElementString("breakskillgroupsincreatemode", _blnBreakSkillGroupsInCreateMode.ToString());
			// <extendanydetectionspell />
			objWriter.WriteElementString("extendanydetectionspell", _blnExtendAnyDetectionSpell.ToString());
			// <allowskilldicerolling />
			objWriter.WriteElementString("allowskilldicerolling", _blnAllowSkillDiceRolling.ToString());
			// <createbackuponcareer />
			objWriter.WriteElementString("createbackuponcareer", _blnCreateBackupOnCareer.ToString());
			// <printleadershipalternates />
			objWriter.WriteElementString("printleadershipalternates", _blnPrintLeadershipAlternates.ToString());
			// <printarcanaalternates />
			objWriter.WriteElementString("printarcanaalternates", _blnPrintArcanaAlternates.ToString());
			// <printnotes />
			objWriter.WriteElementString("printnotes", _blnPrintNotes.ToString());

			// <bpcost>
			objWriter.WriteStartElement("bpcost");
			// <bpattribute />
			objWriter.WriteElementString("bpattribute", _intBPAttribute.ToString());
			// <bpattributemax />
			objWriter.WriteElementString("bpattributemax", _intBPAttributeMax.ToString());
			// <bpcontact />
			objWriter.WriteElementString("bpcontact", _intBPContact.ToString());
			// <bpmartialart />
			objWriter.WriteElementString("bpmartialart", _intBPMartialArt.ToString());
			// <bpmartialartmaneuver />
			objWriter.WriteElementString("bpmartialartmaneuver", _intBPMartialArtManeuver.ToString());
			// <bpskillgroup />
			objWriter.WriteElementString("bpskillgroup", _intBPSkillGroup.ToString());
			// <bpactiveskill />
			objWriter.WriteElementString("bpactiveskill", _intBPActiveSkill.ToString());
			// <bpactiveskillspecialization />
			objWriter.WriteElementString("bpactiveskillspecialization", _intBPActiveSkillSpecialization.ToString());
			// <bpknowledgeskill />
			objWriter.WriteElementString("bpknowledgeskill", _intBPKnowledgeSkill.ToString());
			// <bpspell />
			objWriter.WriteElementString("bpspell", _intBPSpell.ToString());
			// <bpfocus />
			objWriter.WriteElementString("bpfocus", _intBPFocus.ToString());
			// <bpspirit />
			objWriter.WriteElementString("bpspirit", _intBPSpirit.ToString());
			// <bpcomplexform />
			objWriter.WriteElementString("bpcomplexform", _intBPComplexForm.ToString());
			// <bpcomplexformoption />
			objWriter.WriteElementString("bpcomplexformoption", _intBPComplexFormOption.ToString());
			// </bpcost>
			objWriter.WriteEndElement();

			// <karmacost>
			objWriter.WriteStartElement("karmacost");
			// <karmaattribute />
			objWriter.WriteElementString("karmaattribute", _intKarmaAttribute.ToString());
			// <karmaquality />
			objWriter.WriteElementString("karmaquality", _intKarmaQuality.ToString());
			// <karmaspecialization />
			objWriter.WriteElementString("karmaspecialization", _intKarmaSpecialization.ToString());
			// <karmanewknowledgeskill />
			objWriter.WriteElementString("karmanewknowledgeskill", _intKarmaNewKnowledgeSkill.ToString());
			// <karmanewactiveskill />
			objWriter.WriteElementString("karmanewactiveskill", _intKarmaNewActiveSkill.ToString());
			// <karmanewskillgroup />
			objWriter.WriteElementString("karmanewskillgroup", _intKarmaNewSkillGroup.ToString());
			// <karmaimproveknowledgeskill />
			objWriter.WriteElementString("karmaimproveknowledgeskill", _intKarmaImproveKnowledgeSkill.ToString());
			// <karmaimproveactiveskill />
			objWriter.WriteElementString("karmaimproveactiveskill", _intKarmaImproveActiveSkill.ToString());
			// <karmaimproveskillgroup />
			objWriter.WriteElementString("karmaimproveskillgroup", _intKarmaImproveSkillGroup.ToString());
			// <karmaspell />
			objWriter.WriteElementString("karmaspell", _intKarmaSpell.ToString());
			// <karmanewcomplexform />
			objWriter.WriteElementString("karmanewcomplexform", _intKarmaNewComplexForm.ToString());
			// <karmaimprovecomplexform />
			objWriter.WriteElementString("karmaimprovecomplexform", _intKarmaImproveComplexForm.ToString());
			// <karmanuyenper />
			objWriter.WriteElementString("karmanuyenper", _intKarmaNuyenPer.ToString());
			// <karmacontact />
			objWriter.WriteElementString("karmacontact", _intKarmaContact.ToString());
			// <karmacarryover />
			objWriter.WriteElementString("karmacarryover", _intKarmaCarryover.ToString());
			// <karmaspirit />
			objWriter.WriteElementString("karmaspirit", _intKarmaSpirit.ToString());
			// <karmamaneuver />
			objWriter.WriteElementString("karmamaneuver", _intKarmaManeuver.ToString());
			// <karmainitiation />
			objWriter.WriteElementString("karmainitiation", _intKarmaInitiation.ToString());
			// <karmametamagic />
			objWriter.WriteElementString("karmametamagic", _intKarmaMetamagic.ToString());
			// <karmacomplexformoption />
			objWriter.WriteElementString("karmacomplexformoption", _intKarmaComplexFormOption.ToString());
			// <karmacomplexformskillsoft />
			objWriter.WriteElementString("karmacomplexformskillsoft", _intKarmaComplexFormSkillfot.ToString());
			// <karmajoingroup />
			objWriter.WriteElementString("karmajoingroup", _intKarmaJoinGroup.ToString());
			// <karmaleavegroup />
			objWriter.WriteElementString("karmaleavegroup", _intKarmaLeaveGroup.ToString());
			// <karmaanchoringfocus />
			objWriter.WriteElementString("karmaanchoringfocus", _intKarmaAnchoringFocus.ToString());
			// <karmabanishingfocus />
			objWriter.WriteElementString("karmabanishingfocus", _intKarmaBanishingFocus.ToString());
			// <karmabindingfocus />
			objWriter.WriteElementString("karmabindingfocus", _intKarmaBindingFocus.ToString());
			// <karmacenteringfocus />
			objWriter.WriteElementString("karmacenteringfocus", _intKarmaCenteringFocus.ToString());
			// <karmacounterspellingfocus />
			objWriter.WriteElementString("karmacounterspellingfocus", _intKarmaCounterspellingFocus.ToString());
			// <karmadiviningfocus />
			objWriter.WriteElementString("karmadiviningfocus", _intKarmaDiviningFocus.ToString());
			// <karmadowsingfocus />
			objWriter.WriteElementString("karmadowsingfocus", _intKarmaDowsingFocus.ToString());
			// <karmainfusionfocus />
			objWriter.WriteElementString("karmainfusionfocus", _intKarmaInfusionFocus.ToString());
			// <karmamaskingfocus />
			objWriter.WriteElementString("karmamaskingfocus", _intKarmaMaskingFocus.ToString());
			// <karmapowerfocus />
			objWriter.WriteElementString("karmapowerfocus", _intKarmaPowerFocus.ToString());
			// <karmashieldingfocus />
			objWriter.WriteElementString("karmashieldingfocus", _intKarmaShieldingFocus.ToString());
			// <karmaspellcastingfocus />
			objWriter.WriteElementString("karmaspellcastingfocus", _intKarmaSpellcastingFocus.ToString());
			// <karmasummoningfocus />
			objWriter.WriteElementString("karmasummoningfocus", _intKarmaSummoningFocus.ToString());
			// <karmasustainingfocus />
			objWriter.WriteElementString("karmasustainingfocus", _intKarmaSustainingFocus.ToString());
			// <karmasymboliclinkfocus />
			objWriter.WriteElementString("karmasymboliclinkfocus", _intKarmaSymbolicLinkFocus.ToString());
			// <karmaweaponfocus />
			objWriter.WriteElementString("karmaweaponfocus", _intKarmaWeaponFocus.ToString());
			// </karmacost>
			objWriter.WriteEndElement();

			// <books>
			objWriter.WriteStartElement("books");
			foreach (string strBook in _lstBooks)
				objWriter.WriteElementString("book", strBook);
			// </books>
			objWriter.WriteEndElement();

			// <defaultbuild>
			objWriter.WriteStartElement("defaultbuild");
			// <buildmethod />
			objWriter.WriteElementString("buildmethod", _strBuildMethod);
			// <buildpoints />
			objWriter.WriteElementString("buildpoints", _intBuildPoints.ToString());
			// <availability />
			objWriter.WriteElementString("availability", _intAvailability.ToString());
			// </defaultbuild>
			objWriter.WriteEndElement();

			// </settings>
			objWriter.WriteEndElement();

			objWriter.WriteEndDocument();
			objWriter.Close();
			objStream.Close();
		}

		/// <summary>
		/// Load the settings from the settings file.
		/// </summary>
		/// <param name="strFileName">Settings file to load from.</param>
		public bool Load(string strFileName)
		{
			_strFileName = strFileName;
			string strFilePath = Path.Combine(GlobalOptions.ApplicationPath(), "settings");
			strFilePath = Path.Combine(strFilePath, _strFileName);
			XmlDocument objXmlDocument = new XmlDocument();
			try
			{
				// Make sure the settings file exists. If not, ask the user if they would like to use the default settings file instead. A character cannot be loaded without a settings file.
				if (File.Exists(strFilePath))
					objXmlDocument.Load(strFilePath);
				else
				{
					if (MessageBox.Show(LanguageManager.Instance.GetString("Message_CharacterOptions_CannotLoadSetting").Replace("{0}", _strFileName), LanguageManager.Instance.GetString("MessageTitle_CharacterOptions_CannotLoadSetting"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						MessageBox.Show(LanguageManager.Instance.GetString("Message_CharacterOptions_CannotLoadCharacter"), LanguageManager.Instance.GetString("MessageText_CharacterOptions_CannotLoadCharacter"), MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else
					{
						_strFileName = "default.xml";
						objXmlDocument.Load(strFilePath);
					}
				}
			}
			catch
			{
				MessageBox.Show(LanguageManager.Instance.GetString("Message_CharacterOptions_CannotLoadCharacter"), LanguageManager.Instance.GetString("MessageText_CharacterOptions_CannotLoadCharacter"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			// Setting name.
			_strName = objXmlDocument.SelectSingleNode("/settings/name").InnerText;

			// Confirm delete.
			_blnConfirmDelete = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/confirmdelete").InnerText);
			// Confirm Karama Expense.
			_blnConfirmKarmaExpense = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/confirmkarmaexpense").InnerText);
			// Print all Active Skills with a total value greater than 0 (as opposed to only printing those with a Rating higher than 0).
			_blnPrintSkillsWithZeroRating = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/printzeroratingskills").InnerText);
			// More Lethal Gameplay.
			_blnMoreLethalGameplay = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/morelethalgameplay").InnerText);
			// Enforce Skill Maximum Modified Rating.
			_blnEnforceSkillMaximumModifiedRating = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/enforceskillmaximummodifiedrating").InnerText);
			// Cap Skill Rating.
			_blnCapSkillRating = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/capskillrating").InnerText);
			// Print Expenses.
			_blnPrintExpenses = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/printexpenses").InnerText);
			// Nuyen per Build Point
			_intNuyenPerBP = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/nuyenperbp").InnerText);
			// Allow Skill Regrouping.
			_blnAllowSkillRegrouping = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/allowskillregrouping").InnerText);
			// Metatype Costs Karma.
			try
			{
				_blnMetatypeCostsKarma = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/metatypecostskarma").InnerText);
			}
			catch
			{
			}
			// Metatype Costs Karma Multiplier.
			try
			{
				_intMetatypeCostMultiplier = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/metatypecostskarmamultiplier").InnerText);
			}
			catch
			{
			}
			// Limb Count.
			try
			{
				_intLimbCount = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/limbcount").InnerText);
			}
			catch
			{
			}
			// Exclude Limb Slot.
			try
			{
				_strExcludeLimbSlot = objXmlDocument.SelectSingleNode("/settings/excludelimbslot").InnerText;
			}
			catch
			{
			}
			// Strength Affects Recoil.
			try
			{
				_blnStrengthAffectsRecoil = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/strengthaffectsrecoil").InnerText);
			}
			catch
			{
			}
			// Use Maximum Armor Modifications.
			try
			{
				_blnMaximumArmorModifications = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/maximumarmormodifications").InnerText);
			}
			catch
			{
			}
			// Use Armor Suit Capacity.
			try
			{
				_blnArmorSuitCapacity = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/armorsuitcapacity").InnerText);
			}
			catch
			{
			}
			// Allow Armor Degredation.
			try
			{
				_blnArmorDegradation = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/armordegredation").InnerText);
			}
			catch
			{
			}
			// Automatically add Copy Protection Program Option.
			try
			{
				_blnAutomaticCopyProtection = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/automaticcopyprotection").InnerText);
			}
			catch
			{
			}
			// Automatically add Registration Program Option.
			try
			{
				_blnAutomaticRegistration = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/automaticregistration").InnerText);
			}
			catch
			{
			}
			// Whether or not option for Ergonomic Programs affecting a Commlink's effective Response is enabled.
			try
			{
				_blnErgonomicProgramsLimit = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/ergonomicprogramlimit").InnerText);
			}
			catch
			{
			}
			// Whether or not calculated Vehicle Sensor Ratings should be used.
			try
			{
				_blnUseCalculatedVehicleSensorRatings = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/usecalculatedvehiclesensorratings").InnerText);
			}
			catch
			{
			}
			// Number of decimal places to round to when calculating Essence.
			try
			{
				_intEssenceDecimals = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/essencedecimals").InnerText);
			}
			catch
			{
			}
			// Whether or not Capacity limits should be enforced.
			try
			{
				_blnEnforceCapacity = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/enforcecapacity").InnerText);
			}
			catch
			{
			}
			// Whether or not Recoil modifiers are restricted (AR 148).
			try
			{
				_blnRestrictRecoil = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/restrictrecoil").InnerText);
			}
			catch
			{
			}
			// Whether or not a Commlink's Response should be calculated based on the number of programms it has running.
			try
			{
				_blnCalculateCommlinkResponse = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/calculatecommlinkresponse").InnerText);
			}
			catch
			{
			}
			// Whether or not Complex Forms are treated as Spell for BP/Karma costs.
			try
			{
				_blnAlternateComplexFormCost = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/alternatecomplexformcost").InnerText);
			}
			catch
			{
			}
			// Whether or not LOG is used in place of Program Ratings for Matrix Tests.
			try
			{
				_blnAlternateMatrixAttribute = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/alternatematrixattribute").InnerText);
			}
			catch
			{
			}
			// Whether or not the user can mark any piece of Bioware as being Transgenic.
			try
			{
				_blnAllowCustomTransgenics = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/allowcustomtransgenics").InnerText);
			}
			catch
			{
			}
			// Whether or not the user can break Skill Groups while in Create Mode.
			try
			{
				_blnBreakSkillGroupsInCreateMode = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/breakskillgroupsincreatemode").InnerText);
			}
			catch
			{
			}
			// Whether or not any Detection Spell can be taken as Extended range version.
			try
			{
				_blnExtendAnyDetectionSpell = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/extendanydetectionspell").InnerText);
			}
			catch
			{
			}
			// Whether or not dice rolling id allowed for Skills.
			try
			{
				_blnAllowSkillDiceRolling = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/allowskilldicerolling").InnerText);
			}
			catch
			{
			}
			// Whether or not a backup copy of the character should be created before they are placed into Career Mode.
			try
			{
				_blnCreateBackupOnCareer = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/createbackuponcareer").InnerText);
			}
			catch
			{
			}
			// Whether or not the alternate uses for the Leadership Skill should be printed.
			try
			{
				_blnPrintLeadershipAlternates = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/printleadershipalternates").InnerText);
			}
			catch
			{
			}
			// Whether or not the alternate uses for the Arcana Skill should be printed.
			try
			{
				_blnPrintArcanaAlternates = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/printarcanaalternates").InnerText);
			}
			catch
			{
			}
			// Whether or not Notes should be printed.
			try
			{
				_blnPrintNotes = Convert.ToBoolean(objXmlDocument.SelectSingleNode("/settings/printnotes").InnerText);
			}
			catch
			{
			}

			// Attempt to populate the BP vlaues.
			try
			{
				_intBPAttribute = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpattribute").InnerText);
				_intBPAttributeMax = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpattributemax").InnerText);
				_intBPContact = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpcontact").InnerText);
				_intBPMartialArt = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpmartialart").InnerText);
				_intBPMartialArtManeuver = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpmartialartmaneuver").InnerText);
				_intBPSkillGroup = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpskillgroup").InnerText);
				_intBPActiveSkill = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpactiveskill").InnerText);
				_intBPActiveSkillSpecialization = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpactiveskillspecialization").InnerText);
				_intBPKnowledgeSkill = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpknowledgeskill").InnerText);
				_intBPSpell = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpspell").InnerText);
				_intBPFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpfocus").InnerText);
				_intBPSpirit = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpspirit").InnerText);
				_intBPComplexForm = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpcomplexform").InnerText);
				_intBPComplexFormOption = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/bpcost/bpcomplexformoption").InnerText);
			}
			catch
			{
			}

			// Attempt to populate the Karma values.
            try
            {
                _intKarmaAttribute = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaattribute").InnerText);
                _intKarmaQuality = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaquality").InnerText);
                _intKarmaSpecialization = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaspecialization").InnerText);
                _intKarmaNewKnowledgeSkill = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmanewknowledgeskill").InnerText);
                _intKarmaNewActiveSkill = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmanewactiveskill").InnerText);
                _intKarmaNewSkillGroup = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmanewskillgroup").InnerText);
                _intKarmaImproveKnowledgeSkill = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaimproveknowledgeskill").InnerText);
                _intKarmaImproveActiveSkill = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaimproveactiveskill").InnerText);
                _intKarmaImproveSkillGroup = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaimproveskillgroup").InnerText);
                _intKarmaSpell = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaspell").InnerText);
                _intKarmaNewComplexForm = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmanewcomplexform").InnerText);
                _intKarmaImproveComplexForm = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaimprovecomplexform").InnerText);
                _intKarmaNuyenPer = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmanuyenper").InnerText);
                _intKarmaContact = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmacontact").InnerText);
                _intKarmaCarryover = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmacarryover").InnerText);
                _intKarmaSpirit = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaspirit").InnerText);
                _intKarmaManeuver = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmamaneuver").InnerText);
                _intKarmaInitiation = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmainitiation").InnerText);
                _intKarmaMetamagic = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmametamagic").InnerText);
                _intKarmaComplexFormOption = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmacomplexformoption").InnerText);
                _intKarmaJoinGroup = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmajoingroup").InnerText);
                _intKarmaLeaveGroup = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaleavegroup").InnerText);
                _intKarmaComplexFormSkillfot = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmacomplexformskillsoft").InnerText);
            }
            catch
            {
            }

            // Attempt to load the Karma costs for Foci.
			try
			{
				_intKarmaAnchoringFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaanchoringfocus").InnerText);
				_intKarmaBanishingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmabanishingfocus").InnerText);
				_intKarmaBindingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmabindingfocus").InnerText);
				_intKarmaCenteringFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmacenteringfocus").InnerText);
				_intKarmaCounterspellingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmacounterspellingfocus").InnerText);
				_intKarmaDiviningFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmadiviningfocus").InnerText);
				_intKarmaDowsingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmadowsingfocus").InnerText);
				_intKarmaInfusionFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmainfusionfocus").InnerText);
				_intKarmaMaskingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmamaskingfocus").InnerText);
				_intKarmaPowerFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmapowerfocus").InnerText);
				_intKarmaShieldingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmashieldingfocus").InnerText);
				_intKarmaSpellcastingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaspellcastingfocus").InnerText);
				_intKarmaSummoningFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmasummoningfocus").InnerText);
				_intKarmaSustainingFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmasustainingfocus").InnerText);
				_intKarmaSymbolicLinkFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmasymboliclinkfocus").InnerText);
				_intKarmaWeaponFocus = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/karmacost/karmaweaponfocus").InnerText);
			}
			catch
			{
			}

			// Load Books.
			_lstBooks.Clear();
			foreach (XmlNode objXmlBook in objXmlDocument.SelectNodes("/settings/books/book"))
				_lstBooks.Add(objXmlBook.InnerText);

			// Load default build settings.
			_strBuildMethod = objXmlDocument.SelectSingleNode("/settings/defaultbuild/buildmethod").InnerText;
			_intBuildPoints = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/defaultbuild/buildpoints").InnerText);
			_intAvailability = Convert.ToInt32(objXmlDocument.SelectSingleNode("/settings/defaultbuild/availability").InnerText);

			return true;
		}
		#endregion

		#region Properties and Methods
		/// <summary>
		/// Convert a book code into the full name.
		/// </summary>
		/// <param name="strCode">Book code to convert.</param>
		public string BookFromCode(string strCode)
		{
			string strReturn = "";
			XmlNode objXmlBook = _objBookDoc.SelectSingleNode("/chummer/books/book[code = \"" + strCode + "\"]");
			try
			{
				strReturn = objXmlBook["name"].InnerText;
			}
			catch
			{
			}
			return strReturn;
		}

		/// <summary>
		/// Book code (using the translated version if applicable).
		/// </summary>
		/// <param name="strCode">Book code to search for.</param>
		public string LanguageBookShort(string strCode)
		{
			if (strCode == "")
				return "";

			string strReturn = "";
			XmlNode objXmlBook = _objBookDoc.SelectSingleNode("/chummer/books/book[code = \"" + strCode + "\"]");
			try
			{
				if (objXmlBook["altcode"] != null)
					strReturn = objXmlBook["altcode"].InnerText;
				else
					strReturn = strCode;
			}
			catch
			{
			}
			return strReturn;
		}

		/// <summary>
		/// Determine the book's original code by using the alternate code.
		/// </summary>
		/// <param name="strCode">Alternate code to look for.</param>
		public string BookFromAltCode(string strCode)
		{
			if (strCode == "")
				return "";

			XmlNode objXmlBook = _objBookDoc.SelectSingleNode("/chummer/books/book[altcode = \"" + strCode + "\"]");
			if (objXmlBook == null)
				return strCode;
			else
				return objXmlBook["code"].InnerText;
		}

		/// <summary>
		/// Book name (using the translated version if applicable).
		/// </summary>
		/// <param name="strCode">Book code to search for.</param>
		public string LanguageBookLong(string strCode)
		{
			if (strCode == "")
				return "";

			string strReturn = "";
			XmlNode objXmlBook = _objBookDoc.SelectSingleNode("/chummer/books/book[code = \"" + strCode + "\"]");
			try
			{
				if (objXmlBook["translate"] != null)
					strReturn = objXmlBook["translate"].InnerText;
				else
					strReturn = objXmlBook["name"].InnerText;
			}
			catch
			{
			}
			return strReturn;
		}

		/// <summary>
		/// Determine whether or not a given book is in use.
		/// </summary>
		/// <param name="strCode">Book code to search for.</param>
		public bool BookEnabled(string strCode)
		{
			bool blnReturn = false;
			foreach (string strBook in _lstBooks)
			{
				if (strBook == strCode)
				{
					blnReturn = true;
					break;
				}
			}
			return blnReturn;
		}

		/// <summary>
		/// XPath query used to filter items based on the user's selected source books.
		/// </summary>
		public string BookXPath()
		{
			if (_strBookXPath != "")
				return _strBookXPath;

			string strPath = "";

			foreach (string strBook in _lstBooks)
			{
				if (strBook != "")
					strPath += "source = \"" + strBook + "\" or ";
			}
			strPath = strPath.Substring(0, strPath.Length - 4);

			_strBookXPath = strPath;
			
			return strPath;
		}

		/// <summary>
		/// Whether or not all Active Skills with a total score higher than 0 should be printed.
		/// </summary>
		public bool PrintSkillsWithZeroRating
		{
			get
			{
				return _blnPrintSkillsWithZeroRating;
			}
			set
			{
				_blnPrintSkillsWithZeroRating = value;
			}
		}

		/// <summary>
		/// Whether or not the More Lethal Gameplay optional rule is enabled.
		/// </summary>
		public bool MoreLethalGameplay
		{
			get
			{
				return _blnMoreLethalGameplay;
			}
			set
			{
				_blnMoreLethalGameplay = value;
			}
		}

		/// <summary>
		/// Whether or not the maximum Skill rating modifiers are set.
		/// </summary>
		public bool EnforceMaximumSkillRatingModifier
		{
			get
			{
				return _blnEnforceSkillMaximumModifiedRating;
			}
			set
			{
				_blnEnforceSkillMaximumModifiedRating = value;
			}
		}

		/// <summary>
		/// Whether or not total Skill ratings are capped at 20 or 2 x natural Attribute + Rating, whichever is higher.
		/// </summary>
		public bool CapSkillRating
		{
			get
			{
				return _blnCapSkillRating;
			}
			set
			{
				_blnCapSkillRating = value;
			}
		}

		/// <summary>
		/// Whether or not the Karma and Nueyn Expenses should be printed on the character sheet.
		/// </summary>
		public bool PrintExpenses
		{
			get
			{
				return _blnPrintExpenses;
			}
			set
			{
				_blnPrintExpenses = value;
			}
		}

		/// <summary>
		/// Amount of Nuyen gained per BP spent.
		/// </summary>
		public int NuyenPerBP
		{
			get
			{
				return _intNuyenPerBP;
			}
			set
			{
				_intNuyenPerBP = value;
			}
		}

		/// <summary>
		/// Whether or not characters are allowed to put points into a Skill Group again once it is broken and all Ratings are the same.
		/// </summary>
		public bool AllowSkillRegrouping
		{
			get
			{
				return _blnAllowSkillRegrouping;
			}
			set
			{
				_blnAllowSkillRegrouping = value;
			}
		}

		/// <summary>
		/// Whether or not confirmation messages are shown when deleting an object.
		/// </summary>
		public bool ConfirmDelete
		{
			get
			{
				return _blnConfirmDelete;
			}
			set
			{
				_blnConfirmDelete = value;
			}
		}

		/// <summary>
		/// Wehther or not confirmation messages are shown for Karma Expenses.
		/// </summary>
		public bool ConfirmKarmaExpense
		{
			get
			{
				return _blnConfirmKarmaExpense;
			}
			set
			{
				_blnConfirmKarmaExpense = value;
			}
		}

		/// <summary>
		/// Sourcebooks.
		/// </summary>
		public List<string> Books
		{
			get
			{
				return _lstBooks;
			}
		}

		/// <summary>
		/// Setting name.
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
			}
		}

		/// <summary>
		/// Whether or not Metatypes cost Karma equal to their BP when creating a character with Karma.
		/// </summary>
		public bool MetatypeCostsKarma
		{
			get
			{
				return _blnMetatypeCostsKarma;
			}
			set
			{
				_blnMetatypeCostsKarma = value;
			}
		}

		/// <summary>
		/// Mutiplier for Metatype Karma Costs when converting from BP to Karma.
		/// </summary>
		public int MetatypeCostsKarmaMultiplier
		{
			get
			{
				return _intMetatypeCostMultiplier;
			}
			set
			{
				_intMetatypeCostMultiplier = value;
			}
		}

		/// <summary>
		/// Number of Limbs a standard character has.
		/// </summary>
		public int LimbCount
		{
			get
			{
				return _intLimbCount;
			}
			set
			{
				_intLimbCount = value;
			}
		}

		/// <summary>
		/// Exclude a particular Limb Slot from count towards the Limb Count.
		/// </summary>
		public string ExcludeLimbSlot
		{
			get
			{
				return _strExcludeLimbSlot;
			}
			set
			{
				_strExcludeLimbSlot = value;
			}
		}

		/// <summary>
		/// Whether or not a character's Strength affects Weapon Recoil.
		/// </summary>
		public bool StrengthAffectsRecoil
		{
			get
			{
				return _blnStrengthAffectsRecoil;
			}
			set
			{
				_blnStrengthAffectsRecoil = value;
			}
		}

		/// <summary>
		/// Whether or not Maximum Armor Modifications is in use.
		/// </summary>
		public bool MaximumArmorModifications
		{
			get
			{
				return _blnMaximumArmorModifications;
			}
			set
			{
				_blnMaximumArmorModifications = value;
			}
		}

		/// <summary>
		/// Whether or not Armor Suit Capacity is in use.
		/// </summary>
		public bool ArmorSuitCapacity
		{
			get
			{
				return _blnArmorSuitCapacity;
			}
			set
			{
				_blnArmorSuitCapacity = value;
			}
		}

		/// <summary>
		/// Whether or not Armor Degredation is allowed.
		/// </summary>
		public bool ArmorDegradation
		{
			get
			{
				return _blnArmorDegradation;
			}
			set
			{
				_blnArmorDegradation = value;
			}
		}

		/// <summary>
		/// Whether or not the Copy Protection Program Option should automatically be added.
		/// </summary>
		public bool AutomaticCopyProtection
		{
			get
			{
				return _blnAutomaticCopyProtection;
			}
			set
			{
				_blnAutomaticCopyProtection = value;
			}
		}

		/// <summary>
		/// Whether or not the Registration Program Option should automatically be added.
		/// </summary>
		public bool AutomaticRegistration
		{
			get
			{
				return _blnAutomaticRegistration;
			}
			set
			{
				_blnAutomaticRegistration = value;
			}
		}

		/// <summary>
		/// Whether or not option for Ergonomic Programs affecting a Commlink's effective Response is enabled.
		/// </summary>
		public bool ErgonomicProgramLimit
		{
			get
			{
				return _blnErgonomicProgramsLimit;
			}
			set
			{
				_blnErgonomicProgramsLimit = value;
			}
		}

		/// <summary>
		/// Whether or not Vehicles should use the average Rating of all of their Sensors instead of the Sensor Rating given to the Vehicle itself.
		/// </summary>
		public bool UseCalculatedVehicleSensorRatings
		{
			get
			{
				return _blnUseCalculatedVehicleSensorRatings;
			}
			set
			{
				_blnUseCalculatedVehicleSensorRatings = value;
			}
		}

		/// <summary>
		/// Number of decimal places to round to when calculating Essence.
		/// </summary>
		public int EssenceDecimals
		{
			get
			{
				return _intEssenceDecimals;
			}
			set
			{
				_intEssenceDecimals = value;
			}
		}

		/// <summary>
		/// Whether or not Capacity limits should be enforced.
		/// </summary>
		public bool EnforceCapacity
		{
			get
			{
				return _blnEnforceCapacity;
			}
			set
			{
				_blnEnforceCapacity = value;
			}
		}

		/// <summary>
		/// Whether or not Recoil modifiers are restricted (AR 148).
		/// </summary>
		public bool RestrictRecoil
		{
			get
			{
				return _blnRestrictRecoil;
			}
			set
			{
				_blnRestrictRecoil = value;
			}
		}

		/// <summary>
		/// Whether or not a Commlink's Response should be calculated based on the number of programs running on it.
		/// </summary>
		public bool CalculateCommlinkResponse
		{
			get
			{
				return _blnCalculateCommlinkResponse;
			}
			set
			{
				_blnCalculateCommlinkResponse = value;
			}
		}

		/// <summary>
		/// Whether or not Coplex Forms have the same BP/Karma cost as Spells.
		/// </summary>
		public bool AlternateComplexFormCost
		{
			get
			{
				return _blnAlternateComplexFormCost;
			}
			set
			{
				_blnAlternateComplexFormCost = value;
			}
		}

		/// <summary>
		/// Whether or not LOG is used in place of Program Ratings for Matrix Tests.
		/// </summary>
		public bool AlternateMatrixAttribute
		{
			get
			{
				return _blnAlternateMatrixAttribute;
			}
			set
			{
				_blnAlternateMatrixAttribute = value;
			}
		}

		/// <summary>
		/// Whether or not the user can mark any piece of Bioware as being Transgenic.
		/// </summary>
		public bool AllowCustomTransgenics
		{
			get
			{
				return _blnAllowCustomTransgenics;
			}
			set
			{
				_blnAllowCustomTransgenics = value;
			}
		}

		/// <summary>
		/// Whether or not the user is allowed to break Skill Groups while in Create Mode.
		/// </summary>
		public bool BreakSkillGroupsInCreateMode
		{
			get
			{
				return _blnBreakSkillGroupsInCreateMode;
			}
			set
			{
				_blnBreakSkillGroupsInCreateMode = value;
			}
		}

		/// <summary>
		/// Whether or not any Detection Spell can be taken as Extended range version.
		/// </summary>
		public bool ExtendAnyDetectionSpell
		{
			get
			{
				return _blnExtendAnyDetectionSpell;
			}
			set
			{
				_blnExtendAnyDetectionSpell = value;
			}
		}

		/// <summary>
		/// Whether or not dice rolling is allowed for Skills.
		/// </summary>
		public bool AllowSkillDiceRolling
		{
			get
			{
				return _blnAllowSkillDiceRolling;
			}
			set
			{
				_blnAllowSkillDiceRolling = value;
			}
		}

		/// <summary>
		/// Whether or not a backup copy of the character should be created before they are placed into Career Mode.
		/// </summary>
		public bool CreateBackupOnCareer
		{
			get
			{
				return _blnCreateBackupOnCareer;
			}
			set
			{
				_blnCreateBackupOnCareer = value;
			}
		}

		/// <summary>
		/// Whether or not the alternate uses for the Leadership Skill should be printed.
		/// </summary>
		public bool PrintLeadershipAlternates
		{
			get
			{
				return _blnPrintLeadershipAlternates;
			}
			set
			{
				_blnPrintLeadershipAlternates = value;
			}
		}

		/// <summary>
		/// Whether or not a backup copy of the character should be created before they are placed into Career Mode.
		/// </summary>
		public bool PrintArcanaAlternates
		{
			get
			{
				return _blnPrintArcanaAlternates;
			}
			set
			{
				_blnPrintArcanaAlternates = value;
			}
		}

		/// <summary>
		/// Whether or not Notes should be printed.
		/// </summary>
		public bool PrintNotes
		{
			get
			{
				return _blnPrintNotes;
			}
			set
			{
				_blnPrintNotes = value;
			}
		}
		#endregion

		#region BP
		/// <summary>
		/// BP cost for each Attribute = this value.
		/// </summary>
		public int BPAttribute
		{
			get
			{
				return _intBPAttribute;
			}
			set
			{
				_intBPAttribute = value;
			}
		}

		/// <summary>
		/// BP cost to raise an Attribute to its Metatype Maximum = this value.
		/// </summary>
		public int BPAttributeMax
		{
			get
			{
				return _intBPAttributeMax;
			}
			set
			{
				_intBPAttributeMax = value;
			}
		}

		/// <summary>
		/// BP cost for each Loyalty, Connection, and Group point = this value.
		/// </summary>
		public int BPContact
		{
			get
			{
				return _intBPContact;
			}
			set
			{
				_intBPContact = value;
			}
		}

		/// <summary>
		/// BP cost for each Martial Arts Rating = this value.
		/// </summary>
		public int BPMartialArt
		{
			get
			{
				return _intBPMartialArt;
			}
			set
			{
				_intBPMartialArt = value;
			}
		}

		/// <summary>
		/// BP cost for each Martial Art Maneuver = this value.
		/// </summary>
		public int BPMartialArtManeuver
		{
			get
			{
				return _intBPMartialArtManeuver;
			}
			set
			{
				_intBPMartialArtManeuver = value;
			}
		}

		/// <summary>
		/// BP cost for each Skill Group Rating = this value.
		/// </summary>
		public int BPSkillGroup
		{
			get
			{
				return _intBPSkillGroup;
			}
			set
			{
				_intBPSkillGroup = value;
			}
		}

		/// <summary>
		/// BP cost for each Active Skill Rating = this value.
		/// </summary>
		public int BPActiveSkill
		{
			get
			{
				return _intBPActiveSkill;
			}
			set
			{
				_intBPActiveSkill = value;
			}
		}

		/// <summary>
		/// BP cost for each Active Skill Specialization = this value.
		/// </summary>
		public int BPActiveSkillSpecialization
		{
			get
			{
				return _intBPActiveSkillSpecialization;
			}
			set
			{
				_intBPActiveSkillSpecialization = value;
			}
		}

		/// <summary>
		/// BP cost for each Knowledge Skill Rating = this value.
		/// </summary>
		public int BPKnowledgeSkill
		{
			get
			{
				return _intBPKnowledgeSkill;
			}
			set
			{
				_intBPKnowledgeSkill = value;
			}
		}

		/// <summary>
		/// BP cost for each Spell = this value.
		/// </summary>
		public int BPSpell
		{
			get
			{
				return _intBPSpell;
			}
			set
			{
				_intBPSpell = value;
			}
		}

		/// <summary>
		/// BP cost for each Rating of Foci.
		/// </summary>
		public int BPFocus
		{
			get
			{
				return _intBPFocus;
			}
			set
			{
				_intBPFocus = value;
			}
		}

		/// <summary>
		/// BP cost for each service a Sprit owes = this value.
		/// </summary>
		public int BPSpirit
		{
			get
			{
				return _intBPSpirit;
			}
			set
			{
				_intBPSpirit = value;
			}
		}

		/// <summary>
		/// BP cost for each Complex Form Rating = this value.
		/// </summary>
		public int BPComplexForm
		{
			get
			{
				return _intBPComplexForm;
			}
			set
			{
				_intBPComplexForm = value;
			}
		}

		/// <summary>
		/// BP cost for each Complex Form Option Rating = this value.
		/// </summary>
		public int BPComplexFormOption
		{
			get
			{
				return _intBPComplexFormOption;
			}
			set
			{
				_intBPComplexFormOption = value;
			}
		}
		#endregion

		#region Karma
		/// <summary>
		/// Karma cost to improve an Attribute = New Rating X this value.
		/// </summary>
		public int KarmaAttribute
		{
			get
			{
				return _intKarmaAttribute;
			}
			set
			{
				_intKarmaAttribute = value;
			}
		}

		/// <summary>
		/// Karma cost to purchase a Quality = BP Cost x this value.
		/// </summary>
		public int KarmaQuality
		{
			get
			{
				return _intKarmaQuality;
			}
			set
			{
				_intKarmaQuality = value;
			}
		}

		/// <summary>
		/// Karma cost to purchase a Specialization = this value.
		/// </summary>
		public int KarmaSpecialization
		{
			get
			{
				return _intKarmaSpecialization;
			}
			set
			{
				_intKarmaSpecialization = value;
			}
		}

		/// <summary>
		/// Karma cost to purchase a new Knowledge Skill = this value.
		/// </summary>
		public int KarmaNewKnowledgeSkill
		{
			get
			{
				return _intKarmaNewKnowledgeSkill;
			}
			set
			{
				_intKarmaNewKnowledgeSkill = value;
			}
		}

		/// <summary>
		/// Karma cost to purchase a new Active Skill = this value.
		/// </summary>
		public int KarmaNewActiveSkill
		{
			get
			{
				return _intKarmaNewActiveSkill;
			}
			set
			{
				_intKarmaNewActiveSkill = value;
			}
		}

		/// <summary>
		/// Karma cost to purchase a new Skill Group = this value.
		/// </summary>
		public int KarmaNewSkillGroup
		{
			get
			{
				return _intKarmaNewSkillGroup;
			}
			set
			{
				_intKarmaNewSkillGroup = value;
			}
		}

		/// <summary>
		/// Karma cost to improve a Knowledge Skill = New Rating x this value.
		/// </summary>
		public int KarmaImproveKnowledgeSkill
		{
			get
			{
				return _intKarmaImproveKnowledgeSkill;
			}
			set
			{
				_intKarmaImproveKnowledgeSkill = value;
			}
		}

		/// <summary>
		/// Karma cost to improve an Active Skill = New Rating x this value.
		/// </summary>
		public int KarmaImproveActiveSkill
		{
			get
			{
				return _intKarmaImproveActiveSkill;
			}
			set
			{
				_intKarmaImproveActiveSkill = value;
			}
		}

		/// <summary>
		/// Karma cost to improve a Skill Group = New Rating x this value.
		/// </summary>
		public int KarmaImproveSkillGroup
		{
			get
			{
				return _intKarmaImproveSkillGroup;
			}
			set
			{
				_intKarmaImproveSkillGroup = value;
			}
		}

		/// <summary>
		/// Karma cost for each Spell = this value.
		/// </summary>
		public int KarmaSpell
		{
			get
			{
				return _intKarmaSpell;
			}
			set
			{
				_intKarmaSpell = value;
			}
		}

		/// <summary>
		/// Karma cost for a new Complex Form = this value.
		/// </summary>
		public int KarmaNewComplexForm
		{
			get
			{
				return _intKarmaNewComplexForm;
			}
			set
			{
				_intKarmaNewComplexForm = value;
			}
		}

		/// <summary>
		/// Karma cost to improve a Complex Form = New Rating x this value.
		/// </summary>
		public int KarmaImproveComplexForm
		{
			get
			{
				return _intKarmaImproveComplexForm;
			}
			set
			{
				_intKarmaImproveComplexForm = value;
			}
		}

		/// <summary>
		/// Karma cost for Complex Form Options = Rating x this value.
		/// </summary>
		public int KarmaComplexFormOption
		{
			get
			{
				return _intKarmaComplexFormOption;
			}
			set
			{
				_intKarmaComplexFormOption = value;
			}
		}

		/// <summary>
		/// Karma cost for Complex Form Skillsofts = Rating x this value.
		/// </summary>
		public int KarmaComplexFormSkillsoft
		{
			get
			{
				return _intKarmaComplexFormSkillfot;
			}
			set
			{
				_intKarmaComplexFormSkillfot = value;
			}
		}

		/// <summary>
		/// Amount of Nueyn objtained per Karma point.
		/// </summary>
		public int KarmaNuyenPer
		{
			get
			{
				return _intKarmaNuyenPer;
			}
			set
			{
				_intKarmaNuyenPer = value;
			}
		}

		/// <summary>
		/// Karma cost for a Contact = (Connection + Loyalty) x this value.
		/// </summary>
		public int KarmaContact
		{
			get
			{
				return _intKarmaContact;
			}
			set
			{
				_intKarmaContact = value;
			}
		}

		/// <summary>
		/// Maximum amount of remaining Karma that is carried over to the character once they are created.
		/// </summary>
		public int KarmaCarryover
		{
			get
			{
				return _intKarmaCarryover;
			}
			set
			{
				_intKarmaCarryover = value;
			}
		}

		/// <summary>
		/// Karma cost for a Spirit = this value.
		/// </summary>
		public int KarmaSpirit
		{
			get
			{
				return _intKarmaSpirit;
			}
			set
			{
				_intKarmaSpirit = value;
			}
		}

		/// <summary>
		/// Karma cost for a Combat Maneuver = this value.
		/// </summary>
		public int KarmaManeuver
		{
			get
			{
				return _intKarmaManeuver;
			}
			set
			{
				_intKarmaManeuver = value;
			}
		}

		/// <summary>
		/// Karma cost for a Initiation = 10 + (New Rating x this value).
		/// </summary>
		public int KarmaInitiation
		{
			get
			{
				return _intKarmaInitiation;
			}
			set
			{
				_intKarmaInitiation = value;
			}
		}

		/// <summary>
		/// Karma cost for a Metamagic = this value.
		/// </summary>
		public int KarmaMetamagic
		{
			get
			{
				return _intKarmaMetamagic;
			}
			set
			{
				_intKarmaMetamagic = value;
			}
		}

		/// <summary>
		/// Karma cost to join a Group = this value.
		/// </summary>
		public int KarmaJoinGroup
		{
			get
			{
				return _intKarmaJoinGroup;
			}
			set
			{
				_intKarmaJoinGroup = value;
			}
		}

		/// <summary>
		/// Karma cost to leave a Group = this value.
		/// </summary>
		public int KarmaLeaveGroup
		{
			get
			{
				return _intKarmaLeaveGroup;
			}
			set
			{
				_intKarmaLeaveGroup = value;
			}
		}

		/// <summary>
		/// Karma cost for Anchoring Foci.
		/// </summary>
		public int KarmaAnchoringFocus
		{
			get
			{
				return _intKarmaAnchoringFocus;
			}
			set
			{
				_intKarmaAnchoringFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Banishing Foci.
		/// </summary>
		public int KarmaBanishingFocus
		{
			get
			{
				return _intKarmaBanishingFocus;
			}
			set
			{
				_intKarmaBanishingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Binding Foci.
		/// </summary>
		public int KarmaBindingFocus
		{
			get
			{
				return _intKarmaBindingFocus;
			}
			set
			{
				_intKarmaBindingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Centering Foci.
		/// </summary>
		public int KarmaCenteringFocus
		{
			get
			{
				return _intKarmaCenteringFocus;
			}
			set
			{
				_intKarmaCenteringFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Counterspelling Foci.
		/// </summary>
		public int KarmaCounterspellingFocus
		{
			get
			{
				return _intKarmaCounterspellingFocus;
			}
			set
			{
				_intKarmaCounterspellingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Divining Foci.
		/// </summary>
		public int KarmaDiviningFocus
		{
			get
			{
				return _intKarmaDiviningFocus;
			}
			set
			{
				_intKarmaDiviningFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Dowsing Foci.
		/// </summary>
		public int KarmaDowsingFocus
		{
			get
			{
				return _intKarmaDowsingFocus;
			}
			set
			{
				_intKarmaDowsingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Infusion Foci.
		/// </summary>
		public int KarmaInfusionFocus
		{
			get
			{
				return _intKarmaInfusionFocus;
			}
			set
			{
				_intKarmaInfusionFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Masking Foci.
		/// </summary>
		public int KarmaMaskingFocus
		{
			get
			{
				return _intKarmaMaskingFocus;
			}
			set
			{
				_intKarmaMaskingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Power Foci.
		/// </summary>
		public int KarmaPowerFocus
		{
			get
			{
				return _intKarmaPowerFocus;
			}
			set
			{
				_intKarmaPowerFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Shielding Foci.
		/// </summary>
		public int KarmaShieldingFocus
		{
			get
			{
				return _intKarmaShieldingFocus;
			}
			set
			{
				_intKarmaShieldingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Spellcasting Foci.
		/// </summary>
		public int KarmaSpellcastingFocus
		{
			get
			{
				return _intKarmaSpellcastingFocus;
			}
			set
			{
				_intKarmaSpellcastingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Summoning Foci.
		/// </summary>
		public int KarmaSummoningFocus
		{
			get
			{
				return _intKarmaSummoningFocus;
			}
			set
			{
				_intKarmaSummoningFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Sustaining Foci.
		/// </summary>
		public int KarmaSustainingFocus
		{
			get
			{
				return _intKarmaSustainingFocus;
			}
			set
			{
				_intKarmaSustainingFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Symbolic Link Foci.
		/// </summary>
		public int KarmaSymbolicLinkFocus
		{
			get
			{
				return _intKarmaSymbolicLinkFocus;
			}
			set
			{
				_intKarmaSymbolicLinkFocus = value;
			}
		}

		/// <summary>
		/// Karma cost for Weapon Foci.
		/// </summary>
		public int KarmaWeaponFocus
		{
			get
			{
				return _intKarmaWeaponFocus;
			}
			set
			{
				_intKarmaWeaponFocus = value;
			}
		}
		#endregion

		#region Default Build
		/// <summary>
		/// Default build method.
		/// </summary>
		public string BuildMethod
		{
			get
			{
				return _strBuildMethod;
			}
			set
			{
				_strBuildMethod = value;
			}
		}

		/// <summary>
		/// Default number of build points.
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
		/// Default Availability.
		/// </summary>
		public int Availability
		{
			get
			{
				return _intAvailability;
			}
			set
			{
				_intAvailability = value;
			}
		}
		#endregion
	}
}