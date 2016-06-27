EE Questions Handler
=
Made for [Smitty](http://forums.everybodyedits.com/viewtopic.php?id=35362)

##### About
With this you will be able to ask a user a question and then handle the answer when it happens.  
Questions to be asked have an ID and when a user speaks the question is then attempted to be answered.

##### Usage
To initialise  
`playerQuestions pq = new playerQuestions();`

Creating a new user w/ question  
`pq.create(username, 0);`  
Where username is string or integer

Creating a new question  
`pq.newQuestion(string question, int answerID)`

Creating a new answer  
`pq.newAnswer(string[] choices)`

Reloading the questions & answers file  
`pq.reload(string QAfilelocation = "questions.json")`

Save the questions & answers file  
`pq.save(string QAfilelocation = "questions.json")`

Testing to see if a user has an outstanding question  
`bool pq.query(username)`

Deleting the question  
`pq.delete(username);`

Getting question ID linked to user  
`int pq.questionid(username)`  

Getting question linked to user  
`string pq.question(username)`

Testing answer against answers linked to question  
`int pq.test(username, string ans)`

Testing/getting int answer  
`int pq.testint(username, string ans)`

Testing/getting bool answer  
`bool pq.testbool(username, string ans)`