using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionLookOut : InstructionData {


	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionLookOut"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">TriggerTime</param>
	public InstructionLookOut (int TId, float t) : base (TId, t)
	{
		//There is currently not a definete idea in how to use this
		//with further implimentation of features like awareness levels etc. 
		//there will be a usefull aplication then
		//e.g.: A guard beeing cautious about a sertan are. This can maybe tie into
		//the look around instruction
		//TODO: Impliment the functionality completly
	}
}
