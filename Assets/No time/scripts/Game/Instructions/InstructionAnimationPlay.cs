using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionAnimationPlay : InstructionData {

	public string name;
	public Animation animation;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionAnimationPlay"/> class.
	/// </summary>
	/// <param name="TId">Target Entity identifier.</param>
	/// <param name="t">TriggerTime</param>
	/// <param name="n">Animation Name</param>
	public InstructionAnimationPlay (int TId, float t,string n) : base (TId, t)
	{
		name = n;
	}
}
