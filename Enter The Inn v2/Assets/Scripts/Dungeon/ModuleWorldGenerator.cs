using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ModuleWorldGenerator : MonoBehaviour
{
	public Module[] modules;
	public Module startModule;

	public int Iterations = 5;


	void Start()
	{
		var pendingExits = new List<ModuleConnector>(startModule.GetExits());

		for (int iteration = 0; iteration < Iterations; iteration++)
		{
			var newExits = new List<ModuleConnector>();

			foreach (var pendingExit in pendingExits)
			{
				var newTag = GetRandom(pendingExit.Tags);
				var newModulePrefab = GetRandomWithTag(modules, newTag);
				var newModule = Instantiate(newModulePrefab);
				var newModuleExits = newModule.GetExits();
				var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
				MatchExits(pendingExit, exitToMatch);

				newModule.transform.SetParent(transform);

				newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
			}

			pendingExits = newExits;
		}
	}


	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}


	private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[Random.Range(0, array.Length)];
	}


	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
	{
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}


	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
}
