using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Chummer
{
	public partial class frmSelectVehicleMod : Form
	{
		private string _strSelectedMod = "";
		private int _intSelectedRating = 0;
		private int _intVehicleCost = 0;
		private int _intBody = 0;
		private int _intSpeed = 0;
		private int _intAccelRunning = 0;
		private int _intAccelWalking = 0;
		private int _intWeaponCost = 0;
		private int _intTotalWeaponCost = 0;
		private int _intModMultiplier = 1;
		private string _strInputFile = "vehicles";
		private int _intMarkup = 0;

		private int _intMaxResponse = 0;
		private int _intMaxSignal = 0;
		private int _intMaxFirewall = 0;
		private int _intMaxSystem = 0;
		private int _intDeviceRating = 0;
		private bool _blnModularElectronics = false;

        private Mode _objMode = Mode.Vehicle;

		private string _strAllowedCategories = "";
		private bool _blnAddAgain = false;

		private XmlDocument _objXmlDocument = new XmlDocument();
		private readonly Character _objCharacter;

        public enum Mode
        {
            Vehicle = 0,
            WeaponMod = 1,
        }

		#region Control Events
		public frmSelectVehicleMod(Character objCharacter, bool blnCareer = false)
		{
			InitializeComponent();
			LanguageManager.Instance.Load(this);
			chkFreeItem.Visible = blnCareer;
			lblMarkupLabel.Visible = blnCareer;
			nudMarkup.Visible = blnCareer;
			lblMarkupPercentLabel.Visible = blnCareer;
			_objCharacter = objCharacter;
			MoveControls();
		}

		private void frmSelectVehicleMod_Load(object sender, EventArgs e)
		{
			BuildModList();

			if (_objMode == Mode.WeaponMod)
				this.Text = LanguageManager.Instance.GetString("Title_SelectVehicleMod_Weapon");
		}

		private void lstMod_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void nudRating_ValueChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (lstMod.Text != "")
				AcceptForm();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void lstMod_DoubleClick(object sender, EventArgs e)
		{
			if (lstMod.Text != "")
				AcceptForm();
		}

		private void cmdOKAdd_Click(object sender, EventArgs e)
		{
			_blnAddAgain = true;
			cmdOK_Click(sender, e);
		}

		private void chkFreeItem_CheckedChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			BuildModList();
		}

		private void nudMarkup_ValueChanged(object sender, EventArgs e)
		{
			UpdateGearInfo();
		}

		private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Down)
			{
				try
				{
					lstMod.SelectedIndex++;
				}
				catch
				{
					try
					{
						lstMod.SelectedIndex = 0;
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
					lstMod.SelectedIndex--;
					if (lstMod.SelectedIndex == -1)
						lstMod.SelectedIndex = lstMod.Items.Count - 1;
				}
				catch
				{
					try
					{
						lstMod.SelectedIndex = lstMod.Items.Count - 1;
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
		/// Vehicle's Cost.
		/// </summary>
		public int VehicleCost
		{
			set
			{
				_intVehicleCost = value;
			}
		}

		/// <summary>
		/// Weapon's Cost.
		/// </summary>
		public int WeaponCost
		{
			set
			{
				_intWeaponCost = value;
			}
		}

		/// <summary>
		/// Weapon's Total Cost.
		/// </summary>
		public int TotalWeaponCost
		{
			set
			{
				_intTotalWeaponCost = value;
			}
		}

		/// <summary>
		/// Weapon's Modification Cost Multiplier.
		/// </summary>
		public int ModMultiplier
		{
			set
			{
				_intModMultiplier = value;
			}
		}

		/// <summary>
		/// Vehicle's Body.
		/// </summary>
		public int Body
		{
			set
			{
				// If the BOD is 0 (Microdone), treat it as 2 for the purposes of cost.
				if (value > 0)
					_intBody = value;
				else
					_intBody = 2;
			}
		}

		/// <summary>
		/// Vehicle's Speed.
		/// </summary>
		public int Speed
		{
			set
			{
				_intSpeed = value;
			}
		}

		/// <summary>
		/// Vehicle's Acceleration (Running) speed.
		/// </summary>
		public int AccelRunning
		{
			set
			{
				_intAccelRunning = value;
			}
		}

		/// <summary>
		/// Vehicle's Acceleration (Walking) speed.
		/// </summary>
		public int AccelWalking
		{
			set
			{
				_intAccelWalking = value;
			}
		}

		/// <summary>
		/// Name of the Mod that was selected in the dialogue.
		/// </summary>
		public string SelectedMod
		{
			get
			{
				return _strSelectedMod;
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
		/// Whether or not the Vehicle has the Modular Electronics Vehicle Mod.
		/// </summary>
		public bool HasModularElectronics
		{
			set
			{
				_blnModularElectronics = value;

				if (_blnModularElectronics)
				{
					_intMaxResponse = 10;
					_intMaxSystem = 10;
					_intMaxFirewall = 10;
					_intMaxSignal = 10;
				}
			}
		}

		/// <summary>
		/// Vehicle's Device Rating.
		/// </summary>
		public int DeviceRating
		{
			set
			{
				_intMaxResponse = value + 2;
				_intMaxSystem = value;
				_intMaxFirewall = value;
				_intMaxSignal = value + 2;
				_intDeviceRating = value;

				if (_blnModularElectronics)
				{
					_intMaxResponse = 10;
					_intMaxSystem = 10;
					_intMaxFirewall = 10;
					_intMaxSignal = 10;
				}
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
        /// Set the window's Mode to Vehicle or WeaponMod.
        /// </summary>
        public Mode WindowMode
        {
            get
            {
                return _objMode;
            }
            set
            {
                _objMode = value;
                switch (_objMode)
                {
                    case Mode.Vehicle:
                        _strInputFile = "vehicles";
                        break;
                    case Mode.WeaponMod:
                        _strInputFile = "weapons";
                        break;
                }
            }
        }
		#endregion

		#region Methods
		/// <summary>
		/// Build the list of Mods.
		/// </summary>
		private void BuildModList()
		{
			foreach (Label objLabel in this.Controls.OfType<Label>())
			{
				if (objLabel.Text.StartsWith("["))
					objLabel.Text = "";
			}

			// Update the list of Mods based on the selected Category.
			XmlNodeList objXmlModList;

			// Load the Mod information.
			_objXmlDocument = XmlManager.Instance.Load(_strInputFile + ".xml");

			// Retrieve the list of Mods for the selected Category.
			if (txtSearch.Text == "")
				objXmlModList = _objXmlDocument.SelectNodes("/chummer/mods/mod[(" + _objCharacter.Options.BookXPath() + ") and category != \"Special\"]");
			else
				objXmlModList = _objXmlDocument.SelectNodes("/chummer/mods/mod[(" + _objCharacter.Options.BookXPath() + ") and category != \"Special\" and ((contains(translate(name,'abcdefghijklmnopqrstuvwxyzàáâãäåçèéêëìíîïñòóôõöùúûüýß','ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝß'), \"" + txtSearch.Text.ToUpper() + "\") and not(translate)) or contains(translate(translate,'abcdefghijklmnopqrstuvwxyzàáâãäåçèéêëìíîïñòóôõöùúûüýß','ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝß'), \"" + txtSearch.Text.ToUpper() + "\"))]");
			bool blnAdd = true;
			List<ListItem> lstMods = new List<ListItem>();
			foreach (XmlNode objXmlMod in objXmlModList)
			{
				blnAdd = true;
				if (objXmlMod["response"] != null)
				{
					if (Convert.ToInt32(objXmlMod["response"].InnerText) > _intMaxResponse || Convert.ToInt32(objXmlMod["response"].InnerText) <= _intDeviceRating)
						blnAdd = false;
				}
				if (objXmlMod["system"] != null)
				{
					if (Convert.ToInt32(objXmlMod["system"].InnerText) <= _intDeviceRating)
						blnAdd = false;
				}
				if (objXmlMod["firewall"] != null)
				{
					if (Convert.ToInt32(objXmlMod["firewall"].InnerText) <= _intDeviceRating)
						blnAdd = false;
				}
				if (objXmlMod["signal"] != null)
				{
					if (Convert.ToInt32(objXmlMod["signal"].InnerText) > _intMaxSignal || Convert.ToInt32(objXmlMod["signal"].InnerText) <= _intDeviceRating)
						blnAdd = false;
				}

				if (blnAdd)
				{
					ListItem objItem = new ListItem();
					objItem.Value = objXmlMod["id"].InnerText;
					if (objXmlMod["translate"] != null)
						objItem.Name = objXmlMod["translate"].InnerText;
					else
						objItem.Name = objXmlMod["name"].InnerText;
					lstMods.Add(objItem);
				}
			}
			SortListItem objSort = new SortListItem();
			lstMods.Sort(objSort.Compare);
			lstMod.DataSource = null;
			lstMod.ValueMember = "Value";
			lstMod.DisplayMember = "Name";
			lstMod.DataSource = lstMods;
		}
		
		/// <summary>
		/// Accept the selected item and close the form.
		/// </summary>
		private void AcceptForm()
		{
			_strSelectedMod = lstMod.SelectedValue.ToString();
			_intSelectedRating = Convert.ToInt32(nudRating.Value);
			_intMarkup = Convert.ToInt32(nudMarkup.Value);
			this.DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Update the Mod's information based on the Mod selected and current Rating.
		/// </summary>
		private void UpdateGearInfo()
		{
			if (lstMod.Text != "")
			{
                // Retireve the information for the selected Mod.
				XmlNode objXmlMod = _objXmlDocument.SelectSingleNode("/chummer/mods/mod[id = \"" + lstMod.SelectedValue + "\"]");

                string strAvail = "";
                string strModBook = "";
                string strModPage = "";
                int intItemCost = 0;
                int intSlots = 0;


                if (_objMode == Mode.Vehicle)
                {
                    TreeNode objTreeNode = new TreeNode();
                    VehicleMod objVehicleMod = new VehicleMod(_objCharacter);
                    objVehicleMod.Create(objXmlMod, objTreeNode, Convert.ToInt32(nudRating.Value));

                    Vehicle objVehicle = new Vehicle(_objCharacter);
                    objVehicleMod.Parent = objVehicle;
                    if (_intVehicleCost != 0)
                        objVehicle.Cost = _intVehicleCost.ToString();
                    string strAccel = _intAccelWalking.ToString() + "/" + _intAccelRunning.ToString();
                    objVehicle.Accel = strAccel;
                    if (_intBody != 0)
                        objVehicle.Body = _intBody;
                    if (_intSpeed != 0)
                        objVehicle.Speed = _intSpeed;

                    strAvail = objVehicleMod.TotalAvail;
                    intItemCost = objVehicleMod.TotalCost;
                    intSlots = objVehicleMod.CalculatedSlots;
                    strModBook = objVehicleMod.Source;
                    strModPage = objVehicleMod.Page;
                }
                else if (_objMode == Mode.WeaponMod)
                {
                    TreeNode objTreeNode = new TreeNode();
                    WeaponMod objWeaponMod = new WeaponMod(_objCharacter);
                    objWeaponMod.Create(objXmlMod, objTreeNode);

                    if (_intWeaponCost != 0)
                    {
                        Weapon objWeapon = new Weapon(_objCharacter);
                        objWeapon.Cost = _intWeaponCost;
                        objWeaponMod.Parent = objWeapon;
                    }

                    strAvail = objWeaponMod.TotalAvail;
                    intItemCost = objWeaponMod.TotalCost;
                    intSlots = objWeaponMod.Slots;
                    strModBook = objWeaponMod.Source;
                    strModPage = objWeaponMod.Page;
                }

                lblAvail.Text = strAvail;

				// Cost.
				if (objXmlMod["cost"].InnerText.StartsWith("Variable"))
				{
					int intMin = 0;
					int intMax = 0;
					string strCost = objXmlMod["cost"].InnerText.Replace("Variable(", string.Empty).Replace(")", string.Empty);
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
					int intCost = Convert.ToInt32(Convert.ToDouble(intItemCost, GlobalOptions.Instance.CultureInfo));
					intCost *= _intModMultiplier;

					// Apply any markup.
					double dblCost = Convert.ToDouble(intCost, GlobalOptions.Instance.CultureInfo);
					dblCost *= 1 + (Convert.ToDouble(nudMarkup.Value, GlobalOptions.Instance.CultureInfo) / 100.0);
					intCost = Convert.ToInt32(dblCost);

					lblCost.Text = String.Format("{0:###,###,##0¥}", intCost);

					intItemCost = intCost;
				}

				// Update the Avail Test Label.
				lblTest.Text = _objCharacter.AvailTest(intItemCost, lblAvail.Text);

				// If the rating is "qty", we're looking at Tires instead of actual Rating, so update the fields appropriately.
				if (objXmlMod["rating"].InnerText == "qty")
				{
					nudRating.Enabled = true;
					nudRating.Maximum = 20;
					nudRating.Minimum = 1;
					lblRatingLabel.Text = LanguageManager.Instance.GetString("Label_Qty");
				}
				else
				{
					if (Convert.ToInt32(objXmlMod["rating"].InnerText) > 0)
					{
						nudRating.Maximum = Convert.ToInt32(objXmlMod["rating"].InnerText);
						nudRating.Minimum = 1;
						nudRating.Enabled = true;
						lblRatingLabel.Text = LanguageManager.Instance.GetString("Label_Rating");
					}
					else
					{
						nudRating.Minimum = 0;
						nudRating.Maximum = 0;
						nudRating.Enabled = false;
						lblRatingLabel.Text = LanguageManager.Instance.GetString("Label_Rating");
					}
				}

				// Slots.
				lblSlots.Text = intSlots.ToString();

				try
				{
					if (objXmlMod["category"].InnerText == "Weapon Mod")
						lblCategory.Text = LanguageManager.Instance.GetString("String_WeaponModification");
					else
					{
						// Translate the Category if possible.
						if (GlobalOptions.Instance.Language != "en-us")
						{
							XmlNode objXmlCategory = _objXmlDocument.SelectSingleNode("/chummer/modcategories/category[. = \"" + objXmlMod["category"].InnerText + "\"]");
							if (objXmlCategory != null)
							{
								if (objXmlCategory.Attributes["translate"] != null)
									lblCategory.Text = objXmlCategory.Attributes["translate"].InnerText;
								else
									lblCategory.Text = objXmlMod["category"].InnerText;
							}
							else
								lblCategory.Text = objXmlMod["category"].InnerText;
						}
						else
							lblCategory.Text = objXmlMod["category"].InnerText;
					}
				}
				catch
				{
					lblCategory.Text = LanguageManager.Instance.GetString("String_WeaponModification");
				}

				if (objXmlMod["limit"] != null)
				{
					// Translate the Limit if possible.
					if (GlobalOptions.Instance.Language != "en-us")
					{
						XmlNode objXmlLimit = _objXmlDocument.SelectSingleNode("/chummer/limits/limit[. = \"" + objXmlMod["limit"].InnerText + "\"]");
						if (objXmlLimit != null)
						{
							if (objXmlLimit.Attributes["translate"] != null)
								lblLimit.Text = " (" + objXmlLimit.Attributes["translate"].InnerText + ")";
							else
								lblLimit.Text = " (" + objXmlMod["limit"].InnerText + ")";
						}
						else
							lblLimit.Text = " (" + objXmlMod["limit"].InnerText + ")";
					}
					else
						lblLimit.Text = " (" + objXmlMod["limit"].InnerText + ")";
				}
				else
					lblLimit.Text = "";

				string strBook = _objCharacter.Options.LanguageBookShort(strModBook);
                string strPage = strModPage;
				lblSource.Text = strBook + " " + strPage;

				tipTooltip.SetToolTip(lblSource, _objCharacter.Options.LanguageBookLong(strModBook) + " " + LanguageManager.Instance.GetString("String_Page") + " " + strPage);
			}
		}

		private void MoveControls()
		{
			int intWidth = Math.Max(lblCategoryLabel.Width, lblAvailLabel.Width);
			intWidth = Math.Max(intWidth, lblCostLabel.Width);
			intWidth = Math.Max(intWidth, lblSlotsLabel.Width);
			intWidth = Math.Max(intWidth, lblRatingLabel.Width);

			lblCategory.Left = lblCategoryLabel.Left + intWidth + 6;
			lblLimit.Left = lblCategoryLabel.Left + intWidth + 6;
			lblAvail.Left = lblAvailLabel.Left + intWidth + 6;
			lblTestLabel.Left = lblAvail.Left + lblAvail.Width + 16;
			lblTest.Left = lblTestLabel.Left + lblTestLabel.Width + 6;
			lblCost.Left = lblCostLabel.Left + intWidth + 6;
			lblSlots.Left = lblSlotsLabel.Left + intWidth + 6;
			nudRating.Left = lblRatingLabel.Left + intWidth + 6;

			nudMarkup.Left = lblMarkupLabel.Left + lblMarkupLabel.Width + 6;
			lblMarkupPercentLabel.Left = nudMarkup.Left + nudMarkup.Width;

			lblSource.Left = lblSourceLabel.Left + lblSourceLabel.Width + 6;

			lblSearchLabel.Left = txtSearch.Left - 6 - lblSearchLabel.Width;
		}
		#endregion
	}
}