#!/usr/bin/env python

# Python version 3.4.0

# This script takes as its input an NHL '94 ROM, custom or original
# and a savestate taken from a game played with that ROM.  The savestate
# must be taken when the "three stars" window is displayed.

# This script outputs a CSV file of the statistics from the savestate.
# The CSV file contains the team stats for both teams, each team's goals,
# each team's penalties, and the individual player statistics for all
# players on both teams.

# tickenest@gmail.com for questions/support

# 7 February 2014

# Useful references:
# http://www.nhl94.com/html/editing/gens32-savestate.htm
#h ttp://www.nhl94.com/html/editing/edit_bin_player_info.php
# Fun fact: ROM credits start at 5778/22392

import mmap, csv, sys, getopt

################################################################################

def getTeamAbbrevs(ROM, teamOffsets):

    # Gets all the team abbreviations in the ROM and returns a list of them.
    teamAbbrevsList = []

    #print (teamOffsets)
    
    for byteCounter in teamOffsets:
            
        byteCounter += 147

        # Bypass pairs of FF bytes that aren't real data

        while ROM[byteCounter] == 255 and ROM[byteCounter+1] == 255:
            byteCounter += 1

        # Assume that the next bytes are for a player until proven
        # otherwise
        keepReading = True
        
        while keepReading == True:

            # Jump to the potential start of the next player
            byteCounter += ROM[byteCounter] + 8
            #print(byteCounter)

            # If ROM[byteCounter] == 2 then we've reached the end
            # of the player list so stop treating the bytes like
            # player data
            if ROM[byteCounter] == 2:
                keepReading = False
            elif ROM[byteCounter+2] == 2:
                keepReading = False

        # Jump ahead two bytes.  This byte will tell us how much
        # farther to look ahead for the team's abbreviation.
        byteCounter += 2
        byteCounter += ROM[byteCounter] + 1

        # Grab the team abbreviation.  If ROM[byteCounter-1] == 4
        # then we've got a team with a 2-letter abbreviation, so
        # remove the last letter.
        teamAbbrev = (chr(ROM[byteCounter]) + chr(ROM[byteCounter+1]) +
                      chr(ROM[byteCounter+2]))
        if ROM[byteCounter-1] == 4:
            teamAbbrev = teamAbbrev[:-1]

        teamAbbrevsList.append(teamAbbrev)

        #print (teamAbbrev)

        byteCounter += 1

    return teamAbbrevsList

################################################################################

def getNumGoalies(ROM, offset):

    # The two bytes for the numbers of goalies a team has are 80 bytes after
    # the start of the key sequence of bytes 00 92 00 0C 02 that marks the
    # start of a new team.  For each hex digit in those two bytes (four digits
    # in all) that is not a 0, add 1 to the number of goalies on the team.
    # So B0 00 would be 1 goalie, 54 00 would be 2 goalies, CB C0 would be 3
    # goalies, and 9999 would be 4 goalies.

    goalieStartByte = offset + 80

    numGoalies = 0

    if int(ROM[goalieStartByte] / 16) != 0:
        numGoalies += 1
    if ROM[goalieStartByte] % 16 != 0:
        numGoalies += 1
    if int(ROM[goalieStartByte+1] / 16) != 0:
        numGoalies += 1
    if ROM[goalieStartByte+1] % 16 != 0:
        numGoalies += 1

    return numGoalies

################################################################################

def readPlayer(ROM, offset):

    # Starting at ROM[offset] with the length of the upcoming player's name,
    # readPlayer returns a string with that player's name.  Some players have
    # an extra blank space attached to their names in the ROM.  readPlayer
    # removes that extra space.

    nameList      = []
    fixedNameList = []
    
    # Get the name length from the first byte of the player's name.  The
    # value minus 2 equals the length of the player's name, including a
    # possible space at the end.

    nameLength = ROM[offset] - 2

    # Get the decimal values of each letter of the player's name
    for char in ROM[offset+1:offset+nameLength+1]:
        nameList.append(char)

    # If the last value is 0, then it's an extra space that we can remove
    if nameList[-1] == 0:
        nameList = nameList[:-1]

    # Make the decimal values into chars
    for char in nameList:
        fixedNameList.append(chr(char))

    # Make the chars into a single string and return the string
    return ''.join(fixedNameList)

################################################################################

def getTeamOffsets(ROM):

    # Gets all the team offsets from the ROM and returns them in a list
    # for further processing

    # Team offsets begin at 030E/782 and each offset takes up 4 bytes.

    teamOffsets = []

    if len(ROM) == 1048576:

        numberOfTeams = 28

    else:

        numberOfTeams = 30

    for count in range(0,numberOfTeams*4,4):
        teamOffsets.append((ROM[782+count+1]*65536)+
            (ROM[782+count+2]*256)+(ROM[782+count+3]))
        
    #print(teamOffsets)
    return teamOffsets    

################################################################################

