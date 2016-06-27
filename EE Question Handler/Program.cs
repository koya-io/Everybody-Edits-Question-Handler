using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EE_Question_Handler
{
    public class Program
    {
        public static playerQuestions pq = new playerQuestions();
        static void Main(string[] args)
        {
            Console.Title = "EE Question Handler";
            string username = "Koya";
            pq.create(username, 0);



            /* V Within "say" V */
            string ans = "Yes";
            if (pq.query(username)) /*Does the username have an outstanding question?*/
            {
                switch (pq.questionid(username)) 
                    /*Question ID, equally pq.question(username) and add the switch cases as strings*/
                {
                    case 0: /*Would you like to see the tutorial?*/
                        switch (pq.test(username, ans))
                        {
                            case 0: /*Yes*/
                                Console.WriteLine("User would like the tutorial.");
                                break;
                            case 1: /*No*/
                                Console.WriteLine("User would not like the tutorial.");
                                break;
                            default:
                                break;
                        }
                        break;
                    case 1: /*How many cookies would you like?*/
                        if (pq.testint(username, ans) > -1) /*Valid answer*/
                        {
                            int cookies = pq.testint(username, ans);
                            Console.WriteLine($"User would like {cookies} cookies!");
                        }
                        break;
                    case 2: /*What would you like to name your pet?*/
                        string petname = ans;
                        Console.WriteLine($"User would like to name their pet {petname}.");
                        break;
                    case 3: /*You are 18 or older?*/
                        bool old = pq.testbool(username, ans);
                        if (old)
                        {
                            Console.WriteLine("User is 18 or older.");
                        }
                        else
                        {
                            Console.WriteLine("User is under 18.");
                        }
                        break;
                    default:
                        break;
                }
                pq.delete(username);
            }
            /**/
            Console.ReadKey();
        }
    }
    public class playerQuestions
    {
        QAManager qam;
        public List<Asked> asked = new List<Asked>();
        public playerQuestions(string QAfilelocation = "question.json")
        {
            load(QAfilelocation);
        }
        public void reload(string QAfilelocation = "question.json")
        {
            Console.WriteLine("Reloading questions and answers");
            load(QAfilelocation);
        }
        public bool query(int uid)
        {
            if (asked.IndexOf(asked.Find(user => user.uid == uid)) > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool query(string username)
        {
            return query(username.GetHashCode());
        }
        public void create(int uid, int qid)
        {
            if (asked.IndexOf(asked.Find(user => user.uid == uid)) > -1)
            {
                asked.Remove(asked.Find(user => user.uid == uid));
            }
            asked.Add(new Asked(uid, qid, qam.questions[qid].answer));
            if (qam.answers[qam.questions[qid].answer].choices != null)
            {
                Console.WriteLine($"Added user {uid} with the question {qam.questions[qid].question} and answer options [{string.Join(", ", qam.answers[qam.questions[qid].answer].choices)}]");
            }
            else
            {
                Console.WriteLine($"Added user {uid} with the question {qam.questions[qid].question} and answer type should be {qam.answers[qam.questions[qid].answer].type}");
            }
        }
        public void create(string username, int qid)
        {
            create(username.GetHashCode(), qid);
        }
        public void delete(int uid)
        {
            if (asked.IndexOf(asked.Find(user => user.uid == uid)) > -1)
            {
                asked.Remove(asked.Find(user => user.uid == uid));
            }
        }
        public void delete(string username)
        {
            delete(username.GetHashCode());
        }
        public int questionid(int uid) {
            if (asked.IndexOf(asked.Find(user => user.uid == uid)) > -1)
            {
                return (asked.Find(user => user.uid == uid)).qid;
            }
            return -1;
        }
        public int questionid(string username)
        {
            return questionid(username.GetHashCode());
        }
        public string question(int uid)
        {
            if (asked.IndexOf(asked.Find(user => user.uid == uid)) > -1)
            {
                return qam.questions[(asked.Find(user => user.uid == uid)).qid].question;
            }
            return "Error, no user found.";
        }
        public string question(string username)
        {
            return question(username.GetHashCode());
        }
        public string[] answer(int uid)
        {
            if (asked.IndexOf(asked.Find(user => user.uid == uid)) > -1)
            {
                return qam.answers[qam.questions[asked.IndexOf(asked.Find(user => user.uid == uid))].answer].choices;
            }
            return null;
        }
        public string[] answer(string username)
        {
            return answer(username.GetHashCode());
        }
        public int test(int uid, string uanswer)
        {
            string[] ans = answer(uid);
            return Array.IndexOf(ans, uanswer.ToLower());
        }
        public int test(string username, string uanswer)
        {
            return test(username.GetHashCode(), uanswer);
        }
        public int testint(int uid, string uanswer)
        {
            try
            {
                int ans = Convert.ToInt32(uanswer);
                return ans;
            }
            catch
            {
                return -1;
            }
        }
        public int testint(string username, string uanswer)
        {
            return testint(username.GetHashCode(), uanswer);
        }
        public bool testbool(int uid, string uanswer)
        {
            try
            {
                return Convert.ToBoolean(uanswer) || uanswer.ToLower() == "yes";
            }
            catch
            {
                return false;
            }
        }
        public bool testbool(string username, string uanswer)
        {
            return testbool(username.GetHashCode(), uanswer);
        }


        private void load(string QAfilelocation)
        {
            try
            {
                qam = JsonConvert.DeserializeObject<QAManager>(File.ReadAllText(QAfilelocation));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error reading questions from JSON file.");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine($"{QAfilelocation} read:");
            Console.WriteLine($"Answers: {qam.answers.Count} | Questions: {qam.questions.Count}");
        }
        public void save(string QAfilelocation)
        {
            try
            {
                File.WriteAllText(QAfilelocation, JsonConvert.SerializeObject(qam));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error saving questions to JSON file.");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine($"{QAfilelocation} read:");
            Console.WriteLine($"Answers: {qam.answers.Count} | Questions: {qam.questions.Count}");
        }
        public void newQuestion(string question, int aid)
        {
            qam.questions.Add(new Questions(qam.questions.Count, question, aid));
        }
        public void newAnswer(string[] choices)
        {
            qam.answers.Add(new Answers(qam.answers.Count, choices));
        }
    }
    public class Asked
    {
        public int uid { get; set; }
        public int qid { get; set; }
        public int aid { get; set; }
        public Asked(int _uid, int _qid, int _aid)
        {
            uid = _uid;
            qid = _qid;
            aid = _aid;
        }
    }
    public class userQuestion
    {
        public int qid { get; set; }
        public int uid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public bool answered { get; set; }
        public userQuestion(int uid)
        {
            load(uid);
        }
        public userQuestion(string username)
        {
            load(uid.GetHashCode());
        }
        public void load(int uid)
        {
            
        }
    }
    public class QAManager
    {
        public List<Answers> answers = new List<Answers>();
        public List<Questions> questions = new List<Questions>();
    }
    public class Questions
    {
        private int id;
        public string question;
        public int answer;
        public Questions(int _id, string _question, int _answer)
        {
            id = _id;
            question = _question;
            answer = _answer;
        }
    }
    public class Answers
    {
        public int id;
        public string[] choices = null;
        public string type = null;
        public Answers(int _id, string[] _choices)
        {
            id = _id;
            choices = _choices;
            type = null;
        }
    }
}