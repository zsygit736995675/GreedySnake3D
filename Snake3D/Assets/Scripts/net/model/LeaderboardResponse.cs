using System;
using System. Collections. Generic;

[Serializable]
public class LeaderboardResponse
{
    public string userId;
    public string userName;
    public string userScore;
    public string userLastScore;
    public List<LeaderboardUserResponse> listOtherUser = new List<LeaderboardUserResponse>();
}

public class LeaderboardUserResponse {
    public string userId;
    public string userName;
    public string userScore;
    public LeaderboardUserResponse(string id ,string name ,string score) {
        userId = id;
        userName = name;
        userScore = score;
    }
    private LeaderboardUserResponse() { }
}