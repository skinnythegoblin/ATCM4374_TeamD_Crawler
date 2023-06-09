using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandViewer : MonoBehaviour
{
    public static HandViewer Instance;

    [SerializeField] CardDisplay _cardDisplayPrefab;
    [SerializeField] RectTransform _scrollBody;
    [SerializeField] Image _bg;
    [SerializeField] float _distanceBetweenCards = 500f;
    
    public static event Action OnView;
    public static event Action OnClose;

    List<CardDisplay> _cardDisplays = new List<CardDisplay>();
    CardHand _currentHand;
    float _index = 0f;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void ViewHand()
    {
        _index = 0f;
        _currentHand = TurnManager.Instance.CurrentPlayer().Hand;
        _currentHand.EnableVisuals(false);
        _scrollBody.anchoredPosition = Vector2.zero;
        
        DisplayCards();

        Color bgColor = _currentHand.GetComponent<Player>().PlayerColor;
        bgColor.a = 0.75f;
        _bg.color = bgColor;
        _bg.gameObject.SetActive(true);

        _scrollBody.GetComponent<RectTransform>().rotation = Quaternion.Euler(TurnManager.Instance.CurrentPlayer().CardViewRotation);
        
        OnView?.Invoke();
    }

    public void CloseHand(bool invokeClose = true)
    {
        _currentHand.EnableVisuals(true);
        _currentHand = null;
        _bg.gameObject.SetActive(false);

        _scrollBody.anchoredPosition = Vector2.zero;

        ClearDisplay();

        if (invokeClose)
        {
            OnClose?.Invoke();
        }
    }

    public void Scroll(Vector2 movement)
    {
        if (_currentHand.ScrollDirection == Vector2.right)
        {
            if (_scrollBody.anchoredPosition.x + movement.x > 0)
            {
                movement.x = _scrollBody.anchoredPosition.x;
            }
            if (_scrollBody.anchoredPosition.x + movement.x < -_distanceBetweenCards * (_currentHand.Cards.Count - 1))
            {
                movement.x = -((-_distanceBetweenCards * (_currentHand.Cards.Count - 1)) - _scrollBody.anchoredPosition.x);
            }

            _scrollBody.anchoredPosition += new Vector2(movement.x, 0);
            _index += -movement.x / _distanceBetweenCards;
        }
        if (_currentHand.ScrollDirection == Vector2.left)
        {
            if (_scrollBody.anchoredPosition.x + movement.x < 0)
            {
                movement.x = _scrollBody.anchoredPosition.x;
            }
            if (_scrollBody.anchoredPosition.x + movement.x > _distanceBetweenCards * (_currentHand.Cards.Count - 1))
            {
                movement.x = -((_distanceBetweenCards * (_currentHand.Cards.Count - 1)) - _scrollBody.anchoredPosition.x);
            }

            _scrollBody.anchoredPosition += new Vector2(movement.x, 0);
            _index += movement.x / _distanceBetweenCards;
        }
        if (_currentHand.ScrollDirection == Vector2.up)
        {
            if (_scrollBody.anchoredPosition.y + movement.y > 0)
            {
                movement.y = _scrollBody.anchoredPosition.y;
            }
            if (_scrollBody.anchoredPosition.y + movement.y < -_distanceBetweenCards * (_currentHand.Cards.Count - 1))
            {
                movement.y = -((-_distanceBetweenCards * (_currentHand.Cards.Count - 1)) - _scrollBody.anchoredPosition.y);
            }

            _scrollBody.anchoredPosition += new Vector2(0, movement.y);
            _index += -movement.y / _distanceBetweenCards;
        }
        if (_currentHand.ScrollDirection == Vector2.down)
        {
            if (_scrollBody.anchoredPosition.y + movement.y < 0)
            {
                movement.y = _scrollBody.anchoredPosition.y;
            }
            if (_scrollBody.anchoredPosition.y + movement.y > _distanceBetweenCards * (_currentHand.Cards.Count - 1))
            {
                movement.y = -((_distanceBetweenCards * (_currentHand.Cards.Count - 1)) - _scrollBody.anchoredPosition.y);
            }

            _scrollBody.anchoredPosition += new Vector2(0, movement.y);
            _index += movement.y / _distanceBetweenCards;
        }

        ResizeCards();
    }

    void DisplayCards()
    {
        for (int i = 0; i < _currentHand.Cards.Count; i++)
        {
            if (_cardDisplays.Count < i + 1)
            {
                _cardDisplays.Add(Instantiate(_cardDisplayPrefab, _scrollBody.transform));
                _cardDisplays[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(i * _distanceBetweenCards, 0);
            }
            _cardDisplays[i].SetCard(_currentHand.Cards[i]);
        }

        ResizeCards();
    }

    void ClearDisplay()
    {
        foreach (CardDisplay display in _cardDisplays)
        {
            display.SetCard(null);
        }
    }

    void ResizeCards()
    {
        for (int i = 0; i < _currentHand.Cards.Count; i++)
        {
            float scale = Mathf.Clamp(2f * (1 - Mathf.Abs(i - _index)), 0.9f, 2f);
            _cardDisplays[i].transform.localScale = new Vector3(scale, scale, 1);
            _cardDisplays[i].EnablePlay(scale > 1f);
        }
    }
}
