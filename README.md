EndlessRunner

Simple Endless Runner example using an architecture similar to MVU or Redux in frontend world.
The main idea is to separate Unity's MonoBehaviours from the actually simulation logic code.

Keys Points:

1. MonoBehaviours are just plain views that don't have any dependencies from outter world. They only responsible on the self gameObject and can't make any game state changes.
2. Presenters are actually managing the views (MonoBehaviours)
3. The game state can be changed in only one way - through the sending a change command to the Engine Input. Then engine will process the commands, change game state and any Presenter can listen a game state change if it realizes IListener<TCommand> interface to handle a specific command execution in Engine.

![alt text](https://github.com/Darkwing-Duck/EndlessRunner/blob/master/Design/architecture-flow.jpg?raw=true)
