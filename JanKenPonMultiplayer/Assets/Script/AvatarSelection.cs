using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] Image avatarImage;
    [SerializeField] Sprite[] avatarSprites;

    private int selectedIndex;

    public void ShiftSelectedIndex(int shift){
        selectedIndex += shift;
        while(selectedIndex >= avatarSprites.Length)
            selectedIndex -= avatarSprites.Length;
        while(selectedIndex < 0)
            selectedIndex += avatarSprites.Length;

        avatarImage.sprite = avatarSprites[selectedIndex];
    }
}

