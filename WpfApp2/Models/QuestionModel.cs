using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp2
{
    public class Questions
    { 
        Random rand = new Random();
        public Dictionary<int, QuestionModel[]> questionList { get; init; }             
        public QuestionModel[] GetQuestions()
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            foreach (var levelquestions in questionList)
            {                
                questions.Add(levelquestions.Value[rand.Next(0, 10)]);
            }
            return questions.ToArray();
        }
    }
}
