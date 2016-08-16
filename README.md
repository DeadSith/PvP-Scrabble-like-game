#[Latest version of words.db](https://1drv.ms/u/s!AiMRzyTQY-LIsHQanItwghCH5x21)   
More than 2.2 million words! Place it in Assets/SharedAssets. Inform me, if any word is missing.   
**Links are constantly updated.**  
# PvP-Scrabble-like-game[Android version]
###1. What:   
Open-source version of games like scrabble with LAN and hot-seat multiplayer. Written fully in C#.   
###2. Why:   
There are a few multilayer open-source conversions of scrabble-like games. Even fewer with network mode and none in my native language.   
###3. How:   
Unity3d 5.x, everything is done in GUI layer, supports drag&drop. Words are stored in sqlite database. Networking implemented based on HLAPI.   
###4. How to build:
1. Create empty Unity project.
2. Copy Assets folder from this repo.
3. Go to File -> Build Settings and add 2 scenes in the next order:  
  0.MainMenu  
  1.mainField  
4. In Build Settings > Player Settings > API Compatibility Level, change to .NET 2.0
5. Now game should build without any errors

###5. What is done:
![Latest version screenshot](http://i68.tinypic.com/2nkjsch.png)   
Game is fully playable, but database is not woking because Android, will be fixed in next updates.  
###6. Rules:   
Game is based on partially changed rules of scrabble. Most notable change: you can continue your turn even if you submitted a wrong word.
###7. More info:   
The game is fully in Ukrainian, but code and comments are in English. The graphics is not very good, but I [can't into](https://cdn.meme.am/instances/500x/62250317.jpg) smth beautiful.