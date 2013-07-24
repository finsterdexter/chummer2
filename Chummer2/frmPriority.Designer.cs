namespace Chummer
{
    partial class frmPriority
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.tipTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.lblSummary = new System.Windows.Forms.Label();
            this.pnlMetatype = new System.Windows.Forms.Panel();
            this.cboPriorityMetatype = new System.Windows.Forms.ComboBox();
            this.lblMetatype = new System.Windows.Forms.Label();
            this.pnlAttributes = new System.Windows.Forms.Panel();
            this.cboPriorityAttributes = new System.Windows.Forms.ComboBox();
            this.lblAttributes = new System.Windows.Forms.Label();
            this.pnlSpecial = new System.Windows.Forms.Panel();
            this.cboPrioritySpecial = new System.Windows.Forms.ComboBox();
            this.lblSpecial = new System.Windows.Forms.Label();
            this.pnlSkills = new System.Windows.Forms.Panel();
            this.cboPrioritySkills = new System.Windows.Forms.ComboBox();
            this.lblSkills = new System.Windows.Forms.Label();
            this.pnlNuyen = new System.Windows.Forms.Panel();
            this.cboPriorityNuyen = new System.Windows.Forms.ComboBox();
            this.lblNuyen = new System.Windows.Forms.Label();
            this.cboSpecial = new System.Windows.Forms.ComboBox();
            this.cboMetatype = new System.Windows.Forms.ComboBox();
            this.pnlMetatype.SuspendLayout();
            this.pnlAttributes.SuspendLayout();
            this.pnlSpecial.SuspendLayout();
            this.pnlSkills.SuspendLayout();
            this.pnlNuyen.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(529, 457);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 35;
            this.cmdCancel.Tag = "String_Cancel";
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(610, 457);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 34;
            this.cmdOK.Tag = "String_OK";
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // tipTooltip
            // 
            this.tipTooltip.AutoPopDelay = 10000;
            this.tipTooltip.InitialDelay = 250;
            this.tipTooltip.IsBalloon = true;
            this.tipTooltip.ReshowDelay = 100;
            this.tipTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipTooltip.ToolTipTitle = "Chummer Help";
            // 
            // lblSummary
            // 
            this.lblSummary.Location = new System.Drawing.Point(363, 9);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(322, 445);
            this.lblSummary.TabIndex = 95;
            this.lblSummary.Text = "[Summary text goes here]";
            // 
            // pnlMetatype
            // 
            this.pnlMetatype.Controls.Add(this.cboMetatype);
            this.pnlMetatype.Controls.Add(this.cboPriorityMetatype);
            this.pnlMetatype.Controls.Add(this.lblMetatype);
            this.pnlMetatype.Location = new System.Drawing.Point(12, 12);
            this.pnlMetatype.Name = "pnlMetatype";
            this.pnlMetatype.Size = new System.Drawing.Size(342, 30);
            this.pnlMetatype.TabIndex = 104;
            this.pnlMetatype.MouseEnter += new System.EventHandler(this.pnlMetatype_MouseEnter);
            // 
            // cboPriorityMetatype
            // 
            this.cboPriorityMetatype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriorityMetatype.FormattingEnabled = true;
            this.cboPriorityMetatype.Location = new System.Drawing.Point(64, 3);
            this.cboPriorityMetatype.Name = "cboPriorityMetatype";
            this.cboPriorityMetatype.Size = new System.Drawing.Size(74, 21);
            this.cboPriorityMetatype.TabIndex = 39;
            this.cboPriorityMetatype.SelectedIndexChanged += new System.EventHandler(this.cboPriorityMetatype_SelectedIndexChanged);
            // 
            // lblMetatype
            // 
            this.lblMetatype.AutoSize = true;
            this.lblMetatype.Location = new System.Drawing.Point(4, 6);
            this.lblMetatype.Name = "lblMetatype";
            this.lblMetatype.Size = new System.Drawing.Size(54, 13);
            this.lblMetatype.TabIndex = 38;
            this.lblMetatype.Tag = "Label_Metatype";
            this.lblMetatype.Text = "Metatype:";
            // 
            // pnlAttributes
            // 
            this.pnlAttributes.Controls.Add(this.cboPriorityAttributes);
            this.pnlAttributes.Controls.Add(this.lblAttributes);
            this.pnlAttributes.Location = new System.Drawing.Point(12, 48);
            this.pnlAttributes.Name = "pnlAttributes";
            this.pnlAttributes.Size = new System.Drawing.Size(342, 30);
            this.pnlAttributes.TabIndex = 105;
            this.pnlAttributes.MouseEnter += new System.EventHandler(this.pnlAttributes_MouseEnter);
            // 
            // cboPriorityAttributes
            // 
            this.cboPriorityAttributes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriorityAttributes.FormattingEnabled = true;
            this.cboPriorityAttributes.Location = new System.Drawing.Point(64, 3);
            this.cboPriorityAttributes.Name = "cboPriorityAttributes";
            this.cboPriorityAttributes.Size = new System.Drawing.Size(74, 21);
            this.cboPriorityAttributes.TabIndex = 39;
            this.cboPriorityAttributes.SelectedIndexChanged += new System.EventHandler(this.cboAttributes_SelectedIndexChanged);
            // 
            // lblAttributes
            // 
            this.lblAttributes.AutoSize = true;
            this.lblAttributes.Location = new System.Drawing.Point(4, 6);
            this.lblAttributes.Name = "lblAttributes";
            this.lblAttributes.Size = new System.Drawing.Size(54, 13);
            this.lblAttributes.TabIndex = 38;
            this.lblAttributes.Tag = "Label_Attributes";
            this.lblAttributes.Text = "Attributes:";
            // 
            // pnlSpecial
            // 
            this.pnlSpecial.Controls.Add(this.cboSpecial);
            this.pnlSpecial.Controls.Add(this.cboPrioritySpecial);
            this.pnlSpecial.Controls.Add(this.lblSpecial);
            this.pnlSpecial.Location = new System.Drawing.Point(12, 84);
            this.pnlSpecial.Name = "pnlSpecial";
            this.pnlSpecial.Size = new System.Drawing.Size(342, 30);
            this.pnlSpecial.TabIndex = 105;
            this.pnlSpecial.MouseEnter += new System.EventHandler(this.pnlSpecial_MouseEnter);
            // 
            // cboPrioritySpecial
            // 
            this.cboPrioritySpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrioritySpecial.FormattingEnabled = true;
            this.cboPrioritySpecial.Location = new System.Drawing.Point(64, 3);
            this.cboPrioritySpecial.Name = "cboPrioritySpecial";
            this.cboPrioritySpecial.Size = new System.Drawing.Size(74, 21);
            this.cboPrioritySpecial.TabIndex = 39;
            this.cboPrioritySpecial.SelectedIndexChanged += new System.EventHandler(this.cboSpecial_SelectedIndexChanged);
            // 
            // lblSpecial
            // 
            this.lblSpecial.AutoSize = true;
            this.lblSpecial.Location = new System.Drawing.Point(4, 6);
            this.lblSpecial.Name = "lblSpecial";
            this.lblSpecial.Size = new System.Drawing.Size(45, 13);
            this.lblSpecial.TabIndex = 38;
            this.lblSpecial.Tag = "Label_Special";
            this.lblSpecial.Text = "Special:";
            // 
            // pnlSkills
            // 
            this.pnlSkills.Controls.Add(this.cboPrioritySkills);
            this.pnlSkills.Controls.Add(this.lblSkills);
            this.pnlSkills.Location = new System.Drawing.Point(12, 120);
            this.pnlSkills.Name = "pnlSkills";
            this.pnlSkills.Size = new System.Drawing.Size(342, 30);
            this.pnlSkills.TabIndex = 105;
            this.pnlSkills.MouseEnter += new System.EventHandler(this.pnlSkills_MouseEnter);
            // 
            // cboPrioritySkills
            // 
            this.cboPrioritySkills.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrioritySkills.FormattingEnabled = true;
            this.cboPrioritySkills.Location = new System.Drawing.Point(64, 3);
            this.cboPrioritySkills.Name = "cboPrioritySkills";
            this.cboPrioritySkills.Size = new System.Drawing.Size(74, 21);
            this.cboPrioritySkills.TabIndex = 39;
            this.cboPrioritySkills.SelectedIndexChanged += new System.EventHandler(this.cboSkills_SelectedIndexChanged);
            // 
            // lblSkills
            // 
            this.lblSkills.AutoSize = true;
            this.lblSkills.Location = new System.Drawing.Point(4, 6);
            this.lblSkills.Name = "lblSkills";
            this.lblSkills.Size = new System.Drawing.Size(34, 13);
            this.lblSkills.TabIndex = 38;
            this.lblSkills.Tag = "Label_Skills";
            this.lblSkills.Text = "Skills:";
            // 
            // pnlNuyen
            // 
            this.pnlNuyen.Controls.Add(this.cboPriorityNuyen);
            this.pnlNuyen.Controls.Add(this.lblNuyen);
            this.pnlNuyen.Location = new System.Drawing.Point(12, 156);
            this.pnlNuyen.Name = "pnlNuyen";
            this.pnlNuyen.Size = new System.Drawing.Size(342, 30);
            this.pnlNuyen.TabIndex = 105;
            this.pnlNuyen.MouseEnter += new System.EventHandler(this.pnlNuyen_MouseEnter);
            // 
            // cboPriorityNuyen
            // 
            this.cboPriorityNuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriorityNuyen.FormattingEnabled = true;
            this.cboPriorityNuyen.Location = new System.Drawing.Point(64, 3);
            this.cboPriorityNuyen.Name = "cboPriorityNuyen";
            this.cboPriorityNuyen.Size = new System.Drawing.Size(74, 21);
            this.cboPriorityNuyen.TabIndex = 39;
            this.cboPriorityNuyen.SelectedIndexChanged += new System.EventHandler(this.cboNuyen_SelectedIndexChanged);
            // 
            // lblNuyen
            // 
            this.lblNuyen.AutoSize = true;
            this.lblNuyen.Location = new System.Drawing.Point(4, 6);
            this.lblNuyen.Name = "lblNuyen";
            this.lblNuyen.Size = new System.Drawing.Size(41, 13);
            this.lblNuyen.TabIndex = 38;
            this.lblNuyen.Tag = "Label_Nuyen";
            this.lblNuyen.Text = "Nuyen:";
            // 
            // cboSpecial
            // 
            this.cboSpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpecial.FormattingEnabled = true;
            this.cboSpecial.Location = new System.Drawing.Point(144, 3);
            this.cboSpecial.Name = "cboSpecial";
            this.cboSpecial.Size = new System.Drawing.Size(195, 21);
            this.cboSpecial.TabIndex = 40;
            this.cboSpecial.SelectedIndexChanged += new System.EventHandler(this.cboSpecial_SelectedIndexChanged_1);
            // 
            // cboMetatype
            // 
            this.cboMetatype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMetatype.FormattingEnabled = true;
            this.cboMetatype.Location = new System.Drawing.Point(144, 3);
            this.cboMetatype.Name = "cboMetatype";
            this.cboMetatype.Size = new System.Drawing.Size(195, 21);
            this.cboMetatype.TabIndex = 41;
            // 
            // frmPriority
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(697, 492);
            this.ControlBox = false;
            this.Controls.Add(this.pnlNuyen);
            this.Controls.Add(this.pnlSkills);
            this.Controls.Add(this.pnlSpecial);
            this.Controls.Add(this.pnlAttributes);
            this.Controls.Add(this.pnlMetatype);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPriority";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "Title_Priorities";
            this.Text = "Select Priorities";
            this.pnlMetatype.ResumeLayout(false);
            this.pnlMetatype.PerformLayout();
            this.pnlAttributes.ResumeLayout(false);
            this.pnlAttributes.PerformLayout();
            this.pnlSpecial.ResumeLayout(false);
            this.pnlSpecial.PerformLayout();
            this.pnlSkills.ResumeLayout(false);
            this.pnlSkills.PerformLayout();
            this.pnlNuyen.ResumeLayout(false);
            this.pnlNuyen.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ToolTip tipTooltip;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel pnlMetatype;
        private System.Windows.Forms.ComboBox cboPriorityMetatype;
        private System.Windows.Forms.Label lblMetatype;
        private System.Windows.Forms.Panel pnlAttributes;
        private System.Windows.Forms.ComboBox cboPriorityAttributes;
        private System.Windows.Forms.Label lblAttributes;
        private System.Windows.Forms.Panel pnlSpecial;
        private System.Windows.Forms.ComboBox cboPrioritySpecial;
        private System.Windows.Forms.Label lblSpecial;
        private System.Windows.Forms.Panel pnlSkills;
        private System.Windows.Forms.ComboBox cboPrioritySkills;
        private System.Windows.Forms.Label lblSkills;
        private System.Windows.Forms.Panel pnlNuyen;
        private System.Windows.Forms.ComboBox cboPriorityNuyen;
        private System.Windows.Forms.Label lblNuyen;
        private System.Windows.Forms.ComboBox cboSpecial;
        private System.Windows.Forms.ComboBox cboMetatype;
    }
}