def getTeamPlayersFromROM(ROM, team, teamOffsets):

    print(team + " ok")
    bFirst = True;

    for teamOffset in teamOffsets:

        byteCounter = teamOffset + 0

        # Blank list of the names of everyone on a team
        playerList = []

        # Get the number of goalies on the team
        numGoalies = getNumGoalies(ROM,byteCounter)

        # Get the number of forwards and defensemen
        numForwards   = int(ROM[byteCounter+79]/16)
        numDefensemen = ROM[byteCounter+79] % 16

        # Jump 147 bytes
        # to the byte containing the length of the first
        # player's name.
        byteCounter += 147

        # Bypass pairs of FF bytes that aren't real data

        while ROM[byteCounter] == 255: #and ROM[byteCounter+1] == 255:
            byteCounter += 1

        # Assume that the next bytes are for a player until proven
        # otherwise
        keepReading = True

        # for 30 team rom this is required
        if bFirst == True:
            byteCounter += 1
            bFirst = False
            
        while keepReading == True:

            # Parse the player's name from the ROM and add it
            # to playerList
            #print(byteCounter)
            playerList.append(readPlayer(ROM,byteCounter))

            # Jump to the potential start of the next player
            byteCounter += ROM[byteCounter] + 8

            #print (playerList)
            # If ROM[byteCounter] == 2 then we've reached the end
            # of the player list so stop treating the bytes like
            # player data
            if ROM[byteCounter] == 2:
                keepReading = False

        # Jump ahead two bytes.  This byte will tell us how much
        # farther to look ahead for the team's abbreviation.
        byteCounter += 2
        byteCounter += ROM[byteCounter] + 1

        # Grab the team abbreviation.  If ROM[byteCounter-1] == 4
        # then we've got a team with a 2-letter abbreviation, so
        # remove the last letter.
        teamAbbrev = (chr(ROM[byteCounter]) + chr(ROM[byteCounter+1]) +
                      chr(ROM[byteCounter+2]))
        if ROM[byteCounter-1] == 4:
            teamAbbrev = teamAbbrev[:-1]

        #print(teamAbbrev)
            
        # Check whether the team we've just processed is the team
        # that we want.  If it is, then append the number of goalies,
        # forwards, and defensemen on the team and return playerList.
        # If not dump playerList and keep searching through the ROM
        # until we find the next team.

        if teamAbbrev == team:
            playerList.append(numGoalies)
            playerList.append(numForwards)
            playerList.append(numDefensemen)
            #print(playerList)
            return playerList
        else:
            del playerList

        # Jump to the next byte for further searching
        byteCounter += 1

    # If we didn't find the key sequence of bytes, try again
    # with the next byte
    else:
        byteCounter += 1
##
##    # getTeamPlayersFromROM gets a single team's players from the ROM and
##    # returns a list of their names.  The last three items in the list are
##    # the number of goalies on that team, the number of forwards on that
##    # team, and the number of defensemen on that team.
##
##    #Offsets: subtract 2 from decimal value to get player
##    #name length.  Add 8 to the player name length lookup
##    #to get to the start of the next player's name.
##
##    teamsData = (("Anaheim","ANA"),("Boston","BOS"),("Buffalo","BUF"),
##                 ("Calgary","CGY"),("Chicago","CHI"),("Dallas","DAL"),
##                 ("Detroit","DET"),("Edmonton","EDM"),("Florida","FLA"),
##                 ("Hartford","HFD"),("Los Angeles","LA"),("Montreal","MTL"),
##                 ("New Jersey","NJ"),("Islanders","NYI"),("Rangers","NYR"),
##                 ("Ottawa","OTT"),("Philadelphia","PHI"),("Pittsburgh","PIT"),
##                 ("Quebec","QUE"),("San Jose","SJ"),("St. Louis","STL"),
##                 ("Tampa Bay","TB"),("Toronto","TOR"),("Vancouver","VAN"),
##                 ("Washington","WAS"),("Winnipeg","WIN"),("All Stars East","ASE"),
##                 ("All Stars West","ASW"),("Carolina","CAR"),("Columbus","CBJ"))
##
##    #00 92 00 0C is 147 bytes before the first player's name
##
##    # Start looking from the start of the ROM
##    byteCounter = 0
##
##    # Don't go past the end of the ROM
##    while byteCounter < len(ROM)-4:
##

##
##        # Find the key sequence of bytes 00 92 00 0C.  It is 147
##        # bytes before the byte containing the length of the first
##        # player's name.
##        if [ROM[byteCounter],ROM[byteCounter+1],ROM[byteCounter+2],
##            ROM[byteCounter+3]] == [0,146,0,12]:
##

##

##
##    #Team player name starting point offsets:
##    #ASE 410/1040 - 638/1592
##    #ASW 706/1798 - 92B/2347
##    #Boston 9F8/2552 - C1B/3099
##    #Buffalo CDE/3294 - F01/3841
##    #Calgary FCC/4044 - 11FD/4605
##    #Chicago 12C6/4806 - 14F5/5365
##    #Detroit 15C0/5568 - 1809/6153
##    #Edmonton 18DA/6362 - 1B0B/6923
##    #Hartford 1BD6/7126 - 1E15/7701
##    #Los Angeles 1EE4/7908 - 2112/8466
##    #Dallas 21E0/8672 - 240D/9229
##    #Montreal 24D0/9424 - 2700/9984
##    #New Jersey 27D2/10194 - 2A1B/10779
##    #Islanders 2AEA/10986 - 2D22/11554
##    #Rangers 2DEE/11758 - 3016/12310
##    #Ottawa 30E6/12518 - 330F/13071
##    #Philadelphia 33DA/13274 - 3615/13845
##    #Pittsburgh 36D8/14040 - 3906/14598
##    #Quebec 39CE/14798 - 3C08/15368
##    #San Jose 3CD4/15572 - 3EFE/16126
##    #St. Louis 3FC2/16322 - 41EC/16876
##    #Tampa Bay 42A8/17064 - 44D8/17624
##    #Toronto 45AA/17834 - 47DF/18399
##    #Vancouver 48AE/18606 - 4AD0/19152
##    #Winnipeg 4B9C/19356 - 4DCA/19914
##    #Washington 4E8E/20110 - 50AE/20654
##    #Florida 5178/20856 - 52FC/21244
##    #Anaheim 53C2/21442 - 5546/21830

