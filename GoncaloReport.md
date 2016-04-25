#Bugatti Wreck’em Squad
**Goncalo Lourenco (dgonc001)**

**Goldsmiths University of London**
**MSc in Computer Games and Entertainment**
**PHYSICS AND ANIMATION FOR GAMES AND ENTERTAINMENT SYSTEMS**

##Introduction
As a part of Physics coursework it was established that all students would make a racing car game. For this purpose it was assembled a multidisciplinary team that was composed by programmers and artists.  
##Role and responsibilities
I was part of programming team. Game was totally developed in Unity3D using C# programming.
As a part of the team my tasks were:
-Car deformation. I personally chose  via shader. This was achieved by texture displacement.

-Special effects like impacts, exhaust bursts and skidmarks. Also assigning all related sounds.

-Gear system and related motor sound that rise as speed increased or changed as gears change.

-Car dashboard dials visualization like gear number display, speedometer and rotations.

-Game Manager between scenes.

##Challenges
#####Deformation
Car deformation via shader posed as a great challenge has the problem would consist mainly to get the exact point of collision and deform in that particular area. For that would be needed to modify height map image on runtime when some collision occurred. But during tests on that approach it was notest that big surfaces were less “damaged” that surrounded small meshes, usually on corners. And that provoked a weird ripple effect. One way was increasing polygons number which could lead to a slower performance, other was to focus only in larger areas as these are more suitable for deformation. But this last approach would take a lot more research and other tasks would be further delayed. At the end team concluded that would be better to choose another approach. Because,  in the beginning, we predict that car damage would be essential for our game it was assigned same task  to 3 different team members which were working on 3 different car deformation approaches. So in the end we could have a choice based on better performance and easier way to implement.
#####Skidmarks
For the skidmarks, in a rush to achieve objective faster, was used Trail Renderer from Unity.
But that technique fast was proven to be wrong by two main factors:
Air skids were showing even when car had a collision and flyed away skidding in plain air. 
Other important aspect, the most important in fact, was also that was Trail Renderer is highly dependent on camera position. This means that in Multiplayer mode skids would be shown from each player perspective totally distorting the effect. So when player 2 was skidding and player 1 was behind on the left skids would be rotated towards player 1 camera. In a shared screen it would be a dreadful experience.
The way chosen to overcame this was to build own custom trail effect.
This solutions had to be runned on runtime so it was used procedural mesh geometry from Unity. In a first phase we had to know when the car was skidding, verify if it was in contact with the ground and retrieve the contact points from the car wheels. First phase was already done taking out the values from slip ratio or slip angle from the wheel and its rotation. To check if wheel was touching the ground was created a boolean variable at wheel class. Contact points of the wheel was passed same way. With all ingredients was time to implement skid mesh. A fundamental part was to determine if skidding was starting or if it was a continuity of previous one. In the start is drawn 4 vertices, in a continuity is just added more 2 vertices. In both modes is always needed to keep last 2 vertices so next mesh could be drawn. Mesh is builded connecting all vertices. At this time mesh is not very visible that would be overcome when defining correctly normals and uvs.
#####Game Manager
For Game Manager was used singleton as a design pattern to control all main events during the game. Could be used a static class but this way seemed a more efficient and clean way to achieve our results.
#####Team
Another challenge, in a more group perspective, was the coming of new group members which demanded more coordination effort. It worked well in the end thanks mainly to Emmanuel team management skills.
##What went well
Communication in our group always flow like a true team. Constant concern to keep everyone updated as work was going on, doubts shared and discussed. Honesty and straightforward thinking save a lot of time because then you only research what was really relevant for the work avoiding frustration in no-ends.
To start we had an initial meeting where all team members discussed about what the game was about to be and in group aimed into same direction. Once that done tasks were assigned to different team members as well individual and group deadlines. As work was going on it was discussed between us various approaches and how them would influence final goal. Schematics about game plan UI was useful to draw a generic picture of game flow and discover new areas of intervention. As soon they were discovered tasks were assigned. We always worried about the size of the tasks to be fitted in a week time scope, so it could be better tracked.
Facebook as a messenger and Skype for video calls were the most tools used, along with Google Calendar and Trello.
From all I believe Facebook was the best tool as we could make videos of every single progression and receive comments on it. This worked out as a team dynamo of motivation. Keeping memos of meetings worked as a reference all the time when we thought to be out of track. As Facebook chat was the most used it was handy to use Facebook as a work platform. Anyway people get stuck on Facebook all the time and this is always a nice reminder that was work has to be done! I would say that Skype meetings were also important, as team members live in different areas and is quite difficult to commute just to meet as well loosing precious time that could be used in developing.
##What went wrong
Merging program code work is a really difficult job.
Different techniques and software used to version control sometimes collided. Some used command line, others Visual Studio and SourceTree. After first stepback all decided to work on SourceTree which provided a more visual tracking avoiding Git Bash abstraction.
Also different programming techniques used launched, sometimes, havoc on other programmers code. Even working in different versions of Unity bringed some problems as well some tiny different configurations like the one in Force Text in Edit → Project Settings → Editor → Asset Serialization Mode. 
By the end we used pull request as best way to divide work and have better control on what to push. For that Raul was considered as Master Scrum as he performed as Lead programmer.
##Improvements
To be honest I truly believe that all members performed at a very professional manner in a reflection of their commitment to this coursework.
Maybe next time I would just prefer that all team members work with an identical level of expertise as to source control software concern. To cover that maybe the most familiarized team member with that software would give a crash course to other team members.
