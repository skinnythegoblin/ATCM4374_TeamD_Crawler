using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardHand))]
[RequireComponent(typeof(PlayerScore))]
public class Player : MonoBehaviour
{
    [SerializeField] public Vector3 CardViewRotation = Vector3.zero;
    public CardHand Hand { get; private set; }
    public PlayerScore ScoreKeeper { get; private set; }
    public Color PlayerColor = Color.black;
    
    void OnEnable()
    {
        Hand = GetComponent<CardHand>();
        Hand.SetColorField(PlayerColor);
        ScoreKeeper = GetComponent<PlayerScore>();
    }
}
