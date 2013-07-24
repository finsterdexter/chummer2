using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;

namespace Chummer
{
	public class CommonFunctions
	{
		#region Constructor and Instance
		private Character _objCharacter;
		private ContextMenuStrip _cmsArmor;
		private ContextMenuStrip _cmsArmorMod;
		private ContextMenuStrip _cmsCyberware;
		private ContextMenuStrip _cmsWeapon;
		private ContextMenuStrip _cmsWeaponAccessory;
		private ContextMenuStrip _cmsWeaponMod;
		private ContextMenuStrip _cmsGear;
		private ContextMenuStrip _cmsVehicle;
		private ContextMenuStrip _cmsVehicleMod;

		public CommonFunctions(Character objCharacter)
		{
			_objCharacter = objCharacter;
		}

		CommonFunctions()
		{
		}
		#endregion

		#region Properties
		public ContextMenuStrip ContextMenuArmor
		{
			get
			{
				return _cmsArmor;
			}
			set
			{
				_cmsArmor = value;
			}
		}

		public ContextMenuStrip ContextMenuArmorMod
		{
			get
			{
				return _cmsArmorMod;
			}
			set
			{
				_cmsArmorMod = value;
			}
		}

		public ContextMenuStrip ContextMenuCyberware
		{
			get
			{
				return _cmsCyberware;
			}
			set
			{
				_cmsCyberware = value;
			}
		}

		public ContextMenuStrip ContextMenuWeapon
		{
			get
			{
				return _cmsWeapon;
			}
			set
			{
				_cmsWeapon = value;
			}
		}

		public ContextMenuStrip ContextMenuWeaponAccessory
		{
			get
			{
				return _cmsWeaponAccessory;
			}
			set
			{
				_cmsWeaponAccessory = value;
			}
		}

		public ContextMenuStrip ContextMenuWeaponMod
		{
			get
			{
				return _cmsWeaponMod;
			}
			set
			{
				_cmsWeaponMod = value;
			}
		}

		public ContextMenuStrip ContextMenuGear
		{
			get
			{
				return _cmsGear;
			}
			set
			{
				_cmsGear = value;
			}
		}

		public ContextMenuStrip ContextMenuVehicle
		{
			get
			{
				return _cmsVehicle;
			}
			set
			{
				_cmsVehicle = value;
			}
		}

		public ContextMenuStrip ContextMenuVehicleMod
		{
			get
			{
				return _cmsVehicleMod;
			}
			set
			{
				_cmsVehicleMod = value;
			}
		}
		#endregion

		#region Find Functions
        /// <summary>
        /// Find a piece of Equipment.
        /// </summary>
        /// <param name="strGuid">GUID of the Equipment to find.</param>
        /// <param name="lstEquipment">List of items to search for.</param>
        /// <param name="objDesiredType">Type of Equipment to find. Null will return any type that matches the ID.</param>
		public Equipment FindEquipment(string strGuid, List<Equipment> lstEquipment, Type objDesiredType)
		{
			Equipment objReturn = null;
			foreach (Equipment objEquipment in lstEquipment)
			{
				if (objEquipment.InternalId == strGuid)
					objReturn = objEquipment;
				else
				{
					if (objReturn == null && objEquipment.Armors.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.Armors, objDesiredType);
					if (objReturn == null && objEquipment.ArmorMods.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.ArmorMods, objDesiredType);
					if (objReturn == null && objEquipment.Cyberwares.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.Cyberwares, objDesiredType);
					if (objReturn == null && objEquipment.Gears.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.Gears, objDesiredType);
					if (objReturn == null && objEquipment.Vehicles.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.Vehicles, objDesiredType);
					if (objReturn == null && objEquipment.VehicleMods.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.VehicleMods, objDesiredType);
					if (objReturn == null && objEquipment.Weapons.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.Weapons, objDesiredType);
					if (objReturn == null && objEquipment.WeaponAccessories.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.WeaponAccessories, objDesiredType);
					if (objReturn == null && objEquipment.WeaponMods.Count > 0)
						objReturn = FindEquipment(strGuid, objEquipment.WeaponMods, objDesiredType);
				}

				if (objReturn != null)
				{
					if (objReturn.InternalId != Guid.Empty.ToString() && objReturn.Name != "")
					{
						if (objDesiredType == null)
							return objReturn;
						else
						{
							if (objDesiredType != objReturn.GetType())
								return null;
							else
								return objReturn;
						}
					}
				}
			}

			objReturn = null;
			return objReturn;
		}

