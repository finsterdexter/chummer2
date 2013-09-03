using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Chummer
{
    public partial class frmPriority : Form
    {
        private readonly Character _objCharacter;
        private readonly XmlDocument _objXmlDocument;

        public frmPriority(Character objCharacter)
        {
            _objCharacter = objCharacter;
            InitializeComponent();
            LanguageManager.Instance.Load(this);

            _objXmlDocument = XmlManager.Instance.Load("priorities.xml");

            // Create the list of Priorities (A - E).
            List<ListItem> lstPriorities = new List<ListItem>{
                new ListItem("A", LanguageManager.Instance.GetString("String_Priority_A")),
                new ListItem("B", LanguageManager.Instance.GetString("String_Priority_B")),
                new ListItem("C", LanguageManager.Instance.GetString("String_Priority_C")),
                new ListItem("D", LanguageManager.Instance.GetString("String_Priority_D")),
                new ListItem("E", LanguageManager.Instance.GetString("String_Priority_E")),
            };


            // Create a unique list for each category.
            List<ListItem> lstMetatype = new List<ListItem>(lstPriorities);
            List<ListItem> lstAttribute = new List<ListItem>(lstPriorities);
            List<ListItem> lstSpecial = new List<ListItem>(lstPriorities);
            List<ListItem> lstSkill = new List<ListItem>(lstPriorities);
            List<ListItem> lstNuyen = new List<ListItem>(lstPriorities);

  

            cboPriorityMetatype.DisplayMember = "Name";
            cboPriorityMetatype.ValueMember = "Value";
            cboPriorityMetatype.DataSource = lstMetatype;

            cboPriorityAttributes.DisplayMember = "Name";
            cboPriorityAttributes.ValueMember = "Value";
            cboPriorityAttributes.DataSource = lstAttribute;

            cboPrioritySpecial.DisplayMember = "Name";
            cboPrioritySpecial.ValueMember = "Value";
            cboPrioritySpecial.DataSource = lstSpecial;

            cboPrioritySkills.DisplayMember = "Name";
            cboPrioritySkills.ValueMember = "Value";
            cboPrioritySkills.DataSource = lstSkill;

            cboPriorityNuyen.DisplayMember = "Name";
            cboPriorityNuyen.ValueMember = "Value";
            cboPriorityNuyen.DataSource = lstNuyen;
        }

        #region Control Events
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pnlMetatype_MouseLeave(object sender, EventArgs e)
        {
            lblSummary.Text = string.Empty;
        }

        private void pnlAttributes_MouseLeave(object sender, EventArgs e)
        {
            lblSummary.Text = string.Empty;
        }

        private void pnlSpecial_MouseLeave(object sender, EventArgs e)
        {
            lblSummary.Text = string.Empty;
        }

        private void pnlSkills_MouseLeave(object sender, EventArgs e)
        {
            lblSummary.Text = string.Empty;
        }

        private void pnlNuyen_MouseLeave(object sender, EventArgs e)
        {
            lblSummary.Text = string.Empty;
        }

        private void pnlMetatype_MouseEnter(object sender, EventArgs e)
        {
            ShowMetatypeSummary();
        }

        private void pnlAttributes_MouseEnter(object sender, EventArgs e)
        {
            ShowAttributesSummary();
        }

        private void pnlSpecial_MouseEnter(object sender, EventArgs e)
        {
            ShowSpecialSummary();
        }

        private void pnlSkills_MouseEnter(object sender, EventArgs e)
        {
            ShowSkillsSummary();
        }

        private void pnlNuyen_MouseEnter(object sender, EventArgs e)
        {
            ShowNuyenSummary();
        }

        private void cboPriorityMetatype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowMetatypeSummary();

            // Build the list of Metatypes the character can select from based on their chosen Priority.
            List<ListItem> lstMetatypes = new List<ListItem>();

            XmlDocument objXmlMetatypeDocument = XmlManager.Instance.Load("metatypes.xml");
            XmlNode objXmlNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/metatypes/metatype[priority = \"" + cboPriorityMetatype.SelectedValue.ToString() + "\"]");

            foreach (XmlNode objMetatypeNode in objXmlNode.SelectNodes("metatypes/metatype"))
            {
                XmlNode objXmlMetatype = objXmlMetatypeDocument.SelectSingleNode("/chummer/metatypes/metatype[id = \"" + objMetatypeNode["id"].InnerText + "\"]");

                ListItem objItem = new ListItem();
                objItem.Value = objXmlMetatype["id"].InnerText;
                objItem.Name = objXmlMetatype["name"].InnerText;
                lstMetatypes.Add(objItem);
            }

            cboMetatype.ValueMember = "Value";
            cboMetatype.DisplayMember = "Name";
            cboMetatype.DataSource = lstMetatypes;
        }

        private void cboAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowAttributesSummary();
        }

        private void cboSpecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSpecialSummary();

            // Build the list of Special archetypes the character can select from based on their chosen Priority.
            List<ListItem> lstSpecial = new List<ListItem>();

            XmlNode objXmlNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/specials/special[priority = \"" + cboPrioritySpecial.SelectedValue.ToString() + "\"]");
            if (objXmlNode["magician"] != null)
            {
                ListItem objItem = new ListItem();
                objItem.Value = "magician";
                objItem.Name = LanguageManager.Instance.GetString("Label_Magician");
                lstSpecial.Add(objItem);
            }
            if (objXmlNode["mysticadept"] != null)
            {
                ListItem objItem = new ListItem();
                objItem.Value = "mysticadept";
                objItem.Name = LanguageManager.Instance.GetString("Label_MysticAdept");
                lstSpecial.Add(objItem);
            }
            if (objXmlNode["technomancer"] != null)
            {
                ListItem objItem = new ListItem();
                objItem.Value = "technomancer";
                objItem.Name = LanguageManager.Instance.GetString("Label_Technomancer");
                lstSpecial.Add(objItem);
            }
            if (objXmlNode["adept"] != null)
            {
                ListItem objItem = new ListItem();
                objItem.Value = "adept";
                objItem.Name = LanguageManager.Instance.GetString("Label_Adept");
                lstSpecial.Add(objItem);
            }
            if (objXmlNode["aspected"] != null)
            {
                ListItem objItem = new ListItem();
                objItem.Value = "aspected";
                objItem.Name = LanguageManager.Instance.GetString("Label_AspectedMagician");
                lstSpecial.Add(objItem);
            }

            if (lstSpecial.Count == 0)
            {
                ListItem objItem = new ListItem();
                objItem.Value = null;
                objItem.Name = LanguageManager.Instance.GetString("String_None");
                lstSpecial.Add(objItem);
            }

            cboSpecial.ValueMember = "Value";
            cboSpecial.DisplayMember = "Name";
            cboSpecial.DataSource = lstSpecial;
        }

        private void cboSpecial_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ShowSpecialSummary();
        }

        private void cboSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSkillsSummary();
        }

        private void cboNuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowNuyenSummary();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // Make sure that each Priority has only been selected once.
            bool blnValid = true;
            
            if (cboPriorityMetatype.SelectedValue == cboPriorityAttributes.SelectedValue)
                blnValid = false;
            if (cboPriorityMetatype.SelectedValue == cboPrioritySpecial.SelectedValue)
                blnValid = false;
            if (cboPriorityMetatype.SelectedValue == cboPrioritySkills.SelectedValue)
                blnValid = false;
            if (cboPriorityMetatype.SelectedValue == cboPriorityNuyen.SelectedValue)
                blnValid = false;
            if (cboPriorityAttributes.SelectedValue == cboPrioritySpecial.SelectedValue)
                blnValid = false;
            if (cboPriorityAttributes.SelectedValue == cboPrioritySkills.SelectedValue)
                blnValid = false;
            if (cboPriorityAttributes.SelectedValue == cboPriorityNuyen.SelectedValue)
                blnValid = false;
            if (cboPrioritySpecial.SelectedValue == cboPrioritySkills.SelectedValue)
                blnValid = false;
            if (cboPrioritySpecial.SelectedValue == cboPriorityNuyen.SelectedValue)
                blnValid = false;
            if (cboPrioritySkills.SelectedValue == cboPriorityNuyen.SelectedValue)
                blnValid = false;

            if (!blnValid)
            {
                MessageBox.Show(LanguageManager.Instance.GetString("Message_Priorities_UniquePriorities"), LanguageManager.Instance.GetString("MessageTitle_Priorities_UniquePriorities"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                // Determine the number of Special Attribute points that come from the selected Metatype.
                XmlNode objXmlMetatypesNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/metatypes/metatype[priority = \"" + cboPriorityMetatype.SelectedValue.ToString() + "\"]");
                XmlNode objXmlMetatypeNode = objXmlMetatypesNode.SelectSingleNode("metatypes/metatype[id = \"" + cboMetatype.SelectedValue.ToString() + "\"]");
                int intSpecialAttributePoints = Convert.ToInt32(objXmlMetatypeNode["points"].InnerText);

                // Determine the number of Attribute Points selected.
                XmlNode objXmlAttributeNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/attributes/attribute[priority = \"" + cboPriorityAttributes.SelectedValue.ToString() + "\"]");
                int intAttributePoints = Convert.ToInt32(objXmlAttributeNode["points"].InnerText);

                // Determine if the character gets anything from their Special Priority.
                if (cboSpecial.SelectedValue != null)
                {
                    if (cboSpecial.SelectedValue.ToString() != string.Empty)
                    {
                        XmlNode objXmlSpecialNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/specials/special[priority = \"" + cboPrioritySpecial.SelectedValue.ToString() + "\"]/" + cboSpecial.SelectedValue.ToString());
                        if (objXmlSpecialNode["bonus"] != null)
                        {
                            ImprovementManager objImprovementManager = new ImprovementManager(_objCharacter);
                            objImprovementManager.CreateImprovements(Improvement.ImprovementSource.Priority, "Priority", objXmlSpecialNode["bonus"]);
                        }
                    }
                }

                // Determine the number of Skill Points selected.

                // Determine the amount of Nuyen selected.
                XmlNode objXmlNuyenNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/resources/resource[priority = \"" + cboPriorityNuyen.SelectedValue.ToString() + "\"]");
                int intNuyen = Convert.ToInt32(objXmlNuyenNode["nuyen"].InnerText);

                // Load the Metatype and set the character build information.
                _objCharacter.LoadMetatype(Guid.Parse(cboMetatype.SelectedValue.ToString()));
                _objCharacter.SpecialAttributePoints = intSpecialAttributePoints;
                _objCharacter.AttributePoints = intAttributePoints;
                _objCharacter.Nuyen = intNuyen;

                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion

        private void ShowMetatypeSummary()
        {
            string strSummary = LanguageManager.Instance.GetString("String_Priorities_Metatype_Summary");

            if (cboPriorityMetatype.SelectedValue != null)
            {
                XmlDocument objXmlMetatypeDocument = XmlManager.Instance.Load("metatypes.xml");
                XmlNode objXmlNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/metatypes/metatype[priority = \"" + cboPriorityMetatype.SelectedValue.ToString() + "\"]");

                strSummary += "\n\n" + LanguageManager.Instance.GetString("String_Priorities_Metatype").Replace("{0}", LanguageManager.Instance.GetString("String_Priority") + " " + LanguageManager.Instance.GetString("String_Priority_" + cboPriorityMetatype.SelectedValue.ToString()));

                foreach (XmlNode objMetatypeNode in objXmlNode.SelectNodes("metatypes/metatype"))
                {
                    XmlNode objXmlMetatype = objXmlMetatypeDocument.SelectSingleNode("/chummer/metatypes/metatype[id = \"" + objMetatypeNode["id"].InnerText + "\"]");
                    strSummary += "\n" + objXmlMetatype["name"].InnerText + " (" + objMetatypeNode["points"].InnerText + ")";
                }
            }

            lblSummary.Text = strSummary;
        }

        private void ShowAttributesSummary()
        {
            string strSummary = LanguageManager.Instance.GetString("String_Priorities_Attributes_Summary");

            if (cboPriorityAttributes.SelectedValue != null)
            {
                XmlNode objXmlNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/attributes/attribute[priority = \"" + cboPriorityAttributes.SelectedValue.ToString() + "\"]");
                strSummary += "\n\n" + LanguageManager.Instance.GetString("String_Priorities_Attributes").Replace("{0}", LanguageManager.Instance.GetString("String_Priority") + " " + LanguageManager.Instance.GetString("String_Priority_" + cboPriorityAttributes.SelectedValue.ToString())).Replace("{1}", objXmlNode["points"].InnerText);
            }

            lblSummary.Text = strSummary;
        }

        private void ShowSpecialSummary()
        {
            string strSummary = LanguageManager.Instance.GetString("String_Priorities_Special_Summary");

            if (cboPrioritySpecial.SelectedValue != null)
            {
                strSummary += "\n\n" + LanguageManager.Instance.GetString("String_Priorities_Special").Replace("{0}", LanguageManager.Instance.GetString("String_Priority") + " " + LanguageManager.Instance.GetString("String_Priority_" + cboPrioritySpecial.SelectedValue.ToString()));
            }

            lblSummary.Text = strSummary;
        }

        private void ShowSkillsSummary()
        {
            string strSummary = LanguageManager.Instance.GetString("String_Priorities_Skills_Summary");

            if (cboPrioritySkills.SelectedValue != null)
            {
                XmlNode objXmlNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/skills/skill[priority = \"" + cboPrioritySkills.SelectedValue.ToString() + "\"]");
                strSummary += "\n\n" + LanguageManager.Instance.GetString("String_Priorities_Skills").Replace("{0}", LanguageManager.Instance.GetString("String_Priority") + " " + LanguageManager.Instance.GetString("String_Priority_" + cboPrioritySkills.SelectedValue.ToString())).Replace("{1}", objXmlNode["points"].InnerText);
            }

            lblSummary.Text = strSummary;
        }

        private void ShowNuyenSummary()
        {
            string strSummary = LanguageManager.Instance.GetString("String_Priorities_Special_Summary");

            if (cboPriorityNuyen.SelectedValue != null)
            {
                XmlNode objXmlNode = _objXmlDocument.SelectSingleNode("/chummer/priorities/resources/resource[priority = \"" + cboPriorityNuyen.SelectedValue.ToString() + "\"]");
                strSummary += "\n\n" + LanguageManager.Instance.GetString("String_Priorities_Nuyen").Replace("{0}", LanguageManager.Instance.GetString("String_Priority") + " " + LanguageManager.Instance.GetString("String_Priority_" + cboPriorityNuyen.SelectedValue.ToString())).Replace("{1}", objXmlNode["nuyen"].InnerText);
            }

            lblSummary.Text = strSummary;
        }
    }
}