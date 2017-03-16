# [Latest version of words.db](https://1drv.ms/u/s!AiMRzyTQY-LIsHQanItwghCH5x21)   
# [Latest compiled version](https://1drv.ms/u/s!AiMRzyTQY-LIsmS747Kt6_B1Qqvr)
More than 2.2 million words! Place it in Assets/SharedAssets. Inform me, if any word is missing.   
**Links are constantly updated.**  
# PvP-Scrabble-like-game
### [Android version ](https://github.com/DeadSith/-Android-PvP-Scrabble-like-game)
### 1. What:   
Open-source version of games like scrabble with LAN and hot-seat multiplayer. Written fully in C#.   
### 2. Why:   
There are a few multilayer open-source conversions of scrabble-like games. Even fewer with network mode and none in my native language.   
### 3. How:   
Unity3d 5.x, everything is done in GUI layer, supports drag&drop. Words are stored in sqlite database. Networking implemented based on HLAPI.   
### 4. How to build:
1. Create empty Unity project.
2. Copy Assets folder from this repo.
3. Go to File -> Build Settings and add all 3 scenes in the next order:  
  0.MainMenu  
  1.mainField  
  2.mainFieldLAN  
4. In Build Settings > Player Settings > API Compatibility Level, change to .NET 2.0
5. Now game should build without any errors

### 5. What is done:
![Latest version screenshot](http://i66.tinypic.com/nz4s60.png)
Game is fully playable, though there still may be small bugs. Game has hot-seat and networking mode. There is no singlelayer. All the standard features, including letter changing, skipping turns and other are implemented.
### 6. Rules:
Game is based on partially changed rules of scrabble. Most notable change: you can continue your turn even if you submitted a wrong word.
### 7. More info:
The game is fully in Ukrainian, but code and comments are in English. The graphics is not very good, but I [can't into](https://cdn.meme.am/instances/500x/62250317.jpg) smth beautiful.
### Known issues:
Letters can become [stuck on field](https://github.com/DeadSith/PvP-Scrabble-like-game/issues/3). This is Unity issue, I made a workaround, but it does not always work.
