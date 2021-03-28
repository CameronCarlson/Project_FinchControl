using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinchAPI;

namespace Project_FinchControl
{

    // ****************************************************************************************************
    //
    // Title: Finch Control
    // Description: Mission 3 code to control the finch robot
    // Application Type: Console
    // Author: Cameron Carlson
    // Dated Created: 2/17/2021
    // Last Modified: 3/22/2021
    //
    // ****************************************************************************************************

    public enum Command
    {
            //NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            GETTEMPERATURE,
            GETLEFTLIGHTSENSOR,
            GETRIGHTLIGHTSENSOR,
            GETALLSENSORVALUES,
            DANCE,
            DONE
    }
    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.SetWindowSize(150, 40);
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserPrrogrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region USER PROGRAMMING

        /// <summary>
        /// User Programming Main Menu
        /// </summary>
        /// <param name="finchRobot"></param>
        static void UserPrrogrammingDisplayMenuScreen(Finch finchRobot)
        {
            bool quitMenu = false;
            string menuChoice;

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters = (0, 0, 0);
            List<Command> commands = null;
            

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\te) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;
                    case "b":
                        commands = UserProgrammingDisplayGetFinchCommands();
                        break;

                    case "c":
                        UserProgrammingDisplayViewCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteCommands(finchRobot, commands, commandParameters);
                        break;

                    case "e":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Exexute Commands Given by User
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="commands"></param>
        /// <param name="commandParameters"></param>
        static void UserProgrammingDisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            DisplayScreenHeader("Exexute Commands");
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            double waitSeconds = commandParameters.waitSeconds;
            double temperatureSensorValue;
            int leftLightLevel;
            int rightLightLevel;
            int waitMilliseconds = (int)(waitSeconds * 1000);

            Console.WriteLine("\tThe Finch Robot will now execute all commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    //case Command.NONE:
                    //    Console.WriteLine();
                    //    Console.WriteLine("\tDefault Value Error");
                    //    Console.WriteLine();
                    //    break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);

                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliseconds);
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(commandParameters.motorSpeed, 0);
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(0, commandParameters.motorSpeed);
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case Command.GETTEMPERATURE:
                        temperatureSensorValue = finchRobot.getTemperature();
                        Console.WriteLine($"\t\tCurrent Temperature:{temperatureSensorValue}°C");
                        break;

                    case Command.GETLEFTLIGHTSENSOR:
                        leftLightLevel = finchRobot.getLeftLightSensor();
                        Console.WriteLine($"\t\tCurrent Left Light Sensor:{leftLightLevel}");
                        break;

                    case Command.GETRIGHTLIGHTSENSOR:
                        rightLightLevel = finchRobot.getRightLightSensor();
                        Console.WriteLine($"\t\tCurrent Right Light Sensor:{rightLightLevel}");
                        break;

                    case Command.GETALLSENSORVALUES:
                        temperatureSensorValue = finchRobot.getTemperature();
                        Console.WriteLine($"\t\tCurrent Temperature:{temperatureSensorValue}°C");

                        leftLightLevel = finchRobot.getLeftLightSensor();
                        Console.WriteLine($"\t\tCurrent Left Light Sensor:{leftLightLevel}");

                        rightLightLevel = finchRobot.getRightLightSensor();
                        Console.WriteLine($"\t\tCurrent Right Light Sensor:{rightLightLevel}");
                        break;

