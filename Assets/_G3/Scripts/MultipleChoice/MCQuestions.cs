[System.Serializable]
public class MCQuestions {
	public string question;
	public MCAnswers[] answers;
}

[System.Serializable]
public class MCAnswers  {
	public string choice;
	public bool correctAnswer;
}