        /// <summary>
        /// Find a piece of Equipment.
        /// </summary>
        /// <param name="strGuid">External GUID of the Equipment to find.</param>
        /// <param name="lstEquipment">List of items to search for.</param>
        /// <param name="objDesiredType">Type of Equipment to find. Null will return any type that matches the ID.</param>
        public Equipment FindEquipmentByExternalId(string strGuid, List<Equipment> lstEquipment, Type objDesiredType)
        {
            Equipment objReturn = null;
            foreach (Equipment objEquipment in lstEquipment)
            {
                if (objEquipment.ExternalId == strGuid)
                    objReturn = objEquipment;
                else
                {
                    if (objReturn == null && objEquipment.Armors.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.Armors, objDesiredType);
                    if (objReturn == null && objEquipment.ArmorMods.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.ArmorMods, objDesiredType);
                    if (objReturn == null && objEquipment.Cyberwares.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.Cyberwares, objDesiredType);
                    if (objReturn == null && objEquipment.Gears.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.Gears, objDesiredType);
                    if (objReturn == null && objEquipment.Vehicles.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.Vehicles, objDesiredType);
                    if (objReturn == null && objEquipment.VehicleMods.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.VehicleMods, objDesiredType);
                    if (objReturn == null && objEquipment.Weapons.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.Weapons, objDesiredType);
                    if (objReturn == null && objEquipment.WeaponAccessories.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.WeaponAccessories, objDesiredType);
                    if (objReturn == null && objEquipment.WeaponMods.Count > 0)
                        objReturn = FindEquipment(strGuid, objEquipment.WeaponMods, objDesiredType);
                }

                if (objReturn != null)
                {
                    if (objReturn.ExternalId != Guid.Empty.ToString() && objReturn.Name != "")
                    {
                        if (objDesiredType == null)
                            return objReturn;
                        else
                        {
                            if (objDesiredType != objReturn.GetType())
                                return null;
                            else
                                return objReturn;
                        }
                    }
                }
            }

            objReturn = null;
            return objReturn;
        }

		/// <summary>
		/// Locate a piece of Gear by matching on its Weapon ID.
		/// </summary>
		/// <param name="strGuid">InternalId of the Weapon to find.</param>
		/// <param name="lstGear">List of Gear to search.</param>
		public Gear FindGearByWeaponID(string strGuid, List<Equipment> lstGear)
		{
			Gear objReturn = new Gear(_objCharacter);
			foreach (Gear objGear in lstGear)
			{
				if (objGear.WeaponID == strGuid)
					objReturn = objGear;
				else
				{
					if (objGear.Gears.Count > 0)
						objReturn = FindGearByWeaponID(strGuid, objGear.Gears);
				}

				if (objReturn != null)
				{
					if (objReturn.InternalId != Guid.Empty.ToString() && objReturn.Name != "")
						return objReturn;
				}
			}

			objReturn = null;
			return objReturn;
		}

		/// <summary>
		/// Locate a Lifestyle within the character's Lifestyles.
		/// </summary>
		/// <param name="strGuid">InternalId of the Lifestyle to find.</param>
		/// <param name="lstLifestyles">List of Lifestyles to search.</param>
		public Lifestyle FindLifestyle(string strGuid, List<Lifestyle> lstLifestyles)
		{
			foreach (Lifestyle objLifestyle in lstLifestyles)
			{
				if (objLifestyle.InternalId == strGuid)
					return objLifestyle;
			}

			return null;
		}

		/// <summary>
		/// Locate a Complex Form within the character's Complex Forms.
		/// </summary>
		/// <param name="strGuid">InternalId of the Complex Form to find.</param>
		/// <param name="lstPrograms">List of Complex Forms to search.</param>
		public TechProgram FindTechProgram(string strGuid, List<TechProgram> lstPrograms)
		{
			foreach (TechProgram objProgram in lstPrograms)
			{
				if (objProgram.InternalId == strGuid)
					return objProgram;
			}

			return null;
		}

		/// <summary>
		/// Locate a Complex Form Option within the character's Complex Forms.
		/// </summary>
		/// <param name="strGuid">InternalId of the Complex Form Option to Find.</param>
		/// <param name="lstPrograms">List of Complex Forms to search.</param>
		/// <param name="objFoundProgram">Complex Form that the Option was found in.</param>
		public TechProgramOption FindTechProgramOption(string strGuid, List<TechProgram> lstPrograms, out TechProgram objFoundProgram)
		{
			foreach (TechProgram objProgram in lstPrograms)
			{
				foreach (TechProgramOption objOption in objProgram.Options)
				{
					if (objOption.InternalId == strGuid)
					{
						objFoundProgram = objProgram;
						return objOption;
					}
				}
			}

			objFoundProgram = null;
			return null;
		}

		/// <summary>
		/// Locate a Spell within the character's Spells.
		/// </summary>
		/// <param name="strGuid">InternalId of the Spell to find.</param>
		/// <param name="lstSpells">List of Spells to search.</param>
		public Spell FindSpell(string strGuid, List<Spell> lstSpells)
		{
			foreach (Spell objSpell in lstSpells)
			{
				if (objSpell.InternalId == strGuid)
					return objSpell;
			}

			return null;
		}

		/// <summary>
		/// Locate a Critter Power within the character's Critter Powers.
		/// </summary>
		/// <param name="strGuid">InternalId of the Critter Power to find.</param>
		/// <param name="lstCritterPowers">List of Critter Powers to search.</param>
		public CritterPower FindCritterPower(string strGuid, List<CritterPower> lstCritterPowers)
		{
			foreach (CritterPower objPower in lstCritterPowers)
			{
				if (objPower.InternalId == strGuid)
					return objPower;
			}

			return null;
		}

		/// <summary>
		/// Locate a Quality within the character's Qualities.
		/// </summary>
		/// <param name="strGuid">InternalId of the Quality to find.</param>
		/// <param name="lstQualities">List of Qualities to search.</param>
		public Quality FindQuality(string strGuid, List<Quality> lstQualities)
		{
			foreach (Quality objQuality in lstQualities)
			{
				if (objQuality.InternalId == strGuid)
					return objQuality;
			}

			return null;
		}