################################################################################

def getScoringSummary(saveState):

    # The scoring summary begins at 59627.  This byte states the length of
    # the scoring summary.  Each goal is 6 bytes, so a value of 60, for example,
    # means that there are 10 goals.

    # Then each 6-byte chunk is a goal.

    # The first two bytes are the timing.  0000 - 3FFF is a first period goal,
    # 4000 - 7FFF is a second period goal, 8000 - BFFF is a third period goal, 
    # and C000 - FFFF is an overtime goal.

    # The third byte is the team and PP status.

    # Home team: 00 - SH2, 01 - SH, 02 - even, 03 - PP, 04 - PP2
    # Away team: 80 - SH2, 81 - SH, 82 - even, 83 - PP, 84 - PP2

    # The fourth byte is the roster slot that scored the goal.  The fifth and
    # sixth bytes are the roster slots of the players that assisted.  If there
    # were no assists this value will be FFFF.

    # The output is a list of all goals.  The order of the list for each
    # individual goal is [Period Number, Time of Period in seconds, Home or
    # Away, PP/PP2/Even/SH/SH2, Goal Scorer, Assist #1, Assist #2.  If
    # any of the assist positions have a value of 255, no player recorded
    # that assist.

    # Get the number of goals in the summary
    numGoals = int(saveState[59627] / 6)

    goalsList = []

    # Dictionary containing strings corresponding to each goal type/team
    # strength code.
    teamPPDict = {0: 'Home SH2', 1: 'Home SH', 2: 'Home Even',
                  3: 'Home PP', 4: 'Home PP2',
                  128: 'Away SH2', 129: 'Away SH', 130: 'Away Even',
                  131: 'Away PP', 132: 'Away PP2'}

    numGoalsProcessed = 0

    while numGoalsProcessed < numGoals:

        # Blank list for an individual goal
        singleGoalList = []

        # Get the timing of the goal, period and time, and append
        if saveState[59628+numGoalsProcessed*6] < 64:
            perFactor = 0
        elif saveState[59628+numGoalsProcessed*6] >= 64 and saveState[59628+numGoalsProcessed*6] < 128:
            perFactor = 64
        elif saveState[59628+numGoalsProcessed*6] >= 128 and saveState[59628+numGoalsProcessed*6] < 184:
            perFactor = 128
        else:
            perFactor = 192

        singleGoalList.append(int(perFactor/64+1))

        singleGoalList.append((int(saveState[59628+numGoalsProcessed*6]-perFactor)*256)+
                         int(saveState[59628+numGoalsProcessed*6+1]))

        # Get home/away and team strength status of the goal and append
        goalStatus = teamPPDict[int(saveState[59628+numGoalsProcessed*6+2])].split()

        singleGoalList.append(goalStatus[0])
        singleGoalList.append(goalStatus[1])

        # Get the goal scorer
        singleGoalList.append(saveState[59628+numGoalsProcessed*6+3])

        # Get the assists.  A value of 255 means no assist in that slot.
        singleGoalList.append(saveState[59628+numGoalsProcessed*6+4])
        singleGoalList.append(saveState[59628+numGoalsProcessed*6+5])

        # Add the single goal summary to the master list
        goalsList.append(singleGoalList)

        del singleGoalList

        numGoalsProcessed += 1

    return goalsList        

################################################################################

def processScoringSummary(scoringSummary, homePlayers, awayPlayers):

    # scoringSummary is an input list generated by getScoringSummary.

    # Each item in the input list is processed in turn.  The order and format
    # of items in each processed goal summary is ["Home" or "Away",
    # Period ("1st", "2nd", "3rd", or "OT"), Time of Period ("XX:XX" format),
    # Team Strength ("SH2", "SH", "PP", "PP2", or a blank string for even
    # strength), Scorer, Assist #1, Assist #2].  If a goal did not have
    # 2 players with an assist, those spots in the list are empty.

    # The final output is a processed list of all the goals.

    fullSummary = []

    # Dictionary containing strings corresponding to each numerical period.
    periodDict = {1: "1st", 2: "2nd", 3: "3rd", 4: "OT"}

    for goal in scoringSummary:

        # Blank list for a new goal summary
        oneGoalSummary = []

        # Get the team that scored it, home or away
        oneGoalSummary.append(goal[2])
        
        # Get the period
        oneGoalSummary.append(periodDict[goal[0]])

        # Get the number of seconds in the time of goal as a string.  If it's
        # a one-digit number, at a "0" to the start of the string.
        numSeconds = str(goal[1] % 60)

        if len(numSeconds) == 1:
            numSeconds = "0" + numSeconds

        # Get the time of the goal in "XX:XX" format
        oneGoalSummary.append(str(int(goal[1]/60)) + ":" + numSeconds)

        # Get the goal type.  If it's even strength, just use a blank string
        # instead of "Even".
        if goal[3] == 'Even':
            goalStatus = ""
        else:
            goalStatus = goal[3]
            
        oneGoalSummary.append(goalStatus)

        # Get the scorer
        if goal[2] == "Home":
            oneGoalSummary.append(homePlayers[goal[4]])
        else:
            oneGoalSummary.append(awayPlayers[goal[4]])

        # Get the assists.  If there's a blank, leave a blank entry in the list
        if goal[2] == "Home":
            if goal[5] == 255:
                oneGoalSummary.append('')
            else:
                oneGoalSummary.append(homePlayers[goal[5]])
            if goal[6] == 255:
                oneGoalSummary.append('')
            else:
                oneGoalSummary.append(homePlayers[goal[6]])
        else:
            if goal[5] == 255:
                oneGoalSummary.append('')
            else:
                oneGoalSummary.append(awayPlayers[goal[5]])
            if goal[6] == 255:
                oneGoalSummary.append('')
            else:
                oneGoalSummary.append(awayPlayers[goal[6]])

        # Add the processed goal summary to the master list
        fullSummary.append(oneGoalSummary)

        del oneGoalSummary

    return fullSummary

