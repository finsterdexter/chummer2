using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chummer;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Chummer2Tests
{
    [TestClass]
    public class EquipmentTests
    {
        #region Armor Tests
        /// <summary>
        /// Test creating all Armor in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllArmorTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            ContextMenuStrip cmsArmor = new ContextMenuStrip();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("armor.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/armors/armor"))
            {
                Armor objArmor = new Armor(objCharacter);
                objArmor.Create(objXmlNode, objNode, cmsArmor);
            }
        }

        /// <summary>
        /// Test creating all Armor Mods in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllArmorModsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("armor.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/mods/mod"))
            {
                ArmorMod objMod = new ArmorMod(objCharacter);
                objMod.Create(objXmlNode, objNode, 1, lstWeapons, lstTreeNodes);
            }
        }
        #endregion

        #region Cyberware Tests
        /// <summary>
        /// Test creating all Bioware in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllBiowareTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            GradeList objGradeList = new GradeList();
            objGradeList.LoadList(Improvement.ImprovementSource.Bioware);

            XmlDocument objXmlDocument = XmlManager.Instance.Load("bioware.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/biowares/bioware"))
            {
                Cyberware objCyberware = new Cyberware(objCharacter);
                objCyberware.Create(objXmlNode, objCharacter, objGradeList.GetGrade("Standard"), Improvement.ImprovementSource.Bioware, 1, objNode, lstWeapons, lstTreeNodes);
            }
        }

        /// <summary>
        /// Test creating all Cyberware in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllCyberwareTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            GradeList objGradeList = new GradeList();
            objGradeList.LoadList(Improvement.ImprovementSource.Cyberware);

            XmlDocument objXmlDocument = XmlManager.Instance.Load("cyberware.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/cyberwares/cyberware"))
            {
                Cyberware objCyberware = new Cyberware(objCharacter);
                objCyberware.Create(objXmlNode, objCharacter, objGradeList.GetGrade("Standard"), Improvement.ImprovementSource.Cyberware, 1, objNode, lstWeapons, lstTreeNodes);
            }
        }
        #endregion

        #region Gear Tests
        /// <summary>
        /// Test creating all Gear in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllGearTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            ContextMenuStrip cmsGear = new ContextMenuStrip();
            List<Weapon> lstWeapons = new List<Weapon>();
            List<TreeNode> lstTreeNodes = new List<TreeNode>();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("gear.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/gears/gear"))
            {
                Gear objGear = new Gear(objCharacter);
                objGear.Create(objXmlNode, objCharacter, objNode, 1, lstWeapons, lstTreeNodes);
            }
        }
        #endregion

        #region Vehicles Tests
        /// <summary>
        /// Test creating all Vehicles in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllVehiclesTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            ContextMenuStrip cmsVehicle = new ContextMenuStrip();
            ContextMenuStrip cmsVehicleGear = new ContextMenuStrip();
            ContextMenuStrip cmsVehicleWeapon = new ContextMenuStrip();
            ContextMenuStrip cmsVehicleWeaponAccessory = new ContextMenuStrip();
            ContextMenuStrip cmsVehicleWeaponMod = new ContextMenuStrip();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("vehicles.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/vehicles/vehicle"))
            {
                Vehicle objVehicle = new Vehicle(objCharacter);
                objVehicle.Create(objXmlNode, objNode, cmsVehicle, cmsVehicleGear, cmsVehicleWeapon, cmsVehicleWeaponAccessory, cmsVehicleWeaponMod);
            }
        }

        /// <summary>
        /// Test creating all Vehicle Mods in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllVehicleModsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("vehicles.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/mods/mod"))
            {
                VehicleMod objMod = new VehicleMod(objCharacter);
                objMod.Create(objXmlNode, objNode, 1);
            }
        }
        #endregion

        #region Weapons Tests
        /// <summary>
        /// Make sure that Weapon values are behaving correctly.
        /// </summary>
        [TestMethod]
        public void WeaponTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Add a Combat Axe.
            Weapon objWeapon = new Weapon(objCharacter);
            objWeapon.Category = "Blades";
            objWeapon.WeaponType = "Melee";
            objWeapon.Spec = "Axes";
            objWeapon.Reach = 2;
            objWeapon.Damage = "(STR/2+9)P";
            objWeapon.AP = "-4";
            objWeapon.RC = "0";
            objWeapon.Accuracy = "4";
            objWeapon.Ammo = "0";
            objWeapon.Concealability = 0;
            objWeapon.Avail = "9R";
            objWeapon.Cost = 500;
            objWeapon.Source = "SR5";
            objWeapon.Page = "900";

            // Add the Weapon to the character.
            objCharacter.Weapons.Add(objWeapon);

            // Set the character's Blades Skill to 3.
            Skill objSkill = new Skill(objCharacter);
            objSkill = objCharacter.Skills.Find(o => o.DisplayName == "Blades");
            objSkill.Rating = 3;

            // Set the character's STR to 4 and make sure the Weapon attributes are calculated correctly.
            objCharacter.STR.Value = 4;
            Assert.AreEqual("11P", objWeapon.CalculatedDamage(), "Weapon DV does not match the expected value of 11P.");
            // Dice Pool should be 4 since the character has Blades 3 + AGI 1.
            Assert.AreEqual("4", objWeapon.DicePool, "Weapon Dice Pool does not match the expected value of 4.");
            // Add the Specialization to the Skill and check its value again. Should be 4 (6).
            objSkill.Specialization = "Axes";
            Assert.AreEqual("4 (6)", objWeapon.DicePool, "Weapon Dice Pool with Specialization does not match the expected value of 4 (6).");

            // Set the character's STR to 5 and make sure the Weapon attributes are calculated correctly. DV should be 12 since fractions are rounded up.
            objCharacter.STR.Value = 5;
            Assert.AreEqual("12P", objWeapon.CalculatedDamage(), "Weapon DV does not match the expected value of 11P.");
        }

        /// <summary>
        /// Test creating all Weapons in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllWeaponsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();
            ContextMenuStrip cmsWeapon = new ContextMenuStrip();
            ContextMenuStrip cmsWeaponAccessory = new ContextMenuStrip();
            ContextMenuStrip cmsWeaponMod = new ContextMenuStrip();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/weapons/weapon"))
            {
                Weapon objWeapon = new Weapon(objCharacter);
                objWeapon.Create(objXmlNode, objCharacter, objNode, cmsWeapon, cmsWeaponAccessory, cmsWeaponMod, true);
            }
        }

        /// <summary>
        /// Test creating all Weapon Accessories in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllWeaponAccessoriesTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/accessories/accessory"))
            {
                WeaponAccessory objAccessory = new WeaponAccessory(objCharacter);
                objAccessory.Create(objXmlNode, objNode, "Barrel");
            }
        }

        /// <summary>
        /// Test creating all Weapon Mods in the data file.
        /// </summary>
        [TestMethod]
        public void CreateAllWeaponModsTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            TreeNode objNode = new TreeNode();

            XmlDocument objXmlDocument = XmlManager.Instance.Load("weapons.xml");
            foreach (XmlNode objXmlNode in objXmlDocument.SelectNodes("/chummer/mods/mod"))
            {
                WeaponMod objMod = new WeaponMod(objCharacter);
                objMod.Create(objXmlNode, objNode);
            }
        }
        #endregion
    }
}