		/// <summary>
		/// Locate a Metamagic within the character's Metamagics.
		/// </summary>
		/// <param name="strGuid">InternalId of the Metamagic to find.</param>
		/// <param name="lstMetamagics">List of Metamagics to search.</param>
		public Metamagic FindMetamagic(string strGuid, List<Metamagic> lstMetamagics)
		{
			foreach (Metamagic objMetamagic in lstMetamagics)
			{
				if (objMetamagic.InternalId == strGuid)
					return objMetamagic;
			}

			return null;
		}

		/// <summary>
		/// Locate a Martial Art within the character's Martial Arts.
		/// </summary>
		/// <param name="strName">Name of the Martial Art to find.</param>
		/// <param name="lstMartialArts">List of Martial Arts to search.</param>
		public MartialArt FindMartialArt(string strName, List<MartialArt> lstMartialArts)
		{
			foreach (MartialArt objArt in lstMartialArts)
			{
				if (objArt.Name == strName)
					return objArt;
			}

			return null;
		}

		/// <summary>
		/// Locate a Martial Art Advantage within the character's Martial Arts.
		/// </summary>
		/// <param name="strGuid">InternalId of the Martial Art Advantage to find.</param>
		/// <param name="lstMartialArts">List of Martial Arts to search.</param>
		/// <param name="objFoundMartialArt">MartialArt the Advantage was found in.</param>
		public MartialArtAdvantage FindMartialArtAdvantage(string strGuid, List<MartialArt> lstMartialArts, out MartialArt objFoundMartialArt)
		{
			foreach (MartialArt objArt in lstMartialArts)
			{
				foreach (MartialArtAdvantage objAdvantage in objArt.Advantages)
				{
					if (objAdvantage.InternalId == strGuid)
					{
						objFoundMartialArt = objArt;
						return objAdvantage;
					}
				}
			}

			objFoundMartialArt = null;
			return null;
		}

		/// <summary>
		/// Locate a Martial Art Maneuver within the character's Martial Art Maneuvers.
		/// </summary>
		/// <param name="strGuid">InternalId of the Martial Art Maneuver to find.</param>
		/// <param name="lstManeuvers">List of Martial Art Maneuvers to search.</param>
		public MartialArtManeuver FindMartialArtManeuver(string strGuid, List<MartialArtManeuver> lstManeuvers)
		{
			foreach (MartialArtManeuver objManeuver in lstManeuvers)
			{
				if (objManeuver.InternalId == strGuid)
					return objManeuver;
			}

			return null;
		}

		/// <summary>
		/// Find a TreeNode in a TreeView based on its Tag.
		/// </summary>
		/// <param name="strGuid">InternalId of the Node to find.</param>
		/// <param name="treTree">TreeView to search.</param>
		public TreeNode FindNode(string strGuid, TreeView treTree)
		{
			TreeNode objFound = new TreeNode();
			foreach (TreeNode objNode in treTree.Nodes)
			{
				if (objNode.Tag.ToString() == strGuid)
					return objNode;
				else
				{
					objFound = FindNode(strGuid, objNode);
					if (objFound != null)
						return objFound;
				}
			}
			return null;
		}

		/// <summary>
		/// Find a TreeNode in a TreeNode based on its Tag.
		/// </summary>
		/// <param name="strGuid">InternalId of the Node to find.</param>
		/// <param name="objNode">TreeNode to search.</param>
		public TreeNode FindNode(string strGuid, TreeNode objNode)
		{
			TreeNode objFound = new TreeNode();
			foreach (TreeNode objChild in objNode.Nodes)
			{
				if (objChild.Tag.ToString() == strGuid)
					return objChild;
				else
				{
					objFound = FindNode(strGuid, objChild);
					if (objFound != null)
						return objFound;
				}
			}
			return null;
		}

		/// <summary>
		/// Find all of the Commlinks carried by the character.
		/// </summary>
		/// <param name="lstGear">List of Gear to search within for Commlinks.</param>
		public List<Commlink> FindCharacterCommlinks(List<Equipment> lstGear)
		{
			List<Commlink> lstReturn = new List<Commlink>();
			foreach (Gear objGear in lstGear)
			{
				if (objGear.GetType() == typeof(Commlink))
					lstReturn.Add((Commlink)objGear);

				if (objGear.Gears.Count > 0)
				{
					// Retrieve the list of Commlinks in child items.
					List<Commlink> lstAppend = FindCharacterCommlinks(objGear.Gears);
					if (lstAppend.Count > 0)
					{
						// Append the entries to the current list.
						foreach (Commlink objCommlink in lstAppend)
							lstReturn.Add(objCommlink);
					}
				}
			}

			return lstReturn;
		}