################################################################################

def getPenaltySummary(saveState):

    # The penalty summary begins at 59989.  This byte states the length of
    # the penalty summary.  Each penalty is 4 bytes, so a value of 60, for
    # example, means that there are 15 penalty.

    # Then each 4-byte chunk is a penalty.

    # The first two bytes are the timing.  0000 - 3FFF is a first period
    # penalty, 4000 - 7FFF is a second period goal, 8000 - BFFF is a third
    # period goal, and C000 - FFFF is an overtime goal.

    # The third byte is the team and type of penalty.

    # The types of penalties and their codes are:

    # 18: 'Home Boarding', 22: 'Home Charging', 24: 'Home Slashing',
    # 26: 'Home Roughing', 28: 'Home Cross-Checking', 30: 'Home Hooking',
    # 32: 'Home Tripping', 34: 'Home Interference', 36: 'Home Holding',
    # 38: 'Home Fighting'

    # 146: 'Away Boarding', 150: 'Away Charging', 152: 'Away Slashing',
    # 154: 'Away Roughing', 156: 'Away Cross-Checking', 158: 'Away Hooking',
    # 160: 'Away Tripping', 162: 'Away Interference', 164: 'Away Holding',
    # 166: 'Away Fighting'

    # The fourth byte is the roster slot that committed the penalty.

    # The output is a list of all penalties.  The order of the list for each
    # individual penalty is [Period Number, Time of Period in seconds,
    # Home or Away, Type of Penalty, Player].

    # Get the number of penalties in the summary
    numPenalties = int(saveState[59989] / 4)

    penaltiesList = []

    # A dictionary containing strings corresponding to each numerical penalty
    teamPenDict = {18: 'Home Boarding', 22: 'Home Charging', 24: 'Home Slashing',
                  26: 'Home Roughing', 28: 'Home Cross-Checking', 30: 'Home Hooking',
                  32: 'Home Tripping', 34: 'Home Interference', 36: 'Home Holding',
                  38: 'Home Fighting',
                  146: 'Away Boarding', 150: 'Away Charging', 152: 'Away Slashing',
                  154: 'Away Roughing', 156: 'Away Cross-Checking', 158: 'Away Hooking',
                  160: 'Away Tripping', 162: 'Away Interference', 164: 'Away Holding',
                  166: 'Away Fighting'}

    numPenaltiesProcessed = 0

    while numPenaltiesProcessed < numPenalties:

        # Blank list for a single penalty        
        singlePenaltyList = []

        # Get the timing of the penalty, period and time, and append
        if saveState[59990+numPenaltiesProcessed*4] < 64:
            perFactor = 0
        elif saveState[59990+numPenaltiesProcessed*4] >= 64 and saveState[59990+numPenaltiesProcessed*4] < 128:
            perFactor = 64
        elif saveState[59990+numPenaltiesProcessed*4] >= 128 and saveState[59990+numPenaltiesProcessed*4] < 184:
            perFactor = 128
        else:
            perFactor = 192

        singlePenaltyList.append(int(perFactor/64+1))

        singlePenaltyList.append((int(saveState[59990+numPenaltiesProcessed*4]-perFactor)*256)+
                         int(saveState[59990+numPenaltiesProcessed*4+1]))

        # Get the team and type of penalty        
        penaltyTeamAndType = teamPenDict[int(saveState[59990+numPenaltiesProcessed*4+2])].split()

        singlePenaltyList.append(penaltyTeamAndType[0])
        singlePenaltyList.append(penaltyTeamAndType[1])

        # Get the player who committed the penalty
        singlePenaltyList.append(saveState[59990+numPenaltiesProcessed*4+3])

        # Add the single penalty summary to the master list        
        penaltiesList.append(singlePenaltyList)

        del singlePenaltyList

        numPenaltiesProcessed += 1

    return penaltiesList   
        
################################################################################

