/*************************************************************************
* Testing sketch for Freematics OBD-II UART Adapter V2.1
* Reads and prints motion sensor data and computed orientation data
* Distributed under BSD
* Visit https://freematics.com/products for more product information
* Written by Stanley Huang <stanley@freematics.com.au>
*************************************************************************/\

#include <Wire.h>

#include "SparkFun_BNO080_Arduino_Library.h" // Click here to get the library: http://librarymanager/All#SparkFun_BNO080
BNO080 myIMU;

#include <OBD2UART.h>

// On Arduino Leonardo, Micro, MEGA or DUE, hardware serial can be used for output as the adapter occupies Serial1
// On Arduino UNO and those have no Serial1, we use software serial for output as the adapter uses Serial
#ifdef ARDUINO_AVR_UNO
#include <SoftwareSerial.h>
SoftwareSerial mySerial(A2, A3);
#else
#define mySerial Serial
#endif

#if defined(ESP32) && !defined(Serial1)
HardwareSerial Serial1(1);
#endif

COBD obd;
int velocity = 0.0f;
int count = 0;

void setup()
{
  mySerial.begin(115200);
  while (!mySerial);
  
  // this will begin serial

  for (;;) {
    delay(1000);
    byte version = obd.begin();
    mySerial.print("Freematics OBD-II Adapter ");
    if (version > 0) {
      mySerial.println("detected");
      break;
    } else {
      mySerial.println("not detected");
    }
  }
  
  // initialize MEMS with sensor fusion enabled
  bool hasMEMS = obd.memsInit(true);
  mySerial.print("Motion sensor is ");
  mySerial.println(hasMEMS ? "present" : "not present");
  if (!hasMEMS) {
    for (;;) delay(1000);
  }

  Wire.begin();

  if (myIMU.begin() == false)
  {
    Serial.println(F("BNO080 not detected at default I2C address. Check your jumpers and the hookup guide. Freezing..."));
    while (1)
      ;
  }

  Wire.setClock(400000); //Increase I2C data rate to 400kHz

  myIMU.enableRotationVector(40); //Send data update every 50ms

  Serial.println(F("Rotation vector enabled"));
  Serial.println(F("Output in form roll, pitch, yaw"));
}


void loop()
{
    float roll = (myIMU.getRoll()) * 180.0 / PI; // Convert roll to degrees
    float pitch = (myIMU.getPitch()) * 180.0 / PI; // Convert pitch to degrees
    float yaw = (myIMU.getYaw()) * 180.0 / PI; // Convert yaw / heading to degrees

    Serial.print(roll, 1);
    Serial.print(F(","));
    Serial.print(pitch, 1);
    Serial.print(F(","));
    Serial.print(yaw, 1);
    Serial.print(F(","));
    Serial.println(velocity);

  if(count == 4)
  {
    count = 0;
    obd.readPID(PID_SPEED, velocity);  
  }

  count++;
}