        /// <summary>
        /// Get the Display Name for an object based on its GUID.
        /// </summary>
        /// <param name="strGuid">GUID of the object to find the name for.</param>
        public string GetObjectName(string strGuid)
        {
            Quality objQuality = FindQuality(strGuid, _objCharacter.Qualities);
            if (objQuality != null)
                return objQuality.DisplayName;

            Equipment objEquipment = FindEquipment(strGuid, _objCharacter.Armor, null);
            if (objEquipment != null)
                return objEquipment.DisplayName;

            objEquipment = FindEquipment(strGuid, _objCharacter.Cyberware, null);
            if (objEquipment != null)
                return objEquipment.DisplayName;

            objEquipment = FindEquipment(strGuid, _objCharacter.Gear, null);
            if (objEquipment != null)
                return objEquipment.DisplayName;

            objEquipment = FindEquipment(strGuid, _objCharacter.Weapons, null);
            if (objEquipment != null)
                return objEquipment.DisplayName;

            objEquipment = FindEquipment(strGuid, _objCharacter.Vehicles, null);
            if (objEquipment != null)
                return objEquipment.DisplayName;

            return string.Empty;
        }
		#endregion

		#region Delete Functions
		/// <summary>
		/// Recursive method to delete a piece of Gear and its Improvements from the character.
		/// </summary>
		/// <param name="objGear">Gear to delete.</param>
		/// <param name="treWeapons">TreeView that holds the list of Weapons.</param>
		/// <param name="objImprovementManager">Improvement Manager the character is using.</param>
		public void DeleteGear(Gear objGear, TreeView treWeapons, ImprovementManager objImprovementManager)
		{
			// Remove any children the Gear may have.
			foreach (Gear objChild in objGear.Gears)
				DeleteGear(objChild, treWeapons, objImprovementManager);

			// Remove the Gear Weapon created by the Gear if applicable.
			if (objGear.WeaponID != Guid.Empty.ToString())
			{
				// Remove the Weapon from the TreeView.
				TreeNode objRemoveNode = new TreeNode();
				foreach (TreeNode objWeaponNode in treWeapons.Nodes[0].Nodes)
				{
					if (objWeaponNode.Tag.ToString() == objGear.WeaponID)
						objRemoveNode = objWeaponNode;
				}
				treWeapons.Nodes.Remove(objRemoveNode);

				// Remove the Weapon from the Character.
				Weapon objRemoveWeapon = new Weapon(_objCharacter);
				foreach (Weapon objWeapon in _objCharacter.Weapons)
				{
					if (objWeapon.InternalId == objGear.WeaponID)
						objRemoveWeapon = objWeapon;
				}
				_objCharacter.Weapons.Remove(objRemoveWeapon);
			}

			objImprovementManager.RemoveImprovements(Improvement.ImprovementSource.Gear, objGear.InternalId);

			// If a Focus is being removed, make sure the actual Focus is being removed from the character as well.
			if (objGear.Category == "Foci" || objGear.Category == "Metamagic Foci")
			{
				List<Focus> lstRemoveFoci = new List<Focus>();
				foreach (Focus objFocus in _objCharacter.Foci)
				{
					if (objFocus.GearId == objGear.InternalId)
						lstRemoveFoci.Add(objFocus);
				}
				foreach (Focus objFocus in lstRemoveFoci)
					_objCharacter.Foci.Remove(objFocus);
			}

			// If a Stacked Focus is being removed, make sure the Stacked Foci and its bonuses are being removed.
			if (objGear.Category == "Stacked Focus")
			{
				foreach (StackedFocus objStack in _objCharacter.StackedFoci)
				{
					if (objStack.GearId == objGear.InternalId)
					{
						objImprovementManager.RemoveImprovements(Improvement.ImprovementSource.StackedFocus, objStack.InternalId);
						_objCharacter.StackedFoci.Remove(objStack);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Recursive method to delete a piece of Gear and from a Vehicle.
		/// </summary>
		/// <param name="objGear">Gear to delete.</param>
		/// <param name="treVehicles">TreeView that holds the list of Vehicles.</param>
		/// <param name="objVehicle">Vehicle to remove items from.</param>
		public void DeleteVehicleGear(Gear objGear, TreeView treVehicles, Vehicle objVehicle)
		{
			// Remove any children the Gear may have.
			foreach (Gear objChild in objGear.Gears)
				DeleteVehicleGear(objChild, treVehicles, objVehicle);

			// Remove the Gear Weapon created by the Gear if applicable.
			if (objGear.WeaponID != Guid.Empty.ToString())
			{
				// Remove the Weapon from the TreeView.
				TreeNode objRemoveNode = new TreeNode();
				foreach (TreeNode objVehicleNode in treVehicles.Nodes[0].Nodes)
				{
					foreach (TreeNode objWeaponNode in objVehicleNode.Nodes)
					{
						if (objWeaponNode.Tag.ToString() == objGear.WeaponID)
							objRemoveNode = objWeaponNode;
					}
					objVehicleNode.Nodes.Remove(objRemoveNode);
				}
				
				// Remove the Weapon from the Vehicle.
				Weapon objRemoveWeapon = new Weapon(_objCharacter);
				foreach (Weapon objWeapon in objVehicle.Weapons)
				{
					if (objWeapon.InternalId == objGear.WeaponID)
						objRemoveWeapon = objWeapon;
				}
				objVehicle.Weapons.Remove(objRemoveWeapon);
			}
		}
		#endregion

		#region Tree Functions
		/// <summary>
		/// Clear the background colour for all TreeNodes except the one currently being hovered over during a drag-and-drop operation.
		/// </summary>
		/// <param name="treTree">TreeView to check.</param>
		/// <param name="objHighlighted">TreeNode that is currently being hovered over.</param>
		public void ClearNodeBackground(TreeView treTree, TreeNode objHighlighted)
		{
			foreach (TreeNode objNode in treTree.Nodes)
			{
				if (objNode != objHighlighted)
					objNode.BackColor = SystemColors.Window;
				ClearNodeBackground(objNode, objHighlighted);
			}
		}

		/// <summary>
		/// Recursive method to clear the background colour for all TreeNodes except the one currently being hovered over during a drag-and-drop operation.
		/// </summary>
		/// <param name="objNode">Parent TreeNode to check.</param>
		/// <param name="objHighlighted">TreeNode that is currently being hovered over.</param>
		private void ClearNodeBackground(TreeNode objNode, TreeNode objHighlighted)
		{
			foreach (TreeNode objChild in objNode.Nodes)
			{
				if (objChild != objHighlighted)
					objChild.BackColor = SystemColors.Window;
				if (objChild.Nodes.Count > 0)
					ClearNodeBackground(objChild, objHighlighted);
			}
		}

		public void BuildEquipmentTree(List<Equipment> lstEquipment, TreeNode objNode, Equipment objSetParent)
		{
			// ***** NEED TO SOMEHOW TAKE LOCATIONS INTO CONSIDERATION *****
			foreach (Equipment objEquipment in lstEquipment)
			{
				TreeNode objChildNode = new TreeNode();
				objChildNode.Text = objEquipment.DisplayName;
				objChildNode.Tag = objEquipment.InternalId;

				// Determine the ContextMenuStrip to use base on the Equipment type.
				ContextMenuStrip objMenu = new ContextMenuStrip();
				if (objEquipment.GetType() == typeof(Armor))
					objMenu = _cmsArmor;
				if (objEquipment.GetType() == typeof(ArmorMod))
					objMenu = _cmsArmorMod;
				if (objEquipment.GetType() == typeof(Cyberware))
					objMenu = _cmsCyberware;
				if (objEquipment.GetType() == typeof(Gear))
					objMenu = _cmsGear;
				if (objEquipment.GetType() == typeof(Vehicle))
					objMenu = _cmsVehicle;
				if (objEquipment.GetType() == typeof(VehicleMod))
					objMenu = _cmsVehicleMod;
				if (objEquipment.GetType() == typeof(Weapon))
					objMenu = _cmsWeapon;
				if (objEquipment.GetType() == typeof(WeaponAccessory))
					objMenu = _cmsWeaponAccessory;
				if (objEquipment.GetType() == typeof(WeaponMod))
					objMenu = _cmsWeaponMod;

				if (objEquipment.Notes != string.Empty)
					objChildNode.ForeColor = Color.SaddleBrown;
				objChildNode.ToolTipText = objEquipment.Notes;

				objEquipment.Parent = objSetParent;

				objNode.Nodes.Add(objChildNode);

				BuildEquipmentTree(objEquipment.Armors, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.ArmorMods, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.Cyberwares, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.Gears, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.Vehicles, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.VehicleMods, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.Weapons, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.WeaponAccessories, objChildNode, objEquipment);
				BuildEquipmentTree(objEquipment.WeaponMods, objChildNode, objEquipment);

				objChildNode.Expand();
			}
		}

		/// <summary>
		/// Build up the Tree for the current piece of Gear and all of its children.
		/// </summary>
		/// <param name="objGear">Gear to iterate through.</param>
		/// <param name="objNode">TreeNode to append to.</param>
		/// <param name="objMenu">ContextMenuStrip that the new TreeNodes should use.</param>
		public void BuildGearTree(Gear objGear, TreeNode objNode, ContextMenuStrip objMenu)
		{
			foreach (Gear objChild in objGear.Gears)
			{
				TreeNode objChildNode = new TreeNode();
				objChildNode.Text = objChild.DisplayName;
				objChildNode.Tag = objChild.InternalId;
				objChildNode.ContextMenuStrip = objMenu;
				if (objChild.Notes != string.Empty)
					objChildNode.ForeColor = Color.SaddleBrown;
				objChildNode.ToolTipText = objChild.Notes;

				objNode.Nodes.Add(objChildNode);
				objNode.Expand();

				// Set the Gear's Parent.
				objChild.Parent = objGear;

				BuildGearTree(objChild, objChildNode, objMenu);
			}
		}

		/// <summary>
		/// Build up the Tree for the current piece of Cyberware and all of its children.
		/// </summary>
		/// <param name="objCyberware">Cyberware to iterate through.</param>
		/// <param name="objParentNode">TreeNode to append to.</param>
		/// <param name="objMenu">ContextMenuStrip that the new Cyberware TreeNodes should use.</param>
		/// <param name="objGearMenu">ContextMenuStrip that the new Gear TreeNodes should use.</param>
		public void BuildCyberwareTree(Cyberware objCyberware, TreeNode objParentNode, ContextMenuStrip objMenu, ContextMenuStrip objGearMenu)
		{
				TreeNode objNode = new TreeNode();
				objNode.Text = objCyberware.DisplayName;
				objNode.Tag = objCyberware.InternalId;
				if (objCyberware.Notes != string.Empty)
					objNode.ForeColor = Color.SaddleBrown;
				objNode.ToolTipText = objCyberware.Notes;
				objNode.ContextMenuStrip = objMenu;

				objParentNode.Nodes.Add(objNode);
				objParentNode.Expand();

				foreach (Cyberware objChild in objCyberware.Cyberwares)
					BuildCyberwareTree(objChild, objNode, objMenu, objGearMenu);

				foreach (Gear objGear in objCyberware.Gears)
				{
					TreeNode objGearNode = new TreeNode();
					objGearNode.Text = objGear.DisplayName;
					objGearNode.Tag = objGear.InternalId;
					if (objGear.Notes != string.Empty)
						objGearNode.ForeColor = Color.SaddleBrown;
					objGearNode.ToolTipText = objGear.Notes;
					objGearNode.ContextMenuStrip = objGearMenu;

					BuildGearTree(objGear, objGearNode, objGearMenu);

					objNode.Nodes.Add(objGearNode);
					objNode.Expand();
				}

		}

		/// <summary>
		/// Sort the contents of a TreeView alphabetically within each group Node.
		/// </summary>
		/// <param name="treTree">TreeView to sort.</param>
		public void SortTree(TreeView treTree)
		{
			for (int i = 0; i <= treTree.Nodes.Count - 1; i++)
			{
				List<TreeNode> lstNodes = new List<TreeNode>();
				foreach (TreeNode objNode in treTree.Nodes[i].Nodes)
					lstNodes.Add(objNode);
				treTree.Nodes[i].Nodes.Clear();
				try
				{
					SortByName objSort = new SortByName();
					lstNodes.Sort(objSort.Compare);
				}
				catch
				{
				}

				foreach (TreeNode objNode in lstNodes)
					treTree.Nodes[i].Nodes.Add(objNode);

				treTree.Nodes[i].Expand();
			}
		}
		#endregion

		#region TreeNode Creation Methods
		/// <summary>
		/// Add a piece of Armor to the Armor TreeView.
		/// </summary>
		/// <param name="objArmor">Armor to add.</param>
		/// <param name="treArmor">Armor TreeView.</param>
		/// <param name="cmsArmor">ContextMenuStrip for the Armor Node.</param>
		/// <param name="cmsArmorMod">ContextMenuStrip for Armor Mod Nodes.</param>
		/// <param name="cmsArmorGear">ContextMenuStrip for Armor Gear Nodes.</param>
		public void CreateArmorTreeNode(Armor objArmor, TreeView treArmor, ContextMenuStrip cmsArmor, ContextMenuStrip cmsArmorMod, ContextMenuStrip cmsArmorGear)
		{
			TreeNode objNode = new TreeNode();
			objNode.Text = objArmor.DisplayName;
			objNode.Tag = objArmor.InternalId;
			if (objArmor.Notes != string.Empty)
				objNode.ForeColor = Color.SaddleBrown;
			objNode.ToolTipText = objArmor.Notes;

			foreach (ArmorMod objMod in objArmor.ArmorMods)
			{
				TreeNode objChild = new TreeNode();
				objChild.Text = objMod.DisplayName;
				objChild.Tag = objMod.InternalId;
				objChild.ContextMenuStrip = cmsArmorMod;
				if (objMod.Notes != string.Empty)
					objChild.ForeColor = Color.SaddleBrown;
				objChild.ToolTipText = objMod.Notes;
				objNode.Nodes.Add(objChild);
				objNode.Expand();
			}

			foreach (Gear objGear in objArmor.Gears)
			{
				TreeNode objChild = new TreeNode();
				objChild.Text = objGear.DisplayName;
				objChild.Tag = objGear.InternalId;
				if (objGear.Notes != string.Empty)
					objChild.ForeColor = Color.SaddleBrown;
				objChild.ToolTipText = objGear.Notes;

				BuildGearTree(objGear, objChild, cmsArmorGear);

				objChild.ContextMenuStrip = cmsArmorGear;
				objNode.Nodes.Add(objChild);
				objNode.Expand();
			}

			TreeNode objParent = new TreeNode();
			if (objArmor.Location == "")
				objParent = treArmor.Nodes[0];
			else
			{
				foreach (TreeNode objFind in treArmor.Nodes)
				{
					if (objFind.Text == objArmor.Location)
					{
						objParent = objFind;
						break;
					}
				}
			}

			objNode.ContextMenuStrip = cmsArmor;
			objParent.Nodes.Add(objNode);
			objParent.Expand();
		}

		/// <summary>
		/// Add a Vehicle to the TreeView.
		/// </summary>
		/// <param name="objVehicle">Vehicle to add.</param>
		/// <param name="treVehicles">Vehicle TreeView.</param>
		/// <param name="cmsVehicle">ContextMenuStrip for the Vehicle Node.</param>
		/// <param name="cmsVehicleLocation">ContextMenuStrip for Vehicle Location Nodes.</param>
		/// <param name="cmsVehicleWeapon">ContextMenuStrip for Vehicle Weapon Nodes.</param>
		/// <param name="cmsWeaponMod">ContextMenuStrip for Vehicle Weapon Mod Nodes.</param>
		/// <param name="cmsWeaponAccessory">ContextMenuStrip for Vehicle Weapon Accessory Nodes.</param>
		/// <param name="cmsVehicleGear">ContextMenuStrip for Vehicle Gear Nodes.</param>
		public void CreateVehicleTreeNode(Vehicle objVehicle, TreeView treVehicles, ContextMenuStrip cmsVehicle, ContextMenuStrip cmsVehicleLocation, ContextMenuStrip cmsVehicleWeapon, ContextMenuStrip cmsWeaponMod, ContextMenuStrip cmsWeaponAccessory, ContextMenuStrip cmsWeaponAccessoryGear, ContextMenuStrip cmsVehicleGear)
		{
			TreeNode objNode = new TreeNode();
			objNode.Text = objVehicle.DisplayName;
			objNode.Tag = objVehicle.InternalId;
			if (objVehicle.Notes != string.Empty)
				objNode.ForeColor = Color.SaddleBrown;
			objNode.ToolTipText = objVehicle.Notes;

			// Populate the list of Vehicle Locations.
			foreach (string strLocation in objVehicle.Locations)
			{
				TreeNode objLocation = new TreeNode();
				objLocation.Tag = strLocation;
				objLocation.Text = strLocation;
				objLocation.ContextMenuStrip = cmsVehicleLocation;
				objNode.Nodes.Add(objLocation);
			}

			// VehicleMods.
			foreach (VehicleMod objMod in objVehicle.VehicleMods)
			{
				TreeNode objChildNode = new TreeNode();
				objChildNode.Text = objMod.DisplayName;
				objChildNode.Tag = objMod.InternalId;
				if (objMod.IncludedInParent)
					objChildNode.ForeColor = SystemColors.GrayText;
				if (objMod.Notes != string.Empty)
					objChildNode.ForeColor = Color.SaddleBrown;
				objChildNode.ToolTipText = objMod.Notes;

				// Cyberware.
				foreach (Cyberware objCyberware in objMod.Cyberwares)
				{
					TreeNode objCyberwareNode = new TreeNode();
					objCyberwareNode.Text = objCyberware.DisplayName;
					objCyberwareNode.Tag = objCyberware.InternalId;
					if (objCyberware.Notes != string.Empty)
						objCyberwareNode.ForeColor = Color.SaddleBrown;
					objCyberwareNode.ToolTipText = objCyberware.Notes;
					objChildNode.Nodes.Add(objCyberwareNode);
					objChildNode.Expand();
				}

				// VehicleWeapons.
				foreach (Weapon objWeapon in objMod.Weapons)
					CreateWeaponTreeNode(objWeapon, objChildNode, cmsVehicleWeapon, cmsWeaponMod, cmsWeaponAccessory, cmsWeaponAccessoryGear);

				// Attach the ContextMenuStrip.
				objChildNode.ContextMenuStrip = cmsVehicle;

				objNode.Nodes.Add(objChildNode);
				objNode.Expand();
			}

			// Vehicle Weapons (not attached to a mount).
			foreach (Weapon objWeapon in objVehicle.Weapons)
				CreateWeaponTreeNode(objWeapon, objNode, cmsVehicleWeapon, cmsWeaponMod, cmsWeaponAccessory, cmsWeaponAccessoryGear);

			// Vehicle Gear.
			foreach (Gear objGear in objVehicle.Gears)
			{
				TreeNode objGearNode = new TreeNode();
				objGearNode.Text = objGear.DisplayName;
				objGearNode.Tag = objGear.InternalId;
				if (objGear.Notes != string.Empty)
					objGearNode.ForeColor = Color.SaddleBrown;
				objGearNode.ToolTipText = objGear.Notes;

				BuildGearTree(objGear, objGearNode, cmsVehicleGear);

				objGearNode.ContextMenuStrip = cmsVehicleGear;

				TreeNode objParent = new TreeNode();
				if (objGear.Location == "")
					objParent = objNode;
				else
				{
					foreach (TreeNode objFind in objNode.Nodes)
					{
						if (objFind.Text == objGear.Location)
						{
							objParent = objFind;
							break;
						}
					}
				}

				objParent.Nodes.Add(objGearNode);
				objParent.Expand();
			}

			objNode.ContextMenuStrip = cmsVehicle;
			treVehicles.Nodes[0].Nodes.Add(objNode);
			treVehicles.Nodes[0].Expand();
		}

		/// <summary>
		/// Add a Weapon to the TreeView.
		/// </summary>
		/// <param name="objWeapon">Weapon to add.</param>
		/// <param name="objWeaponsNode">Node to append the Weapon Node to.</param>
		/// <param name="cmsWeapon">ContextMenuStrip for the Weapon Node.</param>
		/// <param name="cmsWeaponMod">ContextMenuStrip for Weapon Mod Nodes.</param>
		/// <param name="cmsWeaponAccessory">ContextMenuStrip for Vehicle Accessory Nodes.</param>
		/// <param name="cmsWeaponAccessoryGear">ContextMenuStrip for Vehicle Weapon Accessory Gear Nodes.</param>
		public void CreateWeaponTreeNode(Weapon objWeapon, TreeNode objWeaponsNode, ContextMenuStrip cmsWeapon, ContextMenuStrip cmsWeaponMod, ContextMenuStrip cmsWeaponAccessory, ContextMenuStrip cmsWeaponAccessoryGear)
		{
			TreeNode objNode = new TreeNode();
			objNode.Text = objWeapon.DisplayName;
			objNode.Tag = objWeapon.InternalId;
			if (objWeapon.Category.StartsWith("Cyberware") || objWeapon.Category == "Gear" || objWeapon.Category.StartsWith("Quality"))
				objNode.ForeColor = SystemColors.GrayText;
			if (objWeapon.Notes != string.Empty)
				objNode.ForeColor = Color.SaddleBrown;
			objNode.ToolTipText = objWeapon.Notes;

			// Add attached Weapon Accessories.
			foreach (WeaponAccessory objAccessory in objWeapon.WeaponAccessories)
			{
				TreeNode objChild = new TreeNode();
				objChild.Text = objAccessory.DisplayName;
				objChild.Tag = objAccessory.InternalId;
				objChild.ContextMenuStrip = cmsWeaponAccessory;
				if (objAccessory.Notes != string.Empty)
					objChild.ForeColor = Color.SaddleBrown;
				objChild.ToolTipText = objAccessory.Notes;

				// Add any Gear attached to the Weapon Accessory.
				foreach (Gear objGear in objAccessory.Gears)
				{
					TreeNode objGearChild = new TreeNode();
					objGearChild.Text = objGear.DisplayName;
					objGearChild.Tag = objGear.InternalId;
					if (objGear.Notes != string.Empty)
						objGearChild.ForeColor = Color.SaddleBrown;
					objGearChild.ToolTipText = objGear.Notes;

					BuildGearTree(objGear, objGearChild, cmsWeaponAccessoryGear);

					objGearChild.ContextMenuStrip = cmsWeaponAccessoryGear;
					objChild.Nodes.Add(objGearChild);
					objChild.Expand();
				}

				objNode.Nodes.Add(objChild);
				objNode.Expand();
			}

			// Add Attached Weapon Modifications.
			foreach (WeaponMod objMod in objWeapon.WeaponMods)
			{
				TreeNode objChild = new TreeNode();
				objChild.Text = objMod.DisplayName;
				objChild.Tag = objMod.InternalId;
				objChild.ContextMenuStrip = cmsWeaponMod;
				if (objMod.Notes != string.Empty)
					objChild.ForeColor = Color.SaddleBrown;
				objChild.ToolTipText = objMod.Notes;
				objNode.Nodes.Add(objChild);
				objNode.Expand();
			}

			// Add Underbarrel Weapons.
			if (objWeapon.Weapons.Count > 0)
			{
				foreach (Weapon objUnderbarrelWeapon in objWeapon.Weapons)
					CreateWeaponTreeNode(objUnderbarrelWeapon, objNode, cmsWeapon, cmsWeaponMod, cmsWeaponAccessory, cmsWeaponAccessoryGear);
			}

			// If this is not an Underbarrel Weapon and it has a Location, find the Location Node that this should be attached to instead.
			if (!objWeapon.IsUnderbarrelWeapon && objWeapon.Location != string.Empty)
			{
				foreach (TreeNode objLocationNode in objWeaponsNode.TreeView.Nodes)
				{
					if (objLocationNode.Text == objWeapon.Location)
					{
						objWeaponsNode = objLocationNode;
						break;
					}
				}
			}

			objNode.ContextMenuStrip = cmsWeapon;
			objWeaponsNode.Nodes.Add(objNode);
			objWeaponsNode.Expand();
		}
		#endregion

		#region PDF Functions
		/// <summary>
		/// Open a PDF file using the provided source information.
		/// </summary>
		/// <param name="strSource">Book coode and page number to open.</param>
		public void OpenPDF(string strSource)
		{
			// The user must have specified the path of their PDF application in order to use this functionality.
			if (GlobalOptions.Instance.PDFAppPath == string.Empty)
				return;

			string[] strTemp = strSource.Split(' ');
			string strBook = "";
			string strPage = "";
			string strPath = "";
			int intPage = 0;

			try
			{
				strBook = strTemp[0];
				strPage = strTemp[1];

				// Make sure the page is actually a number that we can use as well as being 1 or higher.
				if (Convert.ToInt32(strPage) < 1)
					return;
				intPage = Convert.ToInt32(strPage);
			}
			catch
			{
				return;
			}

			// Revert the sourcebook code to the one from the XML file if necessary.
			if (_objCharacter != null)
				strBook = _objCharacter.Options.BookFromAltCode(strBook);

			// Retrieve the sourcebook information including page offset and PDF application name.
			bool blnFound = false;
			foreach (SourcebookInfo objInfo in GlobalOptions.Instance.SourcebookInfo)
			{
				if (objInfo.Code == strBook)
				{
					if (objInfo.Path != string.Empty)
					{
						blnFound = true;
						strPath = objInfo.Path;
						intPage += objInfo.Offset;
					}
				}
			}

			// If the sourcebook was not found, we can't open anything.
			if (!blnFound)
				return;

			// Open the PDF.
			// acrord32 /A "page=123" "D:\foo\bar.pdf"
			//string strFilePath = "C:\\Gaming\\Shadowrun\\Books\\Shadowrun 4th ed Anniverary.pdf";
			string strParams = " /n /A \"page=" + intPage.ToString() +"\" \"" + strPath + "\"";
			Process.Start(GlobalOptions.Instance.PDFAppPath, strParams);
		}
		#endregion

		/// <summary>
		/// Verify that the user wants to delete an item.
		/// </summary>
		public bool ConfirmDelete(string strMessage)
		{
			if (!_objCharacter.Options.ConfirmDelete)
				return true;
			else
			{
				if (MessageBox.Show(strMessage, LanguageManager.Instance.GetString("MessageTitle_Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return false;
				else
					return true;
			}
		}
	}
}