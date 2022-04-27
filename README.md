# LobsterInk_Assessment


Steps to run,

cd to folder and run docker compose build

It will throw up error some time in efcore network for first time some times and then it will connect, Just wait for few mins.

Addded sample.json as default format for request object decision tree nested json. use this for post to adventure api.

Since UI is not needed, I followed approach, 

In user screen canvas where drag and drop kind of decisions, We will form json structure as request for adventure game.

Though we can design, For each question addition post request with associated Id, I've done this for time being.


Workflow Adventure 

API => Adventure => POST (Sample.JSON, Data) => Result => AdventureId

                                      (AdventureId => Get from above request)
API => Adventure => StartGame => POST (AdventureId, UserId) => Result => Question Text and [Next questions 1,2... with question route Id]

                                       (User choice QuestionRouteId Get from above request, Until children null)
API => Adventure => MoveToNextLevel => POST (QuestionRouteId, UserId)=> Result => AdventureId








