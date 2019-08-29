# Neon Tetra

Yet another Home Automation System.


## Goals and Objectives
--------------------
* To create "yet another home automation system"
  * Targeted to .NET Core
  * Leverage an Actor Framework (using Akka.Net)
    * Micro-services, fault tolernate, scalability
    * Messaging, cross machine comm, cross platform
    * Extesibility via Actor model (versioning of each)
  * Hosting in docker by default, kubernetes, raspberrypi, linux, windows, macos, or bare metal...
  * 100% self contained (execution).  no other runtime dependancies
    * So this possibly means self-hosting the Web Server, MQTT, etc.
  * Possible heavy reliance on MQTT
  * Dependency Injection
  * Should never have any limitations on scale
  * 100% UI managed.  Editing Yaml/Json config sucks bad
  * Start with Angular for the UI, but 100% must be mobile first (I could care less if its angular or react or whatever)
  * "Proper" REST API for Client UI
  * Secure by default (2FA?)
  * I would like to avoid any sort of Entity Framework (probably just my personal bias against it)
  * 100% Open Source  (not required)
  * VS.NET 2017, 2019 and VS Code supported
  * Scheduled and Automatic updates from github
    * Can be turned off
  * Sensor reporting/trend data
    * Customizable data repository (extensible)

The core of the project will be based on "Sensors" and "Actuators".  and the spirit of the system is adaptability.  

Adaptable in the sense of the devices and the scenarios, deployment and hosting, etc.. which we can support.

So for example, running an entire office or large home, all the way down to a small fish tank.  Leveraging GPIO/SPI on the RPi, to the processing power and storage on the cloud (Azure, Aws, etc.).

## Major built in components
-------------------------
* Scheduler  (extensible)
* IFTTT like behavior
  * Sensor to control actuator
  * sensor to control groups, or scenes
  * actuator to control scenes, etc..
* Management UI
  * Scheduler
  * IFTTT Behaviours
  * All configuration
* 3rd party integrations  (extensible)
  * Weather  (extensible)
  * Sunrise/sunset  (extensible)
  * Twilio / Pushbullet
  * Slack
  * Alexa / Google Home
  * Philips Hue (and any other lighting options, full color, dimmable, color temp, on off, etc..)
  * Queue based devices
  * Presense detection (Bluetooth, Wifi, Arp, queue, Router)
  * Garage doors
  * Motion sensors
  * Facial/Voice recognition
  * Thermostats
  * AutoPi.io and/or Moj.io
  * Sab/Sonarr/Radarr/Plex
  * etc.
