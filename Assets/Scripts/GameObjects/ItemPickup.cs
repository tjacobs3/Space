using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemPickup : vp_Interactable
{

	/// <summary>
	/// try to interact with this object
	/// </summary>
	public override bool TryInteract(vp_PlayerEventHandler player)
	{
		TryGiveTo (player);
		return true;
	}

	#if UNITY_EDITOR
	[vp_ItemID]
	#endif
	public int ID;
	
	#if UNITY_EDITOR
	[vp_ItemAmount]
	#endif
	
	public int Amount;

	protected Type m_ItemType = null;
	protected Type ItemType
	{
		get
		{
			#if UNITY_EDITOR
			if (m_Item.Type == null)
			{
				Debug.LogWarning(string.Format(MissingItemTypeError, this), gameObject);
				return null;
			}
			return m_Item.Type.GetType();
			#else
			if (m_ItemType == null)
				m_ItemType = m_Item.Type.GetType();
			return m_ItemType;
			#endif
		}
	}

	protected vp_ItemType m_ItemTypeObject = null;
	public vp_ItemType ItemTypeObject
	{
		get
		{
			#if UNITY_EDITOR
			if (m_Item.Type == null)
			{
				Debug.LogWarning(string.Format(MissingItemTypeError, this), gameObject);
				return null;
			}
			return m_Item.Type;
			#else
			if (m_ItemTypeObject == null)
				m_ItemTypeObject = m_Item.Type;
			return m_ItemTypeObject;
			#endif
		}
	}
//	
//	protected AudioSource m_Audio = null;
//	protected AudioSource Audio
//	{
//		get
//		{
//			if (m_Audio == null)
//			{
//				if (GetComponent<AudioSource>() == null)
//					gameObject.AddComponent<AudioSource>();
//				m_Audio = GetComponent<AudioSource>();
//			}
//			return m_Audio;
//		}
//	}
//	
//
//
//	
//	
//	//////////////// 'Item' section ////////////////
	[System.Serializable]
	public class ItemSection
	{
		
		public vp_ItemType Type = null;
		
		#if UNITY_EDITOR
		[vp_HelpBox(typeof(ItemSection), UnityEditor.MessageType.None, typeof(vp_ItemPickup), null, true)]
		public float helpbox;
		#endif
		
	}
	[SerializeField]
	protected ItemSection m_Item;
	
	//////////////// 'Sounds' section ////////////////
	[System.Serializable]
	public class SoundSection
	{
		public AudioClip PickupSound = null;		// player triggers the pickup
		public bool PickupSoundSlomo = true;
		public AudioClip PickupFailSound = null;	// player failed to pick up the item (i.e. ammo full)
		public bool FailSoundSlomo = true;
	}
	[SerializeField]
	protected SoundSection m_Sound;


//	//////////////// 'Messages' section ////////////////
	[System.Serializable]
	public class MessageSection
	{
		public string SuccessSingle = "Picked up {2}.";
		public string SuccessMultiple = "Picked up {4} {1}s.";
		public string FailSingle = "Can't pick up {2} right now.";
		public string FailMultiple = "Can't pick up {4} {1}s right now.";
		#if UNITY_EDITOR
		[vp_HelpBox(typeof(MessageSection), UnityEditor.MessageType.None, typeof(vp_ItemPickup), null, true)]
		public float helpbox;
		#endif
	}
	[SerializeField]
	protected MessageSection m_Messages;
	
	// when this is true, the pickup has been triggered and will
	// disappear as soon as the pickup sound has finished playing
	protected bool m_Depleted = false;
	protected int m_PickedUpAmount;
	
	protected string MissingItemTypeError = "Warning: {0} has no ItemType object!";

	

	public void TryGiveTo(vp_PlayerEventHandler player)
	{	
		bool result = false;
		
		//int prevAmount = vp_TargetEventReturn<vp_ItemType, int>.SendUpwards(col, "GetItemCount", m_Item.Type);
		
		if (ItemType == typeof(vp_ItemType))
			result = vp_TargetEventReturn<vp_ItemType, int, bool>.SendUpwards(player, "TryGiveItem", m_Item.Type, ID);
		else if (ItemType == typeof(vp_UnitBankType))
			result = vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.SendUpwards(player, "TryGiveUnitBank", (m_Item.Type as vp_UnitBankType), Amount, ID);
		else if (ItemType == typeof(vp_UnitType))
			result = vp_TargetEventReturn<vp_UnitType, int, bool>.SendUpwards(player, "TryGiveUnits", (m_Item.Type as vp_UnitType), Amount);
		else if (ItemType.BaseType == typeof(vp_ItemType))
			result = vp_TargetEventReturn<vp_ItemType, int, bool>.SendUpwards(player, "TryGiveItem", m_Item.Type, ID);
		else if (ItemType.BaseType == typeof(vp_UnitBankType))
			result = vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.SendUpwards(player, "TryGiveUnitBank", (m_Item.Type as vp_UnitBankType), Amount, ID);
		else if (ItemType.BaseType == typeof(vp_UnitType))
			result = vp_TargetEventReturn<vp_UnitType, int, bool>.SendUpwards(player, "TryGiveUnits", (m_Item.Type as vp_UnitType), Amount);
		
		if (result == true)
		{
			//m_PickedUpAmount = (vp_TargetEventReturn<vp_ItemType, int>.SendUpwards(col, "GetItemCount", m_Item.Type) - prevAmount);	// calculate resulting amount given
			OnSuccess();
		}
		
	}

	void OnSuccess() {
		GameObject.Destroy (this.gameObject);
	}
	
}