def processPenaltySummary(penaltySummary, homePlayers, awayPlayers):

    # penaltySummary is an input list generated by getPenaltySummary.

    # Each item in the input list is processed in turn.  The order and format
    # of items in each processed penalty summary is ["Home" or "Away",
    # Period ("1st", "2nd", "3rd", or "OT"), Time of Period ("XX:XX" format),
    # Player].

    # The final output is a processed list of all the penalties.

    fullSummary = []

    # Dictionary containing strings corresponding to each numerical period.
    periodDict = {1: "1st", 2: "2nd", 3: "3rd", 4: "OT"}

    for penalty in penaltySummary:

        # Blank list for a new penalty summary
        onePenaltySummary = []

        # Get the team that did it, home or away
        onePenaltySummary.append(penalty[2])
        
        # Get the period
        onePenaltySummary.append(periodDict[penalty[0]])

        # Get the number of seconds in the time of penalty as a string.  If it's
        # a one-digit number, at a "0" to the start of the string.
        numSeconds = str(penalty[1] % 60)

        if len(numSeconds) == 1:
            numSeconds = "0" + numSeconds

        # Get the time of the penalty in "X:XX" format
        onePenaltySummary.append(str(int(penalty[1]/60)) + ":" + numSeconds)

        # Get the penalty type
        onePenaltySummary.append(penalty[3])

        # Get the player
        if penalty[2] == "Home":
            onePenaltySummary.append(homePlayers[penalty[4]])
        else:
            onePenaltySummary.append(awayPlayers[penalty[4]])

        # Add the single processed penalty summary to the master list
        fullSummary.append(onePenaltySummary)

        del onePenaltySummary

    return fullSummary        

################################################################################

def processTeamSS(saveState, whichTeam, homeOrAway):

    # processTeamSS gets the home or away team's team statistics as specified.
    # The offsets below are the locations in the ROM for both teams'
    # team statistics and the order in which they appear.  processTeamSS
    # returns the time statistics as formatted strings.  All other entries are
    # integers.  All statistics are returned in order in a list.

    #Team offsets:
    #Team, Goals, Shots, Shooting Pct., PP Goals, PP Attempts,
    #PP Pct., Penalties, Penalty Mins, Attack Zone Time,
    #Faceoffs Won, Body Checks, Pass Attempts, Pass Completions,
    #Passing Pct.
    #1st Period Goals, 2nd Period Goals, 3rd Period Goals, OT Goals,
    #1st Period Shots, 2nd Period Shots, 3rd Period Shots,
    #OT Shots, PP Time, PP Shots, SH Goals, Breakaway Attempts,
    #Breakaway Goals, One Timer Attempts, One Timer Goals,
    #Penalty Shot Attempts, Penalty Shot Goals

    if homeOrAway == "home":

        offsets = [60242,60230,60232,60234,60236,60238,60240,
                   60244,60246,60248,60250,61064,61066,61068,
                   61070,61072,61074,61076,61078,61080,61082,
                   61084,61086,61088,61090,61092,61094,61096]

    else:

        offsets = [61110,61098,61100,61102,61104,61106,61108,
                   61112,61114,61116,61118,61932,61934,61936,
                   61938,61940,61942,61944,61946,61948,61950,
                   61952,61954,61956,61958,61960,61962,61964]

    teamTable = []

    teamTable.append(whichTeam)

    count = 0

    for offset in offsets:

        # Treat the time items differently
        if count == 6 or count == 19:
            numMinutes = str(int((saveState[offset]*256+saveState[offset+1])/60))
            numSeconds = str((saveState[offset]*256+saveState[offset+1])%60)
            if len(numSeconds) == 1:
                numSeconds = "0" + numSeconds
            teamTable.append(numMinutes + ":" + numSeconds)
            
        else:
            
            teamTable.append(int(saveState[offset])*256+int(saveState[offset+1]))

        # Calculate Shooting Pct.

        if count == 1:

            if teamTable[-1] == 0:

                teamTable.append(0)

            else:

                teamTable.append(teamTable[-2]/teamTable[-1])

        # Calculate a percentage
        if count in [3,10,23,25,27]:

            if teamTable[-1] == 0 or teamTable[-2] == 0:

                teamTable.append(0)

            else:
                teamTable.append(teamTable[-1]/teamTable[-2])

        # PP Shooting Percentage
        if count == 20:

            if teamTable[-1] == 0 or teamTable[4] == 0:

                teamTable.append(0)

            else:

                teamTable.append(teamTable[-1]/teamTable[4])
            
        count += 1

    return teamTable

################################################################################

def getTeamSS(teamCode, teamAbbrevs):

    # getTeamSS takes as input the code from the savestate for the identity
    # of one of the teams and returns a string with that team's long
    # identifier.

    ##00 = ANA
    ##01 = BOS
    ##02 = BUF
    ##03 = CGY
    ##04 = CHI
    ##05 = DAL
    ##06 = DET
    ##07 = EDM
    ##08 = FLA
    ##09 = HFD/CAR
    ##0A = LA
    ##0B = MTL
    ##0C = NJ
    ##0D = NYI
    ##0E = NYR
    ##0F = OTT
    ##10 = PHI
    ##11 = PIT
    ##12 = QUE/TOR
    ##13 = SJ
    ##14 = STL
    ##15 = TB
    ##16 = TOR/COL
    ##17 = VAN
    ##18 = WSH
    ##19 = WIN/PHO
    ##1A = ASE/CLB/NSH
    ##1B = ASW/MIN/ATL

    teamsData = [[0,"Anaheim"],[1,"Boston"],[2,"Buffalo"],[3,"Calgary"],
                 [4,"Chicago"],[5,"Dallas"],[6,"Detroit"],[7,"Edmonton"],
                 [8,"Florida"],[9,"Hartford"],[10,"Los Angeles"],[11,"Montreal"],
                 [12,"New Jersey"],[13,"Islanders"],[14,"Rangers"],[15,"Ottawa"],
                 [16,"Philadelphia"],[17,"Pittsburgh"],[18,"Quebec"],[19,"San Jose"],
                 [20,"St. Louis"],[21,"Tampa Bay"],[22,"Toronto"],[23,"Vancouver"],
                 [24,"Washington"],[25,"Winnipeg"],[26,"All Stars East"],
                 [27,"All Stars West"]]

    for team in teamAbbrevs:
        if teamCode == team[0]:
            return team[1]