* Custom URLs to Iframe/_new/etc.
* Scenes (logical grouping of actuators)
* Groups (logical grouping of actuators and/or sensors)
* Device support autodiscovery
  * UPNP
  * ARP like network scans
  * Actors in the system (as DLL's) dynamically loaded up
* Extensible Configuration management 
* NLP based interaction (bot all the things!)
    
Each sensor, actuator and user, etc.. would be actors in the system. 

We could also scan the entire subnet and keep track of each device on the network, and their connectivity state (essentially treat them as sensors).  Wake on lan as an actuator?


## Some sample scenarios
---------------------
* Scenario 1:
  * The fish tank experience
    * Everything runs off of a single Raspberry PI
    * Uses GPIO and SPI, etc. to control motors for things like feeding, water pump, etc.
    * Sunrise and sunset to control the lighting
    * Take temperature, humidty and PH levels every minute 

* Scenario 2:
  * Basic home automation experience
  * Philips hue lights
  * MyQ Garage door
  * Top X number of baked in scenes
    * Lights on and off based on time of day
    * Motion sensor to control a subset of lights

## Some sample experiences
------------------------
* First launch wizard
  * Capture the username and password of "root" account
  * Network and device scans to automatically discover available devices (sensors and actuators)
  * Geolocation in browser to detect location, and automatically wire up the weather for the local machine
  * Setup sunrise/sunset automation
  * Provide an "optional services" section to capture:
    * Additional weather or clocks
    * Twilio or PushBullet API keys
    * Thermostat connectivity
    * Custom URls to include in the UI

* Landing page is a Dashboard
  * Summary of all current activity
  * Large date/time display
  * Most used / recently used actuators listed/actionable
  * Large display of "primary" sensors, and current data
    * Possibly a smaller trendline graph under each


## Core Problems to address
---------------------------
1. Setup project structure with the Akka.NET
2. Get DI working within Akka.NET
    1. Setup CI/CD
3. Configuration storage
4. Telemetry storage
5. Actor snapshot storage
6. Design initial Actor framework (Coodinators/Managers, etc.)
7. Seutp Akka.NET Clustering
8. Docker support, including automated deployments to Rpi and Windows (docker and bare metal)
9. Get a modern (kestrel) Web Server operational, and redundant/microservice setup within the Akka.Net method for microservices.
    1. Basic controller
    2. Basic WebSocket
10. Get a basic library setup to manage core entities
    1. Sensors, Actuators, Devices, Scenes
11. A scheduling service
12. A workflow / IFTTT service
13. Start the Web/Reponsive UI
    1. Browse, Read, Edit, Add, Delete core entities
    2. BREAD schedules
    3. Trigger Actuators
14. Basic Rpi Actor - temp sensor
15. Basic Rpi Actor - GPIO relay
16. Remote execution of a Actor / Actuator from the Web Head


## Core Domain Model
----------------------
* All core object models must be immutable as we will be leveraging the Actor Framework.
* In order to mutate, we must consider a "mutator" or an "activator" object.
* Also keep in mind that since the actuator must be ran on a specific device activting it will most likey need to message another actor to perform the work.  In hybrid scenarios or multi-nodes in our cluster this will be very obvious.
* Each will be represented as an interface vs a concrete type.  This will allow for the greatest amount of flexibility.

* ISensor<T>
  * *Used to represent the current state of a device which provides telemetry*
  * Entity:
    * Timestamp in UTC    //when the last reading took place
    * <T> / value         //value of the last reading
    * string Name         //name for the sensor
    * string Id           //unique id for the sensor

* IActuator<T>
  * *Used to represent the notion of something which can be mutated in a variety of ways*
  * Entity:
    * Sensor<T>    //Last known state of the actuator
    * string Name  //name for the actuator
    * string Id    //uniqid id for the actuator

* IDevice
  * *Used to represent a collection of Sensors and Actuators*
  * Entity:
    * IList<ISensor<T>> Sensors       //a list of sensors attached to the device
    * IList<IActuator<T>> Actuators   //a list of actuators attached to the device
    * string Name  //name for the device
    * string Id    //uniqid id for the device

* IScene
  * *An arbitrary collection of Actuators, irregardless of the device which they belong to.  Useful for automation.  That is, we can trigger a scene which mutates multiple device actuators*
  * Entity:
    * Sensor<T>    //Last known state of the actuator
    * IList<IActuator<T>> Actuators   //a list of actuators attached to the device
    * string Name  //name for the scene
    * string Id    //uniqid id for the scene

* IUser
  * *Used to represent a user, membership and profile data*
  * Security and Access Control (authen/author)
  * Will skip roles, etc. for now.  Will just focus on Authentication.
  * Entity:
    * string Name  //name for the user
    * string Id    //uniqid id for the user
    * string username
    * string password

## Mutation and Sensor Monitoring
----------------------

* IActuatorMutator
  * *Provides the ability to mutate an actuator*
  * Need to consider:
    * Routing to the correct actor for the actuator. 
    * Remote actor activation.  
    * Response values, if any

* ISensorMonitor
  * *Provides the ability to monitor the soft/hard sensor iteself, and it will report any updates to the system and ultimately to the ISensor<T> itself via our Actor model*
  * No state, just read and report
  * Will be deployed onto the physical device via an actor

* IUserManager
  * *Provides the root management for an IUser entity and will possibly mutate the user (login, logout, etc) based on UI crud, etc..*


## Actor Composition
-------------------
* *See https://petabridge.com/blog/top-akkadotnet-design-patterns/*
* *https://petabridge.com/blog/when-should-I-use-actor-selection/*
* *https://petabridge.com/blog/how-actors-recover-from-failure-hierarchy-and-supervision/*
* Coordinators / Managers
* **DeviceCoordinator**
  * *Child-per-Entity pattern*
  * *Responsible for keeping the real-time state of all Sensors and Acuators in the network*
  * 
* **UserCoordinator**
  * *Child-per-Entity pattern*
  * *Responsible for keeping the real-time state of all users loged into the system.
* 