#include <QueueArray.h>


#pragma region Pin Constants

const int direction_wire1 = 3; 

const int step_wire1 = 5;   

#pragma endregion


int stepCounter;
int speed;
bool enableStepper;
volatile bool sendStuff = false;
const int sendInterval = 100; //milliseconds

int sendTimer;

enum sendType {
	NONE = 0,
	STATUS = 1,
	POSITION = 2,
	DIRECTION = 3,
	STEPPERCOUNTER = 4
};
sendType sendData;
QueueArray <sendType> queue;



char delim = ':';
enum commandType {
	STOP = 1,
	CALIBRATE = 2,
	MOVE = 3,
	SLIDE = 4
};
commandType Command;

enum errorTypes {
	BAD_COMMAND_STRING = 0,
	BAD_COMMAND = 1
};
errorTypes Error;

enum ActuatorDirectionCommand {
	UP = 1,
	DOWN = 2
};
ActuatorDirectionCommand cmdDirection;
float cmdSpeed;
float cmdDistance;

#define STEPS_PER_ROTATION 200 //steps per mm

class Actuator {
private:
	const int _directionPin, _pulsePin ; 
	volatile float _position;
	volatile bool _calibrated;
	volatile bool _moving;
	volatile int _StepCounter;
	ActuatorDirectionCommand _direction;
	volatile bool _step_wire_on;
	bool _isSetup;

public:
	Actuator(int directionPin, int pulsePin) :
		_directionPin(directionPin),
		_pulsePin(pulsePin)
	{
		_isSetup = false;

		_StepCounter = 0;
		_position = 0;
		_moving = false;
		_calibrated = false;

		_step_wire_on = false;
	};


void setup() {
	pinMode(_directionPin, OUTPUT);
	pinMode(_pulsePin, OUTPUT);

	digitalWrite(_directionPin, HIGH); //high = raise and "1" is raise
	_direction = ActuatorDirectionCommand::UP;

	_isSetup = true;

	speed = 800;
	enableStepper = false;
}

void stop() {
	_StepCounter = 0;
}

void moveDistance(float steps, ActuatorDirectionCommand dir, unsigned int freq)
{
	if (!_isSetup) return;
	_StepCounter = steps * 2.0;
	_direction = dir;
	sendStuff = true;
	queue.enqueue(sendType::DIRECTION); //direction not as important so only need to flag a send in here
	switch (dir) {
	case ActuatorDirectionCommand::UP:
		digitalWrite(_directionPin, HIGH);
		break;
	case ActuatorDirectionCommand::DOWN:
		digitalWrite(_directionPin, LOW);
		break;
	}
}
void resetSteppers()
{
	_position = 0.0;
}

void stepInterrupt() {
	if (!_isSetup) return;
	if (_StepCounter <= 0)
	{
		_StepCounter = 0;
		//check to see if this just happened
		if (_moving == true) {
			_moving = false;
			//let the sender in main loop now what type it needs to send
			//maybe put an if statement checking if sendType is open (dequeue or pop)
			queue.enqueue(sendType::STATUS);			
			//sendData = sendType::STATUS;
			sendStuff = true;
		}
		// don't do anything
	}
	else {
		//check to see if the command was just sent
		if (_moving == false) {
			_moving = true;
			//let the sender in main loop now what type it needs to send
			queue.enqueue(sendType::STATUS);
			//sendData = sendType::STATUS;
			sendStuff = true;
		}
		//insert check of endstop
		digitalWrite(_pulsePin, _step_wire_on = !_step_wire_on);
		_StepCounter--;
		if (_direction == UP)
		{
			//raising
			_position = _position - 0.5; //dividing by 2 because everytime we enter the interrupt it only executes half a pulse
			//_position = _position - (0.5 / STEPS_PER_ROTATION); //dividing by 2 because everytime we enter the interrupt it only executes half a pulse
		}
		else if (_direction == DOWN)
		{
			//lowering
			_position = _position + 0.5; //dividing by 2 because everytime we enter the interrupt it only executes half a pulse
			//_position = _position + (0.5 / STEPS_PER_ROTATION); //dividing by 2 because everytime we enter the interrupt it only executes half a pulse
		}
	}
}

bool getStatus() {
	return _moving;
}

int getStepCounter() {
	return _StepCounter;
}

float getPosition() {
	return _position;
}

ActuatorDirectionCommand getDirection()
{
	return _direction;
}
};


const char END_OF_TRANSMISSION = '\n';

Actuator act[1] = { Actuator(direction_wire1, step_wire1)
};

unsigned long timer1_counter;
unsigned int freq;

void setup()
{
	Serial.begin(9600); //experiment more with this. changing to 115200 drastically sped things up
	act[0].setup();
	bool enableStepper = false;
	freq = 12500;
	startInterrupt(freq);

	sendTimer = millis();
}