################################################################################

def getPlayerStats(saveState,offset):

    # The home team player stats begin at 60410 and the away team player stats
    # begin at 61278.  Including the starting byte, the first 25 bytes list
    # the number of goals scored for each forward/defenseman and the number of
    # goals allowed for goalies.  26 bytes from the starting byte, the next 25
    # bytes list assists.  52 bytes from the starting byte, the next 25 bytes
    # list shots on goal/shots faced.  78 bytes from the starting byte, the
    # next 25 bytes list penalty minutes.  104 bytes from the starting byte,
    # next 25 bytes list checks.  130 bytes from the starting byte, the next
    # 51 bytes list time on the ice, 2 bytes per player.

    # The output is a list of players on one team with accompanying stats.
    # No distinction is made between a goalie and a forward/defenseman.

    allPlayerStats = []

    for count in range(0,26):

        # Blank list for one player's stats
        onePlayerStats = []

        # Goals Scored/Goals Allowed
        onePlayerStats.append(saveState[offset+count])

        # Assists
        onePlayerStats.append(saveState[offset+count+26])

        # Shots on Goal/Shots Faced
        onePlayerStats.append(saveState[offset+count+52])

        # Penalty Minutes
        onePlayerStats.append(saveState[offset+count+78])

        # Checks
        onePlayerStats.append(saveState[offset+count+104])

        # Time on Ice
        onePlayerStats.append((saveState[offset+count*2+130]*256)+
                              saveState[offset+count*2+131])

        allPlayerStats.append(onePlayerStats)

        del onePlayerStats

    return allPlayerStats

################################################################################

def processPlayerStats(playerStats,players):

    # processPlayerStats takes a list of player stats and a list of
    # corresponding players and combines and reorganizes them into one players
    # list suitable for export.  The output is a list of all the players and
    # their stats, each player having his own list within the master list.

    # The order of the items in each player's list depend upon whether the
    # player is a goalie or not.  In the lists, goalies come first, then
    # forwards, then defensemen.  Because the players list contains the numbers
    # of each position on the team at the end of the list, players can be
    # differentiated into goalies and non-goalies, and their stats can be
    # processed differently.

    # Stat order for goalies:
    # Player Name, Goals Conceded, Shots Faced, Save Pct., Assists, Points,
    # Penalty Minutes, Position Code (not currently used)

    # Stat order for forwards/defensemen:
    # Player Name, Goals, Assists, Points, Shots on Goal, Shooting Pct.,
    # Penalty Minutes, Position Code (not currently used)

    numGoalies    = players[-3]
    numForwards   = players[-2]
    numDefensemen = players[-1]

    count = 0

    allPlayersProcessedStats = []

    for player in playerStats:

        onePlayerProcessedStats = []

        # Get the player's name
        onePlayerProcessedStats.append(players[count])

        # Goalies are processed differently from forwards/defensemen
        if count < numGoalies:

            # Goals Conceded
            onePlayerProcessedStats.append(player[0])

            # Shots Faced
            onePlayerProcessedStats.append(player[2])

            # Save Pct.
            if player[2] == 0:
                onePlayerProcessedStats.append(0.0)
            else:
                onePlayerProcessedStats.append((player[2]-player[0])/player[2])

            # Assists
            onePlayerProcessedStats.append(player[1])

            # Points
            onePlayerProcessedStats.append(player[1])

            # Penalty Minutes
            onePlayerProcessedStats.append(player[3])

            # Checks
            onePlayerProcessedStats.append(player[4])

            # Time on Ice
            numMinutes = str(int(player[5]/60))
            numSeconds = str(player[5]%60)
            if len(numSeconds) == 1:
                numSeconds = "0" + numSeconds
                
            onePlayerProcessedStats.append(numMinutes + ":" + numSeconds)

##            # Position Code
##            onePlayerProcessedStats.append("G")

        else:

            # Goals
            onePlayerProcessedStats.append(player[0])

            # Assists
            onePlayerProcessedStats.append(player[1])

            # Points
            onePlayerProcessedStats.append(player[0] + player[1])

            # Shots on Goal
            onePlayerProcessedStats.append(player[2])

            # Shooting Pct.
            if player[2] == 0:
                onePlayerProcessedStats.append(0.0)
            else:
                onePlayerProcessedStats.append(player[0]/player[2])

            # Penalty Minutes
            onePlayerProcessedStats.append(player[3])

            # Checks
            onePlayerProcessedStats.append(player[4])

            # Time on Ice
            numMinutes = str(int(player[5]/60))
            numSeconds = str(player[5]%60)
            if len(numSeconds) == 1:
                numSeconds = "0" + numSeconds
                
            onePlayerProcessedStats.append(numMinutes + ":" + numSeconds)

##            # Position Code
##            if count < (numGoalies + numForwards):
##                onePlayerProcessedStats.append("F")
##            else:
##                onePlayerProcessedStats.append("D")

        allPlayersProcessedStats.append(onePlayerProcessedStats)

        del onePlayerProcessedStats

        count += 1

        # Stop looping if we've processed every player's stats
        if count == numGoalies + numForwards + numDefensemen:
            break

    return allPlayersProcessedStats        

################################################################################