                    case Command.DANCE:
                        finchRobot.setMotors(commandParameters.motorSpeed, -1 * commandParameters.motorSpeed);
                        UserProgrammingDisplayFlashLightsAndSound(commandParameters, finchRobot);
                        finchRobot.setMotors(-1 * commandParameters.motorSpeed, commandParameters.motorSpeed);
                        UserProgrammingDisplayFlashLightsAndSound(commandParameters, finchRobot);
                        finchRobot.setMotors(0, 0);
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case Command.DONE:

                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tUnkown Command Error");
                        Console.WriteLine();
                        break;
                }

                Console.WriteLine($"\tCommand: {command}");
            }
            finchRobot.setLED(0, 0, 0);
            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// Flash lights and sound for dance in user programming
        /// </summary>
        /// <param name="commandParameters"></param>
        /// <param name="finchRobot"></param>
        static void UserProgrammingDisplayFlashLightsAndSound((int motorSpeed, int ledBrightness, double waitSeconds) commandParameters, Finch finchRobot)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            double waitSeconds = commandParameters.waitSeconds;
            int waitMilliseconds = (int)(waitSeconds * 1000);

            for (int i = 0; i < 4; i++)
            {
                finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                finchRobot.noteOn(10000);
                finchRobot.wait((int)(.25 * waitMilliseconds));

                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
                finchRobot.wait((int)(.25 * waitMilliseconds));
            }
        }

        /// <summary>
        /// Display Commands Entered by user
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgrammingDisplayViewCommands(List<Command> commands)
        {
            DisplayScreenHeader("View Commands");

            Console.WriteLine("\tCommand List");
            Console.WriteLine("\t------------");
            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static List<Command> UserProgrammingDisplayGetFinchCommands()
        {
            Console.SetWindowSize(220, 40);
            List<Command> commands = new List<Command>();
            bool isDone = false;
            string userResponse;
            Command command;

            DisplayScreenHeader("Get Finch Commands");

            Console.Write("\tEnter Commands [ | ");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"{commandName} | ");
            }
            Console.WriteLine("]");
            Console.WriteLine("\tType \"done\" when finished");
            Console.WriteLine();

            do
            {
                Console.Write("\tCommand:");
                userResponse = Console.ReadLine();
                if (userResponse != "done")
                {
                    if (Enum.TryParse(userResponse.ToUpper(), out command))
                    {
                        commands.Add(command);
                    }
                    else
                    {
                        Console.WriteLine("\t\tCommand Unkown. Enter Command From The Listed Above.");
                    }
                }
                else
                {
                    isDone = true;
                }
            } while (!isDone);

            DisplayMenuPrompt("User Programming");
            Console.SetWindowSize(150, 40);
            return commands;
            
        }

        /// <summary>
        /// Get parameters from user
        /// </summary>
        /// <returns>commandParameters</returns>
        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            bool validResponse;
            string userResponse;
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            
            DisplayScreenHeader("Get Parameters");
            do
            {
                validResponse = true;
                Console.Write("\tMotor Spreed [0 to 255]:");
                userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out commandParameters.motorSpeed) && commandParameters.motorSpeed >= 0 && commandParameters.motorSpeed <= 255)
                {

                }
                else
                {
                    Console.WriteLine("\tPlease Enter a Valid Number [0 to 255]");
                    Console.WriteLine();
                    validResponse = false;
                }
            } while (!validResponse);
            Console.WriteLine();

            do
            {
                validResponse = true;
                Console.Write("\tLED Brightness [0 to 255]:");
                userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out commandParameters.ledBrightness) && commandParameters.ledBrightness >= 0 && commandParameters.ledBrightness <= 255)
                {

                }
                else
                {
                    Console.WriteLine("\tPlease Enter a Valid Number [0 to 255]");
                    Console.WriteLine();
                    validResponse = false;
                }
            } while (!validResponse);
            Console.WriteLine();

            do
            {
                validResponse = true;
                Console.Write("\tWait Time (Seconds):");
                userResponse = Console.ReadLine();
                if (double.TryParse(userResponse, out commandParameters.waitSeconds) && commandParameters.waitSeconds >= 0)
                {

                }
                else
                {
                    Console.WriteLine("\tPlease Enter a Valid Positive Number");
                    Console.WriteLine();
                    validResponse = false;
                }
            } while (!validResponse);

            //
            // Echo command parameters
            //
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\tParameters Entered");
            Console.WriteLine();
            Console.WriteLine($"\tMotor Speed:{commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED Brightness:{commandParameters.ledBrightness}");
            Console.WriteLine($"\tWait Time (Seconds):{commandParameters.waitSeconds}");

            DisplayMenuPrompt("User Programming");

            return commandParameters;
        }

        #endregion

        #region ALARM SYSTEM

        /// <summary>
        /// Alarm System Menu
        /// </summary>
        /// <param name="finchRobot"></param>
        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxLightThresholdValue = 0;
            double minMaxTemperatureThresholdValue = 0;
            int timeToMonitor = 0;
            string alarmType = "";

            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Temperature or Light Alarm");
                Console.WriteLine("\tb) Set Sensors to Monitor [Light Alarm Only]");
                Console.WriteLine("\tc) Set Range Type");
                Console.WriteLine("\td) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\te) Set Time to Monitor");
                Console.WriteLine("\tf) Set Alarm");
                Console.WriteLine("\tg) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        alarmType = AlarmSystemDisplaySetAlarmType();
                        break;
                    case "b":
                        sensorsToMonitor = AlarmSystemDisplaySetSensors();
                        break;

                    case "c":
                        rangeType = AlarmSystemDisplaySetRangeType();
                        break;

                    case "d":
                        if (alarmType == "temperature")
                        {
                            minMaxTemperatureThresholdValue = AlarmSystemDisplayGetTemperatureThresholdValue(sensorsToMonitor, finchRobot, rangeType);
                        }
                        else if (alarmType == "light")
                        {
                            minMaxLightThresholdValue = AlarmSystemDisplayGetLightThresholdValue(sensorsToMonitor, finchRobot, rangeType);
                        }
                        else
                        {
                            DisplayScreenHeader("Threshold Value");

                            Console.WriteLine("\tDidn't Enter Alarm Type. Return to Alarm System Menu");

                            DisplayMenuPrompt("Alarm System");
                        }
                        break;

                    case "e":
                        timeToMonitor = AlarmSystemDisplayGetTimeToMonitor();
                        break;

                    case "f":
                        if (alarmType == "temperature")
                        {
                            AlarmSystemDisplaySetTemperatureAlarm(finchRobot, rangeType, minMaxTemperatureThresholdValue, timeToMonitor);
                        }
                        else if (alarmType == "light")
                        {
                            AlarmSystemDisplaySetLightAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxLightThresholdValue, timeToMonitor);
                        }
                        else
                        {
                            DisplayScreenHeader("Set Alarm");

                            Console.WriteLine("\tDidn't Enter Alarm Type. Return to Alarm System Menu");

                            DisplayMenuPrompt("Alarm System");
                        }
                        break;

                    case "g":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Temperature alarm menu and set temperature alarm
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxTemperatureThresholdValue"></param>
        /// <param name="timeToMonitor"></param>
        static void AlarmSystemDisplaySetTemperatureAlarm(Finch finchRobot, string rangeType, double minMaxTemperatureThresholdValue, int timeToMonitor)
        {
            bool thresholdExceeded = false;
            int secondsElapsed = 1;
            double temperatureValue;

            DisplayScreenHeader("Set Alarm");

            // Echo Values to user
            Console.WriteLine($"\tThreshold Entered: {minMaxTemperatureThresholdValue}°C");
            Console.WriteLine($"\tRange Type Entered: {rangeType}");
            Console.WriteLine($"\tTime to Monitor Entered: {timeToMonitor}");

            // Prompt user to start
            DisplayContinuePrompt();

            do
            {
                Console.Clear();

                DisplayScreenHeader("Set Alarm");

                // Echo Values to user
                Console.WriteLine($"\tThreshold Entered: {minMaxTemperatureThresholdValue}°C");
                Console.WriteLine($"\tRange Type Entered: {rangeType}");
                Console.WriteLine($"\tTime to Monitor Entered: {timeToMonitor}");
                //
                // Get current current light levels
                //
                temperatureValue = finchRobot.getTemperature();

                //
                // Display Current Light Levels
                //
                Console.WriteLine();
                Console.WriteLine($"\tCurrent Temperature: {(temperatureValue).ToString("n1")}");

                //
                // Wait 1 second and increment seconds
                //
                Console.WriteLine();
                Console.WriteLine($"\tTime Remaining: {timeToMonitor - secondsElapsed}");
                finchRobot.wait(1000);
                secondsElapsed++;

                //
                // Test for threshold exceeded
                //
                if (rangeType == "minimum")
                {
                    thresholdExceeded = (temperatureValue < minMaxTemperatureThresholdValue);
                }
                else   // maximum
                {
                    thresholdExceeded = (temperatureValue > minMaxTemperatureThresholdValue);
                }

            } while (!thresholdExceeded && secondsElapsed <= timeToMonitor);

            //
            // display result of alarm
            //
            Console.WriteLine();
            if (thresholdExceeded)
            {
                Console.WriteLine("\tThreshold Exceeded");
            }
            else
            {
                Console.WriteLine("\tThreshold Not Exceeded - Time Limit Reached");
            }

            DisplayMenuPrompt("Alarm System");
        }

        /// <summary>
        /// Get threshhold value from user for temperature alarm
        /// </summary>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="finchRobot"></param>
        /// <param name="rangeType"></param>
        /// <returns></returns>
        static double AlarmSystemDisplayGetTemperatureThresholdValue(string sensorsToMonitor, Finch finchRobot, string rangeType)
        {
            double thresholdValue = 0;
            bool validResponse;
            bool validRangeType = true;
            string userResponse;
            double currentTemperatureSensorValue = finchRobot.getTemperature();

            do
            {
                validResponse = true;
                DisplayScreenHeader("Threshold Value");

                //
                // Display ambient temperature values
                //
                Console.WriteLine($"\tCurrent Temperature Sensor Value: {(currentTemperatureSensorValue).ToString("n1")}°C");
                Console.WriteLine();

                //
                // Did they enter a range type
                //
                if (rangeType != "minimum" && rangeType != "maximum")
                {
                    validRangeType = false;
                }

                //
                // If user entered range type, ask for threshold
                //
                if (validRangeType)
                {
                    //
                    // get threshold from user
                    //
                    Console.Write("\tEnter Threshold:");
                    userResponse = Console.ReadLine();

                    if (rangeType == "minimum")
                    {
                        if (double.TryParse(userResponse, out thresholdValue) && thresholdValue >= currentTemperatureSensorValue)
                        {
                            Console.WriteLine("\tPlease Enter a Threshold Less then the Current Temperature [Range Type Entered: Minimum]");
                            validResponse = false;
                            Console.ReadKey();
                        }
                        else if (!double.TryParse(userResponse, out thresholdValue))
                        {
                            Console.WriteLine("\tPlease Enter a Threshold Less then the Current Temperature [Range Type Entered: Minimum]");
                            validResponse = false;
                            Console.ReadKey();
                        }
                        else
                        {
                            double.TryParse(userResponse, out thresholdValue);
                        }
                    }
                    else     //rangeType = maximum
                    {
                        if (double.TryParse(userResponse, out thresholdValue) && thresholdValue <= currentTemperatureSensorValue)
                        {
                            Console.WriteLine("\tPlease Enter a Threshold Higher then the Current Temperature [Range Type Entered: Maximum]");
                            validResponse = false;
                            Console.ReadKey();
                        }
                        else if (!double.TryParse(userResponse, out thresholdValue))
                        {
                            Console.WriteLine("\tPlease Enter a Threshold Higher then the Current Temperature [Range Type Entered: Maximum]");
                            validResponse = false;
                            Console.ReadKey();
                        }
                        else
                        {
                            double.TryParse(userResponse, out thresholdValue);
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tYou Didn't Enter a Range Type. Go to Alarm System Menu.");
                }
            } while (!validResponse);

            //
            // Display threshold value if there is a sensor reference and range type
            //
            if (validRangeType)
            {
                Console.WriteLine();
                Console.WriteLine($"\tThreshold Entered: {thresholdValue}°C");
            }
            DisplayMenuPrompt("Alarm System");

            return thresholdValue;
        }

        /// <summary>
        /// Get alarm type from user (temperature or light)
        /// </summary>
        /// <returns>alarmType</returns>
        static string AlarmSystemDisplaySetAlarmType()
        {
            string alarmType;
            bool validResponse;
            do
            {
                validResponse = true;
                DisplayScreenHeader("Alarm Type");

                Console.Write("\tEnter an Alarm Type [temperature, light]:");
                alarmType = Console.ReadLine().ToLower();
                if (alarmType != "temperature" && alarmType != "light")
                {
                    Console.WriteLine();
                    Console.WriteLine("\tEnter a Valid Alarm Type [temperature, light]");
                    validResponse = false;
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\tAlarm Type Entered: {alarmType}");

            DisplayMenuPrompt("Alarm System");

            return alarmType;
        }

        /// <summary>
        /// Light alarm menu and start light alarm
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThresholdValue"></param>
        /// <param name="timeToMonitor"></param>
        static void AlarmSystemDisplaySetLightAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {
            bool thresholdExceeded = false;
            int secondsElapsed = 1;
            int leftLightSensorValue;
            int rightLightSensorValue;

            DisplayScreenHeader("Set Alarm");

            // Echo Values to user
            Console.WriteLine($"\tSensors to Minitor Entered: {sensorsToMonitor}");
            Console.WriteLine($"\tRange Type Entered: {rangeType}");
            Console.WriteLine($"\tThreshold Entered: {minMaxThresholdValue}");
            Console.WriteLine($"\tTime to Monitor: {timeToMonitor}");

            // Prompt user to start
            DisplayContinuePrompt();

            do
            {
                Console.Clear();

                DisplayScreenHeader("Set Alarm");

                // Echo Values to user
                Console.WriteLine($"\tSensors to Minitor Entered: {sensorsToMonitor}");
                Console.WriteLine($"\tRange Type Entered: {rangeType}");
                Console.WriteLine($"\tThreshold Entered: {minMaxThresholdValue}");
                Console.WriteLine($"\tTime to Monitor: {timeToMonitor}");
                //
                // Get current current light levels
                //
                leftLightSensorValue = finchRobot.getLeftLightSensor();
                rightLightSensorValue = finchRobot.getRightLightSensor();

                //
                // Display Current Light Levels
                //
                Console.WriteLine();
                switch (sensorsToMonitor)
                {
                    case "left":
                        Console.WriteLine($"\tCurrent Left Light Sensor: {leftLightSensorValue}");
                        break;

                    case "right":
                        Console.WriteLine($"\tCurrent Right Light Sensor: {rightLightSensorValue}");
                        break;

                    case "both":
                        Console.WriteLine($"\tCurrent Left Light Sensor: {leftLightSensorValue}");
                        Console.WriteLine($"\tCurrent Right Light Sensor: {rightLightSensorValue}");
                        break;

                    default:
                        Console.WriteLine("\tUnknown Sensor Reference");
                        break;
                }

                //
                // Wait 1 second and increment seconds
                //
                Console.WriteLine();
                Console.WriteLine($"\tTime Remaining: {timeToMonitor - secondsElapsed}");
                finchRobot.wait(1000);
                secondsElapsed++;

                //
                // Test for threshold exceeded
                //
                switch (sensorsToMonitor)
                {
                    case "left":
                        if (rangeType == "minimum")
                        {
                            //if (leftLightSensorValue < minMaxThresholdValue)
                            //{
                            //    thresholdExceeded = true;
                            //}
                            thresholdExceeded = (leftLightSensorValue < minMaxThresholdValue);
                        }
                        else   // maximum
                        {
                            thresholdExceeded = (leftLightSensorValue > minMaxThresholdValue);
                        }
                        break;

                    case "right":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = (rightLightSensorValue < minMaxThresholdValue);
                        }
                        else   // maximum
                        {
                            thresholdExceeded = (rightLightSensorValue > minMaxThresholdValue);
                        }
                        break;

                    case "both":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = (leftLightSensorValue < minMaxThresholdValue || rightLightSensorValue < minMaxThresholdValue);
                        }
                        else   // maximum
                        {
                            thresholdExceeded = (leftLightSensorValue > minMaxThresholdValue || rightLightSensorValue > minMaxThresholdValue);
                        }
                        break;

                    default:
                        
                        break;
                }

            } while (!thresholdExceeded && secondsElapsed <= timeToMonitor);

            //
            // display result of alarm
            //
            Console.WriteLine();
            if (thresholdExceeded)
            {
                Console.WriteLine("\tThreshold Exceeded");
            }
            else
            {
                Console.WriteLine("\tThreshold Not Exceeded - Time Limit Reached");
            }

            DisplayMenuPrompt("Alarm System");
        }

        /// <summary>
        /// Get Time to monitor from user
        /// </summary>
        /// <returns>timeToMonitor</returns>
        static int AlarmSystemDisplayGetTimeToMonitor()
        {
            int timeToMonitor = 0;
            string userResponse;
            bool validResponse;

            do
            {
                validResponse = true;
                DisplayScreenHeader("Time to Monitor");

                Console.Write("\tEnter Time to Monitor:");
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out timeToMonitor))
                {
                    Console.WriteLine("\tPlease Enter a Posotive Integer");
                    validResponse = false;
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (int.TryParse(userResponse, out timeToMonitor) && timeToMonitor <= 0)
                {
                    Console.WriteLine("\tPlease Enter a Posotive Integer");
                    validResponse = false;
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    int.TryParse(userResponse, out timeToMonitor);
                }
            } while (!validResponse);
            Console.WriteLine();
            Console.WriteLine($"\tTime to Monitor Entered: {timeToMonitor}");

            DisplayMenuPrompt("Alarm System");

            return timeToMonitor;
        }

        /// <summary>
        /// Get threshold value from the user for light alarm
        /// </summary>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="finchRobot"></param>
        /// <returns>thresholfValue</returns>
        static int AlarmSystemDisplayGetLightThresholdValue(string sensorsToMonitor, Finch finchRobot, string rangeType)
        {
            int thresholdValue = 0;
            bool validResponse;
            bool validSensorReference = true;
            bool validRangeType = true;
            string userResponse;
            int currentLeftSensorValue = finchRobot.getLeftLightSensor();
            int currentRightSensorValue = finchRobot.getRightLightSensor();

            do
            {
                validResponse = true;
                DisplayScreenHeader("Threshold Value");

                //
                // Display ambient light values
                //
                switch (sensorsToMonitor)
                {
                    case "left":
                        Console.WriteLine($"\tCurrent {sensorsToMonitor} Sensor Value: {currentLeftSensorValue}");
                        break;

                    case "right":
                        Console.WriteLine($"\tCurrent {sensorsToMonitor} Sensor Value: {currentRightSensorValue}");
                        break;

                    case "both":
                        Console.WriteLine($"\tCurrent Left Sensor Value: {currentLeftSensorValue}");
                        Console.WriteLine($"\tCurrent Right Sensor Value: {currentRightSensorValue}");
                        break;

                    default:
                        Console.WriteLine("\tUnknown Sensor Reference. Please Return to Alarm System Manu.");
                        validSensorReference = false;
                        break;
                }
                Console.WriteLine();

                //
                // Did they enter a range type
                //
                if (rangeType != "minimum" && rangeType != "maximum")
                {
                    validRangeType = false;
                }

                //
                // If user entered sensor reference, ask for threshold
                //
                if (validSensorReference)
                {
                    //
                    // If user entered range type, ask for threshold
                    //
                    if (validRangeType)
                    {
                        //
                        // get threshold from user
                        //
                        Console.Write("\tEnter Threshold:");
                        userResponse = Console.ReadLine();

                        if (rangeType == "minimum")
                        {
                            switch (sensorsToMonitor)
                            {
                                case "left":
                                    //
                                    // validate if threshold is less then left light sensor and between 0 and 255
                                    //
                                    if (int.TryParse(userResponse, out thresholdValue) && thresholdValue >= currentLeftSensorValue)
                                    {
                                        Console.WriteLine("\tPlease Enter a Threshold Less then Current Light Sensors [Range Type Entered: Minimum]");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (!int.TryParse(userResponse, out thresholdValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue < 0 || thresholdValue > 255))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int.TryParse(userResponse, out thresholdValue);
                                    }
                                    break;

                                case "right":
                                    //
                                    // validate if threshold is less then right light sensor and between 0 and 255
                                    //
                                    if (int.TryParse(userResponse, out thresholdValue) && thresholdValue >= currentRightSensorValue)
                                    {
                                        Console.WriteLine("\tPlease Enter a Threshold Less then Current Light Sensors [Range Type Entered: Minimum]");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (!int.TryParse(userResponse, out thresholdValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue < 0 || thresholdValue > 255))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int.TryParse(userResponse, out thresholdValue);
                                    }
                                    break;

                                case "both":
                                    //
                                    // validate if threshold is less then both light sensor and between 0 and 255
                                    //
                                    if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue >= currentLeftSensorValue || thresholdValue >= currentRightSensorValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Threshold Less then Current Light Sensors [Range Type Entered: Minimum]");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (!int.TryParse(userResponse, out thresholdValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue < 0 || thresholdValue > 255))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int.TryParse(userResponse, out thresholdValue);
                                    }
                                    break;

                                default:

                                    break;
                            }
                        }
                        else if (rangeType == "maximum")
                        {
                            switch (sensorsToMonitor)
                            {
                                case "left":
                                    //
                                    // validate if threshold is higher then left light sensor and between 0 and 255
                                    //
                                    if (int.TryParse(userResponse, out thresholdValue) && thresholdValue <= currentLeftSensorValue)
                                    {
                                        Console.WriteLine("\tPlease Enter a Threshold Higher then Current Light Sensors [Range Type Entered: Maximum]");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (!int.TryParse(userResponse, out thresholdValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue < 0 || thresholdValue > 255))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int.TryParse(userResponse, out thresholdValue);
                                    }
                                    break;

                                case "right":
                                    //
                                    // validate if threshold is higher then right light sensor and between 0 and 255
                                    //
                                    if (int.TryParse(userResponse, out thresholdValue) && thresholdValue <= currentRightSensorValue)
                                    {
                                        Console.WriteLine("\tPlease Enter a Threshold Higher then Current Light Sensors [Range Type Entered: Maximum]");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (!int.TryParse(userResponse, out thresholdValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue < 0 || thresholdValue > 255))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int.TryParse(userResponse, out thresholdValue);
                                    }
                                    break;

                                case "both":
                                    //
                                    // validate if threshold is higher then both light sensor and between 0 and 255
                                    //
                                    if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue <= currentLeftSensorValue || thresholdValue <= currentRightSensorValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Threshold Higher then Current Light Sensors [Range Type Entered: Maximum]");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (!int.TryParse(userResponse, out thresholdValue))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else if (int.TryParse(userResponse, out thresholdValue) && (thresholdValue < 0 || thresholdValue > 255))
                                    {
                                        Console.WriteLine("\tPlease Enter a Number from 0 to 255.");
                                        validResponse = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int.TryParse(userResponse, out thresholdValue);
                                    }
                                    break;

                                default:

                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tYou Didn't Enter a Range Type. Go to Alarm System Menu.");
                    }
                }
            } while (!validResponse);

            //
            // Display threshold value if there is a sensor reference and range type
            //
            if (validRangeType && validSensorReference)
            {
                Console.WriteLine();
                Console.WriteLine($"\tThreshold Entered: {thresholdValue}");
            }
            DisplayMenuPrompt("Alarm System");

            return thresholdValue;
        }

        /// <summary>
        /// Get range type from user
        /// </summary>
        /// <returns>rangeType</returns>
        static string AlarmSystemDisplaySetRangeType()
        {
            string rangeType = "";
            bool validResponse;

            do
            {
                DisplayScreenHeader("Range Type");

                Console.Write("\tEnter Range Type [minimum, maximum]:");
                rangeType = Console.ReadLine().ToLower();

                if (rangeType == "minimum" || rangeType == "maximum")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease Enter a Range Type [minimum, maximum]. ");
                    validResponse = false;
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\tSenors to Monitor Entered: {rangeType}");

            DisplayMenuPrompt("Alarm System");

            return rangeType;
        }

        /// <summary>
        /// Get Monitor to sensor from user
        /// </summary>
        /// <returns>sensorsToMonitor</returns>
        static string AlarmSystemDisplaySetSensors()
        {
            string sensorsToMonitor = "";
            bool validResponse;

            do
            {
                DisplayScreenHeader("Sensors to Monitor");

                Console.Write("\tEnter Sensors to Monitor [left, right, both]:");
                sensorsToMonitor = Console.ReadLine().ToLower();

                if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease Enter Sensors to Monitor [left, right, both]. ");
                    validResponse = false;
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\tSenors to Monitor Entered: {sensorsToMonitor}");

            DisplayMenuPrompt("Alarm System");

            return sensorsToMonitor;
        }

        #endregion

        #region DATA RECORDER

        /// <summary>
        /// *****************************************************************
        /// *                     Data Recorder Show Menu                   *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;
            bool validResponse;
            string dataType;

            do
            {
                validResponse = true;

                DisplayScreenHeader("Data Recorder Menu");

                Console.Write("\tLight Data or Temperature Data: ");
                dataType = Console.ReadLine().ToLower();
                //
                // Light Data or Temperature Data
                //
                if (dataType == "light" || dataType == "light data")
                {
                    Console.WriteLine();
                    Console.WriteLine("\tYou entered Light Data.");
                    DisplayContinuePrompt();
                    DataRecorderDisplayLightDataMenu(finchRobot);
                }
                else if (dataType == "temperature" || dataType == "temperature data")
                {
                    Console.WriteLine();
                    Console.WriteLine("\tYou entered Temperature Data.");
                    DisplayContinuePrompt();
                    DataRecorderDisplayTemperatureDataMenu(finchRobot);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a letter for the menu choice.");
                    DisplayContinuePrompt();
                    validResponse = false;
                }
            } while (!validResponse);
        }

        /// <summary>
        /// Ligth Data Recorder Main Menu
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DataRecorderDisplayLightDataMenu(Finch finchRobot)
        {
            bool quitMenu = false;

            string menuChoice;

            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            int[] lightDataLeftSensor = null;
            int[] lightDataRightSensor = null;

            do
            {
                DisplayScreenHeader("Light Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        (lightDataLeftSensor, lightDataRightSensor)= DataRecorderDisplayGetLightData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayGetLightDataProcess(lightDataLeftSensor, lightDataRightSensor);
                        break;

                    case "e":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// Light Data Table Process
        /// </summary>
        /// <param name="lightDataLeftSensor"></param>
        /// <param name="lightDataRightSensor"></param>
        static void DataRecorderDisplayGetLightDataProcess(int[] lightDataLeftSensor, int[] lightDataRightSensor)
        {
            DisplayScreenHeader("Light Levels");

            DataRecorderDisplayLightDataTable(lightDataLeftSensor, lightDataRightSensor);

            DisplayMenuPrompt("Data Recorder");
        }

        /// <summary>
        /// Light Data Table
        /// </summary>
        /// <param name="lightDataLeftSensor"></param>
        /// <param name="lightDataRightSensor"></param>
        static void DataRecorderDisplayLightDataTable(int[] lightDataLeftSensor, int[] lightDataRightSensor)
        {
            double averageLeftLightLevel;
            double averageRightLightLevel;

            Console.WriteLine();
            Console.WriteLine(
                "Reading #".PadLeft(20) +
                "Left Light Sensor".PadLeft(30) +
                "Right Light Sensor".PadLeft(30)

                );
            Console.WriteLine(
                "---------".PadLeft(20) +
                "-----------------".PadLeft(30) +
                "------------------".PadLeft(30)
                );
            for (int index = 0; index < lightDataLeftSensor.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(20) +
                (lightDataLeftSensor[index]).ToString("n1").PadLeft(30) +
                (lightDataRightSensor[index]).ToString("n1").PadLeft(30)

                );
            }
            Console.WriteLine();
            Console.WriteLine(
                "Average Left Light Level".PadLeft(40) +
                "Average Right Light Level".PadLeft(40)

                );
            Console.WriteLine(
                "------------------------".PadLeft(40) +
                "-------------------------".PadLeft(40)
                );
            averageLeftLightLevel = DataRecorderDisplayAverageLeftLightSensor(lightDataLeftSensor);
            averageRightLightLevel = DataRecorderDisplayAverageRightLightSensor(lightDataRightSensor);
            Console.WriteLine(
                (averageLeftLightLevel).ToString("n1").PadLeft(40) +
                (averageRightLightLevel).ToString("n1").PadLeft(40)
                );
        }

        /// <summary>
        /// Right Light Sensor Average
        /// </summary>
        /// <param name="lightDataRightSensor"></param>
        /// <returns></returns>
        static double DataRecorderDisplayAverageRightLightSensor(int[] lightDataRightSensor)
        {
            double averageRightLightLevel;

            averageRightLightLevel = lightDataRightSensor.Average();

            return averageRightLightLevel;
        }

        /// <summary>
        /// Left Data Sensor Average
        /// </summary>
        /// <param name="lightDataLeftSensor"></param>
        /// <returns></returns>
        static double DataRecorderDisplayAverageLeftLightSensor(int[] lightDataLeftSensor)
        {
            double averageLeftLightLevel;

            averageLeftLightLevel = lightDataLeftSensor.Average();

            return averageLeftLightLevel;
        }

        /// <summary>
        /// Get ligth sensor data from finch robot
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        static (int[] lightDataLeftSensor, int[] lightDataRightSensor) DataRecorderDisplayGetLightData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            int[] lightDataLeftSensor = new int[numberOfDataPoints];
            int[] lightDataRightSensor = new int[numberOfDataPoints];
            int dataPointFrequencyMs;
            DisplayScreenHeader("Light Data Recorder Menu");

            //
            // Convert the frequenct=y in ms
            //
            dataPointFrequencyMs = (int)(dataPointFrequency * 1000);

            //
            // echo the values
            //
            Console.WriteLine($"\tThe Finch robot will now record {numberOfDataPoints} temperatures {dataPointFrequency} seconds apart.");
            Console.WriteLine();
            Console.WriteLine("\tPress any key to begin");
            Console.ReadKey();
            Console.WriteLine();

            for (int index = 0; index < numberOfDataPoints; index++)
            {
                lightDataLeftSensor[index] = finchRobot.getLeftLightSensor();
                lightDataRightSensor[index] = finchRobot.getRightLightSensor();
                finchRobot.wait(dataPointFrequencyMs);

                Console.WriteLine($"\tLight Level {index + 1}:  {lightDataLeftSensor[index]:n1}     {lightDataRightSensor[index]:n1}");
            }

            DataRecorderDisplayLightDataTable(lightDataLeftSensor, lightDataRightSensor);

            DisplayMenuPrompt("Data Recorder");

            return (lightDataLeftSensor, lightDataRightSensor);
        }

        /// <summary>
        /// Temperature data menu
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DataRecorderDisplayTemperatureDataMenu(Finch finchRobot)
        {
            bool quitMenu = false;

            string menuChoice;

            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperaturesCelsius = null;
            double[] temperaturesFahrenheit = null;

            do
            {
                DisplayScreenHeader("Temperature Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        (temperaturesCelsius, temperaturesFahrenheit) = DataRecorderDisplayGetTemperatureData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayGetTemperatureDataProcess(temperaturesCelsius, temperaturesFahrenheit);
                        break;

                    case "e":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitMenu);
        }

        /// <summary>
        /// create temperature data table
        /// </summary>
        /// <param name="temperaturesCelcius"></param>
        static void DataRecorderDisplayDataTable(double[] temperaturesCelcius, double[] temperaturesFahrenheit)
        {
            double averageTemperatureCelcius;
            double averageTemperatureFehrenheit;

            Console.WriteLine();
            Console.WriteLine(
                "Reading #".PadLeft(20) +
                "Temperature Celcius".PadLeft(30) +
                "Temperature Fahrenheit".PadLeft(30)

                );
            Console.WriteLine(
                "---------".PadLeft(20) +
                "-------------------".PadLeft(30) +
                "----------------------".PadLeft(30)
                );
            for (int index = 0; index < temperaturesCelcius.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(20) +
                ((temperaturesCelcius[index]).ToString("n1") + " °C").PadLeft(30) +
                ((temperaturesFahrenheit[index]).ToString("n1") + " °F").PadLeft(30)

                );
            }
            Console.WriteLine();
            Console.WriteLine(
                "Average Temperature Celcius".PadLeft(40) +
                "Average Temperature Fahrenheit".PadLeft(40)

                );
            Console.WriteLine(
                "---------------------------".PadLeft(40) +
                "------------------------------".PadLeft(40)
                );
            averageTemperatureCelcius = DataRecorderDisplayAverageTemperaturesCelcius(temperaturesCelcius);
            averageTemperatureFehrenheit = DataRecorderDisplayAverageTemperaturesFehrenheit(temperaturesFahrenheit);
            Console.WriteLine(
                ((averageTemperatureCelcius).ToString("n1") + " °C").PadLeft(40) +
                ((averageTemperatureFehrenheit).ToString("n1") + " °F").PadLeft(40)
                );
        }

        /// <summary>
        /// Get average temperature in fehrenheit
        /// </summary>
        /// <param name="temperaturesFahrenheit"></param>
        /// <returns>averageTemperatureFehrenheit</returns>
        static double DataRecorderDisplayAverageTemperaturesFehrenheit(double[] temperaturesFahrenheit)
        {
            double averageTemperatureFehrenheit;

            averageTemperatureFehrenheit = temperaturesFahrenheit.Average();

            return averageTemperatureFehrenheit;
        }

        /// <summary>
        /// Get average temperature in celcius
        /// </summary>
        /// <param name="temperaturesCelcius"></param>
        /// <returns>averageTemperatureCelcius</returns>
        static double DataRecorderDisplayAverageTemperaturesCelcius(double[] temperaturesCelcius)
        {
            double averageTemperatureCelcius;

            averageTemperatureCelcius = temperaturesCelcius.Average();

            return averageTemperatureCelcius;
        }

        /// <summary>
        /// Display temperature data in a table
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayGetTemperatureDataProcess(double[] temperaturesCelsius, double[] temperaturesFahrenheit)
        {
            DisplayScreenHeader("Temperatures");

            DataRecorderDisplayDataTable(temperaturesCelsius, temperaturesFahrenheit);

            DisplayMenuPrompt("Data Recorder");
        }

        /// <summary>
        /// Get temperatures from robot
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="finchRobot"></param>
        /// <returns>termperatues</returns>
        static (double[], double[]) DataRecorderDisplayGetTemperatureData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperaturesCelsius = new double[numberOfDataPoints];
            int dataPointFrequencyMs;
            double[] temperaturesFahrenheit = new double[numberOfDataPoints];

            //
            // Convert the frequenct=y in ms
            //
            dataPointFrequencyMs = (int)(dataPointFrequency * 1000);

            DisplayScreenHeader("Temperatures");

            //
            // echo the values
            //
            Console.WriteLine($"\tThe Finch robot will now record {numberOfDataPoints} temperatures {dataPointFrequency} seconds apart.");
            Console.WriteLine();
            Console.WriteLine("\tPress any key to begin");
            Console.ReadKey();

            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperaturesCelsius[index]  = finchRobot.getTemperature();
                finchRobot.wait(dataPointFrequencyMs);

                //
                // Convert to fahrenheit
                //
                temperaturesFahrenheit = DataRecorderDisplayDataConvertToFahrenheit(temperaturesCelsius);

                //
                // Echo new temperature
                //
                Console.WriteLine($"\tTemperature {index + 1}:  {temperaturesCelsius[index]:n1} °C   {temperaturesFahrenheit[index]:n1} °F");
            }

            DataRecorderDisplayDataTable(temperaturesCelsius, temperaturesFahrenheit);

            DisplayMenuPrompt("Data Recorder");

            return (temperaturesCelsius, temperaturesFahrenheit);
        }

        /// <summary>
        /// Convert temperature to fahrenheit
        /// </summary>
        /// <param name="temperaturesCelsius"></param>
        /// <returns>temperaturesFahrenheit</returns>
        static double[] DataRecorderDisplayDataConvertToFahrenheit(double[] temperaturesCelsius)
        {
            double[] temperaturesFahrenheit = new double[temperaturesCelsius.Length];

            for (int index = 0; index < temperaturesCelsius.Length; index++)
            {
                temperaturesFahrenheit[index] = temperaturesCelsius[index]* 9 / 5 + 32;
            }

            return temperaturesFahrenheit;
        }

        /// <summary>
        /// Get Data Point Frequency From User
        /// </summary>
        /// <returns>Data Point Frequency</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            string userResponse;
            bool validResponse;

            do
            {
                validResponse = true;
                DisplayScreenHeader("Data Points Frequency");

                Console.Write("\tData Point Frequency in Seconds:");
                userResponse = Console.ReadLine();
                if (!double.TryParse(userResponse, out dataPointFrequency))
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid number (2, 5.5, etc.)");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else if (double.TryParse(userResponse, out dataPointFrequency) && dataPointFrequency < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid number (2, 5.5, etc.)");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"\tYou chose {dataPointFrequency} as the data point frequency.");

            DisplayMenuPrompt("Data Recorder");

            return dataPointFrequency;
        }

        /// <summary>
        /// Get Number of Data Points From User
        /// </summary>
        /// <returns>number of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            string userResponse;
            bool validResponse;

            do
            {
                validResponse = true;
                DisplayScreenHeader("Number of Data Points");

                Console.Write("\tNumber of Data Points:");
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out numberOfDataPoints))
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid integer (2, 5, 11, etc.)");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else if (int.TryParse(userResponse, out numberOfDataPoints) && numberOfDataPoints < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid integer (2, 5, 11, etc.)");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }
            } while (!validResponse);
           
            Console.WriteLine();
            Console.WriteLine($"\tYou chose {numberOfDataPoints} as the number of data points.");

            DisplayMenuPrompt("Data Recorder");

            return numberOfDataPoints;
        }

        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void TalentShowDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light Show");
                Console.WriteLine("\tb) Play a Music Note");
                Console.WriteLine("\tc) Spin Around");
                Console.WriteLine("\td) Go Crazy");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        TalentShowDisplayLightShow(finchRobot);
                        break;

                    case "b":
                        TelentShowDisplayMusicNote(finchRobot);
                        break;

                    case "c":
                        TalentShowDisplaySpinAround(finchRobot);
                        break;

                    case "d":
                        TalentShowDisplayGoCrazy(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// Talent Show: Show light based on user input
        /// </summary>
        /// <param name="finchRobot"></param>
        static void TalentShowDisplayLightShow(Finch finchRobot)
        {
            string color;
            bool ValidRepsone;

            do
            {
                ValidRepsone = true;

                DisplayScreenHeader("Light Show");

                Console.WriteLine();
                Console.WriteLine("\tThe Finch Robot Will Now Give You a Light Show");
                Console.WriteLine();

                Console.WriteLine("\ta) Red");
                Console.WriteLine("\tb) Green");
                Console.WriteLine("\tc) Blue");
                Console.WriteLine("\td) Yellow");
                Console.WriteLine("\te) Cyan");
                Console.WriteLine("\tf) Purple");
                Console.WriteLine("\tg) Exit Light Show");

                Console.WriteLine();
                Console.Write("\tPlease Choose a Color:");

                color = Console.ReadLine().ToLower();

                switch (color)
                {
                    case "a":
                        for (int loop = 0; loop < 5; loop++)
                            DisplayColorRed(finchRobot);
                        break;
                    case "b":
                        for (int loop = 0; loop < 5; loop++)
                        {
                            DisplayColorGreen(finchRobot);
                        }
                        break;
                    case "c":
                        for (int loop = 0; loop < 5; loop++)
                        {
                            DisplayColorBlue(finchRobot);
                        }
                        break;
                    case "d":
                        for (int loop = 0; loop < 5; loop++)
                        {
                            DisplayColorYellow(finchRobot);
                        }
                        break;
                    case "e":
                        for (int loop = 0; loop < 5; loop++)
                        {
                            DisplayColorCyan(finchRobot);
                        }
                        break;
                    case "f":
                        for (int loop = 0; loop < 5; loop++)
                        {
                            DisplayColorPurple(finchRobot);
                        }
                        break;
                    case "g":
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        ValidRepsone = false;
                        DisplayContinuePrompt();
                        break;
                }
            } while (!ValidRepsone);

            DisplayMenuPrompt("Talent Show");
        }

        #region Colors

        static void DisplayColorPurple(Finch finchRobot)
        {
            //
            // Turn Purple
            //
            for (int lightLevel = 0; lightLevel <= 255; lightLevel += 5)
            {
                finchRobot.setLED(lightLevel, 0, lightLevel);
            }
            finchRobot.wait(250);
            for (int lightLevel = 255; lightLevel >= 0; lightLevel -= 5)
            {
                finchRobot.setLED(lightLevel, 0, lightLevel);
            }
        }

        static void DisplayColorCyan(Finch finchRobot)
        {
            //
            // Turn Cyan
            //
            for (int lightLevel = 0; lightLevel <= 255; lightLevel += 5)
            {
                finchRobot.setLED(0, lightLevel, lightLevel);
            }
            finchRobot.wait(250);
            for (int lightLevel = 255; lightLevel >= 0; lightLevel -= 5)
            {
                finchRobot.setLED(0, lightLevel, lightLevel);
            }
        }

        static void DisplayColorYellow(Finch finchRobot)
        {
            //
            // Turn Yellow
            //
            for (int lightLevel = 0; lightLevel <= 255; lightLevel += 5)
            {
                finchRobot.setLED(lightLevel, lightLevel, 0);
            }
            finchRobot.wait(250);
            for (int lightLevel = 255; lightLevel >= 0; lightLevel -= 5)
            {
                finchRobot.setLED(lightLevel, lightLevel, 0);
            }
        }

        static void DisplayColorBlue(Finch finchRobot)
        {
            //
            // Turn Blue
            //
            for (int lightLevel = 0; lightLevel <= 255; lightLevel += 5)
            {
                finchRobot.setLED(0, 0, lightLevel);
            }
            finchRobot.wait(250);
            for (int lightLevel = 255; lightLevel >= 0; lightLevel -= 5)
            {
                finchRobot.setLED(0, 0, lightLevel);
            }
        }

        static void DisplayColorGreen(Finch finchRobot)
        {
            //
            // Turn Green
            //
            for (int lightLevel = 0; lightLevel <= 255; lightLevel += 5)
            {
                finchRobot.setLED(0, lightLevel, 0);
            }
            finchRobot.wait(250);
            for (int lightLevel = 255; lightLevel >= 0; lightLevel -= 5)
            {
                finchRobot.setLED(0, lightLevel, 0);
            }
        }

        static void DisplayColorRed(Finch finchRobot)
        {
            //
            // Turn Red
            //
            for (int lightLevel = 0; lightLevel <= 255; lightLevel += 5)
            {
                finchRobot.setLED(lightLevel, 0, 0);
            }
            finchRobot.wait(250);
            for (int lightLevel = 255; lightLevel >= 0; lightLevel -= 5)
            {
                finchRobot.setLED(lightLevel, 0, 0);
            }
        }

        #endregion

        /// <summary>
        /// Talent Show: Play a Frequency based on user input
        /// </summary>
        /// <param name="finchRobot"></param>
        static void TelentShowDisplayMusicNote(Finch finchRobot)
        {
            string userResponse;
            int frequency;
            bool validResponse;

            do
            {
                validResponse = true;

                DisplayScreenHeader("Play a Song");

                Console.WriteLine();
                Console.Write("\tEnter a Frequency For The Finch To Play (15-16000):");
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out frequency))
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid frequency");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else if (int.TryParse(userResponse, out frequency) && frequency < 15)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid frequency");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else if (int.TryParse(userResponse, out frequency) && frequency > 16000)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid frequency");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
                else if (int.TryParse(userResponse, out frequency) && frequency >= 15 && frequency <= 16000)
                {
                    Console.WriteLine();
                    Console.WriteLine($"\tFrequency Enterd: {frequency}");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid frequency");
                    DisplayContinuePrompt();
                    validResponse = false;
                    Console.Clear();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot Will Now Play a Music Note");

            finchRobot.noteOn(frequency);
            finchRobot.wait(5000);
            finchRobot.noteOff();

            DisplayMenuPrompt("Talent Show");
        }

        /// <summary>
        /// Talent Show: Spining Around
        /// </summary>
        /// <param name="finchRobot"></param>
        static void TalentShowDisplaySpinAround(Finch finchRobot)
        {
            DisplayScreenHeader("Spin Around");

            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot Will Now Spin Around");
            DisplayContinuePrompt();

            finchRobot.setMotors(255, 50);
            finchRobot.wait(3000);

            finchRobot.setMotors(50, 255);
            finchRobot.wait(3000);

            finchRobot.setMotors(-255, 255);
            finchRobot.wait(3000);

            finchRobot.setMotors(255, -255);
            finchRobot.wait(3000);

            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show");
        }

        static void TalentShowDisplayGoCrazy(Finch finchRobot)
        {
            int red;
            int green;
            int blue;
            int left = 0;
            int right = 0;
            string direction;
            int frequency;
            bool validResponse;

            red = ValidColorRed();

            green = ValidateColorGreen(red);

            blue = ValidateColorBlue(red, green);

            frequency = ValidateFrequency();

            do
            {
                validResponse = true;
                DisplayScreenHeader("Go Crazy");
                Console.WriteLine();

                Console.Write("\tPlease Enter a Direction (left or right):");
                direction = Console.ReadLine().ToLower();

                if (direction == "right")
                {
                    right = -255;
                    left = 255;

                    Console.WriteLine();
                    Console.WriteLine("\tYour Entered: Right");
                }
                else if (direction == "left")
                {
                    right = 255;
                    left = -255;

                    Console.WriteLine();
                    Console.WriteLine("\tYour Entered: Left");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease Enter a Valid Repsonse (left or right)");
                    DisplayContinuePrompt();
                    Console.Clear();
                    validResponse = false;
                }
            } while (!validResponse);

            DisplayContinuePrompt();
            DisplayScreenHeader("Go Crazy");
            Console.WriteLine("\tYou have entered the following:");
            Console.WriteLine($"\t1){red}");
            Console.WriteLine($"\t2){green}");
            Console.WriteLine($"\t3){blue}");
            Console.WriteLine($"\t4){frequency}");
            Console.WriteLine($"\t5){direction}");

            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot Will Now Go Crazy");
            DisplayContinuePrompt();

            finchRobot.setMotors(left, right);

            for (int Flashing = 0; Flashing < 50; Flashing++)
            {
                finchRobot.setLED(red, green, blue);
                finchRobot.noteOn(frequency+ 4 * Flashing);
                finchRobot.wait(100);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
                finchRobot.wait(100);
            }

            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show");
        }

        static int ValidColorRed()
        {
            bool validResponse;
            int red;
            string userResponse;
            do
            {
                validResponse = true;

                DisplayScreenHeader("Go Crazy");

                Console.WriteLine();
                Console.WriteLine("\tThe Finch Robot Will Now Move, Shine, And Make Some Noise");
                Console.WriteLine();

                Console.WriteLine("\tPlease Enter Three Values from 0-255:");
                Console.Write("\t1)");

                //
                // User response for the color red
                //
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out red))
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out red) && red < 0)
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out red) && red > 255)
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out red) && red >= 0 && red <= 255)
                {

                }
                else
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
            } while (!validResponse);

            return red;
        }

        static int ValidateColorGreen(int red)
        {
            bool validResponse;
            int green;
            string userResponse;
            do
            {
                validResponse = true;

                DisplayScreenHeader("Go Crazy");

                Console.WriteLine();
                Console.WriteLine("\tThe Finch Robot Will Now Move, Shine, And Make Some Noise");
                Console.WriteLine();

                Console.WriteLine("\tPlease Enter Three Values from 0-255:");
                Console.WriteLine($"\t1){red}");
                Console.Write("\t2)");
                //
                // user response for the color green
                //
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out green))
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out green) && green < 0)
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out green) && green > 255)
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out green) && green >= 0 && green <= 255)
                {

                }
                else
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
            } while (!validResponse);

            return green;
        }

        static int ValidateColorBlue(int red, int green)
        {
            bool validResponse;
            int blue;
            string userResponse;
            do
            {
                validResponse = true;

                DisplayScreenHeader("Go Crazy");

                Console.WriteLine();
                Console.WriteLine("\tThe Finch Robot Will Now Move, Shine, And Make Some Noise");
                Console.WriteLine();

                Console.WriteLine("\tPlease Enter Three Values from 0-255:");
                Console.WriteLine($"\t1){red}");
                Console.WriteLine($"\t2){green}");
                Console.Write("\t3)");
                //
                // user response for the color blue
                //
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out blue))
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out blue) && blue < 0)
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out blue) && blue > 255)
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out blue) && blue >= 0 && blue <= 255)
                {

                }
                else
                {
                    InvalidRepsonseNumber();
                    validResponse = false;
                }
            } while (!validResponse);

            return blue;
        }

        static int ValidateFrequency()
        {
            int frequency;
            string userResponse;
            bool validResponse;
            do
            {
                validResponse = true;

                DisplayScreenHeader("Go Crazy");

                Console.WriteLine();
                Console.Write("\tEnter a Frequency For The Finch To Play (15-16000):");
                userResponse = Console.ReadLine();
                if (!int.TryParse(userResponse, out frequency))
                {
                    InvalidFrequency();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out frequency) && frequency < 15)
                {
                    InvalidFrequency();
                    validResponse = false; ;
                }
                else if (int.TryParse(userResponse, out frequency) && frequency > 16000)
                {
                    InvalidFrequency();
                    validResponse = false;
                }
                else if (int.TryParse(userResponse, out frequency) && frequency >= 15 && frequency <= 16000)
                {
                    Console.WriteLine();
                    Console.WriteLine($"\tFrequency Entered: {frequency}");
                }
                else
                {
                    InvalidFrequency();
                    validResponse = false;

                }
            } while (!validResponse);

            return frequency;
        }

        static void InvalidFrequency()
        {
            Console.WriteLine();
            Console.WriteLine("\tPlease enter a valid frequency");
            DisplayContinuePrompt();
            Console.Clear();
        }

        static void InvalidRepsonseNumber()
        {
            bool validResponse;

            Console.WriteLine();
            Console.WriteLine("\tPlease enter a valid number");
            DisplayContinuePrompt();
            Console.Clear();
        }



        //static void TelentShowDisplayPlaySong(Finch finchRobot)
        //{
        //    DisplayScreenHeader("Play my Song");

        //    //Console.WriteLine();
        //    //Console.WriteLine("\tThe Finch Robot Will Now Play a Song.");
        //    //DisplayContinuePrompt();

        //    string userResponse;
        //    int frequency;

        //    Console.WriteLine("Enter Frequency");
        //    userResponse = Console.ReadLine();
        //    frequency = int.Parse(userResponse);

        //    Console.WriteLine("\tThe Finch robot will now play your tone");

        //    finchRobot.noteOn(frequency);
        //    finchRobot.wait(2000);
        //    finchRobot.noteOff();

        //    Console.WriteLine("\tMy song is playing now");



        //    DisplayMenuPrompt("Talent Show");
        //}

        ///// <summary>
        ///// *****************************************************************
        ///// *               Talent Show > Light and Sound                   *
        ///// *****************************************************************
        ///// </summary>
        ///// <param name="finchRobot">finch robot object</param>
        //static void TalentShowDisplayLightAndSound(Finch finchRobot)
        //{
        //    Console.CursorVisible = false;

        //    DisplayScreenHeader("Light and Sound");

        //    Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
        //    DisplayContinuePrompt();

        //    for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
        //    {
        //        finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
        //        finchRobot.noteOn(lightSoundLevel * 100);
        //    }

        //    DisplayMenuPrompt("Talent Show Menu");
        //}

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            if (robotConnected)
            {
                Console.WriteLine();
                Console.WriteLine("\t\tFinch Robot is Connected");
                finchRobot.setLED(0, 255, 255);
                finchRobot.noteOn(5000);
                finchRobot.wait(100);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\t\tFinch Robot is Unable to Connect. Please Try Again.");
            }

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
