using UnityEngine;
using System;
using System.Collections.Generic; 		
using Random = UnityEngine.Random; 		

namespace Game
	
{
	
	public class BoardManager : MonoBehaviour
	{
		[Serializable]
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.
			
			
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}

		public int columns = 8; 										//Number of columns in our game board.
		public int rows = 8;											//Number of rows in our game board.
		public Count wallCount = new Count (5, 9);						//Lower and upper limit for our random number of walls per level.
		public Count foodCount = new Count (1, 5);						//Lower and upper limit for our random number of food items per level.
		public GameObject exit;											//Prefab to spawn for exit.
		public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] wallTiles;									//Array of wall prefabs.
		public GameObject[] foodTiles;									//Array of food prefabs.
		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.

		private List<GameObject> _boardObjects = new List<GameObject>();

		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.

        void InitialiseList ()
		{
			Debug.Log("InitialiseList");
			gridPositions.Clear ();
			
			for(int x = 1; x < columns-1; x++)
			{
				for(int y = 1; y < rows-1; y++)
				{
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}
		
		void BoardSetup ()
		{
            Debug.Log("BoardSetup");
			while (_boardObjects.Count > 0)
			{
				GameObject obj = _boardObjects[_boardObjects.Count - 1];

				_boardObjects.RemoveAt(_boardObjects.Count - 1);

				Destroy(obj);
			}

			if (boardHolder == null)
			{
				boardHolder = new GameObject ("Board").transform;
				boardHolder.SetParent(GameManager.Instance.world);
			}
			for(int x = -1; x < columns + 1; x++)
			{
				for(int y = -1; y < rows + 1; y++)
				{
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					
					if(x == -1 || x == columns || y == -1 || y == rows)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity);
					instance.transform.SetParent (boardHolder);
					_boardObjects.Add(instance);
				}
			}
		}
		
		
		Vector3 RandomPosition ()
		{
            Debug.Log("RandomPosition");
            int randomIndex = Random.Range (0, gridPositions.Count);
			
			Vector3 randomPosition = gridPositions[randomIndex];
			
			gridPositions.RemoveAt (randomIndex);
			
			return randomPosition;
		}
		
		
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
            Debug.Log("LayoutObjectAtRandom");
            int objectCount = Random.Range (minimum, maximum+1);
			
			for(int i = 0; i < objectCount; i++)
			{
				Vector3 randomPosition = RandomPosition();
				
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				GameObject newTile = Instantiate(tileChoice, randomPosition, Quaternion.identity, GameManager.Instance.world);
                _boardObjects.Add(newTile);

            }
		}
		
		
		public void SetupScene (int level)
		{
            Debug.Log("SetupScene");

			InitialiseList ();
			
            BoardSetup();
			
			LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
			
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
			
			int enemyCount = (int)Mathf.Log(level, 2f);
			
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
			
			GameObject exitTile =Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity, GameManager.Instance.world);
            _boardObjects.Add(exitTile);

        }
    }
}