def main(argv):
    #try:
    #    opts, args = getopt.getopt(argv,"hs:r:",["sfile=","rfile="])
        #print "".join(argv)
    #except getopt.GetoptError:
    #    print ('Error: Gens_Stat_Extractor.py -s <savefile> -r <romfile>')
    #    sys.exit(2)
    #for opt, arg in opts:
    #    if opt == '-h':
    #        print ('Gens_Stat_Extractor.py -s <savefile> -r <romfile>')
    #        sys.exit()
    #    elif opt in ("-s", "--sfile"):
    #        saveStateFile = arg
    #    elif opt in ("-r", "--rfile"):
    #        ROMFile = arg
    
    print ('starting the export process; examining rom file')
    # Get the save state data
    saveStateFile = r"C:\Users\wojciecw\Documents\apps\wgens211\Gens\WBFClassic_le.gs7"
    #saveStateFile = r"C:\Users\wojciecw\Documents\apps\wgens211\GensPlus\bak\nhl94.gs6"
    saveStateGet = open(saveStateFile,'r+')
    saveState = mmap.mmap(saveStateGet.fileno(),0)

    # Get the ROM data
    #ROMFile = r"C:\Users\wojciecw\Documents\apps\wgens211\Gens\roms\nhl94_playoffs.bin"
    #ROMFile = r"C:\Users\wojciecw\Documents\apps\wgens211\roms\NHL94.2015_Skip.bin"
    ROMFile = r"C:\Users\wojciecw\Documents\apps\wgens211\Gens\roms\WBFClassic_le.bin"
    ROMFileGet = open(ROMFile,'r+')
    ROM = mmap.mmap(ROMFileGet.fileno(),0)

    homeTeamTable   = []
    awayTeamTable   = []
    otherStatsTable = []

    # Get all team offsets in the ROM
    teamOffsets = getTeamOffsets(ROM)

    # Get all team abbreviations in the ROM
    teamAbbrevs = getTeamAbbrevs(ROM,teamOffsets)

    # Get the home and away team abbreviations.  Bytes 59305 and
    # 59307 contain the numbers of the teams playing.  The number
    # is equivalent to the team's order in the ROM.
    homeTeam = teamAbbrevs[saveState[59305]]
    awayTeam = teamAbbrevs[saveState[59307]]

    print(homeTeam)
    print(awayTeam)

    # Get a list of all the players on the home team and a list of
    # all the players on the away team.  The last three entries in
    # each team's list is the number of goalies, the number of
    # forwards, and the number of defensemen.
    homePlayers = getTeamPlayersFromROM(ROM,homeTeam,teamOffsets)
    awayPlayers = getTeamPlayersFromROM(ROM,awayTeam,teamOffsets)

    # A list of all the team statistics the game tracks.
    # Passing Pct. is calculated from Pass Completions and Pass
    # Attempts.
    statList = ("Team", "Goals", "Shots", "Shooting Pct.", "PP Goals",
                "PP Attempts", "PP Pct.", "Penalties", "Penalty Mintues",
                "Attack Zone Time", "Faceoffs Won", "Body Checks",
                "Pass Attempts", "Pass Completions", "Passing Pct.",
                "1st Period Goals", "2nd Period Goals", "3rd Period Goals",
                "OT Goals", "1st Period Shots", "2nd Period Shots",
                "3rd Period Shots", "OT Shots", "Power Play Time",
                "PP Shots", "PP Shooting Pct.", "SH Goals", "Breakaway Attempts",
                "Breakaway Goals", "Breakaway Pct.", "One Timer Attempts",
                "One Timer Goals", "One Timer Pct.", "Penalty Shot Attempts",
                "Penalty Shot Goals", "Penalty Shot Pct.")

    # Get a nested list of game statistics.  Each entry is the name
    # of the statistic, the home team's value, and the away team's value.
    homeTeamStats = processTeamSS(saveState,homeTeam,"home")
    awayTeamStats = processTeamSS(saveState,awayTeam,"away")

    teamsStats = []

    count = 0

    while count < len(homeTeamStats):

        teamsStats.append((statList[count],awayTeamStats[count],
                          homeTeamStats[count]))

        count += 1
    ##teamsStats = zip(statList,processTeamSS(saveState,homeTeam,"home")
    ##                 ,processTeamSS(saveState,awayTeam,"away"))

    # Insert faceoff win percentage
    if teamsStats[10][1] == 0:

        homeFaceoffWinPct = 0

    else:

        homeFaceoffWinPct = teamsStats[10][1]/(teamsStats[10][1]+teamsStats[10][2])

    if teamsStats[10][2] == 0:

        awayFaceoffWinPct = 0

    else:

        awayFaceoffWinPct = teamsStats[10][2]/(teamsStats[10][1]+teamsStats[10][2])

    teamsStats.insert(11,("Faceoff Win Pct.",homeFaceoffWinPct,awayFaceoffWinPct))

    # Get a nested list of all goals scored
    scoringSummary = getScoringSummary(saveState)
    scoringSummaryProcessed = processScoringSummary(scoringSummary,homePlayers,awayPlayers)

    # Get a nested list of all penalties
    penaltySummary = getPenaltySummary(saveState)
    penaltySummaryProcessed = processPenaltySummary(penaltySummary,homePlayers,awayPlayers)

    # Get each team's scoring summary
    homeScoringSummary = []
    awayScoringSummary = []

    for goal in scoringSummaryProcessed:
        if goal[0] == 'Home':
            homeScoringSummary.append(goal)
        else:
            awayScoringSummary.append(goal)

    # Get each team's penalty summary
    homePenaltySummary = []
    awayPenaltySummary = []

    for penalty in penaltySummaryProcessed:
        if penalty[0] == 'Home':
            homePenaltySummary.append(penalty)
        else:
            awayPenaltySummary.append(penalty)

    # Get the individual player stats
    homePlayerStats = getPlayerStats(saveState,60410)
    awayPlayerStats = getPlayerStats(saveState,61278)

    homePlayerStatsProcessed = processPlayerStats(homePlayerStats,homePlayers)
    awayPlayerStatsProcessed = processPlayerStats(awayPlayerStats,awayPlayers)

    # Close the input files
    ROMFileGet.close()
    saveStateGet.close()

    # Make a CSV writer for writing the output CSV file
    csvWriter = csv.writer(open(r"Gens_SavestateData.csv","w",newline=""))

    # Write the team stats
    for item in teamsStats:
        csvWriter.writerow(item)

    # Write the home team goals
    csvWriter.writerow('')
    csvWriter.writerow('')
    csvWriter.writerow([homeTeam + " Goals"])
    csvWriter.writerow(('Period','Time','Scorer','Assist #1','Assist #2','Team Strength'))

    for item in homeScoringSummary:
        csvWriter.writerow((item[1],item[2],item[4],item[5],item[6],item[3]))

    # Write the away team goals
    csvWriter.writerow('')
    csvWriter.writerow([awayTeam + " Goals"])
    csvWriter.writerow(('Period','Time','Scorer','Assist #1','Assist #2','Team Strength'))

    for item in awayScoringSummary:
        csvWriter.writerow((item[1],item[2],item[4],item[5],item[6],item[3]))

    # Write the home team penalties
    csvWriter.writerow('')
    csvWriter.writerow('')
    csvWriter.writerow([homeTeam + " Penalties"])
    csvWriter.writerow(('Period','Time','Penalty','Player'))

    for item in homePenaltySummary:
        csvWriter.writerow((item[1],item[2],item[4],item[3]))

    #Write the away team penalties
    csvWriter.writerow('')
    csvWriter.writerow([awayTeam + " Penalties"])
    csvWriter.writerow(('Period','Time','Penalty','Player'))

    for item in awayPenaltySummary:
        csvWriter.writerow((item[1],item[2],item[4],item[3]))

    # Write the home team player stats
    csvWriter.writerow('')
    csvWriter.writerow('')
    csvWriter.writerow([homeTeam + " Player Stats"])

    # Write the goalies' stats
    csvWriter.writerow('')
    csvWriter.writerow(["Goalie Stats"])
    csvWriter.writerow(("Name","Goals Conceded","Shots Faced","Save Pct.",
                        "Assists","Points","Penalty Minutes","Checks",
                        "Time on Ice"))

    for player in range(0,homePlayers[-3]):
        csvWriter.writerow(homePlayerStatsProcessed[player])

    # Write the forwards' stats
    csvWriter.writerow('')
    csvWriter.writerow(["Forward Stats"])
    csvWriter.writerow(("Name","Goals","Assists","Points","Shots on Goal",
                        "Shooting Pct.","Penalty Minutes","Checks",
                        "Time on Ice"))

    for player in range(homePlayers[-3],homePlayers[-3]+homePlayers[-2]):
        csvWriter.writerow(homePlayerStatsProcessed[player])

    # Write the defensemen's stats
    csvWriter.writerow('')
    csvWriter.writerow(["Defenseman Stats"])
    csvWriter.writerow(("Name","Goals","Assists","Points","Shots on Goal",
                        "Shooting Pct.","Penalty Minutes","Checks",
                        "Time on Ice"))

    for player in range(homePlayers[-3]+homePlayers[-2],homePlayers[-3]+homePlayers[-2]+homePlayers[-1]):
        csvWriter.writerow(homePlayerStatsProcessed[player])

    # Write the away team player stats
    csvWriter.writerow('')
    csvWriter.writerow('')
    csvWriter.writerow([awayTeam + " Player Stats"])

    # Write the goalies' stats
    csvWriter.writerow('')
    csvWriter.writerow(["Goalie Stats"])
    csvWriter.writerow(("Name","Goals Conceded","Shots Faced","Save Pct.",
                        "Assists","Points","Penalty Minutes","Checks",
                        "Time on Ice"))

    for player in range(0,awayPlayers[-3]):
        csvWriter.writerow(awayPlayerStatsProcessed[player])

    # Write the forwards' stats
    csvWriter.writerow('')
    csvWriter.writerow(["Forward Stats"])
    csvWriter.writerow(("Name","Goals","Assists","Points","Shots on Goal",
                        "Shooting Pct.","Penalty Minutes","Checks",
                        "Time on Ice"))

    for player in range(awayPlayers[-3],awayPlayers[-2]+awayPlayers[-3]):
        csvWriter.writerow(awayPlayerStatsProcessed[player])

    # Write the defensemen's stats
    csvWriter.writerow('')
    csvWriter.writerow(["Defenseman Stats"])
    csvWriter.writerow(("Name","Goals","Assists","Points","Shots on Goal",
                        "Shooting Pct.","Penalty Minutes","Checks",
                        "Time on Ice"))

    for player in range(awayPlayers[-2]+awayPlayers[-3],awayPlayers[-2]+awayPlayers[-3]+awayPlayers[-1]):
        csvWriter.writerow(awayPlayerStatsProcessed[player])

    del csvWriter

    print("stats extracted to Gens_SavestateData.csv")

if __name__ == "__main__":
    main(sys.argv[1:])
