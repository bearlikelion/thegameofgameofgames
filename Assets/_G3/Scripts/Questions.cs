using System.Collections;
using System.Collections.Generic;

public class Questions {
    // True of False Questions
    public TrueFalse[] TFQuestions = {
        new TrueFalse {
            question = "Lightning never strikes in the same place twice",
            answer = false
        },
        new TrueFalse {
            question = "Violets are red",
            answer = false
        },
        new TrueFalse {
            question = "If you cry in space the tears just stick to your face",
            answer = true
        },
        new TrueFalse {
            question = "If you cut an earthworm in half, both halves can regrow their body",
            answer = false
        },
    };

    // Word Scramble
    public WordScramble[] WSWords = {
        new WordScramble {
            word = "that"
        },
        new WordScramble {
            word = "foot"
        },
        new WordScramble {
            word = "light"
        },
        new WordScramble {
            word = "space"
        },
        new WordScramble {
            word = "hair"
        },
        new WordScramble {
            word = "milk"
        },
        new WordScramble {
            word = "cat"
        },
        new WordScramble {
            word = "coat"
        },
    };

    // Fill in the Blank
    public FillBlank[] FBlank = {
        new FillBlank {
            question = "Stone ____ Steve Austin",
            answer = "cold"
        },
        new FillBlank {
            question = "Cat in the ____",
            answer = "hat"
        },       
        new FillBlank {
            question = "A spoonful of _____",
            answer = "sugar"
        }, 
        new FillBlank {
            question = "Cow jumped over the ____",
            answer = "moon"
        },
    };
}
