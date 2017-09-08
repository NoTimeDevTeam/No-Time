using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionInteractionWith : InstructionData {

	public Entity partner;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionInteractionWith"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">TriggerTime</param>
	/// <param name="p">Partner Entity</param>
	public InstructionInteractionWith (int TId, float t,Entity p) : base (TId, t)
	{
		partner = p;
	}
}
