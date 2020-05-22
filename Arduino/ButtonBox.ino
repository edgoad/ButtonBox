/* @file HelloKeypad.pde
|| @version 1.0
|| @author Alexander Brevig
|| @contact alexanderbrevig@gmail.com
||
|| @description
|| | Demonstrates the simplest use of the matrix Keypad library.
|| #
*/
#include <Keypad.h>

const byte ROWS = 3; //four rows
const byte COLS = 5; //three columns

char keys[ROWS][COLS] = {
  {'f','e','d','c','b'},
  {'a','9','8','7','6'},
  {'5','4','3','2','1'}
};

byte rowPins[ROWS] = { 4, 3, 2}; //connect to the row pinouts of the keypad
byte colPins[COLS] = {9, 8, 7, 6, 5}; //connect to the column pinouts of the keypad

Keypad keypad = Keypad( makeKeymap(keys), rowPins, colPins, ROWS, COLS );

void setup(){
  Serial.begin(9600);
}
  
void loop(){
  char key = keypad.getKey();
  
  if (key){
    Serial.println(key);
  }
}
