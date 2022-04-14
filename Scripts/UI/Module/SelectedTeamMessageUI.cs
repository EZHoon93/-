using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class SelectedTeamMessageUI : MonoBehaviour
{
    [SerializeField] Color _hideTeam;
    [SerializeField] Color _seekTeam;

    [SerializeField] TextMeshProUGUI _selectedTeamText;

    public void Setup(Define.Team team)
    {
        Color color = team == Define.Team.Hide ? _hideTeam : _seekTeam;
        var content = team == Define.Team.Hide ? "Hide Team" : "Seek Team";

        _selectedTeamText.text = content;
        _selectedTeamText.color = color;
    }
}
