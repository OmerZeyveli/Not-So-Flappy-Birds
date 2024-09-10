using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PopulationManager : MonoBehaviour {

	[SerializeField] GameObject botPrefab;
	[SerializeField] Transform startingPos;
	[SerializeField] int populationSize = 50;
	List<GameObject> population = new List<GameObject>();

	float elapsed = 0;
	[SerializeField] float trialTime = 5;
	int generation = 1;

	// UI stuff.
	[SerializeField] TMP_Text text_time;
	[SerializeField] TMP_Text text_generation;

	public float GetElapsed()
	{
		return elapsed;
	}


	// Use this for initialization
	void Start () {
		for(int i = 0; i < populationSize; i++)
		{
			GameObject b = Instantiate(botPrefab, startingPos.position, this.transform.rotation);
			b.GetComponent<Brain>().Init();
			population.Add(b);
		}

		Time.timeScale = 5;
	}

	// Breed new generation from upper half of bots.
	void BreedNewPopulation()
	{
		// Sort population by (how far bot runned - how many times bot crashed).
		List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().GetDistanceTraveled() 
															- o.GetComponent<Brain>().GetCrash()).ToList();

		population.Clear();

		// Populate upper quarter of sorted list.
		for (int i = (int) (3*sortedList.Count / 4.0f) - 1; i < sortedList.Count - 1; i++)
		{
    		population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		population.Add(Breed(sortedList[i + 1], sortedList[i]));
    		population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		population.Add(Breed(sortedList[i + 1], sortedList[i]));
		}

		// Destroy all parents and previous population
		for(int i = 0; i < sortedList.Count; i++)
		{
			Destroy(sortedList[i]);
		}
		generation++;
	}

	// Combine brains of 2 parents in a new bot.
	GameObject Breed(GameObject parent1, GameObject parent2)
	{
		// Instantiate offspring bot and get all 3 brains (parent1, parent2 and offspring).
		GameObject offspring = Instantiate(botPrefab, startingPos.position, this.transform.rotation);
		Brain b = offspring.GetComponent<Brain>();
		if(Random.Range(0,100) == 1) //mutate 1 in 100
		{
			b.Init();
			b.GetDna().Mutate();
		}
		else
		{ 
			b.Init();
			b.GetDna().Combine(parent1.GetComponent<Brain>().GetDna(),parent2.GetComponent<Brain>().GetDna());
		}
		return offspring;
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if(elapsed >= trialTime)
		{
			// New generation once in every trialTime.
			BreedNewPopulation();
			elapsed = 0;
		}

		// Update UI.
        text_time.SetText("Time: " + elapsed);
        text_generation.SetText("Generation: " + generation);
	}
}
