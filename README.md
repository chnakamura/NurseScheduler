# Overview 
I decided to work on this project when I learned from friends of mine who are nurses, that scheduling can be a huge pain point that requires a lot of manual work and almost always leaves people working on shifts they don't want to work on.  Scheduling nurses represents a significant challenge because the hospital is open 24/7 and has to be staffed at all times. However, nurses cannot work two shifts in a row because that would be 24 consecutive hours of work, which would be unsafe for both the patients and the staff.
This is an implementation of a generic two-phase stochastic variable neighborhood approach to schedule nurses to shift. This is historically an np-hard problem, but researchers at the University of Western Greece released a very effective approximation algorithm that can find a near-optimal solution in quadratic time (rather than exponential time). You can read more about the research here
The main goal of this project is to bridge the significant gap between the cutting edge of computer science research and what is actually implemented in a way that is accessible to the would-be users. 
I have linked the Github repo because I have not had the chance to create a front-end for this app, but here is an output of it assigning 112 nurses to 224 shifts in just three minutes, with only a single nurse working a single shift they did not want to work.

```
Input:
ASSIGNMENT (11230):
Sunday Day: 0,7,14,21,28,35,42,49,56,63,70,77,84,91,98,105
Sunday Night: 0,7,14,21,28,35,42,49,56,63,70,77,84,91,98,105
Monday Day: 1,8,15,22,29,36,43,50,57,64,71,78,85,92,99,106
Monday Night: 1,8,15,22,29,36,43,50,57,64,71,78,85,92,99,106
Tuesday Day: 2,9,16,23,30,37,44,51,58,65,72,79,86,93,100,107
Tuesday Night: 2,9,16,23,30,37,44,51,58,65,72,79,86,93,100,107
Wednesday Day: 3,10,17,24,31,38,45,52,59,66,73,80,87,94,101,108
Wednesday Night: 3,10,17,24,31,38,45,52,59,66,73,80,87,94,101,108
Thursday Day: 4,11,18,25,32,39,46,53,60,67,74,81,88,95,102,109
Thursday Night: 4,11,18,25,32,39,46,53,60,67,74,81,88,95,102,109
Friday Day: 5,12,19,26,33,40,47,54,61,68,75,82,89,96,103,110
Friday Night: 5,12,19,26,33,40,47,54,61,68,75,82,89,96,103,110
Saturday Day: 6,13,20,27,34,41,48,55,62,69,76,83,90,97,104,111
Saturday Night: 6,13,20,27,34,41,48,55,62,69,76,83,90,97,104,111
------
SuccessiveSegmentSwapMutation took 831.6102ms, and improved weight from 11230 to 217
SuccessiveSegmentSwapMutation took 635.9215ms, and improved weight from 11230 to 15
RunPhaseOne Cycle took 1469.1672ms, and improved weight from 11230 to 15
SelectiveDaySwapMutation took 9707.0435ms, and improved weight from 15 to 1
SuccessiveSegmentSwapMutation took 669.8857ms, and improved weight from 1 to 1
RandomSegmentSwapMutation took 38768.443ms, and improved weight from 1 to 1
SelectiveDaySwapMutation took 9668.0566ms, and improved weight from 15 to 3
SuccessiveSegmentSwapMutation took 658.8158ms, and improved weight from 3 to 3
RandomSegmentSwapMutation took 38437.8531ms, and improved weight from 3 to 2
RunPhaseTwo Cycle took 97912.2315ms, and improved weight from 15 to 1
SelectiveDaySwapMutation took 10238.647ms, and improved weight from 1 to 1
SuccessiveSegmentSwapMutation took 689.1582ms, and improved weight from 1 to 1
RandomSegmentSwapMutation took 39003.2222ms, and improved weight from 1 to 1
SelectiveDaySwapMutation took 10235.7031ms, and improved weight from 2 to 2
SuccessiveSegmentSwapMutation took 681.3089ms, and improved weight from 2 to 2
RandomSegmentSwapMutation took 39351.028ms, and improved weight from 2 to 2
RunPhaseTwo Cycle took 100200.5793ms, and improved weight from 1 to 1
RunCycle took 199591.7636ms to assign 112 nurses to 224 shifts!
Output:
ASSIGNMENT (1):
Sunday Day: 14,28,46,47,49,58,60,70,72,80,93,95,97,99,100,101
Sunday Night: 0,8,9,15,29,31,33,37,44,48,76,77,78,98,102,106
Monday Day: 10,13,16,41,42,43,55,80,81,82,88,92,94,104,107,111
Monday Night: 26,29,30,54,56,62,63,66,67,68,76,78,84,91,103,109
Tuesday Day: 6,10,12,18,34,39,44,47,49,50,64,73,86,96,98,105
Tuesday Night: 5,7,16,21,24,30,40,60,62,63,75,76,83,85,90,99
Wednesday Day: 4,11,23,34,36,55,58,65,67,68,69,74,77,101,110,111
Wednesday Night: 5,7,13,19,20,28,35,52,53,81,86,91,94,97,108,109
Thursday Day: 2,8,17,18,25,45,57,64,70,71,75,76,87,89,95,107
Thursday Night: 1,4,22,24,33,36,42,43,50,51,59,74,79,92,93,104
Friday Day: 0,6,9,26,31,35,37,39,40,41,53,65,66,76,102,103
Friday Night: 2,14,17,19,22,23,27,32,46,48,61,69,72,82,85,87
Saturday Day: 1,38,45,52,54,59,71,73,79,83,89,90,96,100,105,106
Saturday Night: 3,11,12,15,20,25,27,32,56,57,61,76,84,88,108,110
```

If you want to try out this demo youself, just clone the repo and run the executable. You can try messing around with the [cost function](https://github.com/chnakamura/NurseScheduler/blob/main/Nurse_Scheduling/Models/Nurse.cs#L41) to how how it effects the results. 
