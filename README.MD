# The Game of Game of Games

![The Game of Game of Games Logo](./Documentation/images/logo.png)

A game created by [Mark Arneman](https://arneman.me) for the [#TVGameJam](https://itch.io/jam/tvgamejam).

**Play it in your browser [here](https://bearlikelion.itch.io/the-game-of-game-of-games)!**

## About

The Game of Game of Games is my entry into the [#TVGameJam](https://itch.io/jam/tvgamejam). This was a **solo development effort as well as my first released** Unity project! It is inspired by the television show, [Ellen's Game of Games](https://www.nbc.com/ellens-game-of-games).

The player is prompted to select a category and answer the corresponding questions within a ten second time limit. If they get an answer wrong, they fail the category and must choose another.  Three wrong strikes and game over. There is also a challenge mode that endlessly shuffles categories until the three strike limit is reached.

The game treats each correct answer as a point, and stores the time remaining for every correct answer as the player's 'speed.' Scores are submitted to the [leader board](https://arneman.me/thegameofgameofgames/) and sorted by questions correct then by speed.


> Thanks to my wonderful wife Carina for helping with design, play testing, and research

## Development

* [Unity 2017 LTS](https://unity3d.com/)
* Audio from [Freesound](https://freesound.org/)
* Animations using [DOTween](http://dotween.demigiant.com/)
* Highscores powered by [dreamlo](http://dreamlo.com)

### Data Classes

![Data Classes](./Documentation/images/dataclass.png)

The questions are entered by [System.Serializable] classes within Unity's Inspector making them easy to change from within the editor.

### Secret.cs

The 'Secret' class is meant to keep the Dreamlo private keys out of the public git repository. There is a Secret.cs.example file to be renamed to Secret.cs and modified with your information.

### Leaderboard


## License
[MIT](./LICENSE.md) License