void startInterrupt(int frequency) {
	//important, if changing prescaler, must change tccr1b location as well
	int prescaler = 256; //this corresponds to range of 1 hz to 62500 hz **see excel
	timer1_counter = 65536 - (16000000 / prescaler / frequency);
	if (timer1_counter > 65534 || timer1_counter < 1) { return; } //timer1 is 16 bit, so max it goes to is 65535
	 
	// initialize timer1 
	noInterrupts();           // disable all interrupts
	TCCR1A = 0;
	TCCR1B = 0;

	TCNT1 = timer1_counter;   // preload timer
	TCCR1B |= (1 << CS12);    // 256 prescaler 
	TIMSK1 |= (1 << TOIE1);   // enable timer overflow interrupt
	interrupts();             // enable all interrupts
	
}

// This is an overflow type of interrupt
ISR(TIMER1_OVF_vect)        // interrupt service routine 
{
	TCNT1 = timer1_counter;   // preload timer
	// may we move?
	if (!enableStepper) {
		return;
	}
	//putting something here so it doesnt get stuck
	act[0].stepInterrupt();		
}
bool firstFull = true;

void loop() {
	
	if (Serial.available()) {
		ReceiveData();
		if (Command == commandType::STOP) {
			enableStepper = false;
			//Mode = Modes::STOPPED;
		}

		if (Command == commandType::MOVE) //move distance
		{
			enableStepper = false;
			if (cmdSpeed < 2500 && cmdSpeed >= 1) {
				freq = cmdSpeed;
				enableStepper = true;
				act[0].moveDistance(cmdDistance, cmdDirection, cmdSpeed);
				startInterrupt(freq);
			}
		}

		else
		{
			Error = errorTypes::BAD_COMMAND;
		}
	}

	//send stuff whenever you get a chance
	if (sendStuff == true) {
		SendData();
	}

	//send positions when moving
	if (millis() - sendTimer > sendInterval) {
		if (act[0].getStatus()) {
			if (sendStuff == false) { //this will only send when the queue is empty. Only way I could get both to send without clogging up the queue
				queue.enqueue(sendType::POSITION);
				queue.enqueue(sendType::STEPPERCOUNTER);
				sendStuff = true;
			}		
		}
		sendTimer = millis();
	}
	
}

void Calibrate()
{
	//need to figure out way to mount endstops first
	//move like 10 steps at a time
	if (Serial.available()) {
		ReceiveData();
		if (Command == commandType::STOP) {
			enableStepper = false;
			//Mode = Modes::STOPPED;
		}
	}
	act[0].resetSteppers();
}
void SendData()
{
	sendData = queue.dequeue();
	Serial.print("x");
	Serial.print(sendData);
	Serial.print("y");
	switch (sendData) {
	case sendType::STATUS:
		Serial.print(act[0].getStatus());
		queue.enqueue(sendType::STEPPERCOUNTER);
		queue.enqueue(sendType::POSITION);
		break;
	case sendType::POSITION:
		Serial.print(act[0].getPosition());
		break;
	case sendType::DIRECTION:
		Serial.print(act[0].getDirection());
		break;
	case sendType::STEPPERCOUNTER:
		Serial.print(act[0].getStepCounter());
		break;
	}
	Serial.print("y");
	Serial.println("z");
	if (queue.isEmpty()) {
		sendStuff = false;
		//sendData = sendType::NONE;
	}
}

void ReceiveData()
{
	int bytes = Serial.available();
	if (bytes > 0) {
		String read = Serial.readStringUntil(END_OF_TRANSMISSION);
		int delim1Pos = read.indexOf(';');
		String CommandString = (delim1Pos == -1) ? read : read.substring(0, delim1Pos);
		int commandCode = CommandString.toInt();
		if (commandCode == 0) {
			return; // panic;6
		}
		commandType currentCommand = (commandType)commandCode;
		float tmpdistance = cmdDistance, tmpspeed = cmdSpeed;
		ActuatorDirectionCommand tmpdirection = cmdDirection;
		int delim2Pos = -1, delim3Pos = -1, delim4Pos = -1, delim5Pos = -1;
		switch (currentCommand) {
		case commandType::STOP:
			tmpdistance = 0;
			tmpspeed = 0;
			tmpdirection = ActuatorDirectionCommand::UP;
			break;
		case commandType::MOVE:
			//3;distance;direction (1 or 2);speed;
			delim2Pos = read.indexOf(';', delim1Pos + 1);
			if (delim2Pos == -1) {
				return;  // panic
			}
			tmpdistance = read.substring(delim1Pos + 1, delim2Pos).toFloat();

			delim3Pos = read.indexOf(';', delim2Pos + 1);
			if (delim3Pos == -1) {
				return;  // panic
			}
			switch (read.substring(delim2Pos + 1, delim3Pos).toInt())
			{
			case 1: tmpdirection = ActuatorDirectionCommand::UP;
				break;
			case 2: tmpdirection = ActuatorDirectionCommand::DOWN;
				break;
			default: Error = errorTypes::BAD_COMMAND_STRING;
				return; //panic
			}

			delim4Pos = read.indexOf(';', delim3Pos + 1);
			if (delim4Pos == -1) {
				return;  // panic
			}
			tmpspeed = read.substring(delim3Pos + 1, delim4Pos).toFloat();
			break;
		}
		cmdDistance = tmpdistance;
		cmdDirection = tmpdirection;
		cmdSpeed = tmpspeed;
		Command = currentCommand;
	}
}
