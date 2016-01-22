using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	public MonsterEvent master;
	public float fearDist;
    public int life;
    public Vector2 minMaxTimeIfShot;
    public Vector2 minMaxTimeIfFeared;

    public GameObject disapearEffect;

    private int currentLife;

    // Use this for initialization
    void Start () {
		afraidSpirits(true);
        currentLife = life;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

        transform.LookAt(GameMaster.GM.dragon.transform);

		float distFromPlayer = Vector3.Distance(transform.position, GameMaster.GM.dragon.transform.position);
        if (currentLife <= 0)
        {
            afraidSpirits(false);
            Destroy(Instantiate(disapearEffect, transform.position, transform.rotation), 5.0f);

            master.End(minMaxTimeIfShot.x, minMaxTimeIfShot.y);
        }
        else if (distFromPlayer < fearDist)
		{
			afraidSpirits(false);
            Destroy(Instantiate(disapearEffect, transform.position, transform.rotation), 5.0f);

            master.End(minMaxTimeIfFeared.x, minMaxTimeIfFeared.y);
		}

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            currentLife--;

            //Destroy(other.gameObject);
            ((Projectile)other.GetComponent<Projectile>()).DestroyWithEffect();
        }
    }

        void afraidSpirits( bool _afraid)
	{
		for (int i = 0; i < GameMaster.GM.spiritCount; ++i)
		{
			Spirit spirit = ((GameObject) GameMaster.GM.spirits[i]).GetComponent<Spirit>() as Spirit;
			
			if (spirit.collected)
				continue;

			if (_afraid)
				spirit.setPosBeforeAfraid();
			spirit.afraid = _afraid;
		}
	}
}
