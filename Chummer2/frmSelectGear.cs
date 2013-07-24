using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Chummer
{
	public partial class frmSelectGear : Form
	{
		private string _strSelectedGear = "";
        private string _strSelectedCategory = "";
		private int _intSelectedRating = 0;
		private int _intSelectedQty = 1;
		private int _intMarkup = 0;

		private int _intMaxResponse = 0;
		private int _intMaxSignal = 0;
		private int _intMaxFirewall = 0;
		private int _intMaxSystem = 0;

		private int _intAvailModifier = 0;
		private int _intCostMultiplier = 1;

		private string _strAllowedCategories = "";
		private int _intMaximumCapacity = -1;
		private bool _blnAddAgain = false;
		private static string _strSelectCategory = "";
		private bool _blnShowPositiveCapacityOnly = false;
		private bool _blnShowNegativeCapacityOnly = false;
		private bool _blnShowArmorCapacityOnly = false;
		private CapacityStyle _objCapacityStyle = CapacityStyle.Standard;

		private XmlDocument _objXmlDocument = new XmlDocument();
		private readonly Character _objCharacter;

		private List<ListItem> _lstCategory = new List<ListItem>();

		#region Control Events
		public frmSelectGear(Character objCharacter, bool blnCareer = false, int intAvailModifier = 0, int intCostMultiplier = 1)
		{
			InitializeComponent();
			LanguageManager.Instance.Load(this);
			chkFreeItem.Visible = blnCareer;
			lblMarkupLabel.Visible = blnCareer;
			nudMarkup.Visible = blnCareer;
			lblMarkupPercentLabel.Visible = blnCareer;
			_intAvailModifier = intAvailModifier;
			_intCostMultiplier = intCostMultiplier;
			_objCharacter = objCharacter;
			// Stack Checkbox is only available in Career Mode.
			if (!_objCharacter.Created)
			{
				chkStack.Checked = false;
				chkStack.Visible = false;
			}

			MoveControls();
		}

		private void frmSelectGear_Load(object sender, EventArgs e)
		{
			foreach (Label objLabel in this.Controls.OfType<Label>())
			{
				if (objLabel.Text.StartsWith("["))
					objLabel.Text = "";
			}

			XmlNodeList objXmlCategoryList;

			// Load the Gear information.
			_objXmlDocument = XmlManager.Instance.Load("gear.xml");

			// Populate the Gear Category list.
			if (_strAllowedCategories != "")
			{
				if (_strAllowedCategories != "Ammunition")
					nudGearQty.Enabled = false;
				string[] strAllowed = _strAllowedCategories.Split(',');
				string strMount = "";
				foreach (string strAllowedMount in strAllowed)
				{
					if (strAllowedMount != "")
						strMount += ". = \"" + strAllowedMount + "\" or ";
				}
				strMount += "category = \"General\"";
				objXmlCategoryList = _objXmlDocument.SelectNodes("/chummer/categories/category[" + strMount + "]");
			}
			else
			{
				objXmlCategoryList = _objXmlDocument.SelectNodes("/chummer/categories/category");
			}

			// Create a list of any Categories that should not be in the list.
			List<string> lstRemoveCategory = new List<string>();
			foreach (XmlNode objXmlCategory in objXmlCategoryList)
			{
				bool blnRemoveItem = true;

				if (objXmlCategory.Attributes["show"] != null)
				{
					if (objXmlCategory.Attributes["show"].InnerText == "false")
					{
						string[] strAllowed = _strAllowedCategories.Split(',');
						foreach (string strAllowedMount in strAllowed)
						{
							if (strAllowedMount == objXmlCategory.InnerText)
								blnRemoveItem = false;
						}
					}

					if (blnRemoveItem)
						lstRemoveCategory.Add(objXmlCategory.InnerText);
				}

				string strXPath = "/chummer/gears/gear[category = \"" + objXmlCategory.InnerText + "\" and (" + _objCharacter.Options.BookXPath() + ")";
				if (_blnShowArmorCapacityOnly)
					strXPath += " and contains(armorcapacity, \"[\")";
				else
				{
					if (_blnShowPositiveCapacityOnly)
						strXPath += " and not(contains(capacity, \"[\"))";
				}
				strXPath += "]";

				XmlNodeList objItems = _objXmlDocument.SelectNodes(strXPath);
				if (objItems.Count > 0)
					blnRemoveItem = false;

				if (blnRemoveItem)
					lstRemoveCategory.Add(objXmlCategory.InnerText);
			}

			foreach (XmlNode objXmlCategory in objXmlCategoryList)
			{
				// Make sure the Category isn't in the exclusion list.
				bool blnAddItem = true;
				foreach (string strCategory in lstRemoveCategory)
				{
					if (strCategory == objXmlCategory.InnerText)
						blnAddItem = false;
				}
				// Also make sure it is not already in the Category list.
				foreach (ListItem objItem in _lstCategory)
				{
					if (objItem.Value == objXmlCategory.InnerText)
						blnAddItem = false;
				}

				if (blnAddItem)
				{
					ListItem objItem = new ListItem();
					objItem.Value = objXmlCategory.InnerText;
					if (objXmlCategory.Attributes != null)
					{
						if (objXmlCategory.Attributes["translate"] != null)
							objItem.Name = objXmlCategory.Attributes["translate"].InnerText;
						else
							objItem.Name = objXmlCategory.InnerText;
					}
					else
						objItem.Name = objXmlCategory.InnerXml;
					_lstCategory.Add(objItem);
				}
			}
			SortListItem objSort = new SortListItem();
			_lstCategory.Sort(objSort.Compare);
			cboCategory.DataSource = null;
			cboCategory.ValueMember = "Value";
			cboCategory.DisplayMember = "Name";
			cboCategory.DataSource = _lstCategory;

			// Select the first Category in the list.
			if (_strSelectCategory == "")
				cboCategory.SelectedIndex = 0;
			else
				cboCategory.SelectedValue = _strSelectCategory;

			if (cboCategory.SelectedIndex == -1)
				cboCategory.SelectedIndex = 0;

			if (_strSelectedGear != "")
				lstGear.SelectedValue = _strSelectedGear;
		}

		private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCategory.SelectedValue == null)
				return;

			// Update the list of Weapon based on the selected Category.
			XmlNodeList objXmlGearList;
			List<ListItem> lstGears = new List<ListItem>();
			txtSearch.Text = "";

			// Retrieve the list of Gear for the selected Category.
			if (cboCategory.SelectedValue.ToString() != "Commlink Upgrade" && cboCategory.SelectedValue.ToString() != "Commlink Operating System Upgrade")
			{
				if (!_blnShowNegativeCapacityOnly && !_blnShowPositiveCapacityOnly && !_blnShowArmorCapacityOnly)
					objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ")]");
				else
				{
					if (_blnShowArmorCapacityOnly)
						objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ") and contains(armorcapacity, \"[\")]");
					else
					{
						if (_blnShowPositiveCapacityOnly)
							objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ") and not(contains(capacity, \"[\"))]");
						else
							objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ") and contains(capacity, \"[\")]");
					}
				}
			}
			else
			{
				if (cboCategory.SelectedValue.ToString() == "Commlink Upgrade")
				{
					// Custom Commlinks have no MaxResponse or MaxSignal (they're 2 since 0 + 2 = 2).
					if (_intMaxResponse == 2 && _intMaxSignal == 2)
						objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ")]");
					else
						objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ") and ((response > " + (_intMaxResponse - 2) + " and response <= " + _intMaxResponse + ") or (signal > " + (_intMaxSignal - 2) + " and signal <= " + _intMaxSignal + "))]");
				}
				else
				{
					objXmlGearList = _objXmlDocument.SelectNodes("/chummer/gears/gear[category = \"" + cboCategory.SelectedValue + "\" and (" + _objCharacter.Options.BookXPath() + ") and ((firewall > " + (_intMaxFirewall) + ") or (system > " + (_intMaxSystem) + "))]");
				}
			}

			foreach (XmlNode objXmlGear in objXmlGearList)
			{
				ListItem objItem = new ListItem();
				objItem.Value = objXmlGear["id"].InnerText;
				if (objXmlGear["translate"] != null)
					objItem.Name = objXmlGear["translate"].InnerText;
				else
					objItem.Name = objXmlGear["name"].InnerText;
				lstGears.Add(objItem);
			}
			SortListItem objSort = new SortListItem();
			lstGears.Sort(objSort.Compare);
			lstGear.DataSource = null;
			lstGear.ValueMember = "Value";
			lstGear.DisplayMember = "Name";
			lstGear.DataSource = lstGears;

			// Show the Do It Yourself CheckBox if the Commlink Upgrade category is selected.
			if (cboCategory.SelectedValue.ToString() == "Commlink Upgrade")
				chkDoItYourself.Visible = true;
			else
			{
				chkDoItYourself.Visible = false;
				chkDoItYourself.Checked = false;
			}
		}

		private void lstGear_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstGear.SelectedValue == null)
				return;

			// Retireve the information for the selected piece of Cyberware.
			XmlNode objXmlGear;
			objXmlGear = _objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + lstGear.SelectedValue + "\"]");

			// If a Grenade is selected, show the Aerodynamic checkbox.
			if (objXmlGear["name"].InnerText.StartsWith("Grenade:"))
				chkAerodynamic.Visible = true;
			else
			{
				chkAerodynamic.Visible = false;
				chkAerodynamic.Checked = false;
			}

			// Quantity.
			nudGearQty.Minimum = 1;
			if (objXmlGear["costfor"] != null)
			{
				nudGearQty.Value = Convert.ToInt32(objXmlGear["costfor"].InnerText);
				nudGearQty.Increment = Convert.ToInt32(objXmlGear["costfor"].InnerText);
			}
			else
			{
				nudGearQty.Value = 1;
				nudGearQty.Increment = 1;
			}

			UpdateGearInfo();
		}

		private void nudRating_ValueChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (lstGear.Text != "")
				AcceptForm();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			if (txtSearch.Text == "")
			{
				cboCategory_SelectedIndexChanged(sender, e);
				return;
			}

			string strCategoryFilter = "";

			if (_strAllowedCategories != "")
			{
				string[] strAllowed = _strAllowedCategories.Split(',');
				foreach (string strAllowedMount in strAllowed)
				{
					if (strAllowedMount != "")
						strCategoryFilter += ". = \"" + strAllowedMount + "\" or ";
				}
				strCategoryFilter += "category = \"General\"";
			}
			else
				strCategoryFilter += "category != \"General\"";

			// Treat everything as being uppercase so the search is case-insensitive.
			string strSearch = "/chummer/gears/gear[(" + _objCharacter.Options.BookXPath() + ") and ((contains(translate(name,'abcdefghijklmnopqrstuvwxyzàáâãäåçèéêëìíîïñòóôõöùúûüýß','ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝß'), \"" + txtSearch.Text.ToUpper() + "\") and not(translate)) or contains(translate(translate,'abcdefghijklmnopqrstuvwxyzàáâãäåçèéêëìíîïñòóôõöùúûüýß','ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝß'), \"" + txtSearch.Text.ToUpper() + "\"))]";

			XmlNodeList objXmlGearList = _objXmlDocument.SelectNodes(strSearch);
			List<ListItem> lstGears = new List<ListItem>();
			bool blnAddToList;
			foreach (XmlNode objXmlGear in objXmlGearList)
			{
				blnAddToList = true;
				if (_blnShowArmorCapacityOnly)
				{
					if (objXmlGear["armorcapacity"] == null)
						blnAddToList = false;
				}
				// Only add items that appear in the list of Categories.
				bool blnFound = false;
				foreach (object objListItem in cboCategory.Items)
				{
					ListItem objCategoryItem = (ListItem)objListItem;
					if (objCategoryItem.Value == objXmlGear["category"].InnerText)
					{
						blnFound = true;
						break;
					}
				}
				if (!blnFound)
					blnAddToList = false;

				if (blnAddToList)
				{
					ListItem objItem = new ListItem();
                    objItem.Value = objXmlGear["id"].InnerText;
					if (objXmlGear["translate"] != null)
						objItem.Name = objXmlGear["translate"].InnerText;
					else
						objItem.Name = objXmlGear["name"].InnerText;

					try
					{
						objItem.Name += " [" + _lstCategory.Find(objFind => objFind.Value == objXmlGear["category"].InnerText).Name + "]";
						lstGears.Add(objItem);
					}
					catch
					{
					}
				}
			}
			SortListItem objSort = new SortListItem();
			lstGears.Sort(objSort.Compare);
			lstGear.DataSource = null;
			lstGear.ValueMember = "Value";
			lstGear.DisplayMember = "Name";
			lstGear.DataSource = lstGears;
		}

		private void lstGear_DoubleClick(object sender, EventArgs e)
		{
			if (lstGear.Text != "")
				AcceptForm();
		}

		private void cmdOKAdd_Click(object sender, EventArgs e)
		{
			_blnAddAgain = true;
			cmdOK_Click(sender, e);
		}

		private void nudGearQty_ValueChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void chkFreeItem_CheckedChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void chkDoItYourself_CheckedChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void nudMarkup_ValueChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void chkHacked_CheckedChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Down)
			{
				try
				{
					lstGear.SelectedIndex++;
				}
				catch
				{
					try
					{
						lstGear.SelectedIndex = 0;
					}
					catch
					{
					}
				}
			}
			if (e.KeyCode == Keys.Up)
			{
				try
				{
					lstGear.SelectedIndex--;
					if (lstGear.SelectedIndex == -1)
						lstGear.SelectedIndex = lstGear.Items.Count - 1;
				}
				catch
				{
					try
					{
						lstGear.SelectedIndex = lstGear.Items.Count - 1;
					}
					catch
					{
					}
				}
			}
		}

		private void txtSearch_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
				txtSearch.Select(txtSearch.Text.Length, 0);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Whether or not the user wants to add another item after this one.
		/// </summary>
		public bool AddAgain
		{
			get
			{
				return _blnAddAgain;
			}
		}

		/// <summary>
		/// Commlink's Response which will determine the highest Response Upgrade available.
		/// </summary>
		public int CommlinkResponse
		{
			set
			{
				_intMaxResponse = value + 2;
			}
		}

		/// <summary>
		/// Commlink's Signal which will determine the highest Signal Upgrade available.
		/// </summary>
		public int CommlinkSignal
		{
			set
			{
				_intMaxSignal = value + 2;
			}
		}

		/// <summary>
		/// Commlink's Firewall which will determine the highest Firewall Upgrade available.
		/// </summary>
		public int CommlinkFirewall
		{
			set
			{
				_intMaxFirewall = value;
			}
		}

		/// <summary>
		/// Commlink's System which will determine the highest System Upgrade available.
		/// </summary>
		public int CommlinkSystem
		{
			set
			{
				_intMaxSystem = value;
			}
		}

		/// <summary>
		/// Only items that grant Capacity should be shown.
		/// </summary>
		public bool ShowPositiveCapacityOnly
		{
			set
			{
				_blnShowPositiveCapacityOnly = value;
				if (value)
					_blnShowNegativeCapacityOnly = false;
			}
		}

		/// <summary>
		/// Only items that consume Capacity should be shown.
		/// </summary>
		public bool ShowNegativeCapacityOnly
		{
			set
			{
				_blnShowNegativeCapacityOnly = value;
				if (value)
					_blnShowPositiveCapacityOnly = false;
			}
		}

		/// <summary>
		/// Only items that consume Armor Capacity should be shown.
		/// </summary>
		public bool ShowArmorCapacityOnly
		{
			set
			{
				_blnShowArmorCapacityOnly = value;
			}
		}

		/// <summary>
		/// ID of Gear that was selected in the dialogue.
		/// </summary>
		public string SelectedGear
		{
			get
			{
				return _strSelectedGear;
			}
			set
			{
				_strSelectedGear = value;
			}
		}

        /// <summary>
        /// Name of the Category the selected piece of Gear belongs to.
        /// </summary>
        public string SelectedCategory
        {
            get
            {
                return _strSelectedCategory;
            }
        }

		/// <summary>
		/// Rating that was selected in the dialogue.
		/// </summary>
		public int SelectedRating
		{
			get
			{
				return _intSelectedRating;
			}
		}

		/// <summary>
		/// Quantity that was selected in the dialogue.
		/// </summary>
		public int SelectedQty
		{
			get
			{
				return _intSelectedQty;
			}
		}

		/// <summary>
		/// Set the maximum Capacity the piece of Gear is allowed to be.
		/// </summary>
		public int MaximumCapacity
		{
			set
			{
				_intMaximumCapacity = value;
				lblMaximumCapacity.Text = LanguageManager.Instance.GetString("Label_MaximumCapacityAllowed") + " " + _intMaximumCapacity.ToString();
			}
		}

		/// <summary>
		/// Categories that the Gear allows to be used.
		/// </summary>
		public string AllowedCategories
		{
			get
			{
				return _strAllowedCategories;
			}
			set
			{
				_strAllowedCategories = value;
			}
		}

		/// <summary>
		/// Whether or not the item should be added for free.
		/// </summary>
		public bool FreeCost
		{
			get
			{
				return chkFreeItem.Checked;
			}
		}

		/// <summary>
		/// Whether or not the item's cost should be cut in half for being a Do It Yourself component/upgrade.
		/// </summary>
		public bool DoItYourself
		{
			get
			{
				return chkDoItYourself.Checked;
			}
		}

		/// <summary>
		/// Markup percentage.
		/// </summary>
		public int Markup
		{
			get
			{
				return _intMarkup;
			}
		}

		/// <summary>
		/// Whether or not the item was hacked which reduces the cost to 10% of the original value.
		/// </summary>
		public bool Hacked
		{
			get
			{
				return chkHacked.Checked;
			}
		}

		/// <summary>
		/// Whether or not the Gear should stack with others if possible.
		/// </summary>
		public bool Stack
		{
			get
			{
				return chkStack.Checked;
			}
		}

		/// <summary>
		/// Whether or not the Stack Checkbox should be shown (default true).
		/// </summary>
		public bool EnableStack
		{
			set
			{
				chkStack.Visible = value;
				if (!value)
					chkStack.Checked = false;
			}
		}

		/// <summary>
		/// Capacity display style.
		/// </summary>
		public CapacityStyle CapacityDisplayStyle
		{
			set
			{
				_objCapacityStyle = value;
			}
		}

		/// <summary>
		/// Whether or not the item is an Inherent Program for A.I.s.
		/// </summary>
		public bool InherentProgram
		{
			get
			{
				return chkInherentProgram.Checked;
			}
		}

		/// <summary>
		/// Whether or not a Grenade is Aerodynamic.
		/// </summary>
		public bool Aerodynamic
		{
			get
			{
				return chkAerodynamic.Checked;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Update the Gear's information based on the Gear selected and current Rating.
		/// </summary>
		private void UpdateGearInfo()
		{
			if (lstGear.Text != "")
			{
				// Retireve the information for the selected piece of Cyberware.
				XmlNode objXmlGear;
				int intItemCost = 0;

				string strCategory = "";
				objXmlGear = _objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + lstGear.SelectedValue + "\"]");
				strCategory = cboCategory.SelectedValue.ToString();

                TreeNode objTreeNode = new TreeNode();
                List<Weapon> lstWeapons = new List<Weapon>();
                List<TreeNode> lstTreeNodes = new List<TreeNode>();
                Gear objGear = new Gear(_objCharacter);
                Commlink objCommlink = new Commlink(_objCharacter);
                OperatingSystem objOperatingSystem = new OperatingSystem(_objCharacter);
                if (objXmlGear["category"].InnerText == "Commlink" || objXmlGear["category"].InnerText == "Commlink Upgrade")
                {
                    objCommlink.Create(objXmlGear, _objCharacter, objTreeNode, Convert.ToInt32(nudRating.Value), false, true);
                    objGear = (Gear)objCommlink;
                }
                else if (objXmlGear["category"].InnerText == "Operating System" || objXmlGear["category"].InnerText == "Operating System Upgrade")
                {
                    objOperatingSystem.Create(objXmlGear, _objCharacter, objTreeNode, Convert.ToInt32(nudRating.Value), false, true);
                    objGear = (Gear)objCommlink;
                }
                else
                    objGear.Create(objXmlGear, _objCharacter, objTreeNode, Convert.ToInt32(nudRating.Value), lstWeapons, lstTreeNodes, "", chkHacked.Checked, false, false, true, chkAerodynamic.Checked);

				if (_objCharacter.Metatype == "A.I." || _objCharacter.MetatypeCategory == "Technocritters" || _objCharacter.MetatypeCategory == "Protosapients")
				{
					if ((strCategory == "Matrix Programs" || strCategory == "Skillsofts" || strCategory == "Autosofts" || strCategory == "Autosofts, Agent" || strCategory == "Autosofts, Drone") && _objCharacter.Options.BookEnabled("UN") && !lstGear.SelectedValue.ToString().StartsWith("Suite:"))
						chkInherentProgram.Visible = true;
					else
						chkInherentProgram.Visible = false;

					chkInherentProgram.Enabled = !chkHacked.Checked;
					if (!chkInherentProgram.Enabled)
						chkInherentProgram.Checked = false;
				}
				else
					chkInherentProgram.Visible = false;

                if (objGear.GetType() == typeof(Commlink))
                {
                    lblGearResponse.Text = objCommlink.TotalResponse.ToString();
                    lblGearSignal.Text = objCommlink.TotalSignal.ToString();
                    lblGearSystem.Text = "";
                    lblGearFirewall.Text = "";
                }
				else if (objGear.GetType() == typeof(OperatingSystem))
				{
					lblGearResponse.Text = "";
					lblGearSignal.Text = "";
                    lblGearSystem.Text = objOperatingSystem.System.ToString();
                    lblGearFirewall.Text = objOperatingSystem.Firewall.ToString();
				}
				else
				{
					lblGearResponse.Text = "";
					lblGearSignal.Text = "";
					lblGearSystem.Text = "";
					lblGearFirewall.Text = "";
				}

				if (objXmlGear["category"].InnerText.EndsWith("Software") || objXmlGear["category"].InnerText.EndsWith("Programs") || objXmlGear["category"].InnerText == "Program Options" || objXmlGear["category"].InnerText.StartsWith("Autosofts") || objXmlGear["category"].InnerText.StartsWith("Skillsoft") || objXmlGear["category"].InnerText == "Program Packages" || objXmlGear["category"].InnerText == "Software Suites")
					chkHacked.Visible = true;
				else
					chkHacked.Visible = false;

				string strBook = _objCharacter.Options.LanguageBookShort(objGear.Source);
                string strPage = objGear.Page;
				lblSource.Text = strBook + " " + strPage;

				// Avail.
                lblAvail.Text = objGear.TotalAvail();

				double dblMultiplier = Convert.ToDouble(nudGearQty.Value / nudGearQty.Increment, GlobalOptions.Instance.CultureInfo);
				if (chkDoItYourself.Checked)
					dblMultiplier *= 0.5;

				// Cost.
                if (objGear.Cost.StartsWith("Variable"))
                {
                    int intMin = 0;
                    int intMax = 0;
                    string strCost = objGear.Cost.Replace("Variable(", string.Empty).Replace(")", string.Empty);
                    if (strCost.Contains("-"))
                    {
                        string[] strValues = strCost.Split('-');
                        intMin = Convert.ToInt32(strValues[0]);
                        intMax = Convert.ToInt32(strValues[1]);
                    }
                    else
                        intMin = Convert.ToInt32(strCost.Replace("+", string.Empty));

                    if (intMax == 0)
                    {
                        intMax = 1000000;
                        lblCost.Text = String.Format("{0:###,###,##0¥+}", intMin);
                    }
                    else
                        lblCost.Text = String.Format("{0:###,###,##0}", intMin) + "-" + String.Format("{0:###,###,##0¥}", intMax);

                    intItemCost = intMin;
                }
                else
                {
                    double dblCost = Convert.ToDouble(objGear.TotalCost, GlobalOptions.Instance.CultureInfo);
                    dblCost *= dblMultiplier;
                    dblCost *= 1 + (Convert.ToDouble(nudMarkup.Value, GlobalOptions.Instance.CultureInfo) / 100.0);

                    if (chkHacked.Checked)
                        dblCost *= 0.1;

                    intItemCost = Convert.ToInt32(dblCost);

                    lblCost.Text = String.Format("{0:###,###,##0¥}", intItemCost);
                }

				if (chkFreeItem.Checked)
				{
					lblCost.Text = String.Format("{0:###,###,##0¥}", 0);
					intItemCost = 0;
				}

				// Update the Avail Test Label.
				lblTest.Text = _objCharacter.AvailTest(intItemCost * _intCostMultiplier, lblAvail.Text);

				// Capacity.
                lblCapacity.Text = objGear.CalculatedCapacity;
				
				// Rating.
				if (Convert.ToInt32(objXmlGear["rating"].InnerText) > 0)
				{
					nudRating.Maximum = Convert.ToInt32(objXmlGear["rating"].InnerText);
					try
					{
						nudRating.Minimum = Convert.ToInt32(objXmlGear["minrating"].InnerText);
					}
					catch
					{
						nudRating.Minimum = 1;
					}

					if (nudRating.Minimum == nudRating.Maximum)
						nudRating.Enabled = false;
					else
						nudRating.Enabled = true;
				}
				else
				{
					nudRating.Minimum = 0;
					nudRating.Maximum = 0;
					nudRating.Enabled = false;
				}

				tipTooltip.SetToolTip(lblSource, _objCharacter.Options.LanguageBookLong(objGear.Source) + " " + LanguageManager.Instance.GetString("String_Page") + " " + strPage);
			}
		}

		/// <summary>
		/// Add a Category to the Category list.
		/// </summary>
		public void AddCategory(string strCategories)
		{
			string[] strCategoryList = strCategories.Split(',');
			foreach (string strCategory in strCategoryList)
			{
				ListItem objItem = new ListItem();
				objItem.Value = strCategory;
				objItem.Name = strCategory;
				_lstCategory.Add(objItem);
			}
			cboCategory.DataSource = null;
			cboCategory.ValueMember = "Value";
			cboCategory.DisplayMember = "Name";
			cboCategory.DataSource = _lstCategory;
		}

		/// <summary>
		/// Accept the selected item and close the form.
		/// </summary>
		private void AcceptForm()
		{
			if (lstGear.Text != "")
			{
				XmlNode objNode;
				objNode = _objXmlDocument.SelectSingleNode("/chummer/gears/gear[id = \"" + lstGear.SelectedValue + "\"]");

				_strSelectedGear = objNode["id"].InnerText;
				_strSelectCategory = objNode["category"].InnerText;
			}

			_intSelectedRating = Convert.ToInt32(nudRating.Value);
			_intSelectedQty = Convert.ToInt32(nudGearQty.Value);
			_intMarkup = Convert.ToInt32(nudMarkup.Value);

			if (!chkInherentProgram.Visible || !chkInherentProgram.Enabled)
				chkInherentProgram.Checked = false;

			this.DialogResult = DialogResult.OK;
		}

		private void MoveControls()
		{
			int intWidth = Math.Max(lblCapacityLabel.Width, lblAvailLabel.Width);
			intWidth = Math.Max(intWidth, lblCostLabel.Width);
			intWidth = Math.Max(intWidth, lblRatingLabel.Width);
			intWidth = Math.Max(intWidth, lblGearQtyLabel.Width);
			intWidth = Math.Max(intWidth, lblMarkupLabel.Width);

			lblCapacity.Left = lblCapacityLabel.Left + intWidth + 6;
			lblAvail.Left = lblAvailLabel.Left + intWidth + 6;
			lblTestLabel.Left = lblAvail.Left + lblAvail.Width + 16;
			lblTest.Left = lblTestLabel.Left + lblTestLabel.Width + 6;
			lblCost.Left = lblCostLabel.Left + intWidth + 6;
			nudRating.Left = lblRatingLabel.Left + intWidth + 6;
			nudGearQty.Left = lblGearQtyLabel.Left + intWidth + 6;
			chkStack.Left = nudGearQty.Left + nudGearQty.Width + 6;
			nudMarkup.Left = lblMarkupLabel.Left + intWidth + 6;
			lblMarkupPercentLabel.Left = nudMarkup.Left + nudMarkup.Width;

			intWidth = Math.Max(lblGearResponseLabel.Width, lblGearSignalLabel.Width);
			intWidth = Math.Max(intWidth, lblGearSystemLabel.Width);
			intWidth = Math.Max(intWidth, lblGearFirewallLabel.Width);

			lblGearResponse.Left = lblGearResponseLabel.Left + intWidth + 6;
			lblGearSignalLabel.Left = lblGearResponse.Left + lblGearResponse.Width + 16;
			lblGearSignal.Left = lblGearSignalLabel.Left + intWidth + 6;
			lblGearSystem.Left = lblGearSystemLabel.Left + intWidth + 6;
			lblGearFirewallLabel.Left = lblGearSystem.Left + lblGearSystem.Width + 16;
			lblGearFirewall.Left = lblGearFirewallLabel.Left + intWidth + 6;

			chkDoItYourself.Left = chkFreeItem.Left + chkFreeItem.Width + 6;
			chkHacked.Left = chkDoItYourself.Left + chkDoItYourself.Width + 6;

			lblSearchLabel.Left = txtSearch.Left - 6 - lblSearchLabel.Width;
		}
		#endregion
	}
}