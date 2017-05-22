using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ModularWorldGenerator : MonoBehaviour
{
	public Module[] Modules;
    public Module exitCloser;
    public Module endRoom;
	public Module StartModule;

	public int Iterations = 5;

    private bool endRoomPresent = false;

    void Start()
    {
        //Instan a starting mdule
        var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());

        List<Module> allRooms = new List<Module>();
        allRooms.Add(startModule);

            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                //def of exit list
                var newExits = new List<ModuleConnector>();
                //for every exit in the list
                foreach (var pendingExit in pendingExits)
                {
                    bool colPresent = true;
                    int infPreventer = 0;

                    while (colPresent == true)
                    {
                        //conn a valid module and add its exits to the new list
                        var newTag = GetRandom(pendingExit.Tags);
                        //Select a random room with the tag of the exit being checked
                        var newModulePrefab = GetRandomWithTag(Modules, newTag);
                        //instantiate the randomed module
                        var newModule = (Module)Instantiate(newModulePrefab);
                        //Add the exits of the newly created module too a seperate list
                        var newModuleExits = newModule.GetExits();
                        //Gte the Exit of the new module that will be matched with the existing exit
                        var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                        //Match these exits
                        MatchExits(pendingExit, exitToMatch);

                        if (CheckCollisions(newModule, allRooms) == true)
                        {
                            Destroy(newModule.gameObject);
                            Debug.Log("Bingo");
                        }
                        else if (CheckCollisions(newModule, allRooms) == false)
                        {
                            newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                            allRooms.Add(newModule);
                            colPresent = false;
                        }

                        if (infPreventer > 35)
                        {
                            colPresent = false;
                        }
                        infPreventer++;

                        //Add all of the exits from the new module to the existing list Excluding the one that was joined

                    }

                }
                //add all unconnected exits to the pending exits list
                Debug.Log("num: " + iteration);
                pendingExits = newExits;
            }

            //Close exits and place final room
            foreach (var pendingExit in pendingExits)
            {

                if (endRoomPresent == false)
                {
                    //place final room and match the exit
                    var newModule = (Module)Instantiate(endRoom);
                    var newModuleExits = newModule.GetExits();
                    var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                    MatchExits(pendingExit, exitToMatch);
                    //If placed end room collides with another - Destroy - close exit
                    if (CheckCollisions(newModule, allRooms) == true)
                    {
                        //destroy end room
                        Destroy(newModule.gameObject);
                        //close exit
                        var newCloseModule = (Module)Instantiate(endRoom);
                        var newCloseModuleExits = newCloseModule.GetExits();
                        var exitToMatchClose = newCloseModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                        MatchExits(pendingExit, exitToMatchClose);
                    }
                    else if (CheckCollisions(newModule, allRooms) == false)
                    {
                        endRoomPresent = true;
                    }


                }
                //Final Room already present - close all remaining exits
                else
                {
                    //instantiate the closing module
                    var newModule = (Module)Instantiate(exitCloser);
                    //Get room closers exits
                    var newModuleExits = newModule.GetExits();

                    var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                    //Match these exits
                    MatchExits(pendingExit, exitToMatch);
                }

            }

    }


	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
        //corr z & y axis rot
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
        //get the trans required for exit to meet
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        //move it so they meet
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

    //checks coll between rooms
    private bool CheckCollisions(Module moduleToCheck, List<Module> activeModules)
    {
        Collider newCollider = moduleToCheck.transform.GetComponent<Collider>();
        Collider compareCollider = new Collider();

        for (int i = 0; i < activeModules.Count; i++)
        {
            compareCollider = activeModules[i].transform.GetComponent<Collider>();
            
            if(newCollider.bounds.Intersects(compareCollider.bounds))
            {
                Debug.Log("Coll Detected");
                return true;
            }
        }

        return false;
    }
}
