using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chummer;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Chummer2Tests
{
    [TestClass]
    public class CharacterTests
    {
        /// <summary>
        /// Verify that Physical CM is calculated correctly.
        /// </summary>
        [TestMethod]
        public void PhysicalCMTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Set the character's BOD to 4 and verify that they have 10 boxes in their Physical Condition Monitor.
            objCharacter.BOD.Value = 4;
            Assert.AreEqual(10, objCharacter.PhysicalCM, "Physical CM does not equal the expected value of 10.");
            
            // Set the character's BOD to 5 and verify that they have 11 boxes in their Physical Condition Monitor.
            objCharacter.BOD.Value = 5;
            Assert.AreEqual(11, objCharacter.PhysicalCM, "Physical CM does not equal the expected value of 11.");
        }

        /// <summary>
        /// Verify that Stun CM is calculated correctly.
        /// </summary>
        [TestMethod]
        public void StunCMTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Set the character's WIL to 4 and verify that they have 10 boxes in their Stun Condition Monitor.
            objCharacter.WIL.Value = 4;
            Assert.AreEqual(10, objCharacter.StunCM, "Stun CM does not equal the expected value of 10.");

            // Set the character's WIL to 5 and verify that they have 11 boxes in their Stun Condition Monitor.
            objCharacter.WIL.Value = 5;
            Assert.AreEqual(11, objCharacter.StunCM, "Stun CM does not equal the expected value of 11.");
        }

        /// <summary>
        /// Verify that the CM Threshold is calculated correctly.
        /// </summary>
        [TestMethod]
        public void CMThresholdTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Verify that the character's Treshold is correct.
            Assert.AreEqual(3, objCharacter.CMThreshold, "CM Threshold does not equal the expected value of 3.");

            // Add a Threshold Improvement and verify it is correct.
            Improvement objImprovement = new Improvement();
            objImprovement.ImproveType = Improvement.ImprovementType.CMThreshold;
            objImprovement.Value = 1;
            objImprovement.ImproveSource = Improvement.ImprovementSource.Quality;
            objCharacter.Improvements.Add(objImprovement);

            Assert.AreEqual(4, objCharacter.CMThreshold, "CM Threshold does not equal the expected value of 4.");
        }

        /// <summary>
        /// Verify that the CM Threshold Offset is calculated correctly.
        /// </summary>
        [TestMethod]
        public void CMThresholdOffsetTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Verify that the character's Treshold Offset is correct.
            Assert.AreEqual(0, objCharacter.CMThresholdOffset, "CM Threshold Offset does not equal the expected value of 0.");

            // Add a Threshold Offset Improvement and verify it is correct.
            Improvement objImprovement = new Improvement();
            objImprovement.ImproveType = Improvement.ImprovementType.CMThresholdOffset;
            objImprovement.Value = 1;
            objImprovement.ImproveSource = Improvement.ImprovementSource.Quality;
            objCharacter.Improvements.Add(objImprovement);

            Assert.AreEqual(1, objCharacter.CMThresholdOffset, "CM Threshold Offset does not equal the expected value of 1.");
        }

        /// <summary>
        /// Verify that the CM Overflow is calculated correctly.
        /// </summary>
        [TestMethod]
        public void CMOverflowTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Set the character's BOD to 4 and verify the Overflow.
            objCharacter.BOD.Value = 4;
            Assert.AreEqual(5, objCharacter.CMOverflow, "CM Overflow does not equal the expected value of 5.");

            // Add an Overflow Improvement and verify it is correct.
            Improvement objImprovement = new Improvement();
            objImprovement.ImproveType = Improvement.ImprovementType.CMOverflow;
            objImprovement.Value = 1;
            objImprovement.ImproveSource = Improvement.ImprovementSource.Quality;
            objCharacter.Improvements.Add(objImprovement);

            Assert.AreEqual(6, objCharacter.CMOverflow, "CM Overflow does not equal the expected value of 6.");
        }

        /// <summary>
        /// Make sure that Cyberware affectes Essence properly.
        /// </summary>
        [TestMethod]
        public void EssenceCyberwareOnlyTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            Cyberware objCyberware = new Cyberware(objCharacter);
            objCyberware.ESS = "0.3";
            objCharacter.Cyberware.Add(objCyberware);

            Assert.AreEqual(5.7m, objCharacter.Essence, "Essence does not equal the expected value of 5.7.");
            Assert.AreEqual(1, objCharacter.EssencePenalty, "Essence Penalty does not equal the expected value of 1.");
        }

        /// <summary>
        /// Make sure that mixing Cyberware and Bioware produces the correct Essence total.
        /// </summary>
        [TestMethod]
        public void EssenceCyberwareAndBiowareTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            Cyberware objCybeware = new Cyberware(objCharacter);
            objCybeware.SourceType = Improvement.ImprovementSource.Cyberware;
            objCybeware.ESS = "0.6";

            Cyberware objBioware = new Cyberware(objCharacter);
            objBioware.SourceType = Improvement.ImprovementSource.Bioware;
            objBioware.ESS = "0.4";

            objCharacter.Cyberware.Add(objCybeware);
            objCharacter.Cyberware.Add(objBioware);

            // Bioware Essence should only be 0.2 since the lower of the Cyberware/Bioware totals is halved.
            Assert.AreEqual(5.2m, objCharacter.Essence, "Essence does not equal the expected value of 5.6.");
            Assert.AreEqual(0.6m, objCharacter.CyberwareEssence, "Cyberware Essence does not equal the expected value of 0.6.");
            Assert.AreEqual(0.2m, objCharacter.BiowareEssence, "Bioware Essence does not equal the expected value of 0.2.");
        }

        /// <summary>
        /// Make sure that Armor Value and Encumbrance are working correctly.
        /// </summary>
        [TestMethod]
        public void ArmorTest()
        {
            // Create a new Human character.
            Character objCharacter = new Character();
            objCharacter.LoadMetatype(Guid.Parse("e28e7075-f635-4c02-937c-e4fc61c51602"));

            // Add ArmorValue 6 Armor to the character. Their total Armor should be 6 with a Encumbrance penalty of 0 since there is nothing with a +value.
            Armor objArmor = new Armor(objCharacter);
            objArmor.ArmorValue = "6";
            objCharacter.Armor.Add(objArmor);

            Assert.AreEqual(6, objCharacter.ArmorValue, "ArmorValue does not equal the expected value of 6.");
            Assert.AreEqual(6, objCharacter.TotalArmorValue, "TotalArmorValue does not equal the expected value of 6.");
            Assert.AreEqual(0, objCharacter.ArmorEncumbrance, "ArmorEncumbrance does not equal the expected value of 0.");

            // Add an Armor Mod to the Armor. This should bring the Armor value up to 8.
            ArmorMod objMod = new ArmorMod(objCharacter);
            objMod.ArmorValue = 2;
            objCharacter.Armor[0].ArmorMods.Add(objMod);

            Assert.AreEqual(8, objCharacter.ArmorValue, "ArmorValue does not equal the expected value of 8.");
            Assert.AreEqual(8, objCharacter.TotalArmorValue, "TotalArmorValue does not equal the expected value of 8.");
            Assert.AreEqual(0, objCharacter.ArmorEncumbrance, "ArmorEncumbrance does not equal the expected value of 0.");

            // Add an additional +6 value Armor to the character. Their total Aromr should be 14 with an Encumbrance penalty of 2.
            Armor objPlusArmor = new Armor(objCharacter);
            objPlusArmor.ArmorValue = "+6";
            objCharacter.Armor.Add(objPlusArmor);

            Assert.AreEqual(14, objCharacter.ArmorValue, "ArmorValue does not equal the expected value of 14.");
            Assert.AreEqual(14, objCharacter.TotalArmorValue, "TotalArmorValue does not equal the expected value of 14.");
            Assert.AreEqual(2, objCharacter.ArmorEncumbrance, "ArmorEncumbrance does not equal the expected value of 2.");

            // Increase the character's STR to 4. This should reduce the Armor Encumbrance penalty to 1.
            objCharacter.STR.Value = 4;

            Assert.AreEqual(14, objCharacter.ArmorValue, "ArmorValue does not equal the expected value of 14.");
            Assert.AreEqual(14, objCharacter.TotalArmorValue, "TotalArmorValue does not equal the expected value of 14.");
            Assert.AreEqual(1, objCharacter.ArmorEncumbrance, "ArmorEncumbrance does not equal the expected value of 1.");

            // Unequipping the Armor Mod should reduce the Armor value down to 12.
            objCharacter.Armor[0].ArmorMods[0].Equipped = false;

            Assert.AreEqual(12, objCharacter.ArmorValue, "ArmorValue does not equal the expected value of 12.");
            Assert.AreEqual(12, objCharacter.TotalArmorValue, "TotalArmorValue does not equal the expected value of 12.");
            Assert.AreEqual(1, objCharacter.ArmorEncumbrance, "ArmorEncumbrance does not equal the expected value of 1.");

            // Unequipping the +value Armor should put the character back to Armor 6 with no Encumbrance penalty.
            objCharacter.Armor[1].Equipped = false;

            Assert.AreEqual(6, objCharacter.ArmorValue, "ArmorValue does not equal the expected value of 6.");
            Assert.AreEqual(6, objCharacter.TotalArmorValue, "TotalArmorValue does not equal the expected value of 6.");
            Assert.AreEqual(0, objCharacter.ArmorEncumbrance, "ArmorEncumbrance does not equal the expected value of 0.");
        }
    }
}
