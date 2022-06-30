using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersPool : MonoBehaviour
{
    [SerializeField] private ShapeDrawer shapeDrawer;
    [SerializeField] private int startCount;
    [SerializeField] private CharacterBehaviour characterPrefab;
    [SerializeField] private float speed;
    [SerializeField] private Transform drawPlane;
    private Vector3 offset;
    private List<CharacterBehaviour> characters = new List<CharacterBehaviour>();
    private bool gameStarted;

    private void Start()
    {
        offset = new Vector3(0, 0, drawPlane.localScale.z * 10);
        CharacterBehaviour temp;
        for (int i = 0; i < startCount; i++)
        {
            temp = Instantiate(characterPrefab, transform);
            temp.CharactersPool = this;
            characters.Add(temp);
        }
        shapeDrawer.GuysCount = characters.Count;
        shapeDrawer.OnNewFormation += OnFirstNewFormation;
        GameManager.GameOver += GameOverHandler;
    }

    private void GameOverHandler(Transform finish)
    {
        gameStarted = false;
        if (finish != null)
        {
            Vector3 startPos = new Vector3(finish.position.x - (finish.localScale.x / 2f - 1f), transform.position.y,
                finish.position.z + (finish.localScale.z  / 2f - 1f));
            Vector3 point = startPos;
            float line = startPos.z;
            for (int i = 0; i < characters.Count; i++)
            {
                if (point.x == startPos.x + (finish.localScale.x - 1))
                {
                    line--;
                    point = new Vector3(startPos.x, startPos.y, line);
                }
                characters[i].DoMove(point);
                point += Vector3.right;
            }
        }
        shapeDrawer.OnNewFormation -= OnNewInformation;
    }

    private void OnFirstNewFormation(Vector3[] points)
    {
        shapeDrawer.OnNewFormation -= OnFirstNewFormation;
        OnNewInformation(points);
        shapeDrawer.OnNewFormation += OnNewInformation;
        gameStarted = true;
    }

    private void OnNewInformation(Vector3[] points)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].DOLocalMove(points[i] + offset);
        }
    }

    private void Update()
    {
        if(gameStarted)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }

    public void RemoveCharacter(CharacterBehaviour character)
    {
        characters.Remove(character);
        shapeDrawer.GuysCount = characters.Count;
        if(characters.Count < 1)
        {
            GameManager.GameOver?.Invoke(null);
        }
    }

    public void AddCharacter(Vector3 pos)
    {
        CharacterBehaviour character = Instantiate(characterPrefab, pos, Quaternion.identity, transform);
        character.CharactersPool = this;
        characters.Add(character);
        shapeDrawer.GuysCount = characters.Count;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= GameOverHandler;
    }
}
