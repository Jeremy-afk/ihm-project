const int redPin = 9;    // Pin for red LED
const int greenPin = 10; // Pin for green LED
const int xDirPin = A0;  // Pin for joystick X-axis
const int yDirPin = A1;  // Pin for joystick Y-axis
const int buttonPin = 8;  // Pin for Button

String incomingData = ""; // Buffer for serial data
bool wasButtonPressed;

void setup() {
  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(xDirPin, INPUT);
  pinMode(yDirPin, INPUT);
  Serial.begin(9600); // Initialize serial communication
}

void loop() {

  // Check if serial data is available
  while (Serial.available() > 0) {
    incomingData = Serial.readStringUntil('\n');
    manageLeds();
    delay(10);
    Serial.flush();
  }
  sendInput(); // Handle input
  
}

void manageLeds() {
  if (incomingData.startsWith("LED:")) {
    incomingData.remove(0, 4); // Remove "LED:" prefix
    float barValue = incomingData.toFloat(); // Convert to float

    // Ensure barValue is within expected range
    barValue = constrain(barValue, 0.0, 1.0);

    // Calculate LED intensities
    int redIntensity = barValue < 0.5 ? (1.0 - barValue * 2.0) * 255 : 0;
    int greenIntensity = barValue > 0.5 ? (barValue - 0.5) * 2.0 * 255 : 0;

    // Apply intensities to LEDs
    analogWrite(redPin, redIntensity);
    analogWrite(greenPin, greenIntensity);
  }
}

void sendInput() {
  float x = analogRead(xDirPin) / 512.0 - 1;
  float y = analogRead(yDirPin) / 512.0 - 1;
  bool buttonPressed = !digitalRead(buttonPin);

  // Send joystick data to serial
  Serial.print("JOY:");
  Serial.print(x, 3); // Limit to 3 decimal places for clarity
  Serial.print(",");
  Serial.println(y, 3);

  // Send button data to serial only if it changed state (^ is the XOR bitwise operator)
  if (buttonPressed ^ wasButtonPressed) {
    Serial.print("BTN:");
    Serial.println(buttonPressed ? "1" : "0");
    wasButtonPressed = buttonPressed;
  }
}
