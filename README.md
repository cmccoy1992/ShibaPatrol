# ShibaPatrol
Capstone project for Code:You. A text-based adventure where you play as the Shiba Inu, Poppy, and secure her home from potential threats.

===HOW TO PLAY===

Shiba Patrol is a text-based adventure; you perform actions by typing in the letter of the action you would like to perform. 

The player must go through the rooms and "Secure" them by investigating each room. See "Investigation" below.

---Stats---

❤️ Bravery:    Represents Poppy's confidence, and acts like health (HP) in most video games. If this reaches 0, Poppy will retreat to the bedroom.

⚡ Energy:     Poppy's stamina. Certain moves in encounters will consume Stamina. Similar to Bravery, she will retreat to the bedroom if this reaches 0 as she becomes too tired.

🛡️ Security:   How secure the house is. This number increases as Poppy secures rooms, but can decrease if she retreats from an encounter.

⏳ Time:       The number of minutes Poppy has left to complete her patrol. Moving between rooms costs 1 minute. Every 3 actions in an encounter costs 1 minute. Mischief events cost a random amount of time (2-5 minutes.)

⚔️ Power:      The amount of damage dealt to threats.

---Leveling Up---
At the end of encounters, Poppy can gain experience (XP). When she reaches a certain threshold, she levels up, and can increase Bravery, Energy, or Power.

Increasing Power will automatically reflect in the description of each of her moves.

---Investigation---

When Poppy investigates a room, she will engage in an encounter. This functions like a turn-based battle from Pokemon, Final Fantasy, etc.

Type in the letter of the move you would like to perform. Moves with ⚔️ will reduce the target's Threat (Enemy HP). When the threat is "subdued", the encounter ends and Poppy gains XP equal to the Threat.

---Mischief---

If Poppy investigates a room again after completing an investigation, she has the option to get into trouble for a reward. This can be simple recovery of Bravery and Energy to permanent stat increases.

The greater the reward, the more trouble Poppy will get into.

---Ending the Game---

The game ends in one of two ways:

1) Poppy returns to the Bedroom and chooses to end the patrol.
   
2) Poppy runs out of time.

When this occurs, Poppy will get an ending based on:

-How secure the room was

-How much mischief she got into

Final score is then calculated, based on:

-Security

-Remaining Time

-Avoiding Mischief

-Poppy's Level

---Code:You Questionairre---

Q) What did you learn from this project?

A) This greatly expanded my knowledge and understanding of object-oriented programming, as the rooms are objects the player moves in and out of. I especially learned this the hard way when I accidentally had nested
methods entering rooms within rooms within rooms, causing the game to crash.

It was also wonderful practice for putting together a larger solo program. This program was initially an assignment that was due a little after Thanksgiving, and I used what I learned over the remainder of the course
to revisit and turn it into a project I'm seriously proud of. I felt like I outdid myself when I first created this using methods upon methods upon methods, and from a coding standpoint- it's practically an entirely
new program. It taught me how to take an old program and restructure it into something that better suits my understanding today.

-----

Q) What did you learn from this course?

A) I'm not sure where to even begin with this- when I first began, I had a rudimentary understanding of C-languages from programming in Morrowind, Game Maker, and other game modding tools, as well as some self-taught
understanding of C++. And now- here I am, building a silly text-based adventure with combat, music, and room-based events. 

The best way to put it is I learned the confidence to make complex programs. I think the knowledge to learn C# was always there- I've always been a programmer at heart. This course gave me the confidence to fully
realize that potential, and I'm eternally grateful.

-----

Q) If you had more time, what would you have done differently or what additional features would you have added?

A) I wanted to have some of my other Shiba Inus playable initially- Pup-Tart and Akira. But building this game in the first place was a massive undertaking.

I also wanted to clean up the encounter structure (I feel it's a bit messy and hard to follow).
-----

---Accreditation---

Royalty-free music was utilized in this project, the credits are as follows:


"EXPLORE"

"Dance of the Sugar Plum Fairy" Kevin MacLeod (incompetech.com)

Licensed under Creative Commons: By Attribution 4.0 License

http://creativecommons.org/licenses/by/4.0/ 


"MISCHIEF"

Track Name: Coriolan Overture, Op. 62

Composer: Ludwig van Beethoven

Performer/Source: Musopen Symphony (via Musopen)

License: Public Domain (CC PDM 1.0)

Link: [https://musopen.org/music/...](https://musopen.org/music/2604-coriolan-overture-op-62/)


"ENCOUNTER"

"Volatile Reaction" Kevin MacLeod (incompetech.com)

Licensed under Creative Commons: By Attribution 4.0 License

http://creativecommons.org/licenses/by/4.0/ 


"MOTHMAN"

Track Name: Battle for the Kingdom

Source: Pixabay

License: Pixabay License (free for commercial use, no attribution required)

Link: https://pixabay.com/music/upbeat-battle-for-the-kingdom-392556/


"VICTORY"

Track Name: Majestic Brass Fanfare

Source: Pixabay

License: Pixabay License (free for commercial use, no attribution required)

Link: https://pixabay.com/music/marching-band-majestic-brass-fanfare-293125/


"PATROL OVER"

Track Name: Concerto No. 5 in A minor

Composer: Antonio Vivaldi

Performers: Carl Pini, John Tunnell, Anthony Pini, Harold Lester

Source: Musopen

License: Public Domain (CC PDM 1.0)

Link: [https://musopen.org/music/...](https://musopen.org/music/3609-la-cetra-op-9/)
