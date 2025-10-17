# R-CodeReview
A series of Unity projects to show my work, thought process and coding style.

The vast majority of the CS files have been layered with comments to show some of my thoughts both in terms of what
is trying to be achieved and how I might go about it differently in future. The commentary below provides some more 
formal insight.

The repo includes 4 main directories:

Flipbook
--------------------------------------------------------------------------------------------------------------------
This was mentioned in previous discussion. A project focussed on developing an asset-generation 
pipeline for animations in unity with custom animatable properties that can flex to any projects needs. The
project is ambitious and shows some of my work focussed more closely on the tools and asset generation 
workflows rather than content building.

In Flipbook, the UI used to manage the editor window is an attempt to break the functionality into classes without 
any real consideration for the responsibility of said classes, leading to a lot of cyclical communication. A better
approach would be to adopt MVVM pattern and split the window into several views by which the user can modify the 
model, then break the required functionality down from there.

Hex
--------------------------------------------------------------------------------------------------------------------
Another asset-generation focussed project, this time focused on making assets used directly in gameplay.
Hex is features some skeletal work for a digital card-board game in Unity. The aim is to create a framework 
where by a user can quickly script and define standalone instructions for cards to follow, tokenize these 
instructions and string them together in assets to make a combination of different cards from a pool of shared,
modifiable effects for fast iteration and rebalancing.

These two projects were largely over ambitious and saw me stepping into unfamiliar territory, thinking about UX/UI,
serialization, how an engine handles assets, rendering previews and custom editor functionality. 

In Hex, I am still tackling the core problem, appropriately tokenising the card instructions in a way that is 
legible for the computer and the end user. I find that what I might be suggesting in this undertaking is that I am
essentially writing my own scripting language, raising the question of where to draw the line in terms of preventing
the wheel's reinvention.

MVM17
--------------------------------------------------------------------------------------------------------------------
This project was a collaboration between a small group for a game jam producing a metroidvania prototype. My work 
extends to all of the code from, player character, input handling, level building content, enemy AI and controllers 
for audio and animation.

Of note was the CharacterStateMachine (MVM17\States) where I make use of the observer pattern to swap in and out of
different states. The states can be found in the sub directories, outlining a variety of different implementations.
The abstract class provides the ability for quick iteration on any given state and its flow into any other states.

I would endeavour to improve the state machine by reducing the assumptions made in the abstract class using more 
generalised constructors and querying the character on initialisation directly for any important System.Actions to
listen to or other relevant components to track/influence.

Volley
--------------------------------------------------------------------------------------------------------------------
The earliest of these projects, this is a solo-dev prototype game. An arcade sports game where the players are 
encouraged to smash a slurry of random items on to the opposing side of a side-on volleyball court. I worked on all
parts of this project including art and animation (except for the fonts). My technical endeavours were focused on 
solving problems with input for different devices, establishing a smooth asset generation pipeline for the balls by
which many different gameplay ideas could easily be prototyped and refined and a pooling system for said balls in 
order to reduce the overhead caused by an onslaught of spawning and destroying over even 5 minutes.

I gave a little too much energy to proving out the concept of entirely scripted initialisation, leading to some 
cases of pretty illegible classes. While i was confident of where the game was going, the assumptions made in the
animation logic doesn't at all lend itself to any scalability in terms of different characters or animations.

Thank you for taking the time to go through my work again.

Please let me know if you have any feedback or want to discuss anything in further detail. :)
