# Fishing Grounds
If you have ever played a video game, you probably know that they become much more fun when friends are involved. This project set out to explore the pre-released technology of [_Netcode for Gameobject_](https://docs-multiplayer.unity3d.com/netcode/current/about/). 

## Goal
Use knowledge I gain through reading the _Netcode for Gameobjects_ documentation to gain hands-on experience and reinforce what I learned. Also, use this project as an opportunity to apply some of my shader knowledge gained from completing the Course by Freya Holmér, 
[_Shaders for Game Devs_](https://www.youtube.com/playlist?list=PLImQaTpSAdsCnJon-Eir92SZMl7tPBS4Z). 

## Challenges 

### Netcode
From the limited resources available at the time of pre-release, this was one of the first times I had to read through documentation and experiment to try and teach myself a topic. Through error and trial I had eventually created a multiplayer platform on the local network
that would allow a client user to act as a server/host while the other acted as a connected client. This meant that both the host and connected clients needed to perform validation with the server for authentication to prevent cheating. However, this validation and checking 
creates lag for the users connected to the client. This was due to the client awaiting responses from the server that their requested actions were permitted. The solution to this issue is known as [_Client Side Prediction_](https://docs.unity3d.com/Packages/com.unity.netcode@0.0/manual/prediction.html)
and has been used for years. The general idea behind _Client Side Prediction_ is that the client will run smoothly under the assumption that each action requested will be approved. Then, in the event the server does not approve it, a correction will be sent to the client 
and it will update to the corrected position once received. 

### Shader
Prior to this project I had come into the reality that I was missing some incredibly important knowledge for game development. This led me to seek out a course called [_Math for Game Devs_](https://www.youtube.com/playlist?list=PLImQaTpSAdsD88wprTConznD1OY1EfK_V) by 
Freya Holmér, a university professor at FutureGames Academy. In addition to this course, I also completed her course on shaders and felt this project would be a great time to apply this knowledge. At first, it made sense for me to create a shader to act as the small
skill check bar for when a user casts their bobber. However, I then applied this knowledge to create the water shader as well which is supposed to emulate a calm lake water with a cartoon style. 

### Model Creation
The last new skill I wanted to apply in the project was the practice of creating models for myself in blender. I set out to learn yet another game creation skill which resulted in the completion of another course on blender called [_Blender 3.0 Beginner Donut Tutorial_](https://www.youtube.com/playlist?list=PLjEaoINr3zgFX8ZsChQVQsuDSjEqdWMAD)
by a Blender Guru, a long time blender user. The small model of the fishing rod, as seen on my [portfolio showcase](https://www.gonos.dev/projects), is the result of this modeling practice. 

## Conclusion 
The conclusion of this project was more of a revelation on how much work actually goes into creating a video game. From learning math, shaders, netcode, and modeling, just these 4 topics required so much time and effort to learn. This still excludes a large portion of video 
game creation and it was now up to me to decide when and where I would need to spend my time on each project if I ever wanted to complete a project. 
