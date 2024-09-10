using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{

	List<int> genes = new List<int>();
	int dnaLength = 0;
	int maxValues = 0;


	// Constructor
	public DNA(int l, int v)
	{
		dnaLength = l;
		maxValues = v;
		SetRandom();
	}

	public int GetGene(int pos)
	{
		return genes[pos];
	}

	// Change spesific gene of dna.
	public void SetGene(int pos, int value)
	{
		genes[pos] = value;
	}


	// Randomize genes.
	public void SetRandom()
	{
		genes.Clear();
		for (int i = 0; i < dnaLength; i++)
		{
			genes.Add(Random.Range(-maxValues, maxValues));
		}
	}

	// Give Parent genes to offspring half by half
	public void Combine(DNA d1, DNA d2)
	{
		for (int i = 0; i < dnaLength; i++)
		{
			genes[i] = Random.Range(0, 10) < 5 ? d1.genes[i] : d2.genes[i];
		}
	}

	// Set random value in a random gene.
	public void Mutate()
	{
		genes[Random.Range(0, dnaLength)] = Random.Range(-maxValues, maxValues);
	}
}
