using UnityEngine;
using System.Collections;

public class SizeBonus : Bonus {
	public int sizeToAdd;

	protected override void applyBonus(GameObject _player)
	{
		//_player.GetComponent<SnakeAI>().addBodyPart(sizeToAdd);
	}
}
