using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chummer;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Chummer2Tests
{
    [TestClass]
    public class UniqueTests
    {
        #region Attributes Tests
        /// <summary>
        /// Make sure that standard Attributes are working correctly.
        /// </summary>
        [TestMethod]
        public void StandardAttributeTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Test the standard Attribute ranges.
            Assert.AreEqual(1, objCharacter.BOD.MetatypeMinimum, "BOD Minimum is not equal to the expected value of 1.");
            Assert.AreEqual(6, objCharacter.BOD.MetatypeMaximum, "BOD Maximum is not equal to the expected value of 6.");
            Assert.AreEqual(9, objCharacter.BOD.MetatypeAugmentedMaximum, "BOD Augmented Maximum is not equal to the expected value of 9.");

            // Try to go outside of the standard minimum and maximum values.
            objCharacter.BOD.Value = 10;
            Assert.AreEqual(6, objCharacter.BOD.TotalValue, "BOD value was able to extend beyond the Maximum value of 6.");
            objCharacter.BOD.Value = -10;
            Assert.AreEqual(1, objCharacter.BOD.TotalValue, "BOD value was able to extend beyond the Minimum value of 1.");

            // Make sure that adding an Improvement that would extend the Attribute's maximum range still only returns the absolute maximum.
            Improvement objImprovement = new Improvement();
            objImprovement.ImproveType = Improvement.ImprovementType.Attribute;
            objImprovement.Augmented = 20;
            objImprovement.ImprovedName = "BOD";
            objImprovement.CustomName = "BOD Value Improvement";
            objImprovement.ImproveSource = Improvement.ImprovementSource.Quality;
            objCharacter.Improvements.Add(objImprovement);
            Assert.AreEqual(9, objCharacter.BOD.TotalValue, "BOD value can extend beyond the Augmented Maximum value of 9.");

            // Increase the Attribute's augmented maximum and verify that the new maximum value from Improvements is correct.
            Improvement objSecondImprovement = new Improvement();
            objSecondImprovement.ImproveType = Improvement.ImprovementType.Attribute;
            objSecondImprovement.AugmentedMaximum = 1;
            objSecondImprovement.ImprovedName = "BOD";
            objSecondImprovement.CustomName = "BOD Augmented Maximum Improvement";
            objSecondImprovement.ImproveSource = Improvement.ImprovementSource.Quality;
            objCharacter.Improvements.Add(objSecondImprovement);
            Assert.AreEqual(10, objCharacter.BOD.TotalValue, "BOD value increased Augmented Maximum is not the expected value of 10.");
        }

        /// <summary>
        /// Make sure that special Attributes are working correctly.
        /// </summary>
        [TestMethod]
        public void SpecialAttributeTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Test the standard Attribute ranges.
            Assert.AreEqual(1, objCharacter.MAG.MetatypeMinimum, "MAG Minimum is not equal to the expected value of 1.");
            Assert.AreEqual(6, objCharacter.MAG.MetatypeMaximum, "MAG Maximum is not equal to the expected value of 6.");
            Assert.AreEqual(6, objCharacter.MAG.MetatypeAugmentedMaximum, "MAG Augmented Maximum is not equal to the expected value of 6.");

            // Try to go outside of the standard minimum and maximum values.
            objCharacter.MAG.Value = 10;
            Assert.AreEqual(6, objCharacter.MAG.TotalValue, "MAG value was able to extend beyond the Maximum value of 6.");
            objCharacter.MAG.Value = -10;
            Assert.AreEqual(1, objCharacter.MAG.TotalValue, "MAG value was able to extend beyond the Minimum value of 1.");

            // Set the character's MAG to 5, then apply an Essence penalty through Cyberware.
            objCharacter.MAG.Value = 5;

            // MAG should be 4 after adding an Essence Penalty.
            Cyberware objCyberware = new Cyberware(objCharacter);
            objCyberware.ESS = "0.3";
            objCharacter.Cyberware.Add(objCyberware);
            Assert.AreEqual(4, objCharacter.MAG.TotalValue, "MAG value after an Essence reduction is not equal to the expected value of 4.");
        }
        #endregion

        #region Critter Powers Tests
        /// <summary>
        /// Test creating all Metamagics in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllCritterPowersTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("critterpowers.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/powers/power"))
            {
                CritterPower objPower = new CritterPower(objCharacter);
                objPower.Create(objXmlNode, objCharacter, objNode, 0, "", false);
            }
        }
        #endregion

        #region Metamagic Tests
        /// <summary>
        /// Test creating all Metamagics in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllMetamagicsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            ContextMenuStrip cmsGear = new ContextMenuStrip();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("metamagics.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/metamagics/metamagic"))
            {
                Metamagic objMetamagic = new Metamagic(objCharacter);
                objMetamagic.Create(objXmlNode, objCharacter, objNode, Improvement.ImprovementSource.Metamagic);
            }
        }

        /// <summary>
        /// Test creating all Echoes in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllEchoesTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            ContextMenuStrip cmsGear = new ContextMenuStrip();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("echoes.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/metamagics/metamagic"))
            {
                Metamagic objMetamagic = new Metamagic(objCharacter);
                objMetamagic.Create(objXmlNode, objCharacter, objNode, Improvement.ImprovementSource.Metamagic);
            }
        }
        #endregion

        #region Power Tests
        [TestMethod]
        public void CreateAllPowersTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            XmlDocument objXmlDocument = XmlManager.Instance.Load("powers.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/powers/power"))
            {
                Power objPower = new Power(objCharacter);
                objPower.Create(objXmlNode, objCharacter, 1, "", false);
            }
        }
        #endregion

        #region Programs Tests
        /// <summary>
        /// Test creating all Programs in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllProgramsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("programs.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/programs/program"))
            {
                TechProgram objProgram = new TechProgram(objCharacter);
                objProgram.Create(objXmlNode, objCharacter, objNode, "", false);
            }
        }
        #endregion

        #region Qualities Tests
        /// <summary>
        /// Test creating all Weapons in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllQualitiesTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("qualities.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/qualities/quality"))
            {
                Quality objQuality = new Quality(objCharacter);
                objQuality.Create(objXmlNode, objCharacter, QualitySource.Selected, objNode, lstWeapons, lstTreeNodes);
            }
        }
        #endregion

        #region Skills Tests
        /// <summary>
        /// Make sure that a new Skill passes.
        /// </summary>
        [TestMethod]
        public void SkillTest()
        {
            // Create a new character with a BOD of 4.
            Character objCharacter = new Character();
            objCharacter.BOD.Value = 4;

            // Create a new Skill.
            Skill objSkill = new Skill(objCharacter);
            objSkill.Name = "Diving";
            objSkill.Attribute = "BOD";
            objSkill.SkillCategory = "Physical Active";
            objSkill.Default = true;
            objSkill.RatingMaximum = 12;
            objSkill.Rating = 3;
            objSkill.Source = "Test";
            objSkill.Page = "123";

            // Make sure the Skill's properties are valid.
            Assert.AreEqual("Diving", objSkill.Name, "Skill name mismatch.");
            Assert.AreEqual(3, objSkill.Rating, "Skill Rating mismatch.");
            Assert.AreEqual("BOD", objSkill.Attribute, "Skill Attribute mismatch.");
            Assert.AreEqual("Physical Active", objSkill.SkillCategory, "Skill Category mismatch.");
            Assert.AreEqual("Test", objSkill.Source, "Skill Source mistmatch.");
            Assert.AreEqual("123", objSkill.Page, "Skill Page mismatch.");
            Assert.AreEqual(4, objSkill.AttributeModifiers, "Skill's Attribute value does not match actual Attribute value.");
            Assert.AreEqual(7, objSkill.TotalRating, "Skill Rating does not equal the expected value of 7.");
        }
        #endregion

        #region Spell Tests
        /// <summary>
        /// Test creating all Spells in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllSpellsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("spells.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/spells/spell"))
            {
                Spell objSpell = new Spell(objCharacter);
                objSpell.Create(objXmlNode, objCharacter, objNode, "", false, false, false);
            }
        }
        #endregion
    }
}